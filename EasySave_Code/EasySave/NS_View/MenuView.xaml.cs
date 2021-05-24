using EasySave.NS_ViewModel;
using EasySave.Observable;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            int[] indexWorks = GetSelectedWorks();

            // Check if at least one Work is Selected
            if (indexWorks.Length > 0)
            {
                foreach (int indexWork in indexWorks)
                {
                    if (this.menuViewModel.model.works[indexWork].colorProgressBar == "White")
                    {
                        // Remove Work if it isn't Running
                        this.menuViewModel.RemoveWork(indexWork);
                    }
                    else
                    {
                        // Call Error Message if the Backup is Running
                        this.menuViewModel.model.errorMsg?.Invoke("cantDeleteWork");
                    }
                }
            }
            else
            {
                // Call Error Message if no Works Selected
                this.menuViewModel.model.errorMsg?.Invoke("noSelectedWork");
            }
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            int[] indexWorks = GetSelectedWorks();

            // Check if at least one Work is Selected
            if (indexWorks.Length > 0)
            {
                foreach (int indexWork in indexWorks)
                {
                    switch (this.menuViewModel.model.works[indexWork].colorProgressBar)
                    {
                        case "White":
                            this.menuViewModel.UpdateWorkColor(this.menuViewModel.model.works[indexWork], "Green");
                            this.menuViewModel.LaunchBackupWork(indexWork);
                            _listWorks.Items.Refresh();
                            break;

                        case "Orange":
                            this.menuViewModel.UpdateWorkColor(this.menuViewModel.model.works[indexWork], "Green");
                            break;

                        default:
                            break;
                    }
                }
                _listWorks.UnselectAll();
            }
            else
            {
                // Call Error Message if no Works Selected
                this.menuViewModel.model.errorMsg?.Invoke("noSelectedWork");
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

            // Sort and Reverse Array to be sure to Remove the Right Work
            Array.Sort(SelectedWorks);
            Array.Reverse(SelectedWorks);

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
            bool isBackupRunning = false;

            // Check if a Backup is Running
            foreach (var work in this.menuViewModel.model.works)
            {
                if(work.colorProgressBar != "White")
                {
                    isBackupRunning = true;
                    break;
                }
            } 

            // Prevent from Swhitching to Settings if a Backup is Running
            if (button.Tag.ToString() == "settings" && isBackupRunning)
            {
                // Call Error Message if a Works is Running
                this.menuViewModel.model.errorMsg?.Invoke("cantGoToSettings");
            }
            else
            {
                // Change Page
                this.mainWindow.ChangePage(button.Tag.ToString());
            }
        }

        private void CancelBackup_Clicked(object sender, RoutedEventArgs e)
        {
            int[] indexWorks = GetSelectedWorks();
            if (indexWorks.Length > 0)
            {
                foreach (int indexWork in indexWorks)
                {
                    // Change Work State to Cancel
                    if (this.menuViewModel.model.works[indexWork].colorProgressBar != "White")
                    {
                        this.menuViewModel.UpdateWorkColor(this.menuViewModel.model.works[indexWork], "White");
                    }
                    // Wait the reset of the work's state
                    Mouse.OverrideCursor = Cursors.Wait;
                    while (this.menuViewModel.model.works[indexWork].state.progress != 0) { }
                }
                // Refresh View
                _listWorks.UnselectAll();
                _listWorks.Items.Refresh();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            else
            {
                // Call Error Message if no Works Selected
                this.menuViewModel.model.errorMsg?.Invoke("noSelectedWork");
            }
        }

        private void PauseBackup_Clicked(object sender, RoutedEventArgs e)
        {
            int[] indexWorks = GetSelectedWorks();
            if (indexWorks.Length > 0)
            {
                foreach (int indexWork in GetSelectedWorks())
                {
                    // Check if backup is running
                    if (this.menuViewModel.model.works[indexWork].colorProgressBar != "Red" && this.menuViewModel.model.works[indexWork].colorProgressBar != "Orange" && this.menuViewModel.model.works[indexWork].colorProgressBar != "White")
                    {
                        this.menuViewModel.UpdateWorkColor(this.menuViewModel.model.works[indexWork], "Orange");
                    }
                }
                _listWorks.UnselectAll();
            }
            else
            {
                // Call Error Message if no Works Selected
                this.menuViewModel.model.errorMsg?.Invoke("noSelectedWork");
            }
        }

        private void InitSocket(object sender, RoutedEventArgs e)
        {
            if(this.menuViewModel.listener == null)
            {
                this.menuViewModel.SocketOn();
            } else
            {
                MessageBox.Show("Socket running");
            }

        }
    }
}
