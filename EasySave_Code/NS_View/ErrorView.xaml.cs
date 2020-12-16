using System.Windows;
using System.Windows.Controls;
using EasySave.NS_Model;
using EasySave.Langs;

namespace EasySave.NS_View
{
    /// <summary>
    /// Logique d'interaction pour ErrorView.xaml
    /// </summary>
    public partial class ErrorView : Page
    {

        public ErrorView(Model _model)
        {
            _model.errorMsg = DisplayErrorMsg;
        }

        public void DisplayErrorMsg(string _errorName)
        {
            string errorMsg = "";

            switch (_errorName)
            {
                case "errorAddWork":
                    errorMsg = Lang.errorAddWork;
                    break;

                case "errorRemoveWork":
                    errorMsg = Lang.errorRemoveWork;
                    break;

                case "businessSoftwareOn":
                    errorMsg = Lang.businessSoftwareOn;
                    break;

                case "noSelectedWork":
                    errorMsg = Lang.noSelectedWork;
                    break;

                case "unavailableSrcPath":
                    errorMsg = Lang.unavailableSrcPath;
                    break;

                case "unavailableDstPath":
                    errorMsg = Lang.unavailableDstPath;
                    break;

                case "unavailableBackupType":
                    errorMsg = Lang.unavailableBackupType;
                    break;

                case "noChangeSinceLastBackup":
                    errorMsg = Lang.noChangeSinceLastBackup;
                    break;

                case "cannotCreateDstFolder":
                    errorMsg = Lang.cannotCreateDstFolder;                  
                    break;

                case "cannotDelDstFolder":
                    errorMsg = Lang.cannotDelDstFolder;
                    break;

                case "backupFinishedWithError":
                    errorMsg = Lang.backupFinishedWithError;
                    break;

                case "noSpaceDstFolder":
                    errorMsg = Lang.noSpaceDstFolder;
                    break;

                case "diskError":
                    errorMsg = Lang.diskError;
                    break;

                case "cryptoSoftPathNotFound":
                    errorMsg = Lang.cryptoSoftPathNotFound;
                    break;

                case "cryptoSoftPathError":
                    errorMsg = Lang.cryptoSoftPathError;
                    break;

                case "loadLogsError":
                    errorMsg = Lang.loadLogsError;
                    break;

                case "loadSettingsError":
                    errorMsg = Lang.loadSettingsError;
                    break;

                case "loadWorksError":
                    errorMsg = Lang.loadWorksError;
                    break;
                    
                case "cantDeleteWork":
                    errorMsg = Lang.cantDeleteWork;
                    break;

                case "cantGoToSettings":
                    errorMsg = Lang.cantGoToSettings;
                    break;

                default:
                    errorMsg = Lang.unknownError;
                    break;
            }
            MessageBox.Show(errorMsg, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
