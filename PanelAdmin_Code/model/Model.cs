
using PanelAdmin.Observable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace PanelAdmin.model
{ 

    public class Model : ObservableObject
    {
        // --- Attributes ---
        // Prepare options to indent JSON Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        public Settings settings { get; set; }
        private string settingsFilePath { get; set; }
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
            // Initialize Work List
            works = new ObservableCollection<Work>();

            // Initialize Settings
            settingsFilePath = "./Settings.json";
            this.settings = Settings.GetInstance();
            LoadSettings();
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
                    MessageBox.Show(Langs.Lang.loadSettingsError);
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
    }
}
