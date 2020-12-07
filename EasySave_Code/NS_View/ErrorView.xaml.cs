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
using EasySave.NS_Model;
using EasySave.Langs;
using System.Resources;
using System.Globalization;
using System.Collections;

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

            switch (_errorName)
            {
                case "errorAddWork":
                    MessageBox.Show(Lang.errorAddWork);
                    break;

                case "errorRemoveWork":
                    MessageBox.Show(Lang.errorRemoveWork);
                    break;

                case "businessSoftwareOn":
                    MessageBox.Show(Lang.businessSoftwareOn);
                    break;

                case "noSelectedWork":
                    MessageBox.Show(Lang.noSelectedWork);
                    break;

                case "unavailableSrcPath":
                    MessageBox.Show(Lang.unavailableSrcPath);
                    break;

                case "unavailableDstPath":
                    MessageBox.Show(Lang.unavailableDstPath);
                    break;

                case "unavailableBackupType":
                    MessageBox.Show(Lang.unavailableBackupType);
                    break;

                case "noChangeSinceLastBackup":
                    MessageBox.Show(Lang.noChangeSinceLastBackup);
                    break;

                case "cannotCreateDstFolder":
                    MessageBox.Show(Lang.cannotCreateDstFolder);                  
                    break;

                case "backupFinishedWithError":
                    MessageBox.Show(Lang.backupFinishedWithError);
                    break;

                case "noSpaceDstFolder":
                    MessageBox.Show(Lang.noSpaceDstFolder);
                    break;

                case "diskError":
                    MessageBox.Show(Lang.diskError);
                    break;

                case "cryptoSoftPathNotFound":
                    MessageBox.Show(Lang.cryptoSoftPathNotFound);
                    break;

                case "cryptoSoftPathError":
                    MessageBox.Show(Lang.cryptoSoftPathError);
                    break;

                default:
                    MessageBox.Show(Lang.unknownError);
                    break;
            }
            
        }



    }
}
