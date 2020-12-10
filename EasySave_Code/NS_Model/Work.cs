using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace EasySave.NS_Model
{
    public class Work
    {
        // --- Attributes ---
        // Prepare options to indent JSON Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        public string name { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public BackupType backupType { get; set; }
        public bool isCrypted { get; set; }
        public State state { get; set; }
        public string lastBackupDate { get; set; }


        // --- Constructors ---
        // Constructor used by LoadWorks()
        public Work() { }

        // Constructor used by AddWork()
        public Work(string _name, string _src, string _dst, BackupType _backupType, bool _isCrypted)
        {
            this.name = _name;
            this.src = _src;
            this.dst = _dst;
            this.backupType = _backupType;
            this.isCrypted = _isCrypted;
            this.state = null;
        }


        // --- Methods ----
        // Save Log 
        public void SaveLog(Log _newLog)
        {
            // Prepare times log
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            // Create File if it doesn't exists
            if (!Directory.Exists("./Logs"))
            {
                Directory.CreateDirectory("./Logs");
            }

            // Var that will contains Logs File Content
            List<Log> logs = new List<Log>();

            // Get Logs File Content if it Exists
            if (File.Exists($"./Logs/{today}.json"))
            {
                logs = JsonSerializer.Deserialize<List<Log>>(File.ReadAllText($"./Logs/{today}.json"));
            }

            // Add Current Backuped File Log
            logs.Add(_newLog);

            // Write Logs File
            File.WriteAllText($"./Logs/{today}.json", JsonSerializer.Serialize(logs, this.jsonOptions));

        }
    }
}
