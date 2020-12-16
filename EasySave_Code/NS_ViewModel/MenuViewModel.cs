using EasySave.NS_Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EasySave.NS_ViewModel
{
    public class MenuViewModel
    {
        // ----- Attributes -----
        public Model model { get; set; }
        private int currentNbPrioFile { get; set; }
        public Socket client { get; set; }
        public Socket listener { get; set; }

        private static AutoResetEvent autoResetEventWorks = new AutoResetEvent(true);
        private static AutoResetEvent autoResetEventLogs = new AutoResetEvent(true);
        private static AutoResetEvent autoResetEventOverSized = new AutoResetEvent(true);


        // ----- Constructor -----
        public MenuViewModel(Model _model)
        {
            this.currentNbPrioFile = 0;
            this.model = _model;
            this.client = null;
            this.listener = null;
        }


        // ----- Methods -----
        // Update Work State
        public void UpdateWorkColor(Work _work, string _color)
        {
            autoResetEventWorks.WaitOne();
            _work.colorProgressBar = _color;
            if(client != null)
            {
                SendList(client);
            }
            autoResetEventWorks.Set(); 
        }


        // Reset Work Sate
        public void ResetWorkState(Work _work)
        {
            autoResetEventWorks.WaitOne();
            _work.state.InitState(0, 0, 0, "", "");
            _work.lastBackupDate = DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss");
            this.model.SaveWorks();
            autoResetEventWorks.Set();
        }

        public void RemoveWork(int _worksId)
        {
            try
            {
                // Remove Work from the program (at index)
                autoResetEventWorks.WaitOne();
                this.model.works.Remove(this.model.works[_worksId]);
                this.model.SaveWorks();
                autoResetEventWorks.Set();
            }
            catch
            {
                // Return Error Code
                model.errorMsg?.Invoke("errorRemoveWork");
            }
        }


        // Launch Backup Work with a New Thread
        public void LaunchBackupWork(int _worksId)
        {
            Task.Run(() =>
            {
                SaveWork(this.model.works[_worksId]);
            });

        }

        private void SaveWork(Work _workToSave)
        {
            // Take destination disk
            DriveInfo dstDisk = new DriveInfo(_workToSave.dst.Substring(0, 1));

            // Check if current and destination folder and destination disk are correct && Check If the folder can be crypted (true if not crypted work)
            if (IsSaveDirsCorrect(_workToSave.src, _workToSave.dst) && IsDiskReady(dstDisk) && IsEncryptionPossible(_workToSave))
            {
                // Get every files info to copy
                FileInfo[] filesToSave = GetFilesToSave(_workToSave);
                string dstFolder = _workToSave.dst + _workToSave.name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "\\";

                // Check if there is files to copy, enough place in dst folder and we can create destination folder
                if (IsFilesToSave(filesToSave.Length) && IsSpaceInDstDir(dstDisk, _workToSave.state.totalSize) && InitDstFolder(dstFolder))
                {
                    // Save every file and get back the failed files
                    List<string> failedFiles = SaveFiles(_workToSave, filesToSave, dstFolder);

                    // If there is any errors
                    if (failedFiles.Count != 0)
                    {
                        // Return Error Code
                        model.errorMsg?.Invoke("backupFinishedWithError");
                    }
                }

                // Reset the work object
                UpdateWorkColor(_workToSave, "White");
                ResetWorkState(_workToSave);
            }
        }

        private bool IsEncryptionPossible(Work _work)
        {
            if (!(_work.isCrypted && this.model.settings.cryptoSoftPath.Length == 0))
            {
                return true;
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("cryptoSoftPathNotFound");
                return false;
            }
        }

        private bool IsFilesToSave(int _nbFilesToSave)
        {
            if (_nbFilesToSave > 0)
            {
                return true;
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("noChangeSinceLastBackup");
                return false;
            }
        }

        private bool InitDstFolder(string _dstFolder)
        {
            // Create the dst folder
            try
            {
                Directory.CreateDirectory(_dstFolder);
                return true;
            }
            catch
            {
                // Return Error Code
                model.errorMsg?.Invoke("cannotCreateDstFolder");
                return false;
            }
        }

        private bool IsBusinessRunning()
        {
            foreach (string businessSoftware in this.model.settings.businessSoftwares)
            {
                if (Process.GetProcessesByName(businessSoftware).Length > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsDiskReady(DriveInfo _dstDisk)
        {
            if(_dstDisk.IsReady)
            {
                return true;
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("diskError");
                return false;
            }
        }

        private bool IsSpaceInDstDir(DriveInfo _dstDisk, long _totalSize)
        {
            if (_dstDisk.TotalFreeSpace > _totalSize)
            {
                return true;
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("noSpaceDstFolder");
                return false;
            }
        }

        private bool IsSaveDirsCorrect(string _src, string _dst)
        {
            // Check if the source exists
            if (!Directory.Exists(_src))
            {
                // Return Error Code
                model.errorMsg?.Invoke("unavailableSrcPath");
                return false;
            }

            // Check if the destionation folder exists
            if (!Directory.Exists(_dst))
            {
                // Return Error Code
                model.errorMsg?.Invoke("unavailableDstPath");
                return false;
            }
            return true;
        }

        private FileInfo[] GetFilesToSave(Work _work) // TODO - Total Size
        {
            long totalSize = 0;
            int totalPrioFile = 0;

            // Get evvery files of the source directory
            DirectoryInfo srcDir = new DirectoryInfo(_work.src);
            FileInfo[] srcFiles = srcDir.GetFiles("*.*", SearchOption.AllDirectories);
            List<FileInfo> filesToSave = new List<FileInfo>();

            switch (_work.backupType)
            {
                case BackupType.FULL:
                    // Calcul the size of every files
                    foreach (FileInfo file in srcFiles)
                    {
                        // Calcul the size of every files
                        totalSize += file.Length;

                        // Check if its a prio file or note
                        if (file.Name.Contains(".") && this.model.settings.prioExtensions.Contains(file.Name.Substring(file.Name.LastIndexOf("."))))
                        {
                            filesToSave.Insert(0, file);
                            totalPrioFile++;
                        }
                        else
                        {
                            filesToSave.Add(file);
                        }
                    }

                    // Init the state of the current work to save
                    autoResetEventWorks.WaitOne();
                    _work.state.InitState(filesToSave.Count, totalPrioFile, totalSize, "", "");
                    autoResetEventWorks.Set();

                    autoResetEventLogs.WaitOne();
                    currentNbPrioFile += totalPrioFile;
                    autoResetEventLogs.Set();

                    return filesToSave.ToArray();

                case BackupType.DIFFRENTIAL:
                    // Get all directories name of the dest folder
                    DirectoryInfo[] dirs = new DirectoryInfo(_work.dst).GetDirectories();
                    string lastFullDirName = GetFullBackupDir(dirs, _work.name);

                    // If there is no full backup as a ref, we create the first one as full backup
                    if (lastFullDirName.Length == 0) goto case BackupType.FULL;

                    // Get every files of the source directory

                    // Check if there is a modification between the current file and the last full backup
                    foreach (FileInfo file in srcFiles)
                    {
                        string currFullBackPath = lastFullDirName + "\\" + Path.GetRelativePath(_work.src, file.FullName);

                        if (!File.Exists(currFullBackPath) || !IsSameFile(currFullBackPath, file.FullName))
                        {
                            // Calcul the size of every files
                            totalSize += file.Length;

                            // Check if its a prio file or note
                            if (file.Name.Contains(".") && this.model.settings.prioExtensions.Contains(file.Name.Substring(file.Name.LastIndexOf("."))))
                            {
                                filesToSave.Insert(0, file);
                                totalPrioFile++;
                            }
                            else
                            {
                                filesToSave.Add(file);
                            }
                        }
                    }

                    // Init the state of the current work to save
                    autoResetEventWorks.WaitOne();
                    _work.state.InitState(filesToSave.Count, totalPrioFile, totalSize, "", "");
                    autoResetEventWorks.Set();

                    autoResetEventLogs.WaitOne();
                    currentNbPrioFile += totalPrioFile;
                    autoResetEventLogs.Set();

                    return filesToSave.ToArray();

                default:
                    model.errorMsg?.Invoke("unavailableBackupType");
                    return new FileInfo[0];
            }
        }


        // Get the directory name of the first full backup of a differential backup
        private string GetFullBackupDir(DirectoryInfo[] _dstDirs, string _name)
        {
            foreach (DirectoryInfo directory in _dstDirs)
            {
                if (directory.Name.IndexOf("_") > 0 && _name == directory.Name.Substring(0, directory.Name.IndexOf("_")))
                {
                    return directory.FullName;
                }
            }
            return "";
        }


        // Check if the file or the src is the same as the full backup one to know if the files need to be copied or not
        private bool IsSameFile(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);

            if (file1.Length == file2.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private List<string> SaveFiles(Work _work, FileInfo[] _filesToSave, string _dstFolder)
        {
            // Create a name list of failed files
            List<string> failedFiles = new List<string>();
            autoResetEventWorks.WaitOne();
            int totalFile = _work.state.totalFile;
            int totalPrioFile = _work.state.totalPrioFile;
            long leftSize = _work.state.totalSize;
            autoResetEventWorks.Set();

            // Save file one by one
            for (int i = 0; i < totalFile; i++)
            {
                FileInfo curFile = _filesToSave[i];

                DateTime startTimeSave = DateTime.Now;
                int copyTime = 0;
                int encryptionTime = 0;

                autoResetEventWorks.WaitOne();
                int pourcent = Convert.ToInt32((_work.state.totalSize - leftSize) * 100 / _work.state.totalSize);
                autoResetEventWorks.Set();

                string dstFile = GetDstFilePath(curFile, _dstFolder, _work.src);

                // Update the current work status
                autoResetEventWorks.WaitOne();
                _work.state.UpdateState(pourcent, ((totalPrioFile - i > 0) ? totalPrioFile - i : 0), totalFile - i, leftSize, curFile.FullName, dstFile);
                this.model.SaveWorks();
                autoResetEventWorks.Set();

                // Cancel Backup (If User Click on Red Button)
                if (_work.colorProgressBar == "White")
                {
                    CancelBackup(_work, _dstFolder);
                    return failedFiles;
                }

                // Pause Backup (If Click on Orange Button)
                if (_work.colorProgressBar == "Orange")
                {
                    PauseBackup(_work);
                }

                // Block Backup (If a Business Software is Running)
                if (IsBusinessRunning())
                {
                    BlockBusinessRunning(_work);
                }

                // Block Backup (If there are no Priority Files in this Backup but there are on others)
                if (_work.state.leftPrioFile == 0 && currentNbPrioFile != 0)
                {
                    BlockNoPriority(_work);
                }

                // Block Backup (If there are more than one File that exceeds Max Simultaneous File Size)
                if (this.model.settings.maxSimultaneousFilesSize > 0 && curFile.Length >= this.model.settings.maxSimultaneousFilesSize)
                {
                    BlockSaveBandWidth(_work);
                }

                // Check if the file is crypted or not
                if (!(_work.isCrypted && curFile.Name.Contains(".") && this.model.settings.cryptoExtensions.Contains(curFile.Name.Substring(curFile.Name.LastIndexOf(".")))))
                {
                    // Save File
                    copyTime = CopyFile(curFile, dstFile, startTimeSave);
                }
                else
                {
                    // Crypt File
                    encryptionTime = EncryptFile(curFile, dstFile);
                }

                // Decrease Left Files Size
                leftSize -= curFile.Length;

                // Decrease Priority File Counter (if it was one)
                autoResetEventLogs.WaitOne();
                if (_work.state.leftPrioFile > 0)
                {
                    currentNbPrioFile = currentNbPrioFile - 1;
                }
                autoResetEventLogs.Set();

                // Set the Lock of BlockSaveBandWidth (and allow a File that exceeds Max Simultaneous File Size to be copied  
                if (curFile.Length >= this.model.settings.maxSimultaneousFilesSize)
                {
                    autoResetEventOverSized.Set();
                }

                // Save Logs
                autoResetEventLogs.WaitOne();
                this.model.logs.Add(new Log($"{_work.name}", $"{curFile.FullName}", $"{dstFile}", $"{curFile.Length}", $"{startTimeSave}", $"{copyTime}", $"{encryptionTime}"));
                this.model.SaveLog();
                autoResetEventLogs.Set();

                Trace.WriteLine($"{_work.name} {curFile.FullName} {dstFile} {curFile.Length} {startTimeSave} {copyTime} {encryptionTime}");
            }
            return failedFiles;
        }


        // Cancel a Backup
        private void CancelBackup(Work _work, string _dstFolder)
        {
            // Try Delete Current Backup Folder
            try
            {
                Directory.Delete(_dstFolder, true);
            }
            catch (Exception)
            {
                this.model.errorMsg?.Invoke("cannotDelDstFolder");
            }

            // Decrease Priority File Counter 
            autoResetEventLogs.WaitOne();
            currentNbPrioFile -= _work.state.leftPrioFile;
            autoResetEventLogs.Set();

            // Reset Work State to null
            ResetWorkState(_work);
        }


        // Pause Backup (While User do not Change Backup State)
        private void PauseBackup(Work _work)
        {
            // Decrease Priority File Counter
            autoResetEventLogs.WaitOne();
            currentNbPrioFile -= _work.state.leftPrioFile;
            autoResetEventLogs.Set();

            // Pause Program
            while (_work.colorProgressBar == "Orange") { }

            // Increase Priority File Counter
            autoResetEventLogs.WaitOne();
            currentNbPrioFile += _work.state.leftPrioFile;
            autoResetEventLogs.Set();
        }


        // Block Backup (While a Business Software is Running)
        private void BlockBusinessRunning(Work _work)
        {
            // Set Progress Bar Color to Business Software Running Color
            if (!(_work.colorProgressBar == "Orange" || _work.colorProgressBar == "White"))
            {
                UpdateWorkColor(_work, "Red");
            }

            // Return Error Code
            model.errorMsg?.Invoke("businessSoftwareOn"); // TODO - "Cannot launch any backups bc business software ON"

            // Pause Program While a Business Software is Running
            while (IsBusinessRunning())
            {
                // Break to loop to Cancel
                if (_work.colorProgressBar == "White")
                {
                    break;
                }
            }

            // Reset Progress Bar Color
            if (!(_work.colorProgressBar == "Orange" || _work.colorProgressBar == "White"))
            {
                UpdateWorkColor(_work, "Green");
            }
        }


        // Block Backup (While there are no Priority Files in this Backup but there are on others)
        private void BlockNoPriority (Work _work)
        {
            // Set Progress Bar Color to No Priority Color
            if (!(_work.colorProgressBar == "Orange" || _work.colorProgressBar == "White"))
            {
                UpdateWorkColor(_work, "Purple");
            }

            // Pause Program
            while (_work.state.leftPrioFile == 0 && currentNbPrioFile != 0)
            {
                if (_work.colorProgressBar == "Orange" || _work.colorProgressBar == "White")
                {
                    break;
                }
            }

            // Reset Progress Bar Color
            if (!(_work.colorProgressBar == "Orange" || _work.colorProgressBar == "White"))
            {
                UpdateWorkColor(_work, "Green");
            }
        }


        // Block Backup (If one File that exceeds Max Simultaneous File Size is being copied)
        private void BlockSaveBandWidth(Work _work)
        {
            // Set Progress Bar Color to Save Band Width Color
            if (!(_work.colorProgressBar == "Orange" || _work.colorProgressBar == "White"))
            {
                UpdateWorkColor(_work, "DodgerBlue");
            }

            // When the First passes here, it reset lock. Then when an other arrive, it's locked
            // The Lock is Set After a File that exceeds Max Simultaneous File Size is done being copied
            autoResetEventOverSized.WaitOne();

            // Reset Progress Bar Color
            if (!(_work.colorProgressBar == "Orange" || _work.colorProgressBar == "White"))
            {
                UpdateWorkColor(_work, "Green");
            }
        }

        private string GetDstFilePath(FileInfo _srcFile, string _dst, string _src)
        {
            string curDirPath = _srcFile.DirectoryName;
            string dstDirectory = _dst;

            // If there is a directoy, we add the relative path from the directory dst
            if (Path.GetRelativePath(_src, curDirPath).Length > 1)
            {
                dstDirectory += Path.GetRelativePath(_src, curDirPath) + "\\";

                // If the directory dst doesn't exist, we create it
                if (!Directory.Exists(dstDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(dstDirectory);
                    }
                    catch
                    {
                        // If cannot create the dst path
                        // TODO - Error Msg
                        return "";
                    }
                }
            }

            // Return the current dstFile
            return dstDirectory + _srcFile.Name;
        }

        private int CopyFile(FileInfo _curFile, string _dstFile, DateTime _startTime)
        {
            try
            {
                _curFile.CopyTo(_dstFile, true);
                return (int) (DateTime.Now - _startTime).TotalMilliseconds;
            }
            catch
            {
                return -1;
            }
        }

        private int EncryptFile(FileInfo _curFile, string _dstFile)
        {
            try
            {
                ProcessStartInfo process = new ProcessStartInfo(this.model.settings.cryptoSoftPath);
                process.Arguments = "source " + _curFile.FullName + " destination " + _dstFile;
                var proc = Process.Start(process);
                proc.WaitForExit();
                return proc.ExitCode;
            }
            catch
            {
                return -1;
            }
        }

        // Sockets
        public void SocketOn()
        {
            // Launch Socket
            Task.Run(() =>
            {
                // Listen connection
                listener = Connect();
                // Accept connection with the client
                client = AcceptConnection(listener);
                // Send a first list of works and start listen for actions
                SendList(client);
                ListenAction(client);
            });
        }

        // Connect a new socket
        private Socket Connect()
        {
            // IP Address and server Port
            string localIP = "127.0.0.1";
            int port = 8080;

            // Create a listener socket
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress address = IPAddress.Parse(localIP);
            IPEndPoint endPoint = new IPEndPoint(address, 8080);

            // Bind the socket
            listenSocket.Bind(endPoint);
            listenSocket.Listen(10);

            Trace.WriteLine($"Server ON and listening {address}:{port}");

            return listenSocket;
        }

        // Accept connection with a client
        private Socket AcceptConnection(Socket _listen)
        {
            // Accept client
            Socket client = _listen.Accept();

            Trace.WriteLine($"Connection established with : {client.LocalEndPoint}");

            return client;
        }

        // Send a list of works
        private void SendList(Socket client)
        {
                // Create a list
                var workList = new List<WorkToSend>();

                // Add every works in the list
                foreach (Work work in this.model.works)
                {
                    workList.Add(new WorkToSend(work.name, work.state.progress, work.colorProgressBar));
                }

                // Convert the json list to string
                string jsonWork = JsonSerializer.Serialize(workList);
                byte[] state = Encoding.Default.GetBytes(jsonWork);
            try
            {
                // Send the liste
                client.Send(state);
            } catch (SocketException)
            {
                // Client quit
                Trace.WriteLine(Langs.Lang.socketDeconnection);
            }

        }

        // Listen for actions
        private void ListenAction(Socket client)
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[512];
                    int receivedBytes = client.Receive(buffer);

                    // Encode to string the message
                    string messageClient = Encoding.Default.GetString(buffer, 0, receivedBytes);
                    // Deserialize the string into an ReceiveObject
                    var action = JsonSerializer.Deserialize<ReceiveObject>(messageClient);

                    autoResetEventWorks.WaitOne();
                    // Process different progress state for every works on the list
                    foreach (int id in action.selectedId)
                    {
                        if (action.action == "Green")
                        {
                            switch (this.model.works[id].colorProgressBar)
                            {
                                case "White":
                                    this.model.works[id].colorProgressBar = action.action;
                                    this.LaunchBackupWork(id);
                                    break;

                                case "Orange":
                                    this.model.works[id].colorProgressBar = action.action;
                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            this.model.works[id].colorProgressBar = action.action;
                        }

                    }
                    autoResetEventWorks.Set();
                } catch (SocketException)
                {
                    // Socket deconnected
                    this.client = null;
                    this.listener = null;
                    MessageBox.Show(Langs.Lang.socketDeconnection);
                    break;
                }
                
            }
        }
    }
}

