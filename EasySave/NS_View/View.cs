using System;
using System.IO;
using System.Linq;
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
        public int Menu()
        {
            Console.WriteLine("\n1 - Add a work" +
                "\n2 - Remove a work" +
                "\n3 - Make a backup" +
                "\n4 - Quit");

            string inputUser = Console.ReadLine() ;

            return CheckChoiceMenu(inputUser, 1, 4);
        }

        public string AddWorkName()
        {
            Console.WriteLine("\nEnter a name (1 to 20 characters)");

            string name = Console.ReadLine();

            while (CheckName(name) == false )
            {
                Console.WriteLine("\nEnter a VALID name (1 to 20 characters)");
                name = Console.ReadLine();
            }

            return name;
        }

        public string AddWorkSrc()
        {
            Console.WriteLine("\nEnter directory source. ");
            string source = Console.ReadLine() ;

            while (CheckPath(source) == false)
            {
                Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory source. ");
                source = Console.ReadLine();
            }

            return source ;
        }

        public string AddWorkDst()
        {
            Console.WriteLine("\nEnter directory destination.");
            string source = Console.ReadLine();

            while (CheckPath(source) == false)
            {
                Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory direction. ");
                source = Console.ReadLine();
            }

            return source;
        }

        private bool CheckName(string _name)
        {
            int length = _name.Length;

            if (length > 1 && length < 20)
            {
                return true;
            }

            return false;
        }

        private bool CheckPath(string _source)
        { 
            if (Directory.Exists(_source))
            {
                return true;
            }
            return false;
        }

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

        public void AddWorkMsg(int _id)
        {
            switch (_id)
            {
                case 0 :
                    Console.WriteLine("\nWork was added with success!");
                    break;
                case 1 :
                    Console.WriteLine("\nFailed to add work.");
                    break;
                default:
                    Console.WriteLine("\nFailed : error unknow.");
                    break;
            }
        }

        public void RemoveWorkMsg(int _id)
        {
            switch (_id)
            {
                case 0:
                    Console.WriteLine("\nWork was removed with success!");
                    break;
                case 1:
                    Console.WriteLine("\nFailed to remove work.");
                    break;
                default:
                    Console.WriteLine("\nFailed : error unknow.");
                    break;
            }
        }

        private void DisplayWorks()
        {
            for (int i = 0; i < viewModel.model.works.Count; i++)
            {
                Console.WriteLine(i + 1 + " - " + "Name: " + viewModel.model.works[i].name
                    + "\tSource: " + viewModel.model.works[i].src
                    + "\tDestination: " + viewModel.model.works[i].dst
                    + "\tType: " + viewModel.model.works[i].backupType);
            }
        }

        public int MakeBackupChoice()
        {
            Console.WriteLine("\nChoose the work to save : \n0 - all");

            //Display all works 
            DisplayWorks();

            //Check if the user's input is a valid integer
            int idNumberWork = CheckChoiceMenu(Console.ReadLine(), 0, viewModel.model.works.Count);

            return idNumberWork;
        }

        public int RemoveWorkChoice()
        {
            Console.WriteLine("\nChoose the work to remove :");

            //Display all works 
            DisplayWorks();

            //Check if the user's input is a valid integer
            int idNumberWork = CheckChoiceMenu(Console.ReadLine(), 1, viewModel.model.works.Count);

            return idNumberWork;

        }

        private int CheckChoiceMenu(string _inputUser, int _minEntry, int _maxEntry)
        {
            while (!(CheckInt(_inputUser) && (Int32.Parse(_inputUser) >= _minEntry && Int32.Parse(_inputUser) <= _maxEntry)))
            {
                Console.WriteLine("\nPlease enter a valid option.");
                _inputUser = Console.ReadLine();
            }
            return Int32.Parse(_inputUser);
        }

        public void MakeBackupMsg(int _id, string _name)
        {
            switch (_id)
            {
                case 0 :
                    Console.WriteLine("The work '" + _name + "' was saved with success! ");
                        break;
                case 2:
                    Console.WriteLine("Failed to saved work(s) ");
                    break;
                default:
                    Console.WriteLine("\nFailed : error unknow.");
                    break;
            }
        }


        public void MenuMsg()
        {
            Console.WriteLine("\nEnter a valid option");
        }

        public void InitMsg(int _id)
        {
            switch (_id)
            {
                case 0:
                    Console.WriteLine("\n----- WELCOME ON EASYSAVE -----");
                    break;
                case 1:
                    Console.WriteLine("\nPlease restore your JSON backup file.");
                    break;
                default:
                    Console.WriteLine("\nFailed : error unknow.");
                    break;
            }
        }

    }
}
