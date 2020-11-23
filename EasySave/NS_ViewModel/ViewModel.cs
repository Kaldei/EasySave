using System;
using System.IO;
using System.Collections.Generic;
using EasySave.NS_Model;
using EasySave.NS_View;

namespace EasySave.NS_ViewModel
{
    class ViewModel
    {
        // --- Attributes ---
        public Model model;
        private View view;


        // --- Constructor ---
        public ViewModel()
        {
            // Instantiate Model & View
            this.model = new Model();
            this.view = new View(this);

            // Load Works at the beginning of the program (from ./BackupWorkSave.json)
            view.InitMsg(this.model.LoadWorks());
        }


        // --- Methods ---
        public void Run()
        {
            bool isRunning = true;
            int userChoice;

            while (isRunning)
            {
                userChoice = view.Menu();

                switch (userChoice)
                {
                    case 1:
                        view.DisplayWorks();
                        break;

                    case 2:
                        AddWork();
                        break;

                    case 3:
                        MakeBackupWork();
                        break;

                    case 4:
                        RemoveWork();
                        break;

                    case 5:
                        isRunning = false;
                        break;

                    default:
                        view.MenuMsg();
                        break;
                }
            }
        }

        private void AddWork()
        {
            if (model.works.Count < 5)
            {
                string addWorkName = view.AddWorkName();
                string addWorkSrc = view.AddWorkSrc();
                string addWorkDest = view.AddWorkDst();

                BackupType addWorkBackupType = BackupType.FULL;

                switch (view.AddWorkBackupType())
                {
                    case 1:
                        addWorkBackupType = BackupType.FULL;
                        break;

                    case 2:
                        addWorkBackupType = BackupType.DIFFRENTIAL;
                        break;
                }

                view.AddWorkMsg(model.AddWork(addWorkName, addWorkSrc, addWorkDest, addWorkBackupType), addWorkName);
            }
            else
            {
                view.AddWorkMsg(4, "");
            }
        }

        private void RemoveWork()
        {
            if (model.works.Count > 0)
            {
                int RemoveChoice = view.RemoveWorkChoice() - 1;
                string name = model.works[RemoveChoice].name;
                view.RemoveWorkMsg(model.RemoveWork(RemoveChoice), name);
            }
            else
            {
                view.RemoveWorkMsg(3, "");
            }
        }

        private void MakeBackupWork()
        {
            if (model.works.Count > 0)
            {
                int userChoice = view.MakeBackupChoice();

                if (userChoice == 0)
                {
                    // Run every work one by one
                    foreach (Work work in model.works)
                    {
                        view.MakeBackupMsg(model.LaunchBackupType(work), work.name);
                    }
                }
                else
                {
                    // Run one work from his ID in the list
                    int index = userChoice - 1;
                    view.MakeBackupMsg(model.LaunchBackupType(model.works[index]), model.works[index].name);
                }
            }
            else
            {
                view.MakeBackupMsg(3, "");
            }
        }
    }
}