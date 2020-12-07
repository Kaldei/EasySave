using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using EasySave.NS_Model;

namespace EasySave.NS_ViewModel
{
    public class MenuViewModel
    {
        // ----- Attributes -----
        public Model model { get; set; }


        // ----- Constructor -----
        public MenuViewModel(Model _model)
        {
            this.model = _model;
        }


        // ----- Methods -----
        public void RemoveWork(int _index)
        {
            try
            {
                // Remove Work from the program (at index)
                this.model.works.RemoveAt(_index);
                this.model.SaveWorks();
            }
            catch
            {
                // Return Error Code
                model.errorMsg?.Invoke("errorRemoveWork");
            }
        }
    }
}
