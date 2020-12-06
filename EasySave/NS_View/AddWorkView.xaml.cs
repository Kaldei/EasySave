﻿using System;
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
using EasySave.NS_ViewModel;
using EasySave.NS_Model;
using System.IO;

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


        // ----- Constructor -----
        public AddWorkView(AddWorkViewModel _addWorkViewModel, MainWindow _mainWindow)
        {
            // Initaialize Page content
            this.addWorkViewModel = _addWorkViewModel;
            this.mainWindow = _mainWindow;
            DataContext = this.addWorkViewModel;
            InitializeComponent();
            
        }

        private void AddWork_Clicked(object sender, RoutedEventArgs e)
        {
            // Check User Input
            string src = RectifyPath(_src.Text);
            string dst = RectifyPath(_dst.Text);

            bool isValidWorkName = CheckName(_name.Text);
            if (isValidWorkName)
            {
                _nameLabel.Foreground = Brushes.Black;
                _nameLabel.Content = "Name:";
            } 
            else
            {
                _nameLabel.Foreground = Brushes.Red;
                _nameLabel.Content = "Name: Incorrect!";
                return;
            }

            bool isValidSource = Directory.Exists(src);
            if (isValidSource)
            {
                _srcLabel.Foreground = Brushes.Black;
                _srcLabel.Content = "Source:";
            } 
            else
            {
                _srcLabel.Foreground = Brushes.Red;
                _srcLabel.Content = "Source: Incorrect!";
                return;
            }

            bool isValidDestination = CheckWorkDst(src, dst);
            if (isValidDestination)
            {
                _dstLabel.Foreground = Brushes.Black;
                _dstLabel.Content = "Destination:";
            }
            else
            {
                _dstLabel.Foreground = Brushes.Red;
                _dstLabel.Content = "Destination: Incorrect!";
                return;
            }

            // Add Work If All User Input are OK
            addWorkViewModel.AddWork(_name.Text, src, dst, (BackupType)_backupType.SelectedItem, (bool)_isCrypted.IsChecked);

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
                return false;
            }
            return false;
        }

        private void returnMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangePage("menu");
        }
    }
}
