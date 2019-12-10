using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for OpenCaseControl.xaml
    /// </summary>
    public partial class OpenCaseControl : UserControl,INotifyPropertyChanged
    {
        private DateTime openCaseLastRequestTime=DateTime.Now;
        private DateTime sentimetsLastRequestTime = DateTime.Now;
        private DateTime autoReplyLastRequestTime = DateTime.Now;
        int cCurrentIndex = 0;
        int oCurrentIndex = 0;
        int aCurrentIndex = 0;
        string serverURL = Properties.Settings.Default.ServerURL;
        List<OpenCase> OpenCases = new List<OpenCase>();
        List<OpenCase> AutoRCases = new List<OpenCase>();
        List<ClosedCase> ClosedCases = new List<ClosedCase>();
        #region myProperties
        private List<OpenCase> openCasesSubset;
        public List<OpenCase> OpenCasesSubset
        { get
            {
                return openCasesSubset;
            }
            set
            {
                openCasesSubset = value;
                
                OnPropertyChanged("OpenCasesSubset");
            }
        }
        private int aTotalPages = 1;
        public int ATotalPages
        {
            get
            {
                return aTotalPages;
            }
            set
            {
                aTotalPages = value;

                OnPropertyChanged("ATotalPages");
            }
        }
        private int cTotalPages = 1;
        public int CTotalPages
        {
            get
            {
                return cTotalPages;
            }
            set
            {
                cTotalPages = value;

                OnPropertyChanged("CTotalPages");
            }
        }
        private int oTotalPages = 1;
        public int OTotalPages
        {
            get
            {
                return oTotalPages;
            }
            set
            {
                oTotalPages = value;

                OnPropertyChanged("OTotalPages");
            }
        }
        private int oPageNumber = 1;
        public int OPageNumber
        {
            get
            {
                return oPageNumber;
            }
            set
            {
                oPageNumber = value;

                OnPropertyChanged("OPageNumber");
            }
        }
        private int cPageNumber = 1;
        public int CPageNumber
        {
            get
            {
                return cPageNumber;
            }
            set
            {
                cPageNumber = value;

                OnPropertyChanged("CPageNumber");
            }
        }

        private int aPageNumber = 1;
        public int APageNumber
        {
            get
            {
                return aPageNumber;
            }
            set
            {
                aPageNumber = value;

                OnPropertyChanged("APageNumber");
            }
        }
        private List<ClosedCase> closedCasesSubset;
        public List<ClosedCase> ClosedCasesSubset
        {
            get
            {
                return closedCasesSubset;
            }
            set
            {
                closedCasesSubset = value;
                OnPropertyChanged("ClosedCasesSubset");
            }
        }

        private List<OpenCase> autoRepliedCasesSubset;
        public List<OpenCase> AutoRepliedCasesSubset
        {
            get
            {
                return autoRepliedCasesSubset;
            }
            set
            {
                autoRepliedCasesSubset = value;
                OnPropertyChanged("AutoRepliedCasesSubset");
            }
        }

        private List<Sentiment> sentimentsCollection;
        public List<Sentiment> SentimentsCollection
        {
            get
            {
                return sentimentsCollection;
            }
            set
            {
                sentimentsCollection = value;
                OnPropertyChanged("SentimentsCollection");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }

        
        private string openCasesHeader;

        public string OpenCasesHeader
        {
            get
            {
                
                return openCasesHeader;
             
            }
            set
            {
                openCasesHeader = value;
                OnPropertyChanged("OpenCasesHeader");
            }
        }
        private string closedCasesHeader;
        public string ClosedCasesHeader
        {
            get
            {                
                return closedCasesHeader;
               
            }
            set
            {
                closedCasesHeader = value;
                OnPropertyChanged("ClosedCasesHeader");
            }
        }
        private string autoReplyCasesHeader;
        public string AutoReplyCasesHeader
        {
            get
            {
                
                    return autoReplyCasesHeader;
                
            }
            set
            {
                autoReplyCasesHeader = value;
                OnPropertyChanged("AutoReplyCasesHeader");
            }
        }

        private int happyCount;
        public int HappyCount
        {
            get { return happyCount; }
            set
            {
                happyCount = value;
                OnPropertyChanged("HappyCount");
            }
        }
        private int sadCount;
        public int SadCount
        {
            get { return sadCount; }
            set
            {
                sadCount = value;
                OnPropertyChanged("SadCount");
            }
        }
        private int confusedCount;
        public int ConfusedCount
        {
            get { return confusedCount; }
            set
            {
                confusedCount = value;
                OnPropertyChanged("ConfusedCount");
            }
        }
        
        #endregion

        public OpenCaseControl()
        {
            InitializeComponent();
            if (Globals.ThisAddIn.myCredentials != null)
            {
                FireCaseRequest();
                FireSentimentsRequest();
            }
            else
                ShowLoginWindow();
            this.DataContext = this;
        }

        private void OpenCases_Expanded(object sender, RoutedEventArgs e)
        {
            FireCaseRequest();
        }

        private async void FireSentimentsRequest()
        {   
            string emailID = Globals.ThisAddIn.myCredentials.EmailID;
            string SentimentsURL = "";
            if (Globals.ThisAddIn.myCredentials != null)
            {
                SentimentsURL = serverURL+"SentimentServlet.getdata?email=" + emailID;
                //+ "&access_token=" + Globals.ThisAddIn.myCredentials.AccessToken;
                //string PostData = "{"rm_email":}
                string sentimentsResponse = await WebRequestHelper.getResponseAsync(SentimentsURL);
                if (sentimentsResponse != "")
                {
                    formatJsonSentiments(sentimentsResponse);
                }
                
            }
        }

        private async void FireCaseRequest()
        {
            string emailID = Globals.ThisAddIn.myCredentials.EmailID;
            string OpenCasesURL="";
            if (Globals.ThisAddIn.myCredentials!=null)
            {
                OpenCasesURL = serverURL+"displayCase.allcase?rm_email=" +
                emailID + "&type=all&access_token=" + Globals.ThisAddIn.myCredentials.AccessToken
                +"&instance_url="+Globals.ThisAddIn.myCredentials.InstanceURL;
                
                string tokenResponse = await WebRequestHelper.getResponseAsync(OpenCasesURL);
                if (tokenResponse != "")
                {
                    formatJsonCases(tokenResponse);
                }
                else
                {
                    Globals.ThisAddIn.myCredentials.GetTokenViaRefreshToken(Globals.ThisAddIn.myCredentials.RefreshToken);
                    Globals.ThisAddIn.myCredentials.serializeCredentials(Globals.ThisAddIn.myCredentials);
                    tokenResponse = await WebRequestHelper.getResponseAsync(OpenCasesURL);
                    if (tokenResponse != "")
                    {
                        formatJsonCases(tokenResponse);
                    }
                    else
                    {
                        ShowLoginWindow();
                        if (OpenCases == null)
                        {
                            FireCaseRequest();
                        }
                    }
   
                }
            }
            
        }
        private void ShowLoginWindow()
        {
            LoginCustomPane loginPane = null;
            Microsoft.Office.Tools.CustomTaskPane taskPaneValue;
            foreach (Microsoft.Office.Tools.CustomTaskPane pane in Globals.ThisAddIn.CustomTaskPanes)
            {
                if (pane.Title == "Login | Salesforce")
                {
                    taskPaneValue =  pane;
                }
                
                    
            }
            
            if (loginPane == null)
            {
                loginPane = new LoginCustomPane();
                taskPaneValue = Globals.ThisAddIn.CustomTaskPanes.Add(
                    loginPane, "Login | Salesforce");
                taskPaneValue.DockPosition = Microsoft.Office.Core.MsoCTPDockPosition.msoCTPDockPositionFloating;
                taskPaneValue.DockPositionRestrict = Microsoft.Office.Core.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoChange;
                taskPaneValue.Height = 675;
                taskPaneValue.Width = 540;
                taskPaneValue.Visible = true;
            }
        }

        private void formatJsonCases(string response)
        {
            DisplayCase deserializedDisplayCase = JsonConvert.DeserializeObject<DisplayCase>(response);
            OpenCases = deserializedDisplayCase.OpenCases;
            ClosedCases = deserializedDisplayCase.ClosedCases;
            ClosedCasesHeader = "Closed Cases: " + deserializedDisplayCase.ClosedCasesCount;
            OpenCasesHeader = "Open Cases: " +deserializedDisplayCase.OpenedCasesCount; 
            AutoRCases = deserializedDisplayCase.AutoRepliedCases;
            AutoReplyCasesHeader = "Auto Replied Cases: " + deserializedDisplayCase.AutoRepliedCount;

            OTotalPages =( OpenCases.Count / 7) +1;
            CTotalPages = (ClosedCases.Count / 7 )+1;
            ATotalPages=(AutoRCases.Count/7)+1;

            if (oCurrentIndex + 7 > OpenCases.Count)
            {
                OpenCasesSubset = OpenCases.GetRange(oCurrentIndex, OpenCases.Count - oCurrentIndex);
            }
            else
                OpenCasesSubset = OpenCases.GetRange(oCurrentIndex, 7);

            if (cCurrentIndex + 7 > ClosedCases.Count)
            {
                ClosedCasesSubset = ClosedCases.GetRange(cCurrentIndex, ClosedCases.Count - cCurrentIndex);
            }
            else
                ClosedCasesSubset = ClosedCases.GetRange(cCurrentIndex, 7);

            if (aCurrentIndex + 7 > AutoRCases.Count)
            {
                AutoRepliedCasesSubset = AutoRCases.GetRange(aCurrentIndex, AutoRCases.Count - aCurrentIndex);
            }
            else
                AutoRepliedCasesSubset = AutoRCases.GetRange(aCurrentIndex, 7);
        }
        private void formatJsonSentiments(string response)
        {
            try
            {
                DisplaySentiments Sentiments = JsonConvert.DeserializeObject<DisplaySentiments>(response);
                foreach (Sentiment item in Sentiments.Data)
                {
                    if (item.Sentiment_Output == "neutral" || item.Sentiment_Output == "")
                    {
                        ConfusedCount++;
                    }
                    else if (item.Sentiment_Output == "negative")
                    {
                        SadCount++;
                    }
                    else if (item.Sentiment_Output == "positive")
                    {
                        HappyCount++;
                    }
                }
            }
            catch (Exception ex)
            {

                System.Windows.MessageBox.Show("Failed to Format Sentiments response." + response + " " + ex.Message);
            }
            
           
        }

        private void ClosedCasesExpander_Expanded(object sender, RoutedEventArgs e)
        {
            FireCaseRequest();
        }

        private void AutoRepliedCasesExpander_Expanded(object sender, RoutedEventArgs e)
        {
            
        }

        private void custSentimentIndex_Expanded(object sender, RoutedEventArgs e)
        {
            //FireSentimentsRequest();
        }
        
        private void OPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            OPageNumber--;
            oCurrentIndex -= 7;
            if (oCurrentIndex < 0)
            {
                OpenCasesSubset = OpenCases.GetRange(0, 7);
                oCurrentIndex = 0;
            }
            else
            {
                if (oCurrentIndex > OpenCases.Count)
                {
                    OpenCasesSubset = OpenCases.GetRange(oCurrentIndex, OpenCases.Count - oCurrentIndex);
                }
                else
                    OpenCasesSubset = OpenCases.GetRange(oCurrentIndex, 7);
            }
        }

        private void ONextPage_Click(object sender, RoutedEventArgs e)
        {
            OPageNumber++;
            oCurrentIndex += 7;

            if (oCurrentIndex > OpenCases.Count)
            {
                oCurrentIndex -= 7;
            }
            else
            {
                if (oCurrentIndex + 7 > OpenCases.Count)
                {
                    OpenCasesSubset = OpenCases.GetRange(oCurrentIndex, OpenCases.Count - oCurrentIndex);
                }
                else
                    OpenCasesSubset = OpenCases.GetRange(oCurrentIndex, 7);
            }

        }

        private void CPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            CPageNumber--;
            cCurrentIndex -= 7;
            if (cCurrentIndex < 0)
            {
                ClosedCasesSubset = ClosedCases.GetRange(0, 7);
                cCurrentIndex = 0;
            }
            else
            {
                if (cCurrentIndex > ClosedCases.Count)
                {
                    ClosedCasesSubset = ClosedCases.GetRange(cCurrentIndex, ClosedCases.Count - cCurrentIndex);
                }
                else
                    ClosedCasesSubset = ClosedCases.GetRange(cCurrentIndex, 7);
            }
        }

        private void CNextPage_Click(object sender, RoutedEventArgs e)
        {
            CPageNumber++;
            cCurrentIndex += 7;

            if (cCurrentIndex > ClosedCases.Count)
            {
                cCurrentIndex -= 7;
            }
            else
            {
                if (cCurrentIndex + 7 > ClosedCases.Count)
                {
                    ClosedCasesSubset = ClosedCases.GetRange(cCurrentIndex, ClosedCases.Count - cCurrentIndex);
                }
                else
                    ClosedCasesSubset = ClosedCases.GetRange(cCurrentIndex, 7);
            }
        }

        private void APreviousPage_Click(object sender, RoutedEventArgs e)
        {
            APageNumber--;
            aCurrentIndex -= 7;
            if (aCurrentIndex < 0)
            {
                AutoRepliedCasesSubset = AutoRCases.GetRange(0, 7);
                aCurrentIndex = 0;
            }
            else
            {
                if (cCurrentIndex > AutoRCases.Count)
                {
                    AutoRepliedCasesSubset = AutoRCases.GetRange(aCurrentIndex, AutoRCases.Count - aCurrentIndex);
                }
                else
                    AutoRepliedCasesSubset = AutoRCases.GetRange(aCurrentIndex, 7);
            }
        }

        private void ANextPage_Click(object sender, RoutedEventArgs e)
        {
            APageNumber++;
            aCurrentIndex += 7;

            if (aCurrentIndex > AutoRCases.Count)
            {
                aCurrentIndex -= 7;
            }
            else
            {
                if (aCurrentIndex + 7 > AutoRCases.Count)
                {
                    AutoRepliedCasesSubset = AutoRCases.GetRange(cCurrentIndex, AutoRCases.Count - aCurrentIndex);
                }
                else
                    AutoRepliedCasesSubset = AutoRCases.GetRange(aCurrentIndex, 7);
            }
        }
    }
}
