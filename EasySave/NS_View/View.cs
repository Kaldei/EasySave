using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            Console.WriteLine("----- WELCOME ON EASYSAVE -----\n" +
                "1 - Add a work\n" +
                "2 - Remove a work\n" +
                "3 - Make a backup\n" +
                "4 - Quit\n");

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
            Console.WriteLine("\nEnter directory source ");
            string source = Console.ReadLine() ;

            while (CheckInputPath(source) == false)
            {
                Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory source ");
                source = Console.ReadLine();
            }

            return source ;
        }

        public string AddWorkDst()
        {
            Console.WriteLine("\nEnter directory destination ");
            string source = Console.ReadLine();

            while (CheckInputPath(source) == false)
            {
                Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory direction ");
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

        public static bool CheckInt(string input)
        {
            try
            {
                int nbr = int.Parse(input);
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
                    Console.WriteLine("Work was added with success!");
                    break;
                case 1 :
                    Console.WriteLine("Failed to add work");
                    break;
                default:
                    Console.WriteLine("Failed : error unknow ");
                    break;
            }
        }

        public void MenuMsg()
        {
            Console.WriteLine("\nEnter a valid option");
        }

    }
}
