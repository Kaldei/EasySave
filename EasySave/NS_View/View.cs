using System;
using System.IO;
using EasySave.NS_ViewModel;
using System.Text.Json;

namespace EasySave.NS_View
{
    class View
    {
        public string language { get; set; }
        // --- Attributes ---
        private ViewModel viewModel;


        // --- Constructor ---
        public View(ViewModel _viewModel, string _language)
        {
            this.viewModel = _viewModel;
            this.language = _language;
        }


        // --- Methods ---
        //Display menu
        public int Menu()
        {
            Console.Clear();
            ConsoleUpdate(300);
            return CheckChoiceMenu(Console.ReadLine(), 1, 6);
        }

        //Add work name
        public string AddWorkName()
        {
            Console.Clear();
            ConsoleUpdate(301);
            ConsoleUpdate(2);

            ConsoleUpdate(302);
            string name = Console.ReadLine();

            //Check if the name is valid
            while (!CheckName(name))
            {
                name = Console.ReadLine();
            }
            return name;
        }

        private string RectifyPath(string _path)
        {
            if (_path != "0" && _path.Length >= 1)
            {
                _path += (_path.EndsWith("/") || _path.EndsWith("\\")) ? "" : "\\";
                _path = _path.Replace("/", "\\");
            }
            return _path.ToLower();
        }

        //Add work source
        public string AddWorkSrc()
        {
            ConsoleUpdate(303);
            string src = RectifyPath(Console.ReadLine());

            //Check if the path is valid
            while (!Directory.Exists(src) && src != "0")
            {
                ConsoleUpdate(211);
                src = RectifyPath(Console.ReadLine());
            }
            return src;
        }

        //Add work destination
        public string AddWorkDst(string _src)
        {
            ConsoleUpdate(304);
            string dst = RectifyPath(Console.ReadLine());

            //Check if the path is valid
            while (!CheckWorkDst(_src, dst))
            {
                dst = RectifyPath(Console.ReadLine());
            }
            return dst;
        }

        private bool CheckWorkDst(string _src, string _dst)
        {
            if (_dst == "0")
            {
                return true;

            }
            else if (Directory.Exists(_dst))
            {
                if (_src != _dst)
                {
                    if (_dst.Length > _src.Length)
                    {
                        if (_src != _dst.Substring(0, _src.Length))
                        {
                            return true;
                        }
                        else
                        {
                            ConsoleUpdate(217);
                            return false;
                        }
                    }
                    return true;
                }
                ConsoleUpdate(212);
                return false;
            }
            ConsoleUpdate(213);
            return false;
        }

        //Add work backup type
        public int AddWorkBackupType()
        {
            ConsoleUpdate(305);
            return CheckChoiceMenu(Console.ReadLine(), 0, 2);
        }

        //Check if the name is valid and doesn't already exist
        private bool CheckName(string _name)
        {
            int length = _name.Length;

            if (length >= 1 && length <= 20)
            {
                if (!this.viewModel.works.Exists(work => work.name == _name))
                {
                    return true;
                }
                ConsoleUpdate(214);
                return false;
            }
            ConsoleUpdate(215);
            return false;
        }

        //Check if the input is an integer
        private static bool CheckInt(string _input)
        {
            try
            {
                int.Parse(_input);
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
            var works = this.viewModel.works;

            for (int i = 0; i < works.Count; i++)
            {
                Console.WriteLine(
                    "\n" + (i + _shift) + " - " + "Name: " + works[i].name
                    + "\n    Source: " + works[i].src
                    + "\n    Destination: " + works[i].dst
                    + "\n    Type: " + works[i].backupType);
            }
        }

        public void DisplayWorks()
        {
            Console.Clear();
            ConsoleUpdate(307);

            //Display all works 
            LoadWorks(1);
            ConsoleUpdate(1);
        }

        //Choose the work to save
        public int LaunchBackupChoice()
        {
            Console.Clear();
            ConsoleUpdate(306);

            //Display all works 
            LoadWorks(2);
            ConsoleUpdate(2);

            //Check if the user's input is a valid integer
            return CheckChoiceMenu(Console.ReadLine(), 0, this.viewModel.works.Count + 1);
        }

        //Choose the work to remove
        public int RemoveWorkChoice()
        {
            Console.Clear();
            ConsoleUpdate(308);

            //Display all works 
            LoadWorks(1);
            ConsoleUpdate(2);

            //Check if the user's input is a valid integer
            return CheckChoiceMenu(Console.ReadLine(), 0, this.viewModel.works.Count);
        }

        //Choose the language to display
        public int LanguageChoice()
        {
            Console.Clear();
            ConsoleUpdate(309);
            ConsoleUpdate(2);
            Console.WriteLine("1. English");
            Console.WriteLine("2. Français");

            //Check if the user's input is a valid integer
            return CheckChoiceMenu(Console.ReadLine(), 0, 2);
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

        public void DisplayCurrentState(string _name, int _fileLeft, long _leftSize, long _curSize, int _pourcent)
        {
            Console.Clear();
            if(language == "FR")
            {
            Console.WriteLine(
                "Sauvegarde en cours : " + _name
                + "\nTaille du fichier en cours : " + DiplaySize(_curSize)
                + "\nNombre de fichiers restants : " + _fileLeft
                + "\nTaille restante : " + DiplaySize(_leftSize) + "\n");
            } 
            Console.WriteLine(
                "Current backup : " + _name
                + "\nSize of the current file : " + DiplaySize(_curSize)
                + "\nNumber of files left : " + _fileLeft
                + "\nSize of the files left : " + DiplaySize(_leftSize) + "\n");
            

            DisplayProgressBar(_pourcent);
        }

        public void DisplayBackupRecap(string _name, double _transferTime)
        {
            if(language == "FR")
            {
               Console.WriteLine("\n\n" +
               "Sauvegarde : " + _name + " finie\n"
               + "\nTemps : " + _transferTime + " ms\n");
            }
                Console.WriteLine("\n\n" +
                "Backup : " + _name + " finished\n"
                + "\nTime taken : " + _transferTime + " ms\n");
            DisplayProgressBar(100);
        }

        public void DisplayFiledError(string _name)
        {
            if(language == "FR")
            {
                Console.WriteLine("Nom du fichier " + _name + " échoué.");
            }
            Console.WriteLine("File named " + _name + " failed.");
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
            if (_id < 100 && language == "FR")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                switch (_id)
                {
                    //Information message
                    case 1:
                        Console.WriteLine("\nAppuyer sur Entrée pour afficher le menu . . .");
                        Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("\n(Appuyer sur 0 pour revenir au menu)");
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("\nInformation de la sauvegarde :");
                        break;

                    case 4:
                        Console.WriteLine("\nAppuyer sur Entrée pour en voir plus . . .");
                        Console.ReadLine();
                        break;
                }
            }
            else if (_id < 100)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                switch (_id)
                {
                    //Information message
                    case 1:
                        Console.WriteLine("\nPress Enter key to display menu . . .");
                        Console.ReadLine();
                        break;

                    case 2:
                        Console.WriteLine("\n(Enter 0 to return to the menu)");
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("\nBackup information :");
                        break;

                    case 4:
                        Console.WriteLine("\nPress Enter key to show more . . .");
                        Console.ReadLine();
                        break;
                }
            }
            else if (_id < 200 && language == "FR")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                switch (_id)
                {
                    // Success message from 100 to 199
                    case 100:
                        Console.WriteLine("\n----- BIENVENUE SUR EASYSAVE -----");
                        ConsoleUpdate(1);
                        break;

                    case 101:
                        Console.WriteLine("\nLe travail a été correctement ajouter !");
                        ConsoleUpdate(1);
                        break;

                    case 102:
                        Console.WriteLine("\nLe travail a été correctement sauvegarder !");
                        break;

                    case 103:
                        Console.WriteLine("\nLe travail a été correctement supprimer !");
                        ConsoleUpdate(1);
                        break;

                    case 104:
                        Console.WriteLine("\nSauvegarde réussite !");
                        break;

                    case 105:
                        Console.WriteLine("\nAucune modification n'a été faite depuis la dernière sauvegarde complète !\n");
                        break;
                }
            }
            else if (_id < 200)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                switch (_id)
                {
                    // Success message from 100 to 199
                    case 100:
                        Console.WriteLine("\n----- WELCOME ON EASYSAVE -----");
                        ConsoleUpdate(1);
                        break;

                    case 101:
                        Console.WriteLine("\nThe work was added with success!");
                        ConsoleUpdate(1);
                        break;

                    case 102:
                        Console.WriteLine("\nThe work was saved with success!");
                        break;

                    case 103:
                        Console.WriteLine("\nThe work was removed with success!");
                        ConsoleUpdate(1);
                        break;

                    case 104:
                        Console.WriteLine("\nBackup success!");
                        break;

                    case 105:
                        Console.WriteLine("\nNo modification since the last full backup!\n");
                        break;
                }
            }
            else if(_id < 300 && language == "FR")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (_id)
                {
                    // Error message from 200 to 299
                    case 200:
                        Console.WriteLine("\nS'il vous plaît, restaurer votre fichier JSON de sauvegarde.");
                        ConsoleUpdate(1);
                        break;

                    case 201:
                        Console.WriteLine("\nEchec de l'ajout d'un travail.");
                        ConsoleUpdate(1);
                        break;

                    case 202:
                        Console.WriteLine("\nEchec de la sauvegarde du travail.");
                        ConsoleUpdate(1);
                        break;

                    case 203:
                        Console.WriteLine("\nEchec de la suppression d'un travail.");
                        ConsoleUpdate(1);
                        break;

                    case 204:
                        Console.WriteLine("\nLa liste de travail est vide.");
                        ConsoleUpdate(1);
                        break;

                    case 205:
                        Console.WriteLine("\nLa liste de travail est pleine.");
                        ConsoleUpdate(1);
                        break;

                    case 206:
                        Console.WriteLine("\nS'il vous plaît, entrer une option valide :");
                        break;

                    case 207:
                        Console.WriteLine("\nEchec de transférer un fichier, le chemin de destination ou de source n'existe pas.");
                        break;

                    case 208:
                        Console.WriteLine("\nLe type de sauvegarde selection n'existe pas.");
                        break;

                    case 209:
                        Console.WriteLine("\nLa copie d'un fichier a échoué.");
                        ConsoleUpdate(1);
                        break;

                    case 210:
                        Console.WriteLine("\nEchec de la création d'un dossier de sauvegarde.");
                        ConsoleUpdate(1);
                        break;
                    case 211:
                        Console.WriteLine("\nLe dossier n'existe pas. S'il vous plaît, entrer un chemin source valide.");
                        break;

                    case 212:
                        Console.WriteLine("\nVeuillez choisir un chemin de destination différent de celui du source :");
                        break;

                    case 213:
                        Console.WriteLine("\nLe dossier n'existe pas. S'il vous plaît, entrer un chemin de destination valide.");
                        break;

                    case 214:
                        Console.WriteLine("\nLe nom du travail est déjà utiliser. Veuillez en choisir un autre :");
                        break;

                    case 215:
                        Console.WriteLine("\nEntrer un nom valide (1 à 20 caractères) :");
                        break;

                    case 216:
                        Console.WriteLine("\nLa sauvegarde est terminé avec des erreurs.");
                        break;

                    case 217:
                        Console.WriteLine("\nLe dossier de destination ne peut être à l'intérieur du dossier source.");
                        break;

                    default:
                        Console.WriteLine("\nEchec : Erreur inconnue.");
                        ConsoleUpdate(1);
                        break;
                }
            }
            else if (_id < 300)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (_id)
                {
                    // Error message from 200 to 299
                    case 200:
                        Console.WriteLine("\nPlease restore your JSON backup file.");
                        ConsoleUpdate(1);
                        break;

                    case 201:
                        Console.WriteLine("\nFailed to add work.");
                        ConsoleUpdate(1);
                        break;

                    case 202:
                        Console.WriteLine("\nFailed to saved work.");
                        ConsoleUpdate(1);
                        break;

                    case 203:
                        Console.WriteLine("\nFailed to removed work.");
                        ConsoleUpdate(1);
                        break;

                    case 204:
                        Console.WriteLine("\nWork List is empty.");
                        ConsoleUpdate(1);
                        break;

                    case 205:
                        Console.WriteLine("\nWork List is full.");
                        ConsoleUpdate(1);
                        break;

                    case 206:
                        Console.WriteLine("\nPlease enter a valid option");
                        break;

                    case 207:
                        Console.WriteLine("\nFailed to move a file, destination or source file do not exists.");
                        break;

                    case 208:
                        Console.WriteLine("\nSelected backup type doesn't exists.");
                        break;

                    case 209:
                        Console.WriteLine("\nFailed to copy file.");
                        ConsoleUpdate(1);
                        break;

                    case 210:
                        Console.WriteLine("\nFailed to create the backup folder.");
                        ConsoleUpdate(1);
                        break;
                    case 211:
                        Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory source. ");
                        break;

                    case 212:
                        Console.WriteLine("\nChoose a different path from the source. ");
                        break;

                    case 213:
                        Console.WriteLine("\nDirectory doesn't exist. Please enter a valid directory direction. ");
                        break;

                    case 214:
                        Console.WriteLine("\nWorkName already taken. Please enter an other name.");
                        break;

                    case 215:
                        Console.WriteLine("\nEnter a valid name (1 to 20 characters):");
                        break;

                    case 216:
                        Console.WriteLine("\nBackup finished with error.");
                        break;

                    case 217:
                        Console.WriteLine("\nDestination directory cannot be inside the source directory.");
                        break;

                    default:
                        Console.WriteLine("\nFailed : Error Unknown.");
                        ConsoleUpdate(1);
                        break;
                }
            }
            else if (language == "FR")
            {
                switch (_id)
                {
                    case 300:
                        Console.WriteLine(
                            "Menu:" +
                            "\n1 - Voir les travaux" +
                            "\n2 - Ajouter un travail" +
                            "\n3 - Lancer une sauvegarde" +
                            "\n4 - Supprimer un travail" +
                            "\n5 - Choisir une langue" +
                            "\n6 - Quitter");
                        break;

                    case 301:
                        Console.WriteLine("Paramètres pour ajouter un travail :");
                        break;

                    case 302:
                        Console.WriteLine("\nEntrer un nom (1 à 20 caractères) :");
                        break;

                    case 303:
                        Console.WriteLine("\nEntrer le chemin du dossier source :");
                        break;

                    case 304:
                        Console.WriteLine("\nEntrer le chemin du dossier cible :");
                        break;

                    case 305:
                        Console.WriteLine(
                            "\nChoisir un type de sauvegarde: " +
                            "\n1.Complète " +
                            "\n2.Différentiel");
                        break;

                    case 306:
                        Console.WriteLine(
                            "Choisir un travail à sauvegarder : " +
                            "\n\n1 - tous");
                        break;

                    case 307:
                        Console.WriteLine("Liste de travail :");
                        break;

                    case 308:
                        Console.WriteLine("Choisir un travail à supprimer :");
                        break;

                    case 309:
                        Console.WriteLine("Choisir une langue :");
                        break;
                }
            }
            else //if(language == "EN")
            {
                switch (_id)
                {
                    case 300:
                        Console.WriteLine(
                            "Menu:" +
                            "\n1 - Show all works" +
                            "\n2 - Add a work" +
                            "\n3 - Make a backup" +
                            "\n4 - Remove a work" +
                            "\n5 - Choose language" +
                            "\n6 - Quit");
                        break;

                    case 301:
                        Console.WriteLine("Parameter to add a work:");
                        break;

                    case 302:
                        Console.WriteLine("\nEnter a name (1 to 20 characters):");
                        break;

                    case 303:
                        Console.WriteLine("\nEnter directory source:");
                        break;

                    case 304:
                        Console.WriteLine("\nEnter directory destination:");
                        break;

                    case 305:
                        Console.WriteLine(
                            "\nChoose a type of Backup: " +
                            "\n1.Full " +
                            "\n2.Differential");
                        break;

                    case 306:
                        Console.WriteLine(
                            "Choose the work to save : " +
                            "\n\n1 - all");
                        break;

                    case 307:
                        Console.WriteLine("Work list :");
                        break;

                    case 308:
                        Console.WriteLine("Choose the work to remove :");
                        break;

                    case 309:
                        Console.WriteLine("Choose a language:");
                        break;
                }
            }
            Console.ResetColor();
        }
    }
}