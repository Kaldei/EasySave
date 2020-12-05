using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EasySave.NS_View;
using EasySave.NS_ViewModel;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Langs.Lang.Culture = new CultureInfo("en-US");
            DataContext = new DisplayWorksView();
        }

        private void AddWorkButton(object sender, RoutedEventArgs e)
        {
            DataContext = new AddWorkView();
        }

        private void DisplayWorksButton(object sender, RoutedEventArgs e)
        {
            DataContext = new DisplayWorksView();
        }

        private void SelectFrLanguage(object sender, RoutedEventArgs e)
        {
            Langs.Lang.Culture = new CultureInfo("fr-FR");

        }

        private void SelectEnLanguage(object sender, RoutedEventArgs e)
        {
            Langs.Lang.Culture = new CultureInfo("en-US");
        }

    }
}
