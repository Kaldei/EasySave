using System;
using System.Collections.Generic;

namespace EasySave.NS_Model
{
    class Settings
    {
        // --- Attributes ---
        public static Settings instance { get; set; }
        public string cryptoSoftPath { get; set; }
        public List<String> cryptoExtensions { get; set; }
        public List<String> businessSoftwares { get; set; }


        // --- Constructors ---
        // Constructor used by LoadSettings
        private Settings() { }


        // --- Methods ----
        // Singleton 
        public static Settings GetInstance()
        {
            if (instance == null)
            {
                instance = new Settings();
            }
            return instance;
        }

        // Update Settings
        public void Update(string _cryptoSoftPath, List<String> _cryptoExtensions, List<String> _businessSoftwares)
        {
            this.cryptoSoftPath = _cryptoSoftPath;
            this.cryptoExtensions = _cryptoExtensions;
            this.businessSoftwares = _businessSoftwares;
        }
    }
}
