using System;
using System.Collections.Generic;
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

            DataContext = new DisplayWorksView();

            /*            // Init Program
                        ViewModel viewModel = new ViewModel();
                        DataContext = new ShowWorksView();


                        // Run Program
                        viewModel.Run();*/
        }

        private void AddWorkButton(object sender, RoutedEventArgs e)
        {
            DataContext = new AddWorkView();
        }

        private void DisplayWorksButton(object sender, RoutedEventArgs e)
        {
            DataContext = new DisplayWorksView();
        }

    }
}
