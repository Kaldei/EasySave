using PanelAdmin.viewModel;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PanelAdmin.view
{
    /// <summary>
    /// Logique d'interaction pour PanelAdminView.xaml
    /// </summary>
    public partial class PanelAdminView : Window
    {
        ViewModel vm = new ViewModel();

        public PanelAdminView()
        {
            if (Process.GetProcessesByName("PanelAdmin").Length == 1)
            { 
                Langs.Lang.Culture = new CultureInfo(this.vm.model.settings.language);
                DataContext = vm;
                InitializeComponent();
                UpdateSelectedLanguage();
            } else
            {
                MessageBox.Show(Langs.Lang.alwaysRunning);
                App.Current.Shutdown();
            }
        }

        private void Connection_Click(object sender, RoutedEventArgs e)
        {
            string serverAddress = this.Address.Text;
            string serverPort = this.Port.Text;
            if(serverAddress.Length > 1 && serverPort.Length > 1)
            {
                Task.Run(() =>
                {
                    this.vm.Connection(serverAddress, Int32.Parse(serverPort));
                });
            }
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            Make_Action("Green");
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Make_Action("Orange");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Make_Action("White");
        }

        private int[] GetSelectedWorks()
        {
            int nbrTotalWorks = _listWorks.SelectedItems.Count;
            int[] SelectedWorks = new int[nbrTotalWorks];

            // Get Works's Index from Selected Items
            for (int i = 0; i < nbrTotalWorks; i++)
            {
                SelectedWorks[i] = _listWorks.Items.IndexOf(_listWorks.SelectedItems[i]);
            }
            return SelectedWorks;
        }

        private void Make_Action(string action)
        {
            int[] SelectedWorks = GetSelectedWorks();

            if (SelectedWorks.Length > 0)
            {
                this.vm.SendAction(action, GetSelectedWorks());
                foreach (int id in GetSelectedWorks())
                {
                    this.vm.model.works[id].colorProgressBar = action;
                }
            }
        }

        // Click on flags
        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            // Check 
            var button = sender as Button;
            if (this.vm.model.settings.language != button.Tag.ToString())
            {
                // Change Language Setting
                this.vm.model.settings.language = button.Tag.ToString();
                this.vm.model.SaveSettings();

                // Change Program Language
                Langs.Lang.Culture = new CultureInfo(this.vm.model.settings.language);

                // Update Selected Language in View
                UpdateSelectedLanguage();

                MessageBox.Show(Langs.Lang.languageMessage);
            }
        }

        // Select Language in Settings View
        public void UpdateSelectedLanguage()
        {
            if (this.vm.model.settings.language == "en-US")
            {
                enButton.BorderBrush = Brushes.DodgerBlue;
                frButton.BorderBrush = null;
            }
            else if (this.vm.model.settings.language == "fr-FR")
            {
                enButton.BorderBrush = null;
                frButton.BorderBrush = Brushes.DodgerBlue;
            }
        }

    }
}
