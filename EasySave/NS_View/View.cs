using System;
using System.IO;
using EasySave.NS_ViewModel;

namespace EasySave.NS_View
{
    class View
    {
        // --- Attributes ---
        private ViewModel viewModel;


        // --- Constructor ---
        public View(ViewModel _viewModel)
        {
            this.viewModel = _viewModel;
        }


        // --- Methods ---
        //Display menu
        public int Menu()
        {
            Console.Clear();
            Console.WriteLine(
                "Menu:" +
                "\n1 - Show all works" +
                "\n2 - Add a work" +
                "\n3 - Make a backup" +
                "\n4 - Remove a work" +
                "\n5 - Quit");

            string inputUser = Console.ReadLine();
            return CheckChoiceMenu(inputUser, 1, 5);
        }

        //Add work name
        public string AddWorkName()
        {
            Console.Clear();
            Console.WriteLine("Parameter to add a work:");
            ConsoleUpdate(2);

            Console.WriteLine("\nEnter a name (1 to 20 characters):");
            string name = Console.ReadLine();

            //Check if the name is valid
            while (CheckName(name) == false)
            {
                name = Console.ReadLine();
            }
            return name;
        }

        private string RetifyPath(string _path)
        {
            if (_path != "0")
            {
                _path += (_path.EndsWith("/") || _path.EndsWith("\\")) ? "" : "\\";
                _path = _path.Replace("/", "\\");
            }
            return _path;
        }

        //Add work source
        public string AddWorkSrc()
        {
            Console.WriteLine("\nEnter directory source. ");
            string source = RetifyPath(Console.ReadLine());

            //Check if the path is valid
            while (Directory.Exists(source) == false && source != "0")
            {
                Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory source. ");
                source = RetifyPath(Console.ReadLine());
            }
            return source;
        }
        
        //Add work destination
        public string AddWorkDst(string _source)
        {
            Console.WriteLine("\nEnter directory destination.");
            string destination = RetifyPath(Console.ReadLine());

            //Check if the path is valid
            while (CheckDstPath(_source, destination) == false && destination != "0")
            {
                destination = RetifyPath(Console.ReadLine());
            }
            return destination;
        }

        //Add work backup type
        public int AddWorkBackupType()
        {
            Console.WriteLine("\nChoose a type of Backup: \n1.Full \n2.Differential");
            string input = Console.ReadLine();
            int backupType = CheckChoiceMenu(input, 0, 2);
            return backupType;
        }

        //Check if the path exist and if it's different from the source
        private bool CheckDstPath(string _source, string _destination)
        {
            while (Directory.Exists(_destination) == true)
            {
                if (_source.ToUpper() != _destination.ToUpper())
                {
                    return true;
                }
                Console.WriteLine("\nChoose a different path from the source. ");
                return false;
            }
            Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory direction. ");
            return false;
        }

        //Check if the name is valid and doesn't already exist
        private bool CheckName(string _name)
        {
            int length = _name.Length;

            if (length >= 1 && length <= 20)
            {
                if (!viewModel.model.works.Exists(work => work.name == _name))
                {
                    return true;
                }
                Console.WriteLine("\nWorkName already taken. Please enter an other name.");
                return false;
            }
            Console.WriteLine("\nEnter a VALID name (1 to 20 characters):");
            return false;
        }
        
        //Check if the input is an integer
        private static bool CheckInt(string _input)
        {
            try
            {
                int nbr = int.Parse(_input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Display all works 
        private void LoadWorks(int _shift)
        {
            var works = viewModel.model.works;

            for (int i = 0; i < viewModel.model.works.Count; i++)
            {
                Console.WriteLine("\n" +(i+_shift) + " - " + "Name: " + works[i].name
                    + "\n    Source: " + works[i].src
                    + "\n    Destination: " + works[i].dst
                    + "\n    Type: " + works[i].backupType);
            }
        }

        public void DisplayWorks(int _shift)
        {
            Console.Clear();
            Console.WriteLine("Work list:");

            //Display all works 
            LoadWorks(1);
            ConsoleUpdate(1);
        }

        //Choose the work to save
        public int MakeBackupChoice()
        {
            Console.Clear();
            Console.WriteLine("Choose the work to save : \n\n1 - all");

            //Display all works 
            LoadWorks(2);
            ConsoleUpdate(2);

            //Check if the user's input is a valid integer
            int idNumberWork = CheckChoiceMenu(Console.ReadLine(), 0, viewModel.model.works.Count + 1);
            return idNumberWork;
        }

        //Choose the work to remove
        public int RemoveWorkChoice()
        {
            Console.Clear();
            Console.WriteLine("Choose the work to remove :");

            //Display all works 
            LoadWorks(1);
            ConsoleUpdate(2);

            //Check if the user's input is a valid integer
            int idNumberWork = CheckChoiceMenu(Console.ReadLine(), 0, viewModel.model.works.Count);
            return idNumberWork;
        }

        //Check if the input is a integer and in the good range
        private int CheckChoiceMenu(string _inputUser, int _minEntry, int _maxEntry)
        {
            while (!(CheckInt(_inputUser) && (Int32.Parse(_inputUser) >= _minEntry && Int32.Parse(_inputUser) <= _maxEntry)))
            {
                ConsoleUpdate(206);
                _inputUser = Console.ReadLine();
            }
            return Int32.Parse(_inputUser);
        }

        public void DisplayCurrentState(int _id)
        {
            var work = viewModel.model.works[_id];
            Console.Clear();
            Console.WriteLine("Current backup : " + work.name + "\n");
            Console.WriteLine("Number of files left : " + work.state.nbFileLeft);
            Console.WriteLine("Size of the files left : " + DiplaySize(work.state.leftSize) + "\n");
            DisplayProgressBar(work.state.progress);
        }

        public void DisplayBackupRecap(int _id, double _transferTime)
        {
            var work = viewModel.model.works[_id];
            Console.Clear();
            Console.WriteLine("Backup : " + work.name + " finished\n");
            Console.WriteLine("Time taken : " + _transferTime + " ms");
            DisplayProgressBar(100);
        }

        private void DisplayProgressBar(int _pourcent)
        {
            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Progress: [ " + _pourcent + "%]");
            Console.ResetColor();

            Console.Write(" [");
            for (int i = 0; i < 100; i += 5)
            {
                if (_pourcent > i)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.Write("]\n\n");
        }

        private string DiplaySize(long _octet)
        {
            if (_octet > 1000000000000)
            {
                return Math.Round((decimal)_octet / 1000000000000, 2) + " To";
            }
            else if (_octet > 1000000000)
            {
                return Math.Round((decimal)_octet / 1000000000, 2) + " Go";
            }
            else if (_octet > 1000000)
            {
                return Math.Round((decimal)_octet / 1000000, 2) + " Mo";
            }
            else if (_octet > 1000)
            {
                return Math.Round((decimal)_octet / 1000, 2) + " ko";
            }
            else
            {
                return _octet + " o";
            }
        }

        //Display message on the console
        public void ConsoleUpdate(int _id)
        {
            switch (_id)
            {
                //Information message
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPress any key to display menu . . .");
                    Console.ResetColor();
                    Console.ReadLine();
                    break;

                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n(Enter 0 to return to the menu)");
                    Console.ResetColor();
                    break;


                // Success message from 100 to 199
                case 100:
                    Console.WriteLine("\n----- WELCOME ON EASYSAVE -----");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPress any key to display menu . . .");
                    Console.ResetColor();
                    Console.ReadLine();
                    break;

                case 101:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nThe work was added with success!");
                    Console.ResetColor();
                    break;

                case 102:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nThe work was saved with success!");
                    Console.ResetColor();
                    break;

                case 103:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nThe work was removed with success!");
                    Console.ResetColor();
                    break;

                case 104:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nBackup success !");
                    Console.ResetColor();
                    break;
               

                // Error message from 200 to 299
                case 200:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nPlease restore your JSON backup file.");
                    Console.ResetColor();
                    break;

                case 201:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFailed to add work.");
                    Console.ResetColor();
                    break;

                case 202:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFailed to saved work.");
                    Console.ResetColor();
                    break;

                case 203:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFailed to removed work.");
                    Console.ResetColor();
                    break;

                case 204:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nWork List is empty.");
                    Console.ResetColor();
                    break;

                case 205:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nWork List is full.");
                    Console.ResetColor();
                    break;

                case 206:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nPlease enter a valid option");
                    Console.ResetColor();
                    break;

                case 207:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFailed to move a file, destination or source file do not exists.");
                    Console.ResetColor();
                    break;

                case 208:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nSelected backup type doesn't exists.");
                    Console.ResetColor();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nFailed : Error Unknow.");
                    Console.ResetColor();
                    break;
            }
        }
    }
}
