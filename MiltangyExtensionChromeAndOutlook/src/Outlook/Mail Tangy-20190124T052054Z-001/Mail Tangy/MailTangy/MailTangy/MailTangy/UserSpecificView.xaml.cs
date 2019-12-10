using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailTangy
{
    /// <summary>
    /// Interaction logic for UserSpecificView.xaml
    /// </summary>
    public partial class UserSpecificView : UserControl,INotifyPropertyChanged
    {
        string serverURL = Properties.Settings.Default.ServerURL;
        public UserSpecificView()
        {
            InitializeComponent();
            //EnableCollectionSynchronization(EmailBasedCases, _syncLock);
            
            //GetTemplates("rahulsv52@gmail.com");
            //DisplayCaseBasedOnEmail(Globals.ThisAddIn.LoggedinUserID, Globals.ThisAddIn.SenderEmailID.ToString());         
            this.DataContext = this;
            ShowTasksPane = System.Windows.Visibility.Hidden;
            SelectedMailItem = Globals.ThisAddIn.Application.ActiveExplorer().Selection[1] as Outlook.MailItem;
            if (SelectedMailItem!=null)
            {
                CallGmailProfileAPI(SelectedMailItem.SenderEmailAddress);
                //DisplayCaseBasedOnEmail(Globals.ThisAddIn.LoggedinUserID, SelectedMailItem.SenderEmailAddress);
            }
            Globals.ThisAddIn.Application.ActiveExplorer().SelectionChange += new
                Microsoft.Office.Interop.Outlook.ExplorerEvents_10_SelectionChangeEventHandler(SelectionChangeEvent);
            Globals.ThisAddIn.Application.ActiveExplorer().SelectionChange -= new Outlook.ExplorerEvents_10_SelectionChangeEventHandler(Globals.ThisAddIn.CurrentExplorer_SelectionChangeEvent);
            Globals.ThisAddIn.Application.ActiveExplorer().FolderSwitch += new Microsoft.Office.Interop.Outlook.ExplorerEvents_10_FolderSwitchEventHandler(FolderSwitchEvent);
        }

        private void FolderSwitchEvent()
        {
            try
            {
                if (Globals.ThisAddIn.CustomTaskPanes.Count > 0)
                {
                    var taskPanes = Globals.ThisAddIn.CustomTaskPanes;
                    foreach (Microsoft.Office.Tools.CustomTaskPane pane in taskPanes)
                    {
                        if (pane.Title == "MailTangy UserSpecific")
                        {
                            pane.Visible = false;
                        }
                        if (pane.Title == "MailTangy Cases")
                        {
                            pane.Visible = true;
                        }
                        if (pane.Title == "Login | Salesforce")
                        {
                            Globals.ThisAddIn.CustomTaskPanes.Remove(pane);
                        }

                    }
                }
            }
            catch { }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }

        Microsoft.Office.Interop.Outlook.MailItem SelectedMailItem;
        private DateTime caseLastRequestTime = DateTime.Now;
        private void resetUI()
        {
            PositivePercentage = "";
            NegativePercentage = "";
            NeutralPercentage = "";
            ClosedCaseCount = "Closed Case: ";
            OpenCaseCount = "Open Case: ";
            //UserProfileImagePath = "";
            //UserEmail = "";
            //UserName = "";
            ShowTasksPane = System.Windows.Visibility.Hidden;
            OnPropertyChanged("ShowTasksPane");
        }
        private void SelectionChangeEvent()
        {
            
            replyfired = false;
            //Hide Template section
            Outlook.Explorer explorer = Globals.ThisAddIn.Application.Explorers.Application.ActiveExplorer();
            var inlineResponse = explorer.ActiveInlineResponse;
            if (inlineResponse == null)
            {
                ShowTemplateSection = Visibility.Collapsed;
            }
            else
            {
                ShowTemplateSection = Visibility.Visible;
            }
            if (Globals.ThisAddIn.Application.ActiveExplorer().Selection.Count > 0)
            {
                Object selObject = Globals.ThisAddIn.Application.ActiveExplorer().Selection[1];
                if (selObject is Microsoft.Office.Interop.Outlook.MailItem)
                {
                    Outlook.MailItem currSel =(Outlook.MailItem) selObject;
                    if (SelectedMailItem!=null)
                    {
                        foreach (Microsoft.Office.Tools.CustomTaskPane pane in Globals.ThisAddIn.CustomTaskPanes)
                        {
                            if (pane.Title == "MailTangy UserSpecific")
                            {
                                pane.Visible = true;
                            }
                            if (pane.Title == "MailTangy Cases")
                            {
                                pane.Visible = false;
                            }
                            if (pane.Title == "Login | Salesforce")
                            {
                                Globals.ThisAddIn.CustomTaskPanes.Remove(pane);
                            }

                        }
                        try
                        {
                            if (SelectedMailItem.EntryID != currSel.EntryID)
                            {
                                resetUI();
                                SelectedMailItem = selObject as Microsoft.Office.Interop.Outlook.MailItem;
                                ((Outlook.ItemEvents_10_Event)SelectedMailItem).Reply += new Outlook.ItemEvents_10_ReplyEventHandler(MailReplyEventHandler);
                                ((Outlook.ItemEvents_10_Event)SelectedMailItem).ReplyAll += new Outlook.ItemEvents_10_ReplyAllEventHandler(MailReplyEventHandler);
                                ((Outlook.ItemEvents_10_Event)SelectedMailItem).Forward += new Outlook.ItemEvents_10_ForwardEventHandler(MailReplyEventHandler);

                                //Outlook.Recipient rec = Globals.ThisAddIn.Application.GetNamespace("MAPI").Session.CurrentUser;
                                string loggedInUser= Globals.ThisAddIn.myCredentials.EmailID;

                                //Outlook.AddressEntry AddEntry = rec.AddressEntry;
                                //if (AddEntry.Type == "EX")
                                //{
                                //    loggedInUser = rec.AddressEntry.GetExchangeUser().PrimarySmtpAddress;
                                //}
                                //else
                                //{
                                //    loggedInUser = rec.AddressEntry.Address;
                                //}

                                string SenderEmailID = SelectedMailItem.SenderEmailAddress;
                                CallGmailProfileAPI(SenderEmailID);
                                
                                CallSummerizedServletAPI(SenderEmailID,SelectedMailItem.Subject,SelectedMailItem.Body);
                                GetTemplates(SenderEmailID);
                                if (LeadOrCase=="LEAD")
                                {
                                    CallLeadCaseDataBasedOnEmail(SenderEmailID);
                                }
                                if (LeadOrCase=="CASE")
                                {
                                    DisplayCaseBasedOnEmail(loggedInUser, SenderEmailID,SelectedMailItem.Subject);
                                }                              

                            }
                        }
                        catch (System.Exception)
                        {

                            //throw;
                        }
                    }
                    else
                    {
                        SelectedMailItem = selObject as Microsoft.Office.Interop.Outlook.MailItem;
                        ((Outlook.ItemEvents_10_Event)SelectedMailItem).Reply += new Outlook.ItemEvents_10_ReplyEventHandler(MailReplyEventHandler);
                        ((Outlook.ItemEvents_10_Event)SelectedMailItem).ReplyAll += new Outlook.ItemEvents_10_ReplyAllEventHandler(MailReplyEventHandler);
                        ((Outlook.ItemEvents_10_Event)SelectedMailItem).Forward += new Outlook.ItemEvents_10_ForwardEventHandler(MailReplyEventHandler);
                    }
                           
                }
            }
            
        }

        bool replyfired;

        private Outlook.MailItem composedMail;
        private string originalBody;

        private void MailReplyEventHandler(object Response, ref bool Cancel)
        {
            if (replyfired)
            {
                return;
            }
            replyfired = true;
            //Call Template API and set visibility property of the template section.
            ShowTemplateSection = Visibility.Visible;
            composedMail = Response as Outlook.MailItem;
            originalBody = composedMail.HTMLBody;
            //GetTemplates(composedMail.SenderEmailAddress);
            GetTemplates("rahulsv52@gmail.com");
            ((Outlook.ItemEvents_10_Event)SelectedMailItem).Reply -= new Outlook.ItemEvents_10_ReplyEventHandler(MailReplyEventHandler);
        }

        private async void GetTemplates(string email)
        {
            try
            {

                string TemplateURL = "";
                if (Globals.ThisAddIn.myCredentials != null)
                {
                    TemplateURL = serverURL+"SelectTemplates.getdata?email=" + email;
                    string TemplateURLResponse = await WebRequestHelper.getResponseAsync(TemplateURL);
                    if (TemplateURLResponse != "")
                    {
                        //EmailBasedCaseData deserializedEmailBasedCaseData = JsonConvert.DeserializeObject<EmailBasedCaseData>(CaseBasedOnEmailURLResponse);
                        Templates = JsonConvert.DeserializeObject<Templates>(TemplateURLResponse);
                    }
                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show("Exception Occured in SelectTemplates API. " + ex.Message);
            }
        }
        //private static object _syncLock = new object();

        private async void DisplayCaseBasedOnEmail(string currentUserEmailID,string selectedSenderMailID,string selectedMailSubject)
        {
            
            try
            {
                string CaseBasedOnEmailURL = "";
                if (Globals.ThisAddIn.myCredentials != null)
                {
                    CaseBasedOnEmailURL = serverURL+"displayCaseBasedOnEmail.displaycase?rm_email=" + currentUserEmailID + "&email=" + selectedSenderMailID
                        +"&subject="+ selectedMailSubject
                        + "&access_token=" + Globals.ThisAddIn.myCredentials.AccessToken
                        + "&instance_url=" + Globals.ThisAddIn.myCredentials.InstanceURL;
                    string CaseBasedOnEmailURLResponse = await WebRequestHelper.getResponseAsync(CaseBasedOnEmailURL);
                    if (CaseBasedOnEmailURLResponse != "" && !CaseBasedOnEmailURLResponse.Contains("null"))
                    {

                        var details = JObject.Parse(CaseBasedOnEmailURLResponse);
                        ClosedCaseCount = "Closed Case: " + details["totalClosedCaseEmail"].ToString();
                        OpenCaseCount = "Open Case: " + details["totalOpenCaseEmail"].ToString();
                        var escaletedcasesArray = details["escalated_cases"];
                        var opencasesArray = details["opencase"];
                        var autoRepliedCase = details["autoreplied_case"];
                        var currentCase = details["currentsubject"];
                        PositivePercentage = details["positive_percentage"].ToString();
                        NegativePercentage = details["nagative_percentage"].ToString();
                        NeutralPercentage = details["neutral_percentage"].ToString();
                        List<OpenCase> tempList = currentCase.ToObject<List<OpenCase>>();
                        tempList.AddRange( escaletedcasesArray.ToObject<List<OpenCase>>());
                        tempList.AddRange(opencasesArray.ToObject<List<OpenCase>>());
                        tempList.AddRange(autoRepliedCase.ToObject<List<OpenCase>>());
                        //lock (_syncLock)
                        //{
                        //    EmailBasedCases = escaletedcasesArray.ToObject<List<OpenCase>>();
                        //    EmailBasedCases.AddRange(opencasesArray.ToObject<List<OpenCase>>());
                        //}
                        int index = tempList.FindIndex(a => a.CaseSubject == SelectedMailItem.Subject);
                        if (index >= 0)
                        {
                            OpenCase selectedCase = tempList[index];
                            selectedCase.IsSelectedCase = true;
                            tempList.RemoveAt(index);
                            tempList.Insert(0, selectedCase);
                        }

                        EmailBasedCases = tempList;
                        emailListBox.Dispatcher.Invoke((System.Windows.Forms.MethodInvoker)delegate {
                            emailListBox.Focus();
                        });

                    }
                }
            }
            catch (System.Exception ex)
            {

                MessageBox.Show("Exception Occured in displayCaseBasedOnEmail API." + ex.Message);
            }
            
        }

        public static void EnableCollectionSynchronization(IEnumerable collection, object lockObject)
        {
            // Equivalent to .NET 4.5:
            // BindingOperations.EnableCollectionSynchronization(collection, lockObject);
            MethodInfo method = typeof(BindingOperations).GetMethod("EnableCollectionSynchronization", new Type[] { typeof(IEnumerable), typeof(object) });
            if (method != null)
            {
                method.Invoke(null, new object[] { collection, lockObject });
            }
        }


        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var systemPath = System.Environment.
                             GetFolderPath(
                                 Environment.SpecialFolder.CommonApplicationData
                             );
            var complete = System.IO.Path.Combine(systemPath, "Point5Nyble\\MailTangy");
            if (Directory.Exists(complete))
            {
                Directory.Delete(complete, true);
                //unhook selection event
                Globals.ThisAddIn.myCredentials = null;
                Globals.ThisAddIn.Application.ActiveExplorer().SelectionChange -= new
                Microsoft.Office.Interop.Outlook.ExplorerEvents_10_SelectionChangeEventHandler(SelectionChangeEvent);
            }
            foreach (Microsoft.Office.Tools.CustomTaskPane pane in Globals.ThisAddIn.CustomTaskPanes)
            {
                pane.Visible = false;
            }
            
        }

        private async void CallGmailProfileAPI(string emailId)
        {
            string GmailURL = "";
            if (Globals.ThisAddIn.myCredentials != null)
            {
                GmailURL = serverURL+"getGmailProfile.gmailprofile?p_email=" + emailId;
                string gmailProfileResponse = await WebRequestHelper.getResponseAsync(GmailURL);
                if (gmailProfileResponse != "")
                {
                    var details = JObject.Parse(gmailProfileResponse);
                    UserName = details["name"].ToString();
                    UserProfileImagePath = details["photo_link"].ToString();
                    UserEmail = details["email"].ToString();
                }
                else
                {
                    UserName = emailId.Substring(0,emailId.IndexOf('@'));
                    UserProfileImagePath = "https://i.stack.imgur.com/34AD2.jpg";
                    UserEmail = emailId;
                }
            }
        }

        //function for fetching LeadData and CaseData
        private async void CallLeadCaseDataBasedOnEmail(string emailId)
        {
            string MailDataURL = "";
            if (Globals.ThisAddIn.myCredentials != null)
            {
                MailDataURL = serverURL+"MailDataBasedOnEmail.getdata?email=" + emailId +"&to=" + Globals.ThisAddIn.myCredentials.EmailID;
                string EmailDataResponse = await WebRequestHelper.getResponseAsync(MailDataURL);
                if (EmailDataResponse != "")
                {
                    EmailBasedCaseData deserializedEmailBasedCaseData = JsonConvert.DeserializeObject<EmailBasedCaseData>(EmailDataResponse);
                    EmailBasedLeads = deserializedEmailBasedCaseData.LeadDataCollection;
                }
            }
        }
        private void CallSummerizedServletAPI(string emailID,string subject,string summ_txt)
        {

            //emailID = "khushbumehta102@gmail.com";
            //subject = "Change of Address";
            //summ_txt = "Dear Ram, Changes of address in Present address Regards, Khushbu";
            string SummerizerURL = "";
            if (Globals.ThisAddIn.myCredentials != null)
            {
                SummerizerURL = serverURL+"summerizerServlet.summery?email=" + emailID
                + "&subject=" + subject + "&summ_txt=" + summ_txt + "&access_token=" + Globals.ThisAddIn.myCredentials.AccessToken
                +"&instance_url="+Globals.ThisAddIn.myCredentials.InstanceURL;
               
                string summarizerResponse =  WebRequestHelper.getResponse(SummerizerURL);
                
                if (summarizerResponse != "{}" && summarizerResponse!="null" )
                {
                    var details = JObject.Parse(summarizerResponse);
                    LeadOrCase = details["type"].ToString();
                    if (LeadOrCase== "NO CASE NO LEAD")
                    {
                        return;
                    }
                    
                }
            }
        }

        #region Properties

        private string closedCaseCount;
        public string ClosedCaseCount
        {
            get
            {
                return closedCaseCount;

            }
            set
            {
                closedCaseCount = value;
                OnPropertyChanged("ClosedCaseCount");
            }
        }
        private string openCaseCount;
        public string OpenCaseCount
        {
            get
            {
                return openCaseCount;

            }
            set
            {
                openCaseCount = value;
                OnPropertyChanged("OpenCaseCount");
            }
        }
        private Visibility showTemplateSection=Visibility.Collapsed;

        public Visibility ShowTemplateSection
        {
            get
            {
                return showTemplateSection;

            }
            set
            {
                showTemplateSection = value;
                OnPropertyChanged("ShowTemplateSection");
            }
        }
        private List<LeadData> emailBasedLeads;
        public List<LeadData> EmailBasedLeads
        {
            get
            {
                return emailBasedLeads;

            }
            set
            {
                emailBasedLeads = value;
                OnPropertyChanged("EmailBasedLeads");
            }
        }
        private Templates templates;
        public Templates Templates
        {
            get
            {
                return templates;

            }
            set
            {
                templates = value;
                OnPropertyChanged("Templates");
            }
        }
        private Tasks allTasks;
        public  Tasks   AllTasks
        {
            get { return allTasks; }
            set
            {
                if (value!=allTasks)
                {
                    allTasks = value;
                    OnPropertyChanged("AllTasks");
                }
            }
        }
        private List<OpenCase> emailBasedCases;
        public List<OpenCase> EmailBasedCases
        {
            get
            {
                if (emailBasedCases != null)
                {
                    return emailBasedCases.FindAll(x => x.CaseStatus != "Closed").ToList();
                }
                else
                    return null;
                

            }
            set
            {
                emailBasedCases = value;
                OnPropertyChanged("EmailBasedCases");
            }
        }

        private string sentimentOutput;
        public string SentimentOutput
        {
            get
            {
                return sentimentOutput;

            }
            set
            {
                sentimentOutput = value;
                OnPropertyChanged("SentimentOutput");
            }
        }

        private string summarizerOutput;
        public string SummarizerOutput
        {
            get
            {
                return summarizerOutput;

            }
            set
            {
                summarizerOutput = value;
                OnPropertyChanged("SummarizerOutput");
            }
        }
        
        private string leadOrCase;
        public string LeadOrCase
        {
            get
            {
                return leadOrCase;

            }
            set
            {
                leadOrCase = value;
                OnPropertyChanged("LeadOrCase");
            }
        }
        private bool isAutoReplied;
        public bool IsAutoReplied
        {
            get
            {
                return isAutoReplied;

            }
            set
            {
                isAutoReplied = value;
                OnPropertyChanged("IsAutoReplied");
            }
        }
        
        private string userName;
        public string UserName
        {
            get
            {
                return userName;

            }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }
        private string userEmail;
        public string UserEmail
        {
            get
            {
                return userEmail;

            }
            set
            {
                userEmail = value;
                OnPropertyChanged("UserEmail");
            }
        }
        private string userProfileImagePath;
        public string UserProfileImagePath
        {
            get
            {
                return userProfileImagePath;

            }
            set
            {
                userProfileImagePath = value;
                OnPropertyChanged("UserProfileImagePath");
            }
        }
        private bool showPopup;
        public bool ShowPopup
        {
            get
            {
                return showPopup;

            }
            set
            {
                showPopup = value;
                OnPropertyChanged("ShowPopup");
            }
        }
        private string closeCaseNumber;
        public string CloseCaseNumber
        {
            get
            {
                return closeCaseNumber;

            }
            set
            {
                closeCaseNumber = value;
                OnPropertyChanged("CloseCaseNumber");
            }
        }
        private string positivePercentage;
        public string PositivePercentage
        {
            get
            {
                return positivePercentage;

            }
            set
            {
                positivePercentage = value;
                OnPropertyChanged("PositivePercentage");
            }
        }
        private string negativePercentage;
        public string NegativePercentage
        {
            get
            {
                return negativePercentage;

            }
            set
            {
                negativePercentage = value;
                OnPropertyChanged("NegativePercentage");
            }
        }
        private string neutralPercentage;
        
        private Visibility showNoTasksMessage= System.Windows.Visibility.Hidden;
        
        public Visibility ShowNoTasksMessage
        {
            get { return showNoTasksMessage;}
            set
            {
                if (value!=showNoTasksMessage)
                {
                    showNoTasksMessage = value;
                    OnPropertyChanged("ShowNoTasksMessage");
                }
            }
        }

        private Visibility hideTasksList = System.Windows.Visibility.Visible;
        public Visibility HideTasksList
        {
            get { return hideTasksList; }
            set
            {
                if (value != hideTasksList)
                {
                    hideTasksList = value;
                    OnPropertyChanged("HideTasksList");
                }
            }
        }
        public string NeutralPercentage
        {
            get
            {
                return neutralPercentage;

            }
            set
            {
                neutralPercentage = value;
                OnPropertyChanged("NeutralPercentage");
            }
        }

        public Visibility ShowTasksPane { get; private set; }
        #endregion

        private void BtnMailSummary_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnCloseCase_Click(object sender, RoutedEventArgs e)
        {
            ShowPopup = true;
            Button button = sender as Button;
            OpenCase mycase = button.DataContext as OpenCase;
            CloseCaseNumber = mycase.CaseNumber; 
        }

        private async void BtnOKClose_Click(object sender, RoutedEventArgs e)
        {
            BtnOKClose.IsEnabled = false;
            //Fire Case Close API and hide Popup
            string CaseCloseURL = "";
            if (Globals.ThisAddIn.myCredentials != null)
            {
                CaseCloseURL = serverURL+"closeCaseServlet.caseclose?access_token=" + Globals.ThisAddIn.myCredentials.AccessToken
                    + "&instance_url=" + Globals.ThisAddIn.myCredentials.InstanceURL + "&case_no="
                    +CloseCaseNumber+"&case_close_reason="+CaseClosureReason.Text;
                string caseCloseResponse = await WebRequestHelper.getResponseAsync(CaseCloseURL);
                if (caseCloseResponse.Contains("Case closed and reason saved successfully"))
                {
                    var itemToRemove = EmailBasedCases.Single(r => r.CaseNumber == CloseCaseNumber);
                    EmailBasedCases.Remove(itemToRemove);
                    emailListBox.Items.Refresh();
                }
            }
            ShowPopup = false;
            BtnOKClose.IsEnabled = true;
        }

        private void ClosePopup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPopup = false;
        }
        
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (TemplateComboBox.SelectedItem != null)
            {
                if (composedMail!=null)
                {
                    Template selectedTemplate = (Template)TemplateComboBox.SelectedItem;
                    composedMail.HTMLBody =  selectedTemplate.temp_txt + originalBody ;
                }
            }
        }

        private async void btnAllTasks_Click(object sender, RoutedEventArgs e)
        {
            string getTasksURL;
            if (Globals.ThisAddIn.myCredentials != null)
            {
                getTasksURL = Properties.Settings.Default.ServerURL + "DisplayAllTasks.displayalltasks?" +
               "access_token=" + Globals.ThisAddIn.myCredentials.AccessToken +
               "&instance_url=https://ap4.salesforce.com&rm_email=" + SelectedMailItem.SenderEmailAddress;

                string tasksResponse = await WebRequestHelper.getResponseAsync(getTasksURL);
                if (tasksResponse != "")
                {
                    AllTasks= JsonConvert.DeserializeObject<Tasks>(tasksResponse);
                    if (AllTasks.Number_of_records==0)
                    {
                        //MessageBox.Show("No Tasks Found");
                        ShowNoTasksMessage = System.Windows.Visibility.Visible;
                        HideTasksList = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        ShowNoTasksMessage = System.Windows.Visibility.Collapsed;
                        HideTasksList = System.Windows.Visibility.Visible;

                    }
                    ShowTasksPane = System.Windows.Visibility.Visible;
                    OnPropertyChanged("AllTasks");
                    OnPropertyChanged("ShowTasksPane");
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ShowTasksPane = System.Windows.Visibility.Hidden;
            OnPropertyChanged("ShowTasksPane");
        }
    }
}
