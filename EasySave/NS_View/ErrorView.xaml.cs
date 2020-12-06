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
            MessageBox.Show(_errorName);
        }
    }
}
