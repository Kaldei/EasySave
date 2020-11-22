using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EasySave.NS_Model;
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
        public string Menu()
        {
            Console.WriteLine("\n----- WELCOME ON EASYSAVE -----" +
                "\n1 - Add a work" +
                "\n2 - Remove a work" +
                "\n3 - Make a backup" +
                "\n4 - Quit");

            string id = Console.ReadLine() ; 

            while (CheckInt(id) == false)
            {
                id = Console.ReadLine();
            }

            return id;
        }

        public string AddWorkName()
        {
            Console.WriteLine("\nEnter a name (1 to 20 characters)");

            string name = Console.ReadLine();

            while (CheckInputName(name) == false )
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

            while (CheckInputPath(source) == false)
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

            while (CheckInputPath(source) == false)
            {
                Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory direction. ");
                source = Console.ReadLine();
            }

            return source;
        }

        public bool CheckInputName(string _name)
        {
            int length = _name.Length;

            if (length > 1 && length < 20)
            {
                return true;
            }

            return false;
        }

        public bool CheckInputPath(string _source)
        { 
            if (Directory.Exists(_source))
            {
                return true;
            }
            return false;
        }

        public static bool CheckInt(string _input)
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

        public void DisplayWorks()
        {
            for (int i = 0; i < viewModel.model.test.Count; i++)
            {
                Console.WriteLine(i + 1 + " - " + "Name: " + viewModel.model.works[i].name
                    + "\tSource: " + viewModel.model.works[i].src
                    + "\tDestination: " + viewModel.model.works[i].dst
                    + "\tType: " + viewModel.model.works[i].backupType);
            }
        }

        public int MakeBackupChoice()
        {
            Console.WriteLine("Chose the work to save :"+
                "\n0 - all");

            DisplayWorks();

            string input;
            bool isValid = false;
            int idNumberBackup = 0  ;

            while (!isValid)
            {
                input = Console.ReadLine();
                
                //check if the input is a integer
                if (CheckInt(input))
                {
                    idNumberBackup = Int32.Parse(input);

                    //check if the input is a valid option menu
                    if (idNumberBackup >= 0 && idNumberBackup <= viewModel.model.works.Count)
                    {
                        isValid = true;
                    }
                }

                if (!isValid)
                {
                    Console.WriteLine("\nPlease enter a valid option.");
                }
            }
            return idNumberBackup; 
        }

        public int RemoveWorkChoice()
        {
            Console.WriteLine("Chose the work to remove :");
            DisplayWorks();

            string input;
            bool isValid = false;
            int idNumberBackup = 0;

            while (!isValid)
            {
                input = Console.ReadLine();

                //check if the input is a integer
                if (CheckInt(input))
                {
                    idNumberBackup = Int32.Parse(input);

                    //check if the input is a valid option menu
                    if (idNumberBackup >= 0 && idNumberBackup <= viewModel.model.works.Count)
                    {
                        isValid = true;
                    }
                }

                if (!isValid)
                {
                    Console.WriteLine("\nPlease enter a valid option.");
                }
            }
            return idNumberBackup;

        }

        public void MenuMsg()
        {
            Console.WriteLine("\nEnter a valid option");
        }

    }
}
