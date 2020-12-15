using System;
using System.Collections.Generic;
using System.Text;

namespace PanelAdmin.model
{
    public class Settings
    {
        // --- Attributes ---
        public static Settings instance { get; set; }
        public string language { get; set; }


        // --- Constructors ---
        // Constructor used by LoadSettings
        private Settings() { }

        // Constructor used by GetInstance
        private Settings(string _language) 
        {
            this.language = _language;
        }


        // --- Methods ----
        // Singleton 
        public static Settings GetInstance()
        {
            if (instance == null)
            {
                instance = new Settings("en-US");
            }
            return instance;
        }
    }
}
