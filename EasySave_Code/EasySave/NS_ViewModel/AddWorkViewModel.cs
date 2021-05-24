using EasySave.NS_Model;

namespace EasySave.NS_ViewModel
{
    public class AddWorkViewModel
    {
        // ----- Attributes -----
        public Model model { get; set; }


        // ----- Constructor -----
        public AddWorkViewModel(Model _model)
        {
            this.model = _model;
        }

        public void AddWork(string _name, string _src, string _dst, BackupType _backupType, bool _isCrypted)
        {
            try
            {
                // Add Work in the program (at the end of the List)
                this.model.works.Add(new Work(_name, _src, _dst, _backupType, _isCrypted));
                this.model.SaveWorks();
            }
            catch
            {
                // Return Error Code
                model.errorMsg?.Invoke("errorAddWork");
            }
        }
    }
}
