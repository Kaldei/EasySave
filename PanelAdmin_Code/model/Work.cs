using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using PanelAdmin.model;
using PanelAdmin.Observable;

namespace PanelAdmin.model
{
    public class Work : ObservableObject
    {
        // --- Attributes ---
        public string name { get; set; }
        private int Progress { get; set; }
        public int progress {
            get { return Progress; }
            set {
                if (Progress != value)
                {
                    Progress = value;
                    OnPropertyChanged("progress");
                }
            }
        }
        public string ColorProgressBar { get; set; }
        public string colorProgressBar
        {
            get { return ColorProgressBar; }
            set
            {
                if (ColorProgressBar != value)
                {
                    ColorProgressBar = value;
                    OnPropertyChanged("colorProgressBar");
                }
            }
        }
    }
}
