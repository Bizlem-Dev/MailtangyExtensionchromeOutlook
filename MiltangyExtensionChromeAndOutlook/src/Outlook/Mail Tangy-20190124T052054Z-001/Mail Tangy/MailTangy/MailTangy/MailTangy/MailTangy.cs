using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Core;
using System.Threading.Tasks;
using Microsoft.Win32;
using Outlook = Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace MailTangy
{
    public partial class MailTangyRibbon
    {
        Outlook.NameSpace outlookNameSpace;
        Outlook.MAPIFolder inbox;
        Outlook.Items items;

        //private LoginCustomPane loginPane;
        private LoginForm loginForm;
       // private Microsoft.Office.Tools.CustomTaskPane taskPaneValue;
        Credentials myCredentials=new Credentials();
        string serverIP = Properties.Settings.Default.ServerURL;
        //// MainPage Container.
        private MailTangyContainer mailTangyContainer;
        public MailTangyContainer MailTangyContainer
        {
            get { return this.mailTangyContainer; }
        }
        private Microsoft.Office.Tools.CustomTaskPane taskPaneContainer;

        private void MailTangy_Load(object sender, RibbonUIEventArgs e)
        {
            myCredentials=myCredentials.deserializeCredentials();
            
            //check if credentials have not expired.
            if (HasLoginExpired().Result==true)
            {
                Globals.ThisAddIn.myCredentials = null;
                //if (loginPane == null)
                //{

                //    loginPane = new LoginCustomPane();

                //    taskPaneValue = Globals.ThisAddIn.CustomTaskPanes.Add(
                //        loginPane, "Login | Salesforce");
                //    taskPaneValue.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
                //    taskPaneValue.DockPositionRestrict = MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoChange;
                //    //Move Login Window to Center

                //    //End
                //    taskPaneValue.Height = 575;
                //    taskPaneValue.Width = 540;
                //    taskPaneValue.Visible = true;
                //    taskPaneValue.VisibleChanged += TaskPaneValue_VisibleChanged;
                //}
                if (loginForm==null)
                {
                    loginForm = new LoginForm();
                    loginForm.Show();
                    loginForm.FormClosed += LoginForm_FormClosed;
                }
            }
            else
            {
                Globals.ThisAddIn.myCredentials = myCredentials;
                ShowCasesWindow();
            }
            
            
        }

        private void LoginForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            ShowCasesWindow();
        }

        private async void items_ItemAdd(object Item)
        {

            try
            {
                string eventHandlerServletURL = "";
                if (Globals.ThisAddIn.myCredentials != null)
                {

                    Outlook.MailItem newMail = Item as Outlook.MailItem;
                    if (newMail!=null)
                    {
                        eventHandlerServletURL = Properties.Settings.Default.ServerURL + "eventHandlerServlet.addnode?access_token="
                        + Globals.ThisAddIn.myCredentials.AccessToken + "&instance_url=" + Globals.ThisAddIn.myCredentials.InstanceURL;
                        string res=await WebRequestHelper.PostFormDataAsync(eventHandlerServletURL, newMail);
                        sendMail(res);
                    }
                    
                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show("Exception Occured in New Mail Handler,eventHandlerServlet.addnode API. " + ex.Message +"\n"+ex.StackTrace.ToString());
            }
        }

        private void sendMail(string autoReplyResponse)
        {
            //string mailsent="false";
            try
            {
                if (autoReplyResponse != null)
                {
                    var AutoReplied = JObject.Parse(autoReplyResponse);
                    var isAutoReplied = AutoReplied["auto_reply_status"];
                    if (isAutoReplied.ToString() == "true")
                    {
                        //create a new mail and  send
                        Outlook.MailItem mailItem = (Outlook.MailItem)Globals.ThisAddIn.Application.CreateItem(Outlook.OlItemType.olMailItem);
                        mailItem.Subject = AutoReplied["replySubject"].ToString();
                        mailItem.To = AutoReplied["from"].ToString();
                        mailItem.HTMLBody = AutoReplied["replyText"].ToString();
                        //var attachmentData = AutoReplied["attachments"];
                        List<Attachment> attachments = JsonConvert.DeserializeObject<List<Attachment>>(AutoReplied["attachments"].ToString());
                        //create attachments
                        foreach (Attachment item in attachments)
                        {
                            string fPath=WebRequestHelper.SaveByteDataAsFile(item.data, item.fileName);
                            mailItem.Attachments.Add(fPath, Outlook.OlAttachmentType.olByValue, 1, item.fileName);
                        }
                        mailItem.Display(false);
                        Outlook.Accounts accounts = Globals.ThisAddIn.Application.Session.Accounts;
                        Outlook.Account acc = null;
                        var sFromAddress = Globals.ThisAddIn.myCredentials.EmailID;
                        //Look for our account in the Outlook
                        foreach (Outlook.Account account in accounts)
                        {
                            if (account.SmtpAddress.Equals(sFromAddress, StringComparison.CurrentCultureIgnoreCase))
                            {
                                //Use it
                                acc = account;
                                break;
                            }
                        }
                        if (acc != null)
                        {
                            //Use this account to send the e-mail. 
                            mailItem.SendUsingAccount = acc;
                            mailItem.Send();
                        }
                        else
                        {
                            throw new Exception("Account does not exist in Outlook: " + sFromAddress);
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occured in Send Mail Method. " + ex.Message + "\n" + ex.StackTrace.ToString());
            }
         
        }

        


        //check if features submitted
        private string AreFeatureSubmitted()
        {
            string featuresURL = Properties.Settings.Default.ServerURL + "checkEmailInFeaturesServlet.check?email=" +
                Globals.ThisAddIn.myCredentials.EmailID;
                
            Globals.ThisAddIn.Application.ActiveExplorer().SelectionChange += new
                Outlook.ExplorerEvents_10_SelectionChangeEventHandler(Globals.ThisAddIn.CurrentExplorer_SelectionChangeEvent);
            outlookNameSpace = Globals.ThisAddIn.Application.GetNamespace("MAPI");

            inbox = outlookNameSpace.Stores[Globals.ThisAddIn.myCredentials.EmailID].GetDefaultFolder(
                    Microsoft.Office.Interop.Outlook.
                    OlDefaultFolders.olFolderInbox);

            items = inbox.Items;
            items.ItemAdd += new Microsoft.Office.Interop.Outlook.ItemsEvents_ItemAddEventHandler(items_ItemAdd);
            string hasfsub= WebRequestHelper.getResponse(featuresURL);
            return hasfsub;
            
            //Read Features Submitted Registry ,False
            //const string subkey = @"Software\MailTangy";

            //using (var key = Registry.CurrentUser.OpenSubKey(subkey))
            //{
            //    if (key != null)
            //    {

            //        return key.GetValue("HaveFeaturesSubmitted", false).ToString();
            //    }
            //    else
            //        return "False";
            //}



        }
        private void TaskPaneValue_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Globals.ThisAddIn.CustomTaskPanes.Count > 0)
                {
                    foreach (Microsoft.Office.Tools.CustomTaskPane pane in Globals.ThisAddIn.CustomTaskPanes)
                    {
                        if (pane.Title == "Login | Salesforce")
                        {
                            ShowCasesWindow();
                            Globals.ThisAddIn.CustomTaskPanes.Remove(pane);
                            break;                    
                        }


                    }
                }
            }
            catch (Exception)
            {

                //throw;
            }
            
            
        }

        private async Task<bool> HasLoginExpired()
        {
            try
            {
                string CaseBasedOnEmailURL = "";
                if (myCredentials != null)
                {
                    //Globals.ThisAddIn.Application.Session.CurrentUser.AddressEntry.Address
                    CaseBasedOnEmailURL = serverIP + "displayCase.allcase?rm_email=" + myCredentials.EmailID
                        + "&type=all"
                        + "&access_token=" + myCredentials.AccessToken+"&instance_url="+myCredentials.InstanceURL;
                    string CaseBasedOnEmailURLResponse = await WebRequestHelper.getResponseAsync(CaseBasedOnEmailURL);
                    if (CaseBasedOnEmailURLResponse == "")
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return true;
            }
            catch (Exception)
            {

                return true;
            }
            
        }
        MailTangyContainer FeatureContainer;

        private void BtnSignIn_Click(object sender, RibbonControlEventArgs e)
        {
            //if (loginPane == null && myCredentials==null)
            //{
            //    loginPane = new LoginCustomPane();
            //    taskPaneValue = Globals.ThisAddIn.CustomTaskPanes.Add(
            //        loginPane, "Login | Salesforce");
            //    taskPaneValue.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
            //    taskPaneValue.DockPositionRestrict = MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoChange;
            //    taskPaneValue.Height = 675;
            //    taskPaneValue.Width = 540;
            //    taskPaneValue.Visible = true;
            //}

        }

        private void ShowCasesWindow()
        {
            if (Globals.ThisAddIn.myCredentials==null)
            {
                return;
            }            
            foreach (Microsoft.Office.Tools.CustomTaskPane item in Globals.ThisAddIn.CustomTaskPanes)
            {
                if (item.Title == "MailTangy UserSpecific")
                {
                    item.Visible = false;
                }
                if (item.Title == "MailTangy Cases")
                {
                    item.Visible = true;
                }
                if (item.Title == "MailTangy Features")
                {
                    Globals.ThisAddIn.CustomTaskPanes.Remove(item);
                    break;
                }
            }
            System.Threading.Thread.Sleep(500);
            if (MailTangyContainer == null)
            {
                if (AreFeatureSubmitted().Contains("false"))
                {
                    FeatureContainer = new MailTangyContainer(PaneType.FeaturesPane);
                    taskPaneContainer = Globals.ThisAddIn.CustomTaskPanes.Add(FeatureContainer, "MailTangy Features");
                    taskPaneContainer.Visible = true;
                    taskPaneContainer.VisibleChanged += new EventHandler(FeaturesPane_VisibleChanged);
                    taskPaneContainer.Width = 340;
                    
                }
                else
                {
                    mailTangyContainer = new MailTangyContainer(PaneType.CasesPane);
                    taskPaneContainer = Globals.ThisAddIn.CustomTaskPanes.Add(mailTangyContainer, "MailTangy Cases");
                    //taskPaneValue.DockPosition = MsoCTPDockPosition.msoCTPDockPositionFloating;
                    //taskPaneValue.Height = 420;
                    taskPaneContainer.Visible = true;
                    taskPaneContainer.VisibleChanged += new EventHandler(taskPaneValue_VisibleChanged);
                    taskPaneContainer.Width = 340;              
                }
            }
    
        }

        private void FeaturesPane_VisibleChanged(object sender, EventArgs e)
        {
            
            mailTangyContainer = null;
                               
            taskPaneContainer.VisibleChanged -= new EventHandler(FeaturesPane_VisibleChanged);
            ShowCasesWindow();

        }

        private void BtnCases_Click(object sender, RibbonControlEventArgs e)
        {
            ShowCasesWindow();
        }

        private void taskPaneValue_VisibleChanged(object sender, EventArgs e)
        {
            if (taskPaneContainer != null)
            {
                Globals.Ribbons.MailTangy.btnCases.Enabled = !taskPaneContainer.Visible;
            }

        }

        private void Button1_Click(object sender, RibbonControlEventArgs e)
        {
            
        }

        
    }
}
