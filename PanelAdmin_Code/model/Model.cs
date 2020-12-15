
using PanelAdmin.Observable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

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
        }
    }
}
