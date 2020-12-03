using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace EasySave.NS_Model
{
    class Work
    {
        // --- Attributes ---
        public string name { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public BackupType backupType { get; set; }
        public State state { get; set; }
        public string lastBackupDate { get; set; }

        // Prepare options to indent JSON Files
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };


        // --- Constructors ---
        // Constructor used by LoadWorks()
        public Work() { }

        // Constructor used by AddWork()
        public Work(string _name, string _src, string _dst, BackupType _backupType)
        {
            this.name = _name;
            this.src = _src;
            this.dst = _dst;
            this.backupType = _backupType;
            this.state = null;
        }


        // --- Methods ----
        // Save Log 
        public void SaveLog(DateTime _startDate, string _src, string _dst, long _size, bool isError)
        {
            // Prepare dates
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string startTime = _startDate.ToString("yyyy-MM-dd_HH-mm-ss");
            string elapsedTime = (DateTime.Now - _startDate).ToString();

            if (isError)
            {
                elapsedTime = "-1";
            }

            // Create File if it doesn't exists
            if (!Directory.Exists("./Logs"))
            {
                Directory.CreateDirectory("./Logs");
            }

            // Var that will contains Logs File Content
            var logs = new List<Log>();

            // Get Logs File Content if it Exists
            if (File.Exists($"./Logs/{today}.json"))
            {
                logs = JsonSerializer.Deserialize<List<Log>>(File.ReadAllText($"./Logs/{today}.json"));
            }

            // Add Current Backuped File Log
            logs.Add(new Log($"{this.name}", $"{_src}", $"{_dst}", $"{_size}", $"{startTime}", $"{elapsedTime}"));

            // Write Logs File
            File.WriteAllText($"./Logs/{today}.json", JsonSerializer.Serialize(logs, this.jsonOptions));

            /*
            // Log in .txt version
            File.AppendAllText($"./Logs/{today}.txt", $"{startTime}: {this.name}" +
                $"\nSource: {_src}" +
                $"\nDestination: {_dst}" +
                $"\nSize (Bytes): {_size}" +
                $"\nElapsed Time: {elapsedTime}" +
                "\n\r\n");
            */
        }
    }
}
