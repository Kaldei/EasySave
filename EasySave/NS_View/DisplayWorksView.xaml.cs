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
    /// Logique d'interaction pour DisplayWorksView.xaml
    /// </summary>
    public partial class DisplayWorksView : Page
    {
        ViewModel viewModel = new ViewModel();

        public DisplayWorksView()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            int nbrTotalWorks = _listWorks.SelectedItems.Count;
            int[] SelectedWorks = new int[nbrTotalWorks];

            // Get Works's Index from Selected Items
            for (int i = 0; i < nbrTotalWorks; i++)
            {
                SelectedWorks[i] = _listWorks.Items.IndexOf(_listWorks.SelectedItems[i]);
            }

            // Sort Array of Index to be sure to Remove the Right Work
            Array.Reverse(SelectedWorks);

            // Remove Selected Works
            foreach (int index in SelectedWorks)
            {
                viewModel.RemoveWork(index);
            }
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            int Length = ListWorks.SelectedItems.Count;
            if(Length > 0)
            {
                int[] SelectedWorks = new int[Length];

                for (int i = 0; i < Length; i++)
                {
                    SelectedWorks[i] = ListWorks.Items.IndexOf(ListWorks.SelectedItems[i]);
                }
                Array.Reverse(SelectedWorks);
                viewModel.LaunchBackupWork(SelectedWorks);
                

            } else
            {
                Console.WriteLine("Pas de works !");
            }
           

        }

        private void SelectAll_Clicked(object sender, RoutedEventArgs e)
        {
            // Select All or Unselect All If there are All Alerady Selected
            if (_listWorks.SelectedItems.Count != viewModel.works.Count)
            {
                _listWorks.SelectAll();            
            }
            else
            {
                _listWorks.UnselectAll();
            }
        }

    }
}
