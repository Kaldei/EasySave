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
        private void SrcOpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // Open Folder Browser
            this.folderBrowserDialog.ShowDialog();
            // Set Selected Path in Source TextBox
            _src.Text = this.folderBrowserDialog.SelectedPath;
        }

        private void DstOpenFolderButton_Click(object sender, RoutedEventArgs e)
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
            bool isValidWorkName = IsNameValid(_name.Text);
            bool isValidSource = Directory.Exists(src); // TODO - Add a custom function to handle this error
            if (isValidSource)
            {
                srcLabel.Foreground = Brushes.Black;
                srcLabel.Content = Langs.Lang.source;
            } 
            else
            {
                srcLabel.Foreground = Brushes.Red;
                srcLabel.Content = Langs.Lang.incorrectSource;
            }

            bool isValidDestination = CheckWorkDst(src, dst);

            if(isValidWorkName && isValidSource && isValidDestination)
            {
                // Add Work If All User Input are OK
                addWorkViewModel.AddWork(_name.Text, src, dst, (BackupType)_backupType.SelectedItem, (bool)_isCrypted.IsChecked);

                // Reset Fields
                _name.Text = "";
                _src.Text = "";
                _dst.Text = "";
                _backupType.SelectedIndex = 0;
                _isCrypted.IsChecked = false;

                // Return to Menu
                this.mainWindow.ChangePage("menu");
            }
        }

        private bool IsNameValid(string _name)
        {
            int length = _name.Length;

            if (length >= 1 && length <= 20)
            {
                foreach(Work work in this.addWorkViewModel.model.works)
                {
                    if (work.name == _name)
                    {
                        nameLabel.Foreground = Brushes.Red; // TODO - Possible to custom even more this error
                        nameLabel.Content = Langs.Lang.incorrectName;
                        return false;
                    }
                }
                nameLabel.Foreground = Brushes.Black;
                nameLabel.Content = Langs.Lang.name;
                return true;
            }
            nameLabel.Foreground = Brushes.Red;
            nameLabel.Content = Langs.Lang.incorrectName; // TODO - Possible to custom even more this error
            return false;
        }

        private string RectifyPath(string _path)
        {
            _path += (_path.EndsWith("/") || _path.EndsWith("\\")) ? "" : "\\";
            _path = _path.Replace("/", "\\");
            return _path.ToLower();
        }

        private bool CheckWorkDst(string _src, string _dst)
        {
            if (Directory.Exists(_dst)) // TODO - Better way to handle this
            {
                if (_src != _dst)
                {
                    if (_dst.Length > _src.Length)
                    {
                        if(_src != _dst.Substring(0, _src.Length))
                        {
                            dstLabel.Foreground = Brushes.Black;
                            dstLabel.Content = Langs.Lang.destination;
                            return true;
                        }
                        else
                        {
                            dstLabel.Foreground = Brushes.Red;
                            dstLabel.Content = Langs.Lang.errorDstInSrc;
                            return false;
                        }
                    }
                    dstLabel.Foreground = Brushes.Black;
                    dstLabel.Content = Langs.Lang.destination;
                    return true;
                }
                dstLabel.Foreground = Brushes.Red;
                dstLabel.Content = Langs.Lang.incorrectDestinationSource;
                return false;
            }
            dstLabel.Foreground = Brushes.Red;
            dstLabel.Content = Langs.Lang.incorrectDestinationExist;
            return false;
        }

        private void ReturnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangePage("menu");
        }
    }
}
