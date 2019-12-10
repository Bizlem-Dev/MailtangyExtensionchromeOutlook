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

namespace MailTangy.CreateItemsControls
{
    /// <summary>
    /// Interaction logic for NewItemFrame.xaml
    /// </summary>
    public partial class NewItemFrame : UserControl
    {
        public NewItemFrame()
        {
            InitializeComponent();
        }

        public NewItemFrame(string WindowName)
        {
            InitializeComponent();
            switch (WindowName)
            {
                case "Case":
                    mainFrame.Navigate(new NewCaseControl());
                    break;
                case "Task":
                    mainFrame.Navigate(new NewTaskControl());
                    break;
                case "Lead":
                    mainFrame.Navigate(new NewLeadControl());
                    break;
                default:
                    break;
            }
        }
    }
}
