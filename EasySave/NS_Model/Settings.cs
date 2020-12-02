using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.NS_Model
{
    class Settings
    {
        // --- Attributes ---
        public static Settings instance { get; set; }
        public string cryptoSoftPath { get; set; }
        public List<String> cryptoExtensions { get; set; }
        public string businessSoftwarePath { get; set; }


        // --- Constructors ---
        // Constructor
        private Settings(){ }


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
        public void Update(string _cryptoSoftPath, List<String> _cryptoExtensions, string _businessSoftwarePath)
        {
            this.cryptoSoftPath = _cryptoSoftPath;
            this.cryptoExtensions = _cryptoExtensions;
            this.businessSoftwarePath = _businessSoftwarePath;
        }
    }
}
