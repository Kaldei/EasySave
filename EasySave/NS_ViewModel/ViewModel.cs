using System;
using System.IO;
using System.Collections.Generic;
using EasySave.NS_Model;
using EasySave.NS_View;

namespace EasySave.NS_ViewModel
{
    class ViewModel
    {
        // --- Attributes ---
        public Model model;
        public View view;


        // --- Constructor ---
        public ViewModel()
        {
            // Instantiate Model & View
            this.model = new Model();
            this.view = new View(this);

            // Load Works at the beginning of the program (from ./BackupWorkSave.json)
            this.view.ConsoleUpdate(this.model.LoadWorks());
        }


        // --- Methods ---
        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                switch (this.view.Menu())
                {
                    case 1:
                        DisplayWorks();
                        break;

                    case 2:
                        AddWork();
                        break;

                    case 3:
                        LaunchBackupWork();
                        break;

                    case 4:
                        RemoveWork();
                        break;

                    case 5:
                        isRunning = false;
                        break;

                    default:
                        this.view.ConsoleUpdate(207);
                        break;
                }
            }
        }

        private void DisplayWorks()
        {
            if (this.model.works.Count > 0)
            {
                this.view.DisplayWorks();
            }
            else
            {
                this.view.ConsoleUpdate(204);
            }
        }

        private void AddWork()
        {
            if (this.model.works.Count < 5)
            {
                string addWorkName = view.AddWorkName();
                if (addWorkName == "0") return;

                string addWorkSrc = view.AddWorkSrc();
                if (addWorkSrc == "0") return;

                string addWorkDest = view.AddWorkDst(addWorkSrc);
                if (addWorkDest == "0") return;

                BackupType addWorkBackupType;
                switch (view.AddWorkBackupType())
                {
                    case 0:
                        return;

                    case 1:
                        addWorkBackupType = BackupType.FULL;
                        break;

                    case 2:
                        addWorkBackupType = BackupType.DIFFRENTIAL;
                        break;

                    default:
                        addWorkBackupType = BackupType.FULL;
                        break;
                }
                this.view.ConsoleUpdate(model.AddWork(addWorkName, addWorkSrc, addWorkDest, addWorkBackupType));
            }
            else
            {
                this.view.ConsoleUpdate(205);
            }
        }

        private void LaunchBackupWork()
        {
            if (this.model.works.Count > 0)
            {
                int userChoice = view.MakeBackupChoice();

                switch (userChoice)
                {
                    // Return to the menu
                    case 0:
                        return;

                    // Run every work one by one
                    case 1:
                        foreach (Work work in this.model.works)
                        {
                            this.view.ConsoleUpdate(LaunchBackupType(work));
                        }
                        break;

                    // Run one work from his ID in the list
                    default:
                        int indexWork = userChoice - 2;
                        this.view.ConsoleUpdate(LaunchBackupType(this.model.works[indexWork]));
                        break;
                }
            }
            else
            {
                this.view.ConsoleUpdate(204);
            }
        }
        public int LaunchBackupType(Work _work)
        {
            DirectoryInfo dir = new DirectoryInfo(_work.src);

            // Check if the source & destionation folder exists
            if (!dir.Exists && !Directory.Exists(_work.dst))
            {
                // Return Error Code
                return 207;
            }

            // Run the correct backup (Full or Diff)
            switch (_work.backupType)
            {
                case BackupType.DIFFRENTIAL:
                    string fullBackupDir = GetFullBackupDir(_work);

                    // If there is no first full backup, we create the first one (reference of the next diff backup)
                    if (fullBackupDir != null)
                    {
                        return DifferentialBackupSetup(_work, dir, fullBackupDir);
                    }
                    return FullBackupSetup(_work, dir);

                case BackupType.FULL:
                    return FullBackupSetup(_work, dir);

                default:
                    // Return Error Code
                    return 208;
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
        private int FullBackupSetup(Work _work, DirectoryInfo _dir)
        {
            long totalSize = 0;

            // Get evvery files of the source directory
            FileInfo[] files = _dir.GetFiles("*.*", SearchOption.AllDirectories);

            // Calcul the size of every files
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }
            return DoBackup(_work, files, totalSize);
        }

        // Differential Backup
        private int DifferentialBackupSetup(Work _work, DirectoryInfo _dir, string _fullBackupDir)
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
                return 105;
            }
            return DoBackup(_work, filesToCopy.ToArray(), totalSize);
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
        public int DoBackup(Work _work, FileInfo[] _files, long _totalSize)
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
                return 210;
            }

            int result = CopyFiles(_work, _files, _totalSize, dst);

            // Calculate the time of the all process of copy
            DateTime endTime = DateTime.Now;
            TimeSpan workTime = endTime - startTime;
            double transferTime = workTime.TotalMilliseconds;

            // Update the current work status
            _work.state = null;
            this.model.SaveWorks();

            // Write the log
            this.view.DisplayBackupRecap(_work.id, transferTime);
            // Return Success Code
            return result;
        }

        private void RemoveWork()
        {
            if (this.model.works.Count > 0)
            {
                int RemoveChoice = this.view.RemoveWorkChoice() - 1;
                if (RemoveChoice == -1) return;

                this.view.ConsoleUpdate(this.model.RemoveWork(RemoveChoice));
            }
            else
            {
                this.view.ConsoleUpdate(204);
            }
        }

        private int CopyFiles(Work _work, FileInfo[] _files, long _totalSize, string _dst)
        {
            // Files Size
            long leftSize = _totalSize;
            // Number of Files
            int totalFile = _files.Length;
            bool hasError = false;

            // Copy every file
            for (int i = 0; i < _files.Length; i++)
            {
                // Update the size remaining to copy (octet)
                int pourcent = (i * 100 / totalFile);
                long curSize = _files[i].Length;
                leftSize -= curSize;

                if(this.model.CopyFile(_work, _files[i], curSize, _dst, leftSize, totalFile, i, pourcent))
                {
                    //this.view.DisplayCurrentState(_work.name, (totalFile - i), leftSize, curSize, pourcent);
                }
                else
                {
                    // Set error somewhere
                    this.view.ConsoleUpdate(209);
                    hasError = true;
                }
            }

            if(!hasError)
            {
                // Return Success Message
                return 104;
            } else
            {
                // Return Error Message
                return 216;
            }
        }
    }
}