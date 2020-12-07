using EasySave.NS_ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasySave.NS_View
{
    /// <summary>
    /// Logique d'interaction pour BackupView.xaml
    /// </summary>
    public partial class BackupView : Page
    {
        // ----- Attributes -----
        private BackupViewModel backupViewModel { get; set; }
        private MainWindow mainWindow { get; set; }
        private List<string> backupInfos { get; set; }

        // ----- Constructor -----
        public BackupView(BackupViewModel _backupViewModel, MainWindow _mainWindow)
        {
            // Initaialize Page content
            this.backupViewModel = _backupViewModel;
            this.mainWindow = _mainWindow;

            this.backupInfos = new List<string>();

            backupViewModel.currentBackupInfo = UpdateBackupInfo;
            InitializeComponent();
        }

        public void UpdateBackupInfo(string _name, int _totalFileSuccess, int _totalFile, int _timeTaken)
        {
            string info = (backupInfos.Count + 1) + ". " + _name + Langs.Lang.progressBasResult + _totalFileSuccess + "/" + _totalFile + Langs.Lang.inMessage + _timeTaken + " ms";
            backupInfos.Add(info);
            BackuFinished.ItemsSource = null;
            BackuFinished.ItemsSource = backupInfos;

            if (backupInfos.Count == this.mainWindow.selectedWorksId.Length)
            {
                MessageBox.Show(Langs.Lang.progressBarDone);
                backupInfos = new List<string>();
                this.mainWindow.ChangePage("menu");
            }
        }

        public void RunSave()
        {
            this.backupViewModel.LaunchBackupWork(this.mainWindow.selectedWorksId);
        }
    }
}
