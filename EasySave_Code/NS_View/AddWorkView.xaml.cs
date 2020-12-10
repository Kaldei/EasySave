using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EasySave.NS_ViewModel;
using EasySave.NS_Model;
using System.IO;
using System.Windows.Forms;

namespace EasySave.NS_View
{
    /// <summary>
    /// Logique d'interaction pour AddWorkView.xaml
    /// </summary>
    public partial class AddWorkView : Page
    {
        // ----- Attributes -----
        private AddWorkViewModel addWorkViewModel { get; set; }
        public MainWindow mainWindow { get; set; }

        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();


        // ----- Constructor -----
        public AddWorkView(AddWorkViewModel _addWorkViewModel, MainWindow _mainWindow)
        {
            // Initaialize Page content
            this.addWorkViewModel = _addWorkViewModel;
            this.mainWindow = _mainWindow;
            DataContext = this.addWorkViewModel;
            InitializeComponent();
            
        }


        // ----- Methods -----
        private void srcOpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Folder Browser
            this.folderBrowserDialog.ShowDialog();
            // Set Selected Path in Source TextBox
            _src.Text = this.folderBrowserDialog.SelectedPath;
        }

        private void dstOpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Folder Browser
            this.folderBrowserDialog.ShowDialog();
            // Set Selected Path in Destination TextBox
            _dst.Text = this.folderBrowserDialog.SelectedPath;
        }

        private void AddWork_Click(object sender, RoutedEventArgs e)
        {
            // Rectify Paths
            string src = RectifyPath(_src.Text);
            string dst = RectifyPath(_dst.Text);

            // Checks user inputs
            bool isValidWorkName = CheckName(_name.Text);
            if (isValidWorkName)
            {
                nameLabel.Foreground = Brushes.Black;
                nameLabel.Content = Langs.Lang.name;
            } 
            else
            {
                nameLabel.Foreground = Brushes.Red;
                nameLabel.Content = Langs.Lang.incorrectName;
                return;
            }

            bool isValidSource = Directory.Exists(src);
            if (isValidSource)
            {
                srcLabel.Foreground = Brushes.Black;
                srcLabel.Content = Langs.Lang.source;
            } 
            else
            {
                srcLabel.Foreground = Brushes.Red;
                srcLabel.Content = Langs.Lang.incorrectSource;
                return;
            }

            bool isValidDestination = CheckWorkDst(src, dst);
            if (isValidDestination)
            {
                dstLabel.Foreground = Brushes.Black;
                dstLabel.Content = Langs.Lang.destination;

            }
            else
            {
                dstLabel.Foreground = Brushes.Red;
                return;
            }

            // Add Work If All User Input are OK
            addWorkViewModel.AddWork(_name.Text, src, dst, (BackupType)_backupType.SelectedItem, (bool)_isCrypted.IsChecked);

            // Reset Fields
            _name.Text = "";
            _src.Text ="";
            _dst.Text = "";
            _backupType.SelectedIndex = 0;
            _isCrypted.IsChecked = false;

            // Return to Menu
            this.mainWindow.ChangePage("menu");
        }

       private bool CheckName(string _name)
        {
            int length = _name.Length;
            if (length >= 1 && length <= 20)
            {
                foreach(Work work in this.addWorkViewModel.model.works)
                {
                    if (work.name == _name)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private string RectifyPath(string _path)
        {
            if (_path.Length >= 1)
            {
                _path += (_path.EndsWith("/") || _path.EndsWith("\\")) ? "" : "\\";
                _path = _path.Replace("/", "\\");
            }
            return _path.ToLower();
        }

        private bool CheckWorkDst(string _src, string _dst)
        {
            if (Directory.Exists(_dst))
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
                            return false;
                        }
                    }               
                    return true;
                }
                dstLabel.Content = Langs.Lang.incorrectDestinationSource;
                return false;
            }
            dstLabel.Content = Langs.Lang.incorrectDestinationExist;
            return false;
        }

        private void returnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangePage("menu");
        }
    }
}
