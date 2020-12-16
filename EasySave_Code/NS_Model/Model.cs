using EasySave.Observable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace EasySave.NS_Model
{
    public delegate void ErrorMsg(string _errorName);

    public class Model : ObservableObject
    {
        // --- Attributes ---
        // Prepare options to indent JSON Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        public Settings settings { get; set; }
        public ErrorMsg errorMsg { get; set; }

        public string stateFilePath { get; set; }
        public string settingsFilePath { get; set; }
        public WorkToSend workToSend { get; set; }
        public ReceiveObject receiveObject { get; set; }

        public List<Log> logs { get; set; }

        private ObservableCollection<Work> Works { get; set;}
        public ObservableCollection<Work> works {
            get {
                return Works; 
            }
            set
            {
                if(Works != value)
                {
                    Works = value;
                    OnPropertyChanged("works");
                }
            }
        }


        // --- Constructor ---
        public Model()
        {
            // Initialize Config Files Path
            stateFilePath = "./State.json";
            settingsFilePath = "./Settings.json";

            // Initialize Work List
            works = new ObservableCollection<Work>();

            // Initialize Settings
            this.settings = Settings.GetInstance();

            // Load Works at the beginning of the program (from ./State.json)
            LoadWorks();

            // Load Settings at the beginning of the program (from ./Settings.json)
            LoadSettings();

            logs = new List<Log>();
        }


        // --- Methods ---
        // Load Works and States (at the beginning of the program)
        public void LoadWorks()
        {
            // Check if backupWorkSave.json File exists
            if (File.Exists(stateFilePath))
            {
                try
                {
                    // Read Works from JSON File (from ./BackupWorkSave.json) (use Work() constructor)
                    this.works = JsonSerializer.Deserialize<ObservableCollection<Work>>(File.ReadAllText(this.stateFilePath));

                    // Reset Work States
                    foreach(Work work in this.works)
                    {
                        if (work.colorProgressBar != "White")
                        {
                            work.colorProgressBar = "White";
                        }

                        if (work.state.progress != 0)
                        {
                            work.state.InitState(0, 0, 0, "", "");
                        }
                    }
                    // Save Work File
                    SaveWorks();
                }
                catch
                {
                    // Return Error Code
                    errorMsg?.Invoke("loadWorksError");
                }
            }
            else
            {
                // Create Work File
                SaveWorks();
            }
        }

        // Save Works
        public void SaveWorks()
        {
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            File.WriteAllText(this.stateFilePath, JsonSerializer.Serialize(this.works, this.jsonOptions));
        }

        // Load Settings (at the beginning of the program)
        public void LoadSettings()
        {
            // Check if backupWorkSave.json File exists
            if (File.Exists(settingsFilePath))
            {
                try
                {
                    // Read Works from JSON File (from ./BackupWorkSave.json) (use Work() constructor)
                    this.settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(this.settingsFilePath));
                }
                catch
                {
                    // Return Error Code
                    errorMsg?.Invoke("loadSettingsError");
                }
            }
            else
            {
                // Create Settings File
                SaveSettings();
            }
        }

        // Save Settings
        public void SaveSettings()
        {
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            File.WriteAllText(this.settingsFilePath, JsonSerializer.Serialize(this.settings, this.jsonOptions));
        }


        // Load Logs (at the first backup)
        public void LoadLogs(string _today)
        {
            // Check if backupWorkSave.json File exists
            if (File.Exists(settingsFilePath))
            {
                if (!Directory.Exists("./Logs"))
                {
                    try
                    {
                        // Create Folder if it doesn't exists
                        Directory.CreateDirectory("./Logs");
                    }
                    catch
                    {
                        // Return Error Code
                        errorMsg?.Invoke("loadLogsError");
                    }
                }
            }

            // Get Logs File Content if it Exists
            if (File.Exists($"./Logs/{_today}.json"))
            {
                logs = JsonSerializer.Deserialize<List<Log>>(File.ReadAllText($"./Logs/{_today}.json"));
            }
        }

        // Save Log 
        public void SaveLog()
        {
            // Prepare times log
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            if (!Directory.Exists("./Logs"))
            {
                Directory.CreateDirectory("./Logs");
            }

            // Load if necessary the Logs 
            if (logs.Count == 0)
            {
                LoadLogs(today);
            }

            // Write Logs File
            File.WriteAllText($"./Logs/{today}.json", JsonSerializer.Serialize(logs, this.jsonOptions));
        }
    }
}
