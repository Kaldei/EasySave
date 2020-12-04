using System;
using System.Collections.Generic;

namespace EasySave.NS_Model
{
    class Settings
    {
        // --- Attributes ---
        public static Settings instance { get; set; }
        public Languages language { get; set; }


        // --- Constructors ---
        // Constructor used by LoadSettings
        private Settings() {}


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
        public void Update(Languages _language)
        {
            this.language = _language;
        }
    }
}
