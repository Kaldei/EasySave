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
using EasySave.NS_ViewModel;

namespace EasySave.NS_View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page
    {
        ViewModel viewModel = new ViewModel();

        public SettingsView()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ChangeLanguage(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Langs.Lang.Culture = new CultureInfo(button.Tag.ToString());
        }

        private void cryptoSoftathButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Crypto Soft Path given is Correct
            bool isValidCryptoSoftPath = File.Exists(_cryptoSoftPath.Text);
            if (isValidCryptoSoftPath)
            {
                cryptoSoftPathLabel.Foreground = Brushes.Black;
                cryptoSoftPathLabel.Content = "CryptoSoft Path:";
            }
            else
            {
                cryptoSoftPathLabel.Foreground = Brushes.Red;
                cryptoSoftPathLabel.Content = "CryptoSoft Path: Incorrect!";
                return;
            }

            // Update Crypto Soft Path
            this.viewModel.settings.cryptoSoftPath = _cryptoSoftPath.Text;
            this.viewModel.SaveSettings();
        }

        private void addExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Extension given is Correct
            bool isValidExtention = checkExtension(_addExtension.Text);
            if (isValidExtention)
            {
                addExtensionLabel.Foreground = Brushes.Black;
                addExtensionLabel.Content = "File Extension to Encrypt:";
            }
            else
            {
                addExtensionLabel.Foreground = Brushes.Red;
                addExtensionLabel.Content = "File Extension to Encrypt: Incorrect Extension Given!";
                return;
            }

            // Add Extension
            this.viewModel.settings.cryptoExtensions.Add(_addExtension.Text);
            this.viewModel.SaveSettings();
        }

        private bool checkExtension(string _addExtension)
        {
            if (_addExtension.StartsWith(".") && !this.viewModel.settings.cryptoExtensions.Exists(extention => extention == _addExtension))
            {
                return true;
            }
            return false;
        }

        private void removeExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove Extension
            this.viewModel.settings.cryptoExtensions.Remove((string)_removeExtension.SelectedItem);
            this.viewModel.SaveSettings();
        }

        private void addBusinessSoftwareButton_Click(object sender, RoutedEventArgs e)
        {
            // Check If Business Software isn't alerady in list
            bool isValidExtention = checkBusinessSoftware(_addBusinessSoftware.Text);
            if (isValidExtention)
            {
                addBusinessSoftwareLabel.Foreground = Brushes.Black;
                addBusinessSoftwareLabel.Content = "Business Software:";
            }
            else
            {
                addBusinessSoftwareLabel.Foreground = Brushes.Red;
                addBusinessSoftwareLabel.Content = "Business Software: Incorrect Business Software Given!";
                return;
            }

            // Add Business Software
            this.viewModel.settings.businessSoftwares.Add(_addBusinessSoftware.Text);
            this.viewModel.SaveSettings();
        }

        private bool checkBusinessSoftware(string _addBusinessSoftware)
        {
            if (!this.viewModel.settings.businessSoftwares.Exists(businessSoftware => businessSoftware == _addBusinessSoftware))
            {
                return true;
            }
            return false;
        }

        private void removeBusinessSoftwareButton_Click(object sender, RoutedEventArgs e)
        {
            // Remove Business Sofware
            this.viewModel.settings.businessSoftwares.Remove((string)_removeBusinessSoftware.SelectedItem);
            this.viewModel.SaveSettings();
        }
    }
}
