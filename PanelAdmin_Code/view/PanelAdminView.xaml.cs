using PanelAdmin.viewModel;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PanelAdmin.view
{
    /// <summary>
    /// Logique d'interaction pour PanelAdminView.xaml
    /// </summary>
    public partial class PanelAdminView : Window
    {
        private bool isFirstInstance;
        ViewModel vm = new ViewModel();
        public PanelAdminView()
        {
            isFirstInstance = false;
            new Mutex(true, "PanelAdmin", out isFirstInstance);
            if (isFirstInstance)
            {
                DataContext = vm;
                InitializeComponent();
            } else
            {
                MessageBox.Show("Admin Panel is already running !");
                App.Current.Shutdown();
            }
        }

        private void Connection_Click(object sender, RoutedEventArgs e)
        {
            string serverAddress = this.Address.Text;
            int serverPort = Int32.Parse(this.Port.Text);
            Task.Run(() =>
            {
                this.vm.Connection(serverAddress, serverPort);
            });
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            Make_Action("Green");
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Make_Action("Orange");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Make_Action("White");
        }
        private int[] GetSelectedWorks()
        {
            int nbrTotalWorks = _listWorks.SelectedItems.Count;
            int[] SelectedWorks = new int[nbrTotalWorks];

            // Get Works's Index from Selected Items
            for (int i = 0; i < nbrTotalWorks; i++)
            {
                SelectedWorks[i] = _listWorks.Items.IndexOf(_listWorks.SelectedItems[i]);
            }
            return SelectedWorks;
        }

        private void Make_Action(string action)
        {
            int[] SelectedWorks = GetSelectedWorks();

            if (SelectedWorks.Length > 0)
            {
                this.vm.SendAction(action, GetSelectedWorks());
                foreach (int id in GetSelectedWorks())
                {
                    this.vm.model.works[id].colorProgressBar = action;
                }
            }
        }
    }
}
