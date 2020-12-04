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

        private void RemoveOne_Clicked(object sender, RoutedEventArgs e)
        {
            //ListWorks.Items.RemoveAt(ListWorks.SelectedIndex);

           // ListWorks.SelectedItems;
        }
        private void RemoveAll_Clicked(object sender, RoutedEventArgs e)
        {
        }
    }
}
