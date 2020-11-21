using System;
using System.Collections.Generic;
using System.Text;
using EasySave.NS_Model;
using EasySave.NS_View;

namespace EasySave.NS_ViewModel
{
    class ViewModel
    {
        // --- Attributes ---
        private Model model;
        private View view;


        // --- Constructor ---
        public ViewModel()
        {
            // Instantiate Model & View
            this.model = new Model();
            this.view = new View(this);
        }


        // --- Methods ---
        public void Run()
        {
            Console.WriteLine("EasySave is ready to be developed!");
        }

        public void AddWork()
        {
            if (model.Works.length < 5)
            {
                string addWorkName = view.AddWorkName();
                string addWorkSrc = view.AddWorkSrc();
                string addWorkDest = view.AddWorkDest();
                string addWorkBackupType = view.AddWorkBackupType();

                view.AddWorkMsg(model.AddWork(addWorkName, addWorkSrc, addWorkDest, addWorkBackupType));
            }
            else
            {
                view.AddWorkMsg( X ); //TODO Put correct msg number
            }
        }

        public void RemoveWork()
        {

        }

        public void MakeBackupWork()
        {

        }

        public void DoBackup(int _userChoice)
        {

        }
    }
}
