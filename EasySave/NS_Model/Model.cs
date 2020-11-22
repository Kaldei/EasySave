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
        private string jsonPath = "./BackupWorkSave.json";
        public List<Work> works { get; set; }


        // --- Constructor ---
        public Model()
        {
            // Initalize Work List
            works = new List<Work>();

            // Load Works at the beginning of the program (from ./BackupWorkSave.json)
            LoadWorks();
        }


        // --- Methods ---

        // Add Work
        public int AddWork(string _name, string _src, string _dst, BackupType _backupType)
        {
            try
            {
                // Add Work in the program (at the end of the List)
                works.Add(new Work(_name, _src, _dst, _backupType));

                // Save Works out of the program (at ...)
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

        // Load Works
        private void LoadWorks()
        {
            // Check if JSON File exists
            if (File.Exists(jsonPath))
            {
                // Read Works from JSON File (from ./BackupWorkSave.json) (use Work() constructor)
                works = JsonSerializer.Deserialize<List<Work>>(File.ReadAllText(jsonPath));
            }

        }

        // Save Works
        private void SaveWorks()
        {
            // Prepare options to indent the JSON
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            // Write Work list into JSON file (at ./BackupWorkSave.json)
            File.WriteAllText(jsonPath, JsonSerializer.Serialize(works, jsonOptions));

        }
    }
}
