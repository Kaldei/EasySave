using EasySave.Observable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
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

        public ErrorMsg errorMsg;

        public string stateFilePath { get; set; }
        public string settingsFilePath { get; set; }

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
        public Settings settings { get; set; }


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
            this.settings.Update("", new ObservableCollection<string>(), new ObservableCollection<string>(), "en-US");

            // Load Works at the beginning of the program (from ./State.json)
            LoadWorks();

            // Load Settings at the beginning of the program (from ./Settings.json)
            LoadSettings(); // ---- TODO : Handle Error Message in View ---- //
        }


        // --- Methods ---
        // Load Works and States (at the beginning of the program)
        public int LoadWorks()
        {
            // Check if backupWorkSave.json File exists
            if (File.Exists(stateFilePath))
            {
                try
                {
                    // Read Works from JSON File (from ./BackupWorkSave.json) (use Work() constructor)
                    this.works = JsonSerializer.Deserialize<ObservableCollection<Work>>(File.ReadAllText(this.stateFilePath));
                }
                catch
                {
                    // Return Error Code
                    return 200;
                }
            }
            else
            {
                // Create Settings File
                SaveWorks();
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

        // Load Settings (at the beginning of the program)
        public int LoadSettings()
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
                    return 200;
                }
            }
            else
            {
                // Create Settings File
                SaveSettings();
            }
            // Return Success Code
            return 100;
        }

        // Save Settings
        public void SaveSettings()
        {
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            File.WriteAllText(this.settingsFilePath, JsonSerializer.Serialize(this.settings, this.jsonOptions));
        }
    }
}
