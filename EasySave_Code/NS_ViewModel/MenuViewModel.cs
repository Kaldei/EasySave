﻿using EasySave.NS_Model;

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
        public void RemoveWorks(int[] _worksToRemove)
        {
            foreach(int workId in _worksToRemove)
            {
                RemoveWork(workId);
            }
        }

        private void RemoveWork(int _index)
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