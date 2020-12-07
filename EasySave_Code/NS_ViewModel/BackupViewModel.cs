using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using EasySave.NS_Model;

public delegate void CurrentBackupInfo(string _name, int _totalFileSuccess, int _totalFile, int _timeTaken);

namespace EasySave.NS_ViewModel
{
    public class BackupViewModel
    {
        public CurrentBackupInfo currentBackupInfo;

        // ----- Attributes -----
        public Model model { get; set; }


        // ----- Constructor -----
        public BackupViewModel(Model _model)
        {
            this.model = _model;
        }

        public void LaunchBackupWork(int[] idWorkToSave)
        {
            if (this.model.works.Count > 0 && idWorkToSave.Length > 0)
            {
                for (int i = 0; i < idWorkToSave.Length; i++)
                {
                    // Get id of one Running Business Software from the List (-1 if none) 
                    foreach (string businessSoftware in this.model.settings.businessSoftwares)
                    {
                        if (Process.GetProcessesByName(businessSoftware).Length > 0)
                        {
                            for (int j = i; j < idWorkToSave.Length; j++)
                            {
                                currentBackupInfo?.Invoke("error", 0, 0, 0);
                            }
                            // Return Error Code
                            model.errorMsg?.Invoke("businessSoftwareOn");

                            return;
                        }
                    }
                    LaunchBackupType(this.model.works[idWorkToSave[i]]);
                    // this.view.ConsoleUpdate(LaunchBackupType(this.works[idWorkToSave[i]]));
                    // this.view.ConsoleUpdate(4);
                }

                // Retour menu 
                //this.view.ConsoleUpdate(1);
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("noSelectedWork");
            }
        }

        public void LaunchBackupType(Work _work)
        {
            // Check if the source exists
            if (!Directory.Exists(_work.src))
            {
                // Return Error Code
                model.errorMsg?.Invoke("unavailableSrcPath");
                return;
            }
            DirectoryInfo dir = new DirectoryInfo(_work.src);

            // Check if the destionation folder exists
            if (!Directory.Exists(_work.dst))
            {
                // Return Error Code
                model.errorMsg?.Invoke("unavailableDstPath");
                return;
            }

            // Run the correct backup (Full or Diff)
            switch (_work.backupType)
            {
                case BackupType.DIFFRENTIAL:
                    string fullBackupDir = GetFullBackupDir(_work);

                    // If there is no first full backup, we create the first one (reference of the next diff backup)
                    if (fullBackupDir != null)
                    {
                        DifferentialBackupSetup(_work, dir, fullBackupDir);
                        return;
                    }
                    FullBackupSetup(_work, dir);
                    return;

                case BackupType.FULL:
                    FullBackupSetup(_work, dir);
                    return;

                default:
                    // Return Error Code
                    model.errorMsg?.Invoke("unavailableBackupType");
                    return;
            }
        }

        // Get the directory of the first full backup of a differential backup
        private string GetFullBackupDir(Work _work)
        {
            // Get all directories name of the dest folder
            DirectoryInfo[] dirs = new DirectoryInfo(_work.dst).GetDirectories();

            foreach (DirectoryInfo directory in dirs)
            {
                if (directory.Name.IndexOf("_") > 0 && _work.name == directory.Name.Substring(0, directory.Name.IndexOf("_")))
                {
                    return directory.FullName;
                }
            }
            return null;
        }

        // Full Backup
        private void FullBackupSetup(Work _work, DirectoryInfo _dir)
        {
            long totalSize = 0;

            // Get evvery files of the source directory
            FileInfo[] files = _dir.GetFiles("*.*", SearchOption.AllDirectories);

            // Calcul the size of every files
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }
            DoBackup(_work, files, totalSize);
        }

        // Differential Backup
        private void DifferentialBackupSetup(Work _work, DirectoryInfo _dir, string _fullBackupDir)
        {
            long totalSize = 0;

            // Get evvery files of the source directory
            FileInfo[] srcFiles = _dir.GetFiles("*.*", SearchOption.AllDirectories);
            List<FileInfo> filesToCopy = new List<FileInfo>();

            // Check if there is a modification between the current file and the last full backup
            foreach (FileInfo file in srcFiles)
            {
                string currFullBackPath = _fullBackupDir + "\\" + Path.GetRelativePath(_work.src, file.FullName);

                if (!File.Exists(currFullBackPath) || !IsSameFile(currFullBackPath, file.FullName))
                {
                    // Calcul the size of every files
                    totalSize += file.Length;

                    // Add the file to the list
                    filesToCopy.Add(file);
                }
            }

            // Test if there is file to copy
            if (filesToCopy.Count == 0)
            {
                _work.lastBackupDate = DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss");
                this.model.SaveWorks();
                // Return Error Code
                model.errorMsg?.Invoke("noChangeSinceLastBackup");
            }
            DoBackup(_work, filesToCopy.ToArray(), totalSize);
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

        // Do Backup
        private void DoBackup(Work _work, FileInfo[] _files, long _totalSize)
        {
            DriveInfo disk = new DriveInfo(_work.dst.Substring(0, 1));

            if (disk.IsReady)
            {
                if (disk.TotalFreeSpace > _totalSize)
                {
                    // Create the state file
                    DateTime startTime = DateTime.Now;
                    string dst = _work.dst + _work.name + "_" + startTime.ToString("yyyy-MM-dd_HH-mm-ss") + "\\";

                    // Update the current work status
                    _work.state = new State(_files.Length, _totalSize, _work.src, dst);
                    _work.lastBackupDate = startTime.ToString("yyyy/MM/dd_HH:mm:ss");

                    // Create the dst folder
                    try
                    {
                        Directory.CreateDirectory(dst);
                    }
                    catch
                    {
                        // Return Error Code
                        model.errorMsg?.Invoke("cannotCreateDstFolder");
                    }
                    List<string> failedFiles = CopyFiles(_work, _files, _totalSize, dst);

                    // Calculate the time of the all process of copy
                    DateTime endTime = DateTime.Now;
                    TimeSpan workTime = endTime - startTime;
                    double transferTime = workTime.TotalMilliseconds;

                    // Update the current work status
                    _work.state = null;
                    this.model.SaveWorks();
                    //this.view.ConsoleUpdate(3); //TODO

                    foreach (string failedFile in failedFiles)
                    {
                        //this.view.DisplayFiledError(failedFile); //TODO
                    }
                    //this.view.DisplayBackupRecap(_work.name, transferTime); //TODO

                    if (failedFiles.Count != 0)
                    {
                        // Return Success Code
                        model.errorMsg?.Invoke("backupFinishedWithError");
                    }
                }
                else
                {
                    // Return Error Code
                    model.errorMsg?.Invoke("noSpaceDstFolder");
                }
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("diskError");
            }
        }

        private List<string> CopyFiles(Work _work, FileInfo[] _files, long _totalSize, string _dst)
        {
            // Files Size
            long leftSize = _totalSize;
            // Number of Files
            int totalFile = _files.Length;
            int totalFileSuccess = _files.Length;
            List<string> failedFiles = new List<string>();
            DateTime startTimeBackup = DateTime.Now;

            if (!(_work.isCrypted && this.model.settings.cryptoSoftPath.Length == 0))
            {
                // Copy every file
                for (int i = 0; i < _files.Length; i++)
                {
                    // Update the size remaining to copy (octet)
                    int pourcent = (i * 100 / totalFile);
                    long curSize = _files[i].Length;
                    leftSize -= curSize;

                    if (this.BackupFile(_work, _files[i], curSize, _dst, leftSize, totalFile, i, pourcent)) // ----------------------
                    {
                        //this.view.DisplayCurrentState(_work.name, (totalFile - i), leftSize, curSize, pourcent); // TODO
                    }
                    else
                    {
                        totalFileSuccess -= 1;
                        _totalSize -= _files[i].Length;
                        failedFiles.Add(_files[i].Name);
                    }
                }
            }
            else
            {
                // Return Error Code
                model.errorMsg?.Invoke("cryptoSoftPathNotFound");
            }

            double timeTaken = (DateTime.Now - startTimeBackup).TotalMilliseconds;
            currentBackupInfo?.Invoke(_work.name, totalFileSuccess, totalFile, (int)timeTaken);
            return failedFiles;
        }

        private bool BackupFile(Work _work, FileInfo _currentFile, long _curSize, string _dst, long _leftSize, int _totalFile, int fileIndex, int _pourcent)
        {
            // Time at when file copy start (use by SaveLog())
            DateTime startTimeFile = DateTime.Now;
            string curDirPath = _currentFile.DirectoryName;
            string dstDirectory = _dst;

            // If there is a directoy, we add the relative path from the directory dst
            if (Path.GetRelativePath(_work.src, curDirPath).Length > 1)
            {
                dstDirectory += Path.GetRelativePath(_work.src, curDirPath) + "\\";

                // If the directory dst doesn't exist, we create it
                if (!Directory.Exists(dstDirectory))
                {
                    Directory.CreateDirectory(dstDirectory);
                }
            }

            // Get the current dstFile
            string dstFile = dstDirectory + _currentFile.Name;

            try
            {
                // Update the current work status
                _work.state.UpdateState(_pourcent, (_totalFile - fileIndex), _leftSize, _currentFile.FullName, dstFile);
                this.model.SaveWorks();
                int elapsedTime = -1;
                int cryptedTime = 0;

                // Copy the current file
                if (!(this.model.settings.cryptoSoftPath.Length != 0 && this.model.settings.cryptoExtensions.Count != 0 && _currentFile.Name.Contains(".") && this.model.settings.cryptoExtensions.Contains(_currentFile.Name.Substring(_currentFile.Name.LastIndexOf(".")))))
                {
                    _currentFile.CopyTo(dstFile, true);
                    elapsedTime = (int)(DateTime.Now - startTimeFile).TotalMilliseconds;
                }
                // Crypt the current file
                else
                {
                    try
                    {
                        ProcessStartInfo process = new ProcessStartInfo(this.model.settings.cryptoSoftPath);
                        process.Arguments = "source " + _currentFile.FullName + " destination " + dstFile;
                        var proc = Process.Start(process);
                        proc.WaitForExit();
                        cryptedTime = proc.ExitCode;
                    }
                    catch
                    {
                        // Return Error Code
                        cryptedTime = -1;
                        model.errorMsg?.Invoke("cryptoSoftPathError");
                    }
                }

                // Save Log
                _work.SaveLog(startTimeFile, _currentFile.FullName, dstFile, _curSize, elapsedTime, cryptedTime);
                return true;
            }
            catch
            {
                _work.SaveLog(startTimeFile, _currentFile.FullName, dstFile, _curSize, -1, 0);
                return false;
            }
        }
    }
}
