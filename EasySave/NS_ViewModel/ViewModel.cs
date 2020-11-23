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
        private View view;


        // --- Constructor ---
        public ViewModel()
        {
            // Instantiate Model & View
            this.model = new Model();
            this.view = new View(this);

            // Load Works at the beginning of the program (from ./BackupWorkSave.json)
            // view.InitMsg(this.model.LoadWorks());
        }


        // --- Methods ---
        public void Run()
        {
            bool isRunning = true;
            int userChoice;

            while (isRunning)
            {
                userChoice = view.Menu();

                switch (userChoice)
                {
                    case 1:
                        view.DisplayWorks();
                        break;

                    case 2:
                        AddWork();
                        break;

                    case 3:
                        MakeBackupWork();
                        break;

                    case 4:
                        RemoveWork();
                        break;

                    case 5:
                        isRunning = false;
                        break;

                    default:
                        //   view.MenuMsg();
                        break;
                }
            }
        }

        private void AddWork()
        {
            if (model.works.Count < 5)
            {
                string addWorkName = view.AddWorkName();
                string addWorkSrc = view.AddWorkSrc();
                string addWorkDest = view.AddWorkDst(addWorkSrc);

                BackupType addWorkBackupType = BackupType.FULL;

                switch (view.AddWorkBackupType())
                {
                    case 1:
                        addWorkBackupType = BackupType.FULL;
                        break;

                    case 2:
                        addWorkBackupType = BackupType.DIFFRENTIAL;
                        break;
                }

                model.AddWork(addWorkName, addWorkSrc, addWorkDest, addWorkBackupType);
            }
            else
            {
                //  view.AddWorkMsg(4, "");
            }
        }

        private void RemoveWork()
        {
            if (model.works.Count > 0)
            {
                int RemoveChoice = view.RemoveWorkChoice() - 1;
                string name = model.works[RemoveChoice].name;
                model.RemoveWork(RemoveChoice);
            }
            else
            {
                //view.RemoveWorkMsg(3, "");
            }
        }

        private void MakeBackupWork()
        {
            if (model.works.Count > 0)
            {
                int userChoice = view.MakeBackupChoice();

                if (userChoice == 0)
                {
                    // Run every work one by one
                    foreach (Work work in model.works)
                    {
                        LaunchBackupType(work);
                    }
                }
                else
                {
                    // Run one work from his ID in the list
                    int index = userChoice - 1;
                    LaunchBackupType(model.works[index]);
                }
            }
            else
            {
                // view.MakeBackupMsg(3, "");
            }
        }

        private int LaunchBackupType(Work _work)
        {
            int code;
            string dst = _work.dst;
            string src = _work.src;

            DirectoryInfo dir = new DirectoryInfo(src);

            // Check if the source & destionation folder exists
            if (!dir.Exists && !Directory.Exists(dst))
            {
                return 2;
            }

            // Run the correct backup (Full or Diff)
            switch (_work.backupType)
            {
                case BackupType.FULL:
                    code = FullBackupSetup(_work, dir);
                    break;

                case BackupType.DIFFRENTIAL:
                    string fullBackupDir = getFullBackupDir(_work);

                    // If there is no first full backup, we create the first one (reference of the next diff backup)
                    if (fullBackupDir != null)
                    {
                        code = DiffentialBackupSetup(_work, dir, fullBackupDir);
                        break;
                    }
                    goto case BackupType.FULL;

                default:
                    code = 1;
                    break;
            }
            return code;
        }

        // Get the directory of the first full backup of a differential backup
        private string getFullBackupDir(Work _work)
        {
            string name = _work.name;
            string dst = _work.dst;

            DirectoryInfo dir = new DirectoryInfo(dst);
            DirectoryInfo[] dirs = dir.GetDirectories();

            foreach (DirectoryInfo directory in dirs)
            {
                if (name == directory.Name.Substring(0, directory.Name.IndexOf("_")))
                {
                    return directory.FullName;
                }
            }
            return null;
        }

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

            // Launch the backup
            return DoBackup(_work, files, totalSize);
        }

        private int DiffentialBackupSetup(Work _work, DirectoryInfo _dir, string _fullBackupDir)
        {
            string fullBackPath = _fullBackupDir + "\\";
            long totalSize = 0;

            // Get evvery files of the source directory
            FileInfo[] srcFiles = _dir.GetFiles("*.*", SearchOption.AllDirectories);

            List<FileInfo> filesToCopy = new List<FileInfo>();

            // Check if there is a modification between the current file and the last full backup
            foreach (FileInfo file in srcFiles)
            {
                string currFullBackPath = fullBackPath + Path.GetRelativePath(_work.src, file.FullName);

                if (!File.Exists(currFullBackPath) || !IsSameFile(currFullBackPath, file.FullName))
                {
                    // Calcul the size of every files
                    totalSize += file.Length;

                    // Add the file to the list
                    filesToCopy.Add(file);
                }
            }

            // Launch the backup
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

        private int DoBackup(Work _work, FileInfo[] _files, long _totalSize)
        {
            string name = _work.name;
            string src = _work.src;
            string dst = _work.dst;

            // Create the state file
            DateTime startTime = DateTime.Now;
            State state = new State()
            {
                timestamp = Convert.ToString(startTime),
                name = name,
                currentWork = new CurrentWork
                {
                    totalFile = _files.Length,
                    totalSize = _totalSize,
                    progress = 0,
                    nbFileLeft = _files.Length,
                    leftSize = _totalSize,
                    currentPathSrc = src,
                    currentPathDest = dst

                }
            };

            // Create the dst folder
            dst += name + "_" + startTime.ToString("yyyy-MM-dd_HH-mm-ss") + "\\";
            Directory.CreateDirectory(dst);

            long leftSize = _totalSize;
            int totalFile = _files.Length;

            // Copy every file
            for (int i = 0; i < _files.Length; i++)
            {
                string dstDirectory = dst;
                int pourcent = (i * 100 / _files.Length);

                // If there is a directoy, we add the relative path from the directory dst
                if (Path.GetRelativePath(src, _files[i].DirectoryName).Length > 1)
                {
                    dstDirectory += Path.GetRelativePath(src, _files[i].DirectoryName) + "\\";

                    // If the directory dst doesn't exist, we create it
                    if (!Directory.Exists(dstDirectory))
                    {
                        Directory.CreateDirectory(dstDirectory);
                    }
                }

                // Get the current dstFile
                string dstFile = dstDirectory + _files[i].Name;

                // Uptade the state file
                state.UpdateState(state, pourcent, (totalFile - i), leftSize, _files[i].FullName, dstFile);

                // Copy the current file
                _files[i].CopyTo(dstFile, true);

                // We update the size remaining to copy (octet)
                leftSize -= _files[i].Length;
            }
            // Calculate the time of the all process of copy
            DateTime endTime = DateTime.Now;
            TimeSpan workTime = endTime - startTime;
            double transferTime = workTime.TotalMilliseconds;

            // Update for the last time the state file
            state.UpdateState(state, 100, 0, 0, "", "");

            // Write the log
            //TODO
            return 0;
        }
    }
}