using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace EasySave.NS_Model
{
    public class Settings : INotifyPropertyChanged
    {
        // --- Handler ---
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises OnPropertychangedEvent when property changes
        /// </summary>
        /// <param name="name">String representing the property name</param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // --- Attributes ---
        public static Settings instance { get; set; }
        public string cryptoSoftPath { get; set; }
        private ObservableCollection<String> CryptoExtensions { get; set; }
        public ObservableCollection<String> cryptoExtensions
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

        private ObservableCollection<String> BusinessSoftwares { get; set; }
        public ObservableCollection<String> businessSoftwares 
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
        public void Update(string _cryptoSoftPath, ObservableCollection<String> _cryptoExtensions, ObservableCollection<String> _businessSoftwares)
        {
            this.cryptoSoftPath = _cryptoSoftPath;
            this.cryptoExtensions = _cryptoExtensions;
            this.businessSoftwares = _businessSoftwares;
        }
    }
}
