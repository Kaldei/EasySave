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
        private string stateFilePath = "./State.json";
        private string settingsFilePath = "./Settings.json";
        public List<Work> works { get; set; }
        public Settings settings { get; set; }

        // Prepare options to indent JSON Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };


        // --- Constructor ---
        public Model()
        {
            // Initalize Work List
            this.works = new List<Work>();
            this.settings = Settings.GetInstance();
        }


        // --- Methods ---
        // Add Work
        public int AddWork(string _name, string _src, string _dst, BackupType _backupType)
        {
            try
            {
                // Add Work in the program (at the end of the List)
                this.works.Add(new Work(_name, _src, _dst, _backupType));
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

        // Load Works and States (at the beginning of the program)
        public int LoadWorks()
        {
            // Check if State.json exists
            if (File.Exists(stateFilePath))
            {
                try
                {
                    // Read Works from State File 
                    this.works = JsonSerializer.Deserialize<List<Work>>(File.ReadAllText(this.stateFilePath));
                }
                catch
                {
                    // Return Error Code
                    return 200;
                }
            }
            else
            {
                // Create State File if it doesn't exists
                File.WriteAllText(this.stateFilePath, JsonSerializer.Serialize(this.works, this.jsonOptions));
            }
            // Return Success Code
            return 100;
        }

        // Load Settings (at the beginning of the program)
        public int LoadSettings()
        {
            // Check if Settings.json exists
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    // Read Settings from Settings File
                    this.settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(this.settingsFilePath));
                }
                catch
                {
                    // Return Error Code
                    return 200;
                }
            }
            else
            {
                // Create Settings File if it doesn't exists
                File.WriteAllText(this.settingsFilePath, JsonSerializer.Serialize(this.settings, this.jsonOptions));
            }
            // Return Success Code
            return 100;
        }

        // Save Works
        public void SaveWorks()
        {
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            File.WriteAllText(this.stateFilePath, JsonSerializer.Serialize(this.works, this.jsonOptions));
        }

        public bool CopyFile(Work _work, FileInfo _currentFile, long _curSize, string _dst, long _leftSize, int _totalFile, int fileIndex, int _pourcent)
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
                SaveWorks();

                // Copy the current file
                _currentFile.CopyTo(dstFile, true);

                // Save Log
                _work.SaveLog(startTimeFile, _currentFile.FullName, dstFile, _curSize, false);
                return true;
            }
            catch
            {
                _work.SaveLog(startTimeFile, _currentFile.FullName, dstFile, _curSize, true);
                return false;
            }
        }
    }
}
