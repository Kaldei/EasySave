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
            this.view.ConsoleUpdate(this.model.LoadWorks());
        }


        // --- Methods ---
        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                switch (this.view.Menu())
                {
                    case 1:
                        DisplayWorks();
                        break;

                    case 2:
                        AddWork();
                        break;

                    case 3:
                        LaunchBackupWork();
                        break;

                    case 4:
                        RemoveWork();
                        break;

                    case 5:
                        isRunning = false;
                        break;

                    default:
                        this.view.ConsoleUpdate(207);
                        break;
                }
            }
        }

        private void DisplayWorks()
        {
            if (this.model.works.Count > 0)
            {
                this.view.DisplayWorks();
            }
            else
            {
                this.view.ConsoleUpdate(204);
            }
        }

        private void AddWork()
        {
            if (this.model.works.Count < 5)
            {
                string addWorkName = view.AddWorkName();
                if (addWorkName == "0") return;

                string addWorkSrc = view.AddWorkSrc();
                if (addWorkSrc == "0") return;

                string addWorkDest = view.AddWorkDst(addWorkSrc);
                if (addWorkDest == "0") return;

                BackupType addWorkBackupType;
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

                    default:
                        addWorkBackupType = BackupType.FULL;
                        break;
                }
                this.view.ConsoleUpdate(model.AddWork(addWorkName, addWorkSrc, addWorkDest, addWorkBackupType));
            }
            else
            {
                this.view.ConsoleUpdate(205);
            }
        }

        private void LaunchBackupWork()
        {
            if (this.model.works.Count > 0)
            {
                int userChoice = view.MakeBackupChoice();

                switch (userChoice)
                {
                    // Return to the menu
                    case 0:
                        return;

                    // Run every work one by one
                    case 1:
                        foreach (Work work in this.model.works)
                        {
                            this.view.ConsoleUpdate(this.model.LaunchBackupType(work));
                        }
                        break;

                    // Run one work from his ID in the list
                    default:
                        int indexWork = userChoice - 2;
                        this.view.ConsoleUpdate(this.model.LaunchBackupType(this.model.works[indexWork]));
                        break;
                }
            }
            else
            {
                this.view.ConsoleUpdate(204);
            }
        }

        private void RemoveWork()
        {
            if (this.model.works.Count > 0)
            {
                int RemoveChoice = this.view.RemoveWorkChoice() - 1;
                if (RemoveChoice == -1) return;

                this.view.ConsoleUpdate(this.model.RemoveWork(RemoveChoice));
            }
            else
            {
                this.view.ConsoleUpdate(204);
            }
        }
    }
}