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
        private Model model;
        private View view;


        // --- Constructor ---
        public ViewModel()
        {
            // Instantiate Model & View
            this.model = new Model();
            this.view = new View(this);
        }


        // --- Methods ---
        public void Run()
        {
            bool isRunning = true;
            int userChoice;

            while(isRunning)
            {
                userChoice = view.Menu();

                switch(userChoice)
                {
                    case 1:
                        AddWork();
                        break;

                    case 2:
                        MakeBackupWork();
                        break;

                    case 3:
                        RemoveWork();
                        break;

                    case 4:
                        isRunning = false;
                        break;

                    default:
                        view.MenuMsg();
                        break;
                }
            }
        }

        private void AddWork()
        {
            if(model.works.length < 5)
            {
                string addWorkName = view.AddWorkName();
                string addWorkSrc = view.AddWorkSrc();
                string addWorkDest = view.AddWorkDest();
                BackupType addWorkBackupType = view.AddWorkBackupType();

                view.AddWorkMsg(model.AddWork(addWorkName, addWorkSrc, addWorkDest, addWorkBackupType));
            }
            else
            {
                view.AddWorkMsg( X ); //TODO Put correct msg number
            }
        }

        private void RemoveWork()
        {
            if(model.works.length > 0)
            {
                view.RemoveWorkMsg(model.RemoveWork(view.RemoveWorkName()));
            }
            else
            {
                view.RemoveWorkMsg( X ); //TODO Put correct msg number
            }
        }

        private void MakeBackupWork()
        {
            if(model.works.length > 0)
            {
                int userChoice = view.MakeBackupChoice(model.works);

                if (userChoice == 0)
                {
                    foreach (Work work in model.works)
                    {
                        DoBackup(work, true);
                    }
                    // All Works
                }
                else
                {
                    DoBackup(model.works[userChoice - 1], false);
                    // One works
                }
            }
            else
            {
                view.MakeBackupMsg( X, name? ) //TODO Put correct msg number
            }
        }

        private int DoBackup(Work _work)
        {
            int code;

            switch(_work.BackupType)
            {
                case "?":
                    code = FullBackup(_work);
                    break;

                case "!":
                    code = DifferentialBackup(_work);
                    break;

                default:
                    code = 1;
                    break;
            }
            return code;
        }

        private int FullBackup(Work _work)
        {
            // Add Work Link (replace string)
            // User Input
            string src = "C:/Users/Clement/Desktop/test/";
            string dst = "C:/Users/Clement/Desktop/TestCopy/";

            // Open Directory
            DirectoryInfo dir = new DirectoryInfo(src);

            if (!dir.Exists && !Directory.Exists(dst))
            {
                return 0;
            }

            // Open Files
            FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

            // Calcul the size of every files
            long totalSize = 0;
            foreach (FileInfo file in files)
            {
                totalSize += file.Length;
            }

            // Check errors
            SaveFiles(files, dst, src, totalSize);
            return 1;
        }

        private int DifferentialBackup(Work _work, DirectoryInfo _fullBackupDir)
        {
            string name = "Name";
            string dst = "C:/Users/Clement/Desktop/TestCopy/";
            string src = "C:/Users/Clement/Desktop/test/";
            string fullBackPath = _fullBackupDir.FullName + "/";

            DirectoryInfo dir = new DirectoryInfo(src);

            if (!dir.Exists && !Directory.Exists(dst))
            {
                return 0;
            }

            // Open Files
            FileInfo[] srcFiles = dir.GetFiles("*.*", SearchOption.AllDirectories);
            List<FileInfo> filesToCopy = new List<FileInfo>();

            // Calcul the size of every files
            long totalSize = 0;
            foreach (FileInfo file in srcFiles)
            {
                string currFullBackPath = fullBackPath + Path.GetRelativePath(src, file.FullName);

                if (!File.Exists(currFullBackPath) || !IsSameFile(currFullBackPath, file.FullName))
                {
                    Console.WriteLine(file.Name);
                    totalSize += file.Length;
                    filesToCopy.Add(file);
                }
            }

            // Check errors
            SaveFiles(filesToCopy.ToArray(), dst, src, totalSize);
            return 1;
        }
        private void SaveFiles(FileInfo[] _files, string _dst, string _src, long _totalSize)
        {
            DateTime startTime = DateTime.Now;

            // Create the dst folder
            _dst += "Name" + "_" + startTime.ToString("yyyy-MM-dd_HH-mm-ss") + "/";
            Directory.CreateDirectory(_dst);

            long leftSize = _totalSize;
            for (int i = 0; i < _files.Length; i++)
            {
                string dstDirectory = _dst;
                int pourcent = (i * 100 / _files.Length);

                // If there is a directoy, we add the relative path from the directory dst
                if (Path.GetRelativePath(_src, _files[i].DirectoryName).Length > 1)
                {
                    dstDirectory += Path.GetRelativePath(_src, _files[i].DirectoryName) + "/";
                }

                // If the directory dst doesn't exist, we create it
                if (!Directory.Exists(dstDirectory))
                {
                    Directory.CreateDirectory(dstDirectory);
                }

                // Copy the current file
                string dstFile = dstDirectory + _files[i].Name;
                _files[i].CopyTo(dstFile, true);

                // We update the save file
                //saveState(_files[i], dstFile, pourcent, (_files.Length - i), leftSize);
                leftSize -= _files[i].Length;
            }
            DateTime endTime = DateTime.Now;
            TimeSpan workTime = endTime - startTime;
            double transferTime = workTime.TotalMilliseconds;

            //saveLog(startTime, "Test", _src, _dst, _totalSize, transferTime);
        }

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
    }
}
