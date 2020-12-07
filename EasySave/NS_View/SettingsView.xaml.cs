using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using EasySave.NS_Model;
using EasySave.NS_ViewModel;

namespace EasySave.NS_View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page
    {
        // ----- Attributes -----
        private SettingsViewModel settingsViewModel { get; set; }
        public MainWindow mainWindow { get; set; }


        // ----- Constructor -----
        public SettingsView(SettingsViewModel _settingsViewModel, MainWindow _mainWindow)
        {
            // Initaialize Page content
            this.settingsViewModel = _settingsViewModel;
            this.mainWindow = _mainWindow;
            DataContext = this.settingsViewModel;
            InitializeComponent();
            updateSelectedLanguage();
            _cryptoSoftPath.Text = this.settingsViewModel.model.settings.cryptoSoftPath;

        }


        // ----- Methods -----
        // Select Language in Settings View
        public void updateSelectedLanguage()
        {
            if (this.settingsViewModel.model.settings.language == "en-US")
            {
                enButton.BorderBrush = Brushes.DodgerBlue;
                frButton.BorderBrush = null;
            }
            else if (this.settingsViewModel.model.settings.language == "fr-FR")
            {
                enButton.BorderBrush = null;
                frButton.BorderBrush = Brushes.DodgerBlue;
            }
        }

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            // Change Language Setting
            var button = sender as Button;

            if (this.settingsViewModel.model.settings.language != button.Tag.ToString())
            {
                this.settingsViewModel.model.settings.language = button.Tag.ToString();
                this.settingsViewModel.model.SaveSettings();

                // Change Program Language

                Langs.Lang.Culture = new CultureInfo(this.settingsViewModel.model.settings.language);

                updateSelectedLanguage();

                mainWindow.RefreshLanguage();
            }

        }

        private void cryptoSoftathButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Crypto Soft Path given is Correct
            bool isValidCryptoSoftPath = File.Exists(_cryptoSoftPath.Text) && _cryptoSoftPath.Text.EndsWith(".exe");
            if (isValidCryptoSoftPath)
            {
                cryptoSoftPathLabel.Foreground = Brushes.Black;
                cryptoSoftPathLabel.Content = Langs.Lang.cryptosoftPath;
            }
            else
            {
                cryptoSoftPathLabel.Foreground = Brushes.Red;
                cryptoSoftPathLabel.Content = Langs.Lang.incorrectCryptosoftPath;

                if (this.settingsViewModel.model.settings.cryptoSoftPath != "")
                {
                    _cryptoSoftPath.Text = this.settingsViewModel.model.settings.cryptoSoftPath;
                }
                else
                {
                    _cryptoSoftPath.Text = "";
                }

                return;
            }

            // Update Crypto Soft Path
            this.settingsViewModel.model.settings.cryptoSoftPath = _cryptoSoftPath.Text;
            this.settingsViewModel.model.SaveSettings();

            _cryptoSoftPath.Text = this.settingsViewModel.model.settings.cryptoSoftPath;
        }

        private void addExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Extension given is Correct
            bool isValidExtention = checkExtension(_addExtension.Text);
            if (isValidExtention)
            {
                addExtensionLabel.Foreground = Brushes.Black;
                addExtensionLabel.Content = Langs.Lang.extensionFile;
            }
            else
            {
                addExtensionLabel.Foreground = Brushes.Red;
                addExtensionLabel.Content = Langs.Lang.incorrectExtension;
                return;
            }

            // Add Extension
            this.settingsViewModel.model.settings.cryptoExtensions.Add(_addExtension.Text);
            this.settingsViewModel.model.SaveSettings();

            // reset Field
            _addExtension.Text = "";
        }

        private bool checkExtension(string _addExtension)
        {
            if (_addExtension.StartsWith("."))
            {
                foreach (string extension in this.settingsViewModel.model.settings.cryptoExtensions)
                {
                    if (extension == _addExtension)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private void removeExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove Extension
            this.settingsViewModel.model.settings.cryptoExtensions.Remove((string)_removeExtension.SelectedItem);
            this.settingsViewModel.model.SaveSettings();

            // Reset Item
            _removeExtension.SelectedIndex = 0;
        }

        private void addBusinessSoftwareButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Business Software isn't alerady in list
            bool isValidExtention = checkBusinessSoftware(_addBusinessSoftware.Text);
            if (isValidExtention)
            {
                addBusinessSoftwareLabel.Foreground = Brushes.Black;
                addBusinessSoftwareLabel.Content = Langs.Lang.businessSoftware;
            }
            else
            {
                addBusinessSoftwareLabel.Foreground = Brushes.Red;
                addBusinessSoftwareLabel.Content = Langs.Lang.incorrectBusinessSoftware;
                return;
            }

            // Add Business Software
            this.settingsViewModel.model.settings.businessSoftwares.Add(_addBusinessSoftware.Text);
            this.settingsViewModel.model.SaveSettings();

            // reset Field
            _addBusinessSoftware.Text = "";
        }

        private bool checkBusinessSoftware(string _addBusinessSoftware)
        {
            foreach (string businessSoftware in this.settingsViewModel.model.settings.businessSoftwares)
            {
                if (businessSoftware == _addBusinessSoftware)
                {
                    return false;
                }
            }
            return true;
        }

        private void removeBusinessSoftwareButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove Business Sofware
            this.settingsViewModel.model.settings.businessSoftwares.Remove((string)_removeBusinessSoftware.SelectedItem);
            this.settingsViewModel.model.SaveSettings();

            // Reset Item
            _removeBusinessSoftware.SelectedIndex = 0;
        }

        private void returnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangePage("menu");
        }
    }
}
