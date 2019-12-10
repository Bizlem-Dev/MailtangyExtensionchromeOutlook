using MailTangy.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MailTangy.CreateItemsControls
{
    /// <summary>
    /// Interaction logic for NewTaskControl.xaml
    /// </summary>
    public partial class NewTaskControl : UserControl
    {
        public static event EventHandler onCancelClick;

        public NewTaskControl()
        {
            InitializeComponent();
        }

        private async void SearchName_Click(object sender, RoutedEventArgs e)
        {
            contactEmail = "";
            var userData = await searchConstraints("contact");
            if (userData.Count == 0)
            {
                MessageBox.Show("No Records Found");
                return;
            }
            var dialogVM = new DisplaySearchResultViewModel()
            {
                SearchResultHeader = $"Search Results ({userData.Count})",
                SearchResultSubHeader = "Contact",
                searchData = new ObservableCollection<UserData>(userData)
            };

            var displayResultsWindow = new DisplaySearchResultsWindows(dialogVM);

            if (displayResultsWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var context = (displayResultsWindow.ElHost.Child as SearchResults).DataContext;
                var cont = context as DisplaySearchResultViewModel;
                var selectedUser = cont.SelectedUser;
                if (selectedUser==null)
                {
                    selectedUser = cont.searchData[0];
                }
                ContactTextBox.Text = selectedUser?.FirstName+" "+selectedUser?.LastName;
                contactEmail = selectedUser?.Email;
            }
        }

        private async void SearchAccount_Click(object sender, RoutedEventArgs e)
        {
            accountEmail = "";
            var userData = await searchConstraints("account");
            foreach (var item in userData)
            {
                if (item.FirstName == null)
                {
                    item.FirstName = item.Name;
                }

            }
            if (userData.Count == 0)
            {
                MessageBox.Show("No Records Found");
                return;
            }
            var dialogVM = new DisplaySearchResultViewModel()
            {
                SearchResultHeader = $"Search Results ({userData.Count})",
                SearchResultSubHeader = "Account",
                searchData = new ObservableCollection<UserData>(userData)
            };

            var displayResultsWindow = new DisplaySearchResultsWindows(dialogVM);
            if (displayResultsWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var context = (displayResultsWindow.ElHost.Child as SearchResults).DataContext;
                var cont = context as DisplaySearchResultViewModel;
                var selectedUser = cont.SelectedUser;
                if (selectedUser==null)
                {
                    selectedUser = cont.searchData[0];
                }
                AccountTextBox.Text = selectedUser?.Name;
                accountEmail = selectedUser?.Email;
            }
        }
        string accountEmail, contactEmail;
        private async Task<List<UserData>> searchConstraints(string searchType)
        {
            var resultUsersData = new List<UserData>();
            string searchConstraintURL = "";
            string fName = "", lName = "";
            if (searchType=="contact")
            {
                if (ContactTextBox.Text.Contains(" "))
                {
                    fName = ContactTextBox.Text.Split(' ')[0];
                    lName = ContactTextBox.Text.Split(' ')[1];
                }
                else
                    fName = ContactTextBox.Text;
            }
            else
            {
                if (AccountTextBox.Text.Contains(" "))
                {
                    fName = AccountTextBox.Text.Split(' ')[0];
                    lName = AccountTextBox.Text.Split(' ')[1];
                }
                else
                    fName = AccountTextBox.Text;
            }
            
            if (Globals.ThisAddIn.myCredentials != null)
            {
                searchConstraintURL = Properties.Settings.Default.ServerURL + "SearchConstrainData.searchConstrain?" +
               "access_token=" + Globals.ThisAddIn.myCredentials.AccessToken +
               "&instance_url=https://ap4.salesforce.com&first_name=" + fName + "&last_name=" + lName + "&constrain=" + searchType;

                string constraintResponse = await WebRequestHelper.getResponseAsync(searchConstraintURL);
                if (constraintResponse != "")
                {
                    if (searchType == "contact")
                    {
                        ContactData searchData = JsonConvert.DeserializeObject<ContactData>(constraintResponse);
                        resultUsersData = searchData.contactData;
                    }
                    if (searchType == "account")
                    {
                        AccountData searchData = JsonConvert.DeserializeObject<AccountData>(constraintResponse);
                        resultUsersData = searchData.accountData;
                    }
                }
            }
            return resultUsersData;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            onCancelClick?.Invoke(this, EventArgs.Empty);
        }

        private async void SaveClick(object sender, RoutedEventArgs e)
        {
            if (cbSubject.Text=="--NONE--"||cbStatus.Text=="--NONE--"||cbPriority.Text== "--NONE--"||CommentsTextBox.Text=="")
            {
                return;
            }
            string saveNewTaskURL = string.Concat(
                Properties.Settings.Default.ServerURL,
                "createSfObject.newtask?",
                $"assigned_to={AssignedToTextBox.Text}",
                $"&Subject={cbSubject.Text}",
                $"&Priority={cbPriority.Text}",
                $"&Status={cbStatus.Text}",
                $"&Name={ContactTextBox.Text}",
                $"&Related_To={AccountTextBox.Text}",
                $"&Comments={CommentsTextBox.Text}",
                $"&access_token={Globals.ThisAddIn.myCredentials.AccessToken}",
                $"&instance_url=https://ap4.salesforce.com",
                $"&searchcontactemail={contactEmail}",
                $"&searchaccountemail={accountEmail}",
                $"&account={AccountTextBox.Text}",
                $"&contact={ContactTextBox.Text}"
                );

            var saveTaskResponse = await WebRequestHelper.getResponseAsync(saveNewTaskURL);

            if (!string.IsNullOrEmpty(saveTaskResponse))
            {
                // Process response
                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<CreateTaskApiResponse>(saveTaskResponse);
                    MessageBox.Show(apiResponse.Message);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally { onCancelClick?.Invoke(this, EventArgs.Empty); }
                
            }

            //createSfObject.newtask
            /*assigned_to=""
             * &Subject=task creating
             * &Priority=High
             * &Status=Completed
             * &Name=""
             * &Related_To=""
             * &Comments=""
             * &access_token=00D6F000002VXkq!AQQAQOZhO7gwh1wsaRCxnWnEYnVydGn_3jgPvFDgXNfybiE.x_WrGtlTNEvM6AOZwnaS1d_ONMviCh_7eu_dPP4v6LPcOgGS
             * &instance_url=https://ap4.salesforce.com
             */
        }
    }
}
