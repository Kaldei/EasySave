using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.NS_Model
{
    public class WorkToSend
    {
        public string name { get; set; }
        public int progress { get; set; }
        public string colorProgressBar { get; set; }

        public WorkToSend(string _name, int _progress, string _colorProgressBar)
        {
            this.name = _name;
            this.progress = _progress;
            this.colorProgressBar = _colorProgressBar;
        }
    }
}
