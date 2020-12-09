using EasySave.NS_Model;

namespace EasySave.NS_ViewModel
{
    public class SettingsViewModel
    {
        // ----- Attributes -----
        public Model model { get; set; }


        // ----- Constructor -----
        public SettingsViewModel(Model _model)
        {
            this.model = _model;
        }

        
        // ----- Methods -----
        public void SaveSettings()
        {
            this.model.SaveSettings();
        }
    }
}
