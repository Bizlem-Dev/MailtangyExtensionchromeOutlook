using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
    /// Interaction logic for SaveFeatures.xaml
    /// </summary>
    public partial class SaveFeatures : UserControl
    {
        public SaveFeatures()
        {
            InitializeComponent();
        }

        string featureList;
        public SaveFeatures(string featuresListString) : this()
        {
            featureList = featuresListString;
        }

        private async void btnSubmitFeatures_Click(object sender, RoutedEventArgs e)
        {

            // string UserEmail = Globals.ThisAddIn.Application.Session.CurrentUser.AddressEntry.Address;
            string UserEmail = Globals.ThisAddIn.myCredentials.EmailID;
            StringBuilder featureAPIRequestData =new StringBuilder("rm_email=" + UserEmail);
            
            if (featureList.Contains("Email to Case Email Based"))
            {
                featureAPIRequestData.Append("&Email_to_Case_Email_Based=true");
            }
            else
            {
                featureAPIRequestData.Append("&Email_to_Case_Email_Based=false");
            }
            if (featureList.Contains("Email to Case Lead Based"))
            {
                featureAPIRequestData.Append("&Email_to_Case_Lead_Based=true");
            }
            else
                featureAPIRequestData.Append("&Email_to_Case_Lead_Based=false");
            if (featureList.Contains("Email to Case Content Based"))
            {
                featureAPIRequestData.Append("&Email_to_Case_Content_Based=true");
            }
            else
                featureAPIRequestData.Append("&Email_to_Case_Content_Based=false");

            if (featureList.Contains("Calendar Share"))
            {
                featureAPIRequestData.Append("&Calendar_Share=true");
            }
            else
                featureAPIRequestData.Append("&Calendar_Share=false");
            if (featureList.Contains("Mail Merge"))
            {
                featureAPIRequestData.Append("&Mail_Merge=true");
            }
            else
                featureAPIRequestData.Append("&Mail_Merge=false");
            if (featureList.Contains("Email Tracking"))
            {
                featureAPIRequestData.Append("&Email_Tracking=true");
            }
            else
                featureAPIRequestData.Append("&Email_Tracking=false");
            if (featureList.Contains("Smartphone Templates"))
            {
                featureAPIRequestData.Append("&Smartphone_Templates=true");
            }
            else
                featureAPIRequestData.Append("&Smartphone_Templates=false");

            if (featureList.Contains("Summarizer"))
            {
                featureAPIRequestData.Append("&Summarizer=true");
            }
            else
                featureAPIRequestData.Append("&Summarizer=false");
            if (featureList.Contains("Contact Detail Verification"))
            {
                featureAPIRequestData.Append("&Contact_Detail_Varification=true");
            }
            else
                featureAPIRequestData.Append("&Contact_Detail_Varification=false");
            if (featureList.Contains("Plug-ins for Gmail,Outlook,Office 365,Productivity Meter"))
            {
                featureAPIRequestData.Append("&plugins_for_gmail_outlook_office_365=true");
            }
            else
                featureAPIRequestData.Append("&plugins_for_gmail_outlook_office_365=false");
            if (featureList.Contains("Email Templates"))
            {
                featureAPIRequestData.Append("&email_templates=true");
            }
            else
                featureAPIRequestData.Append("&email_templates=false");
            if (featureList.Contains("Create SalesForce Records"))
            {
                featureAPIRequestData.Append("&Create_SalesForce_Records=true");
            }
            else
                featureAPIRequestData.Append("&Create_SalesForce_Records=false");
            if (featureList.Contains("Mailbox Inside SFDC"))
            {
                featureAPIRequestData.Append("&mail_box_inside_sfdc=true");
            }
            else
                featureAPIRequestData.Append("&mail_box_inside_sfdc=false");
            if (featureList.Contains("Lead to Case Content Based"))
            {
                featureAPIRequestData.Append("&lead_to_case_content_based=true");
            }
            else
                featureAPIRequestData.Append("&lead_to_case_content_based=false");

            if (featureList.Contains("Content Based Auto Case Type Assignment"))
            {
                featureAPIRequestData.Append("&content_based_auto_case_type_assignment=true");
            }
            else
                featureAPIRequestData.Append("&content_based_auto_case_type_assignment=false");
            if (featureList.Contains("Sentiment Analysis"))
            {
                featureAPIRequestData.Append("&sentiment_analysis=true");
            }
            else
                featureAPIRequestData.Append("&sentiment_analysis=false");
            if (featureList.Contains("Cognitive Autoreply"))
            {
                featureAPIRequestData.Append("&cognitive_auto_reply=true");
            }
            else
                featureAPIRequestData.Append("&cognitive_auto_reply=false");
            if (featureList.Contains("Lead Propensity to Buy Score Productivity Meter"))
            {
                featureAPIRequestData.Append("&lead_propensity_to_buy_score_productivity_meter=true");
            }
            else
                featureAPIRequestData.Append("&lead_propensity_to_buy_score_productivity_meter=false");
            if (featureList.Contains("Social Footprint"))
            {
                featureAPIRequestData.Append("&social_footprint=true");
            }
            else
                featureAPIRequestData.Append("&social_footprint=false");
            if (featureList.Contains("Productivity Meter"))
            {
                featureAPIRequestData.Append("&productivity_meter=true");
            }
            else
                featureAPIRequestData.Append("&productivity_meter=false");


            featureAPIRequestData.Append("&domain="+InternalDomain.Text+"&emailid="+EmailInternal.Text);
            try
            {
                var response = await WebRequestHelper.HttpPOST(featureAPIRequestData.ToString(), Properties.Settings.Default.ServerURL+"featureServletNew.addnode");
                //if (response=="feature saved")
                //{
                //    //Update Registry
                //    const string subkey = @"Software\MailTangy";
                //    string user = Environment.UserDomainName + "\\" + Environment.UserName;
                //    RegistryAccessRule rule = new RegistryAccessRule(user, RegistryRights.FullControl,
                //    AccessControlType.Allow);
                //    RegistrySecurity security = new RegistrySecurity();
                //    security.AddAccessRule(rule);

                //    using (var key = Registry.CurrentUser.OpenSubKey(subkey, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl))
                //    {
                //        if (key != null)
                //        {
                //            key.SetAccessControl(security);
                //            key.SetValue("HaveFeaturesSubmitted", "True");
                //            //close the custom Pane

                //        }

                //    }

                //}
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Failed while saving Features" + ex.Message);
            }
            finally
            {
                for (int i = 0; i < Globals.ThisAddIn.CustomTaskPanes.Count; i++)
                {
                    Microsoft.Office.Tools.CustomTaskPane pane = Globals.ThisAddIn.CustomTaskPanes[i];
                    pane.Visible = false;
                    //pane = null;
                }
                Globals.Ribbons.MailTangy.btnCases.Enabled = true;
            }
            
        }
    }
}
