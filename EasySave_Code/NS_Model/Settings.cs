﻿using EasySave.Observable;
using System.Collections.ObjectModel;

namespace EasySave.NS_Model
{
    public class Settings : ObservableObject
    {
        // --- Attributes ---
        public static Settings instance { get; set; }
        public string cryptoSoftPath { get; set; }
        private ObservableCollection<string> CryptoExtensions { get; set; }
        public ObservableCollection<string> cryptoExtensions
        {
            get
            {
                return CryptoExtensions;
            }
            set
            {
                if(CryptoExtensions != value)
                {
                    CryptoExtensions = value;
                    OnPropertyChanged("cryptoExtensions");
                }
            }
        }

        private ObservableCollection<string> BusinessSoftwares { get; set; }
        public ObservableCollection<string> businessSoftwares 
        {
            get
            {
                return BusinessSoftwares;
            }
            set
            {
                if (BusinessSoftwares != value)
                {
                    BusinessSoftwares = value;
                    OnPropertyChanged("businessSoftwares");
                }
            }
        }

        public string language { get; set; }


        // --- Constructors ---
        // Constructor used by LoadSettings
        private Settings() { }

        // Constructor used by GetInstance
        private Settings(string _cryptoSoftPath, ObservableCollection<string> _cryptoExtensions, ObservableCollection<string> _businessSoftwares, string _language)
        {
            this.cryptoSoftPath = _cryptoSoftPath;
            this.cryptoExtensions = _cryptoExtensions;
            this.businessSoftwares = _businessSoftwares;
            this.language = _language;
        }


        // --- Methods ----
        // Singleton 
        public static Settings GetInstance()
        {
            if (instance == null)
            {
                instance = new Settings("", new ObservableCollection<string>(), new ObservableCollection<string>(), "en-US");
            }
            return instance;
        }
    }
}
