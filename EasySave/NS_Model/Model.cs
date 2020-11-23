using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

namespace EasySave.NS_Model
{
    class Model
    {
        // --- Attributes ---
        private string backupWorkSavePath = "./BackupWorkSave.json";
        public List<Work> works { get; set; }
        // Prepare options to indent JSON Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };


        // --- Constructor ---
        public Model()
        {
            // Initalize Work List
            works = new List<Work>();

        }


        // --- Methods ---

        // Add Work
        public int AddWork(string _name, string _src, string _dst, BackupType _backupType)
        {
            try
            {
                // Add Work in the program (at the end of the List)
                Work work = new Work(_name, _src, _dst, _backupType);
                works.Add(work);

                // Save Works out of the program (at ./BackupWorkSave.json)
                SaveWorks();

                // Return Confiramation Code
                return 0;
            }
            catch
            {
                // Return Error Code
                return 1;
            }

        }

        // Remove Work
        public int RemoveWork(int _index)
        {
            try
            {
                // Remove Work from the program (at index)
                works.RemoveAt(_index);

                // Save Works out of the program (at ./BackupWorkSave.json)
                SaveWorks();

                // Return Confiramation Code
                return 0;
            }
            catch
            {
                // Return Error Code
                return 1;
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
                    works = JsonSerializer.Deserialize<List<Work>>(File.ReadAllText(backupWorkSavePath));
                }
                catch
                {
                    // Return Error Code
                    return 1;
                }
            }
            // Return Confirm Message
            return 0;
        }

        // Save Works
        private void SaveWorks()
        {
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            File.WriteAllText(backupWorkSavePath, JsonSerializer.Serialize(works, jsonOptions));

        }

        public int LaunchBackupType(Work _work)
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
                case BackupType.DIFFRENTIAL:
                    string fullBackupDir = getFullBackupDir(_work);

                    // If there is no first full backup, we create the first one (reference of the next diff backup)
                    if (fullBackupDir != null)
                    {
                        code = DifferentialBackupSetup(_work, dir, fullBackupDir);
                        break;
                    }
                    code = FullBackupSetup(_work, dir);
                    break;

                case BackupType.FULL:
                    code = FullBackupSetup(_work, dir);
                    break;

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

            // Launch the backup
            return DoBackup(_work, files, totalSize);
        }

        // Differential Backup
        private int DifferentialBackupSetup(Work _work, DirectoryInfo _dir, string _fullBackupDir)
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

        // Do Backup
        public int DoBackup(Work _work, FileInfo[] _files, long _totalSize)
        {
            // Create the state file
            DateTime startTime = DateTime.Now;
            string dst = _work.dst + _work.name + "_" + startTime.ToString("yyyy-MM-dd_HH-mm-ss") + "\\";

            // Uptade the current work status
            _work.state = new State(_files.Length, _totalSize, _work.src, dst);
            _work.lastBackupDate = startTime.ToString("yyyy/MM/dd_HH:mm:ss");

            // Create the dst folder
            Directory.CreateDirectory(dst);

            long leftSize = _totalSize;
            int totalFile = _files.Length;

            // Copy every file
            for (int i = 0; i < _files.Length; i++)
            {
                string dstDirectory = dst;
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

                // Uptade the current work status
                _work.state.UpdateState(pourcent, (totalFile - i), leftSize, _files[i].FullName, dstFile);
                SaveWorks();

                // Copy the current file
                _files[i].CopyTo(dstFile, true);

                // Update the size remaining to copy (octet)
                leftSize -= _files[i].Length;
            }
            // Calculate the time of the all process of copy
            DateTime endTime = DateTime.Now;
            TimeSpan workTime = endTime - startTime;
            double transferTime = workTime.TotalMilliseconds;

            // Uptade the current work status
            _work.state = null;
            SaveWorks();

            // Write the log
            //TODO
            return 0;
        }
    }
}
