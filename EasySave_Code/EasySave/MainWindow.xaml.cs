using System.Globalization;
using System.Windows;
using EasySave.NS_View;
using EasySave.NS_ViewModel;
using EasySave.NS_Model;
using System.Threading;
using System.Diagnostics;
using System;
using System.Windows.Media.Imaging;

namespace EasySave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ----- Attributes -----
        public int[] selectedWorksId { get; set; }
        public Model model { get; set; }

        private MenuView menuView { get; set; }
        private AddWorkView addWorkView { get; set; }
        private SettingsView settingsView { get; set; }
        private ErrorView errorView { get; set; }

        public MenuViewModel menuViewModel { get; set; }
        public AddWorkViewModel addWorkViewModel { get; set; }
        public SettingsViewModel settingsViewModel { get; set; }

        // ----- Constructor -----
        public MainWindow()
        {
            if (Process.GetProcessesByName("EasySave").Length == 1)
            {

                // Initialize Model
                this.model = new Model();

                // Initialize ViewModel
                this.errorView = new ErrorView(model);
                this.menuViewModel = new MenuViewModel(model);

                // Load Language
                Langs.Lang.Culture = new CultureInfo(model.settings.language);

                // Set Main Window Datacontent
                menuView = new MenuView(menuViewModel, this);
                DataContext = menuView;

                // Initialize Main Window
                InitializeComponent();
            }
            else
            {
                MessageBox.Show(Langs.Lang.alreadyRunning);
                App.Current.Shutdown();
            }
        }


        // ----- Methods -----
        // Change Main Window Content (Change Datacontent)
        public void ChangePage(string _route)
        {
            switch (_route)
            {
                case "menu":
                    DataContext = menuView;
                    return;

                case "addWork":
                    if (addWorkView == null)
                    {
                        this.addWorkViewModel = new AddWorkViewModel(model);
                        addWorkView = new AddWorkView(addWorkViewModel, this);
                    }
                    DataContext = addWorkView;
                    return;

                case "settings":
                    if (settingsView == null)
                    {
                        this.settingsViewModel = new SettingsViewModel(model);
                        settingsView = new SettingsView(settingsViewModel, this);
                    }
                    DataContext = settingsView;
                    return;
            }
        }
    }
}
