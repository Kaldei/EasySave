using EasySave.NS_Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EasySave.NS_ViewModel
{
    public class MenuViewModel
    {
        // ----- Attributes -----
        public Model model { get; set; }
        private int currentNbPrioFile { get; set; }

        private static AutoResetEvent autoResetEventWorks = new AutoResetEvent(true);
        private static AutoResetEvent autoResetEventLogs = new AutoResetEvent(true);
        private static AutoResetEvent autoResetEventOverSized = new AutoResetEvent(true);



        // ----- Constructor -----
        public MenuViewModel(Model _model)
        {
            this.currentNbPrioFile = 0;
            this.model = _model;
        }


        // ----- Methods -----
        // Update Work State
        public void UpdateWorkColor(int _index, string _color)
        {
            autoResetEventWorks.WaitOne();
            this.model.works[_index].colorProgressBar = _color;
            autoResetEventWorks.Set();

            if (_color == "White")
            {
                autoResetEventWorks.WaitOne();
                this.model.works[_index].state = null;
                autoResetEventWorks.Set();
            }
            
        }


        private List<Work> getWorksById(int[] _worksId)
        {
            List<Work> works = new List<Work>();

            foreach (int workId in _worksId)
            {
                works.Add(model.works[workId]);
            }
            return works;
        }

        private bool IsWorkSelected(int[] _worksId) // TODO - Look if we can do better or did we let this here
        {
            if (_worksId.Length > 0)
            {
                return true;
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("noSelectedWork");
                return false;
            }
        }

        public void RemoveWorks(int[] _worksId)
        {
            if (IsWorkSelected(_worksId))
            {
                List<Work> worksToRemove = getWorksById(_worksId);

                foreach (Work workToRemove in worksToRemove)
                {
                    RemoveWork(workToRemove);
                }
            }
        }

        private void RemoveWork(Work _workToRemove)
        {
            try
            {
                // Remove Work from the program (at index)
                this.model.works.Remove(_workToRemove);
                this.model.SaveWorks();
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
                    autoResetEventWorks.WaitOne();
                    _workToSave.colorProgressBar = "Green";
                    autoResetEventWorks.Set();
                    List<string> failedFiles = SaveFiles(_workToSave, filesToSave, dstFolder);

                    // If there is any errors
                    if (failedFiles.Count != 0)
                    {
                        // Return Error Code
                        model.errorMsg?.Invoke("backupFinishedWithError");
                    }
                }

                // Reset the work object
                autoResetEventWorks.WaitOne();
                _workToSave.colorProgressBar = "White";
                _workToSave.lastBackupDate = DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss");
                this.model.SaveWorks();
                autoResetEventWorks.Set();
                
                Trace.WriteLine(_workToSave.name + " finished " + DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss"));
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

                    Trace.WriteLine(filesToSave.Count);
                    Trace.WriteLine(totalPrioFile);
                    // Init the state of the current work to save
                    autoResetEventWorks.WaitOne();
                    _work.state = new State(filesToSave.Count, totalPrioFile, totalSize, "", "");
                    autoResetEventWorks.Set();

                    autoResetEventWorks.WaitOne();
                    currentNbPrioFile += totalPrioFile;
                    autoResetEventWorks.Set();

                    return filesToSave.ToArray();

                case BackupType.DIFFRENTIAL:
                    // Get all directories name of the dest folder
                    DirectoryInfo[] dirs = new DirectoryInfo(_work.dst).GetDirectories();
                    string lastFullDirName = GetFullBackupDir(dirs, _work.name);

                    // If there is no full backup as a ref, we create the first one as full backup
                    if (lastFullDirName.Length == 0) goto case BackupType.FULL;

                    // Get evvery files of the source directory

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
                    _work.state = new State(filesToSave.Count, totalPrioFile, totalSize, "", "");
                    autoResetEventWorks.Set();

                    autoResetEventWorks.WaitOne();
                    currentNbPrioFile += totalPrioFile;
                    autoResetEventWorks.Set();

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
            int totalFile = _work.state.totalFile;
            int totalPrioFile = _work.state.totalPrioFile;
            long leftSize = _work.state.totalSize;

            // Save file one by one
            for (int i = 0; i < totalFile; i++)
            {
                // Pause Backup if a Business Software is Running
                if (IsBusinessRunning())
                {
                    // Set Progress Bar Color to Error Color
                    autoResetEventWorks.WaitOne();
                    _work.colorProgressBar = "Red";
                    autoResetEventWorks.Set();

                    // Return Error Code
                    model.errorMsg?.Invoke("businessSoftwareOn"); // TODO - "Cannot launch any backups bc business software ON"

                    // Pause Program
                    while (IsBusinessRunning()) { }

                    // Reset Progress Bar Color
                    autoResetEventWorks.WaitOne();
                    _work.colorProgressBar = "Green";
                    autoResetEventWorks.Set();
                }

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


                // Lock if there are more than one oversized File
                if (curFile.Length >= this.model.settings.maxSimultaneousFilesSize)
                {
                    autoResetEventWorks.WaitOne();
                    _work.colorProgressBar = "DodgerBlue";
                    autoResetEventWorks.Set();

                    autoResetEventOverSized.WaitOne();

                    autoResetEventWorks.WaitOne();
                    _work.colorProgressBar = "Green";
                    autoResetEventWorks.Set();
                }


                // Lock if there are priority Files
                if (_work.state.leftPrioFile == 0 && currentNbPrioFile != 0)
                {
                    // Set Progress Bar Color to Error Color
                    autoResetEventWorks.WaitOne();
                    _work.colorProgressBar = "Purple";
                    autoResetEventWorks.Set();

                    // Pause Program
                    while (_work.state.leftPrioFile == 0 && currentNbPrioFile != 0) { }

                    // Reset Progress Bar Color
                    autoResetEventWorks.WaitOne();
                    _work.colorProgressBar = "Green";
                    autoResetEventWorks.Set();
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

                leftSize -= curFile.Length;

                // Decrase Priority file if it was one
                autoResetEventWorks.WaitOne();
                if (_work.state.leftPrioFile > 0)
                {
                    currentNbPrioFile = currentNbPrioFile - 1;
                }
                autoResetEventWorks.Set();


                // Free the Overzied File Lock
                if(curFile.Length >= this.model.settings.maxSimultaneousFilesSize)
                {
                    autoResetEventOverSized.Set();
                }

                // Save Logs
                autoResetEventLogs.WaitOne();
                this.model.logs.Add(new Log($"{_work.name}", $"{curFile.FullName}", $"{dstFile}", $"{curFile.Length}", $"{startTimeSave}", $"{copyTime}", $"{encryptionTime}"));
                this.model.SaveLog();
                autoResetEventLogs.Set();

                // If User Ask to Cancel a Backup
                if (_work.colorProgressBar == "White")
                {
                    try
                    {
                        Directory.Delete(_dstFolder,true);                   
                    }
                    catch (Exception)
                    {
                        this.model.errorMsg?.Invoke("cannotDelDstFolder");
                    }
                    return failedFiles;
                }

                // Check User ask to pause
                if (_work.colorProgressBar == "Orange")
                {
                    // Pause Program
                    while (_work.colorProgressBar == "Orange") { }

                    // Reset Progress Bar Color
                    autoResetEventWorks.WaitOne();
                    _work.colorProgressBar = "Green";
                    autoResetEventWorks.Set();
                }

                Trace.WriteLine($"{_work.name} {curFile.FullName} {dstFile} {curFile.Length} {startTimeSave} {copyTime} {encryptionTime}");
            }
            return failedFiles;
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
    }
}
