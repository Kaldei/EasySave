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
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : Page
    {
        // ----- Attributes -----
        private MenuViewModel menuViewModel { get; set; }
        public MainWindow mainWindow { get; set; }


        // ----- Constructor -----
        public MenuView(MenuViewModel _menuViewModel, MainWindow _mainWindow)
        {
            // Initaialize Page content
            this.menuViewModel = _menuViewModel;
            this.mainWindow = _mainWindow;
            DataContext = this.menuViewModel;
            InitializeComponent();
        }


        // ----- Methods
        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            int nbrTotalWorks = _listWorks.SelectedItems.Count;
            int[] SelectedWorks = new int[nbrTotalWorks];

            // Get Works's Index from Selected Items
            for (int i = 0; i < nbrTotalWorks; i++)
            {
                SelectedWorks[i] = _listWorks.Items.IndexOf(_listWorks.SelectedItems[i]);
            }

            // Sort and Reverse Array to be sure to Remove the Right Work
            Array.Sort(SelectedWorks);
            Array.Reverse(SelectedWorks);

            // Remove Selected Works
            foreach (int index in SelectedWorks)
            {
                menuViewModel.RemoveWork(index);
            }
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            int Length = _listWorks.SelectedItems.Count;

            int[] SelectedWorks = new int[Length];

            for (int i = 0; i < Length; i++)
            {
                SelectedWorks[i] = _listWorks.Items.IndexOf(_listWorks.SelectedItems[i]);
            }
            Array.Sort(SelectedWorks);
            Array.Reverse(SelectedWorks);

            this.mainWindow.selectedWorksId = SelectedWorks;
            this.mainWindow.ChangePage("backup");
        }

        private void SelectAll_Clicked(object sender, RoutedEventArgs e)
        {
            // Select All or Unselect All If there are All Alerady Selected
            if (_listWorks.SelectedItems.Count != menuViewModel.model.works.Count)
            {
                _listWorks.SelectAll();
            }
            else
            {
                _listWorks.UnselectAll();
            }
        }

        private void Settings_Clicked(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangePage("settings");
        }

        private void addWorkButton_Click(object sender, RoutedEventArgs e)
        {
            this.mainWindow.ChangePage("addWork");
        }
    }
}
