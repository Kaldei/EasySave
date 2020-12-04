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
using EasySave.NS_ViewModel;

namespace EasySave.NS_View
{
    /// <summary>
    /// Logique d'interaction pour AddWorkView.xaml
    /// </summary>
    public partial class AddWorkView : Page
    {
        ViewModel _main = new ViewModel();

        public AddWorkView()
        {
            InitializeComponent();
            DataContext = _main;
        }

        private void AddWork_Clicked(object sender, RoutedEventArgs e)
        {
            _main.TESTANTHO();
        }
    }
}
