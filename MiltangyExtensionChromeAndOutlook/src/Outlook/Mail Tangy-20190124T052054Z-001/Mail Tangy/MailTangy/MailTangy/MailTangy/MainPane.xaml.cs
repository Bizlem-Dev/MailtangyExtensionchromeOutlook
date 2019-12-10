using MailTangy.ViewModels;
using Microsoft.Win32;
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

namespace MailTangy
{
    /// <summary>
    /// Interaction logic for MainPane.xaml
    /// </summary>
    public partial class MainPane : UserControl
    {
        string selectedFeatures="Email to Case Email Based,Email to Case Lead Based,Email to Case Content Based,Calendar Share,Mail Merge,Email Tracking,Smartphone Templates," +
            "Summarizer,Contact Detail Verification,Plug-ins for Gmail,Outlook,Office 365,Productivity Meter,Email Templates,Create SalesForce Records,Mailbox Inside SFDC,Lead to Case Content Based,"
            + "Content Based Auto Case Type Assignment,Sentiment Analysis,Cognitive Autoreply,Lead Propensity to Buy Score Productivity Meter,Social Footprint,";

        //private OpenCaseControl openCases = new OpenCaseControl();
        private FeatureListControl myListControl = new FeatureListControl();
        
        public MainPane()
        {
            InitializeComponent();
            this.DataContext = new MainPaneViewModel();

            if (AreFeatureSubmitted()=="False")
            {
               
                mainFrame.Navigate(myListControl);
                myListControl.FeatureListSelectedValueChanged += MyListControl_FeatureListSelectedValueChanged;
                btnNavigateToSubmit.Visibility = Visibility.Hidden;
            }
            else
            {
                
                //mainFrame.Navigate(openCases);
                
            }
            //Globals.ThisAddIn.Application.ActiveExplorer().SelectionChange += MainPane_SelectionChange;
        }
        
        public MainPane(PaneType paneType)
        {
            InitializeComponent();
            this.DataContext = new MainPaneViewModel();

            switch (paneType)
            {
                case PaneType.UserSpecificPane:
                    
                    mainFrame.Navigate(new UserSpecificView());
                    btnNavigateToSubmit.Visibility = Visibility.Hidden;
                    
                    break;
                case PaneType.FeaturesPane:
                    mainFrame.Navigate(myListControl);
                    myListControl.FeatureListSelectedValueChanged += MyListControl_FeatureListSelectedValueChanged;
                    btnNavigateToSubmit.Visibility = Visibility.Visible;
                    break;
                case PaneType.CasesPane:
                    mainFrame.Navigate(new OpenCaseControl());
                    btnNavigateToSubmit.Visibility = Visibility.Hidden;
                    break;
                case PaneType.ReplyPane:
                    break;
                default:
                    break;
            }
        }

        private string AreFeatureSubmitted()
        {
            //Read Features Submitted Registry ,False
            const string subkey = @"Software\MailTangy";
            
            using (var key = Registry.CurrentUser.OpenSubKey(subkey))
            {
                if (key != null)
                {

                    return key.GetValue("HaveFeaturesSubmitted", false).ToString();
                }
                else
                    return "False";
            }
            
        }

        private void MyListControl_FeatureListSelectedValueChanged(object sender, SelectionChangedEventArgs e)
        {
            //Feature selected = (Feature)myListControl.FeatureListSelectedValue;
            if (e.RemovedItems.Count == 1)
            {
                Feature removedFeature = (Feature)e.RemovedItems[0];

                if (selectedFeatures.Contains(removedFeature.Name))
                {
                    selectedFeatures = selectedFeatures.Replace(removedFeature.Name + ",", "");
                }
            }

            else if (e.AddedItems.Count==1)
            {
                Feature addedFeature = (Feature)e.AddedItems[0];

                if (!selectedFeatures.Contains(addedFeature.Name))
                {
                    selectedFeatures = selectedFeatures + addedFeature.Name + ",";
                }
            }                
        }

        private void BtnNavigateToSubmit_Click(object sender, RoutedEventArgs e)
        {

            mainFrame.Navigate(new SaveFeatures(selectedFeatures.ToString().Substring(0,selectedFeatures.ToString().Length-1)));
            
            btnNavigateToSubmit.Visibility = System.Windows.Visibility.Hidden;
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            //var temp = NewItemCb.Text;
            //NewItemCb.Text = "New";
            //if (!temp.Equals("New"))
            //{
            //        CreateNewItemWindow newItemWin = new CreateNewItemWindow(temp);
            //        newItemWin.ShowDialog();
            //}
        }

        private void NewItemCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ComboBox).Text = "New";
        }
    }

    
}
