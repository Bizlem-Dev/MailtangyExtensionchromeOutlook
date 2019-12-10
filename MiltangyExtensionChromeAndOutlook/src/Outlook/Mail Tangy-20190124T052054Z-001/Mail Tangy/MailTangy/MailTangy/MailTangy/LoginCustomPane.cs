using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MailTangy
{
    public partial class LoginCustomPane : UserControl
    {
        Credentials myCredentials = new Credentials();
        string redirectURL = Properties.Settings.Default.RedirectURL;
        public LoginCustomPane()
        {
            InitializeComponent();
            string url = "https://login.salesforce.com/services/oauth2/authorize?response_type=code&client_id=" +
              "3MVG9d8..z.hDcPLTrjNrmyBE6aaQd4ppOmDoPDosVQgzlKdbddzLx48u68paKUn8o_UK2ZcZBFu9aQ7_DFPz&redirect_uri=" + redirectURL;
            //string url = "http://www.google.com";
            webBrowserLogin.Navigate(url);           
            webBrowserLogin.TabIndex = 0;
            webBrowserLogin.TabStop = true;
            webBrowserLogin.NewWindow += new CancelEventHandler(wb_newWindow);
            webBrowserLogin.Visible = true;         
        }

        private void wb_newWindow(object sender, CancelEventArgs e)
        {
           e.Cancel = true;
        }
        //bool isEventRegistered = false;
        //HtmlElement loginBtn;
        private async void webBrowserLogin_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //var doc = webBrowserLogin.Document;
            webBrowserLogin.Focus();
            //loginBtn = webBrowserLogin.Document.GetElementById("Login");
            //if (loginBtn!=null && !isEventRegistered)
            //{
            //    loginBtn.MouseDown += BtnLogin_Click;
            //    isEventRegistered = true;
            //}
            
            if (e.Url.ToString().Contains("?code="))
            {
                string code = e.Url.ToString().Split(new string[] { "code="}, StringSplitOptions.None)[1];
                try
                {
                    await myCredentials.GetTokens(code);
                    
                    myCredentials.EmailID = getLoggedInUser();
                    Globals.ThisAddIn.myCredentials = myCredentials;
                    myCredentials.serializeCredentials(myCredentials);
                }
                catch (Exception ex)
                {

                    System.Windows.MessageBox.Show("Failed to Fetch Tokens." + ex.Message);
                }
                //Not usable now.Webbrowser not working in taskpane.
                foreach (Microsoft.Office.Tools.CustomTaskPane pane in Globals.ThisAddIn.CustomTaskPanes)
                {
                    if (pane.Title== "Login | Salesforce")
                    {
                        //((WebBrowser)sender).Dispose();
                        pane.Visible = false;
                        //Globals.ThisAddIn.CustomTaskPanes.Remove(pane);
                        break;
                    }
                }
                //Ends here..
                this.ParentForm.Invoke((MethodInvoker)delegate{
                    this.ParentForm.Close();
                });
                //loginPane = null;              
            }
            
        }

        //private void BtnLogin_Click(object sender, HtmlElementEventArgs e)
        //{
        //    switch (e.MouseButtonsPressed)
        //    {
        //        case MouseButtons.Left:
        //            HtmlElement element = webBrowserLogin.Document.GetElementFromPoint(e.ClientMousePosition);
        //            if (element != null && "submit".Equals(element.GetAttribute("type"), StringComparison.OrdinalIgnoreCase))
        //            {
        //                HtmlElement logId = webBrowserLogin.Document.GetElementById("username");
        //            }
        //            break;
        //    }
        //}

        private string getLoggedInUser()
        {
            string LoggedinUserID = String.Empty;
            Microsoft.Office.Interop.Outlook.Recipient item = Globals.ThisAddIn.Application.ActiveExplorer().Session.CurrentUser;
            if (item.AddressEntry.Type == "SMTP")
            {
                LoggedinUserID = item.AddressEntry.Address;
            }
            //If the User is Exchange type then extraction of Email ID is different.
            else if (item.AddressEntry.Type == "EX")
            {
                Microsoft.Office.Interop.Outlook.AddressEntry rec = item.AddressEntry;

                string psmtp = item.AddressEntry.GetExchangeUser().PrimarySmtpAddress;
                if (psmtp != null)
                {
                    LoggedinUserID = psmtp;
                }

                else
                {
                    //Emailid if not fetched from above method trying to fetch it using MAPI property.
                    //System.Windows.Forms.MessageBox.Show("PrimarySMTPAddress Property of exchange returned null.Trying with MAPI properties now.");
                    try
                    {
                        item.Resolve();
                        string PR_SMTP_ADDRESS = @"http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
                        string email = "";
                        email = rec.PropertyAccessor.GetProperty(PR_SMTP_ADDRESS).ToString();
                        if (email != "")
                        {
                            LoggedinUserID = email;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Failed to get Email via MAPI, " + ex.Message);
                    }

                }


            }
            return LoggedinUserID;
        }

        private void webBrowserLogin_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Console.WriteLine(e.Url);
        }
    }
}
