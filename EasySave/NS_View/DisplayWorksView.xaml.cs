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

        List<int> idWorkSelected = new List<int>();

        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {

            int Length = ListWorks.SelectedItems.Count;
            int[] SelectedWorks = new int[Length];

            for (int i = 0; i < Length; i++)
            {
                SelectedWorks[i] = ListWorks.Items.IndexOf(ListWorks.SelectedItems[i]);
            } 
            viewModel.testRemove(SelectedWorks);
           // ListWorks.SelectedItems;
        }
        private void Save_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void SelectAll_Clicked(object sender, RoutedEventArgs e)
        {

            if (ListWorks.SelectedItems.Count != viewModel.works.Count)
            {
                 ListWorks.SelectAll();            
            }
            else
            {
                 ListWorks.UnselectAll();
            }
        }

    }
}
