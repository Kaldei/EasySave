﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EasySave.NS_ViewModel;
using Microsoft.Win32;

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

        OpenFileDialog openFileDialog = new OpenFileDialog();


        // ----- Constructor -----
        public SettingsView(SettingsViewModel _settingsViewModel, MainWindow _mainWindow)
        {
            // Initaialize Page content
            this.settingsViewModel = _settingsViewModel;
            this.mainWindow = _mainWindow;
            DataContext = this.settingsViewModel;
            InitializeComponent();

            
            updateSelectedLanguage();
            openFileDialog.Filter = "Text files (*.exe)|*.exe|All files (*.*)|*.*";
            _cryptoSoftPath.Text = this.settingsViewModel.model.settings.cryptoSoftPath;
            _maxSimultaneousFilesSize.Text = this.settingsViewModel.model.settings.maxSimultaneousFilesSize.ToString();
        }


        // ----- Retrun to Menu -----
        private void returnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangePage("menu");
        }


        // ----- CryptoSoft Path -----
        private void cryptoSoftPathButton_Click(object sender, RoutedEventArgs e)
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
        }


        // ----- Extensions to Encrypt -----
        private void addExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Extension given is Correct
            bool isValidExtention = checkExtension(_addExtension.Text, this.settingsViewModel.model.settings.cryptoExtensions);
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

        private void removeExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove Extension
            this.settingsViewModel.model.settings.cryptoExtensions.Remove((string)_removeExtension.SelectedItem);
            this.settingsViewModel.model.SaveSettings();

            // Reset Item
            _removeExtension.SelectedIndex = 0;
        }


        // ----- Priority Extensions ------
        private void addPrioExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Extension given is Correct
            bool isValidExtention = checkExtension(_addPrioExtension.Text, this.settingsViewModel.model.settings.prioExtensions);
            if (isValidExtention)
            {
                addPrioExtensionLabel.Foreground = Brushes.Black;
                addPrioExtensionLabel.Content = Langs.Lang.prioExtensionLabel;
            }
            else
            {
                addPrioExtensionLabel.Foreground = Brushes.Red;
                addPrioExtensionLabel.Content = Langs.Lang.incorrectPrioExtensionLabel;
                return;
            }

            // Add Extension
            this.settingsViewModel.model.settings.prioExtensions.Add(_addPrioExtension.Text);
            this.settingsViewModel.model.SaveSettings();

            // reset Field
            _addPrioExtension.Text = "";
        }

        private void removePrioExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove Extension
            this.settingsViewModel.model.settings.prioExtensions.Remove((string)_removePrioExtension.SelectedItem);
            this.settingsViewModel.model.SaveSettings();

            // Reset Item
            _removePrioExtension.SelectedIndex = 0;
        }


        // ----- Business Software -----
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


        // ----- Max Simultaneous Files Size
        private void maxSimultaneousFilesSizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if input is a number and stricly posisitve
            int.TryParse(_maxSimultaneousFilesSize.Text, out int maxSimultaneousFilesSize);
            if (maxSimultaneousFilesSize >= 0)
            {
                maxSimultaneousFilesSizeLabel.Foreground = Brushes.Black;
                maxSimultaneousFilesSizeLabel.Content = Langs.Lang.maxSimultaneousFilesSize;
            }
            else
            {
                maxSimultaneousFilesSizeLabel.Foreground = Brushes.Red;
                maxSimultaneousFilesSizeLabel.Content = Langs.Lang.incorrectMaxSimultaneousFilesSize;

                if (this.settingsViewModel.model.settings.maxSimultaneousFilesSize > 0)
                {
                    _maxSimultaneousFilesSize.Text = this.settingsViewModel.model.settings.maxSimultaneousFilesSize.ToString();
                }
                else
                {
                    _maxSimultaneousFilesSize.Text = "";
                }
                return;
            }

            // Update Max Simultaneous Files Size
            this.settingsViewModel.model.settings.maxSimultaneousFilesSize = int.Parse(_maxSimultaneousFilesSize.Text);
            this.settingsViewModel.model.SaveSettings();
        }


        // ----- Language -----
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

        // Click on flags
        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            // Check 
            var button = sender as Button;
            if (this.settingsViewModel.model.settings.language != button.Tag.ToString())
            {
                // Change Language Setting
                this.settingsViewModel.model.settings.language = button.Tag.ToString();
                this.settingsViewModel.model.SaveSettings();

                // Change Program Language
                Langs.Lang.Culture = new CultureInfo(this.settingsViewModel.model.settings.language);

                // Update Selected Language in View
                updateSelectedLanguage();

                // Reload App
                mainWindow.RefreshLanguage();
            }

        }


        // ----- Shared Methods ------
        // Extension Checker
        private bool checkExtension(string _extension, ObservableCollection<string> extensionList)
        {
            if (_extension.StartsWith("."))
            {
                foreach (string extension in extensionList)
                {
                    if (extension == _extension)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        // File Browser
        private void cryptoSoftPathOpenFolderButtonButton_Click(object sender, RoutedEventArgs e)
        {
            // Open File Browser
            this.openFileDialog.ShowDialog();
            // Set Selected Path in CryptoSoft Path TextBox
            _cryptoSoftPath.Text = this.openFileDialog.FileName;

        }
    }
}
