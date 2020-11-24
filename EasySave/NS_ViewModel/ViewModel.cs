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
        public View view;


        // --- Constructor ---
        public ViewModel()
        {
            // Instantiate Model & View
            this.model = new Model(this);
            this.view = new View(this);

            // Load Works at the beginning of the program (from ./BackupWorkSave.json)
            view.ConsoleUpdate(this.model.LoadWorks());
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
                        DisplayWorks();
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
                        view.ConsoleUpdate(207);
                        break;
                }

            }
        }

        private void DisplayWorks()
        {
            if (model.works.Count > 0)
            {
                view.DisplayWorks(1);
            }
            else
            {
                view.ConsoleUpdate(204);
            }
        }

        private void AddWork()
        {
            if (model.works.Count < 5)
            {
                string addWorkName = view.AddWorkName();
                //Return menu
                if (addWorkName == "0")
                {
                    return;
                }

                string addWorkSrc = view.AddWorkSrc();
                //Return menu
                if (addWorkSrc == "0")
                {
                    return;
                }

                string addWorkDest = view.AddWorkDst(addWorkSrc);
                //Return menu
                if (addWorkDest == "0")
                {
                    return;
                }

                BackupType addWorkBackupType = BackupType.FULL;

                switch (view.AddWorkBackupType())
                {
                    case 0:
                        return;

                    case 1:
                        addWorkBackupType = BackupType.FULL;
                        break;

                    case 2:
                        addWorkBackupType = BackupType.DIFFRENTIAL;
                        break;
                }

                view.ConsoleUpdate(model.AddWork(addWorkName, addWorkSrc, addWorkDest, addWorkBackupType));
                view.ConsoleUpdate(1);
            }
            else
            {
                view.ConsoleUpdate(205);
            }
        }

        private void RemoveWork()
        {
            if (model.works.Count > 0)
            {
                int RemoveChoice = view.RemoveWorkChoice() - 1;
                //Return menu
                if(RemoveChoice == -1)
                {
                    return;
                }
                string name = model.works[RemoveChoice].name;
                view.ConsoleUpdate(model.RemoveWork(RemoveChoice));
                view.ConsoleUpdate(1);
            }
            else
            {
                view.ConsoleUpdate(204);
            }
        }

        private void MakeBackupWork()
        {
            if (model.works.Count > 0)
            {
                int userChoice = view.MakeBackupChoice();

                //Return menu
                if (userChoice == 0)
                {
                    return;
                }

                if (userChoice == 1)
                {
                    // Run every work one by one
                    foreach (Work work in model.works)
                    {
                        view.ConsoleUpdate(model.LaunchBackupType(work));
                    }                        
                    view.ConsoleUpdate(1);

                }
                else
                {
                    // Run one work from his ID in the list
                    int index = userChoice - 2;
                    view.ConsoleUpdate(model.LaunchBackupType(model.works[index]));
                    view.ConsoleUpdate(1);
                }
            }
            else
            {
                view.ConsoleUpdate(204);
            }
        }
    }
}