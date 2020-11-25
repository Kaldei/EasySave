using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using EasySave.NS_ViewModel;

namespace EasySave.NS_Model
{
    class Model
    {
        // --- Attributes ---
        private string backupWorkSavePath = "./BackupWorkSave.json";
        public List<Work> works { get; set; }
        public ViewModel viewModel { get; set; }

        // Prepare options to indent JSON Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };


        // --- Constructor ---
        public Model(ViewModel _viewModel)
        {
            // Initalize Work List
            this.works = new List<Work>();
            this.viewModel = _viewModel;
        }


        // --- Methods ---
        // Add Work
        public int AddWork(string _name, string _src, string _dst, BackupType _backupType)
        {
            try
            {
                // Add Work in the program (at the end of the List)
                this.works.Add(new Work(this.works.Count, _name, _src, _dst, _backupType));
                SaveWorks();

                // Return Success Code
                return 101;
            }
            catch
            {
                // Return Error Code
                return 201;
            }
        }

        // Remove Work
        public int RemoveWork(int _index)
        {
            try
            {
                // Remove Work from the program (at index)
                this.works.RemoveAt(_index);
                for(int i = 0; i < this.works.Count; i++)
                {
                    this.works[i].id = i;
                }
                SaveWorks();

                // Return Success Code
                return 103;
            }
            catch
            {
                // Return Error Code
                return 203;
            }
        }

        // Load Works and States at the beginning of the program
        public int LoadWorks()
        {
            // Check if backupWorkSave.json File exists
            if (File.Exists(backupWorkSavePath))
            {
                try
                {
                    // Read Works from JSON File (from ./BackupWorkSave.json) (use Work() constructor)
                    this.works = JsonSerializer.Deserialize<List<Work>>(File.ReadAllText(this.backupWorkSavePath));
                }
                catch
                {
                    // Return Error Code
                    return 200;
                }
            }
            // Return Success Code
            return 100;
        }

        // Save Works
        private void SaveWorks()
        {
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            File.WriteAllText(this.backupWorkSavePath, JsonSerializer.Serialize(this.works, this.jsonOptions));
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
                if (_work.name == directory.Name.Substring(0, directory.Name.IndexOf("_")))
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
                SaveWorks();
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

            // Files Size
            long leftSize = _totalSize;
            // Number of Files
            int totalFile = _files.Length;

            // Copy every file
            CopyFiles(_work, _files, _totalSize, dst, leftSize, totalFile);

            // Calculate the time of the all process of copy
            DateTime endTime = DateTime.Now;
            TimeSpan workTime = endTime - startTime;
            double transferTime = workTime.TotalMilliseconds;

            // Update the current work status
            _work.state = null;
            SaveWorks();

            // Write the log
            this.viewModel.view.DisplayBackupRecap(_work.id, transferTime);
            // Return Success Code
            return 104;
        }

        private void CopyFiles(Work _work, FileInfo[] _files, long _totalSize, string _dst, long _leftSize, int _totalFile)
        {
            for (int i = 0; i < _files.Length; i++)
            {
                // Time at when file copy start (use by SaveLog())
                DateTime startTimeFile = DateTime.Now;

                string dstDirectory = _dst;
                int pourcent = (i * 100 / _files.Length);

                // If there is a directoy, we add the relative path from the directory dst
                if (Path.GetRelativePath(_work.src, _files[i].DirectoryName).Length > 1)
                {
                    dstDirectory += Path.GetRelativePath(_work.src, _files[i].DirectoryName) + "\\"; 

                    // If the directory dst doesn't exist, we create it
                    if (!Directory.Exists(dstDirectory))
                    {
                        Directory.CreateDirectory(dstDirectory);
                    }
                }

                // Get the current dstFile
                string dstFile = dstDirectory + _files[i].Name;

                // Update the current work status
                _work.state.UpdateState(pourcent, (_totalFile - i), _leftSize, _files[i].FullName, dstFile);
                this.viewModel.view.DisplayCurrentState(_work.id);
                SaveWorks();

                // Copy the current file
                _files[i].CopyTo(dstFile, true);

                // Update the size remaining to copy (octet)
                _leftSize -= _files[i].Length;

                // Save Log
                SaveLog(startTimeFile, _work.name, _files[i].ToString(), dstFile, _files[i].Length);
            }
        }


        // Save Log 
        public void SaveLog(DateTime _startDate, string _name, string _src, string _dst, long _size)
        {
            // Prepare times log
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string startTime = _startDate.ToString("yyyy-MM-dd_HH-mm-ss");
            string elapsedTime = (DateTime.Now - _startDate).ToString();

            // Create File if it doesn't exists
            if (!Directory.Exists("./Logs"))
            {
                Directory.CreateDirectory("./Logs");
            }

            // Write log
            File.AppendAllText($"./Logs/{today}.txt", $"{startTime}: {_name}" +
                $"\nSource: {_src}" +
                $"\nDestination: {_dst}" +
                $"\nSize (Bytes): {_size}" +
                $"\nElapsed Time: {elapsedTime}" +
                "\n\r\n");
        }

    }
}
