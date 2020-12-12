using EasySave.NS_ViewModel;
using EasySave.Observable;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            InitialiseBackup();
        }


        // ----- Methods
        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            int[] SelectedWorks = GetSelectedWorks();

            // Remove Selected Works
            menuViewModel.RemoveWorks(SelectedWorks);
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (int indexWork in GetSelectedWorks())
            {
                switch (this.menuViewModel.model.works[indexWork].workState)
                {
                    case NS_Model.WorkState.RUN:
                        break;

                    case NS_Model.WorkState.PAUSE:
                        this.menuViewModel.model.works[indexWork].workState = NS_Model.WorkState.RUN;
                        break;

                    default:
                        this.menuViewModel.LaunchBackupWork(GetSelectedWorks());
                        _listWorks.Items.Refresh();
                        break;
                }

            }
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

        private void ChangePage(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            this.mainWindow.ChangePage(button.Tag.ToString());
        }

        private void StopBackup_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (int indexWork in GetSelectedWorks())
            {
                this.menuViewModel.model.works[indexWork].workState = NS_Model.WorkState.CANCEL;
            }
            _listWorks.Items.Refresh();
        }

        private void PauseBackup_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (int indexWork in GetSelectedWorks())
            {
                // Check if backup is running
                if (this.menuViewModel.model.works[indexWork].workState == NS_Model.WorkState.RUN)
                {
                    this.menuViewModel.model.works[indexWork].workState = NS_Model.WorkState.PAUSE;
                    this.menuViewModel.model.works[indexWork].state.colorProgressBar = "Orange";
                }
            }
        }

        private void InitialiseBackup()
        {
            // Set backup state to FINISH if EasySave has been force exited
            for (int i = 0; i < _listWorks.Items.Count; i++)
            {
                this.menuViewModel.model.works[i].workState = NS_Model.WorkState.FINISH;
            }
            _listWorks.Items.Refresh();
        }

    }
}
