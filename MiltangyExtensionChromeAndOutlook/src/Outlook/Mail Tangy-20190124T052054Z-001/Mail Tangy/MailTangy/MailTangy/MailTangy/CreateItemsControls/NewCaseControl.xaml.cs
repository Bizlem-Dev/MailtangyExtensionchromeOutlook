using MailTangy.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MailTangy.CreateItemsControls
{
    /// <summary>
    /// Interaction logic for NewCaseControl.xaml
    /// </summary>
    public partial class NewCaseControl : UserControl
    {
        public static event EventHandler onCancelClick;

        public NewCaseControl()
        {
            InitializeComponent();
            // Set owner text box to logged in user
            ownerTxtBox.Text = Globals.ThisAddIn.myCredentials.EmailID;
        }
        private string contactEmail="", accountEmail="";
        private async void SearchName_Click(object sender, RoutedEventArgs e)
        {
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

            if(displayResultsWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var context = (displayResultsWindow.ElHost.Child as SearchResults).DataContext;
                var cont = context as DisplaySearchResultViewModel;
                var selectedUser = cont.SelectedUser;
                if (selectedUser==null)
                {
                    selectedUser = cont.searchData[0];
                }
                contactName.Text = selectedUser?.FirstName+" "+selectedUser?.LastName;
                contactEmail = selectedUser?.Email;
                InitializeFields(selectedUser);
            }
        }

        private async void SearchAccount_Click(object sender, RoutedEventArgs e)
        {
            var userData = await searchConstraints("account");
            foreach (var item in userData)
            {
                if (item.FirstName==null)
                {
                    item.FirstName = item.Name;
                }
                
            }
            if (userData.Count==0)
            {
                MessageBox.Show("No Records Found");
                return;
            }
            var dialogVM = new DisplaySearchResultViewModel()
            {
                SearchResultHeader = $"Search Results ({userData.Count})",
                SearchResultSubHeader = "Account",
                searchData = new ObservableCollection<UserData>(userData),
                
            };
            
            var displayResultsWindow = new DisplaySearchResultsWindows(dialogVM);
            if(displayResultsWindow.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                    var context = (displayResultsWindow.ElHost.Child as SearchResults).DataContext;
                    var cont = context as DisplaySearchResultViewModel;
                    var selectedUser = cont.SelectedUser;
                if (selectedUser==null)
                {
                    selectedUser = cont.searchData[0];
                }
                    accountName.Text = selectedUser?.Name;
                    accountEmail = selectedUser?.Email;
                    InitializeFields(selectedUser);
     
            }
        }

        private async Task<List<UserData>> searchConstraints(string searchType)
        {
            var resultUsersData = new List<UserData>();
            string searchConstraintURL = "";
            string fName="", lName = "";
            if (searchType=="contact")
            {
                if (contactName.Text.Contains(' '))
                {
                    fName = contactName.Text.Split(' ')[0];
                    lName = contactName.Text.Split(' ')[1];
                }
                else
                    fName = contactName.Text;
            }
            else
            {
                if (accountName.Text.Contains(' '))
                {
                    fName = accountName.Text.Split(' ')[0];
                    lName = accountName.Text.Split(' ')[1];
                }
                else
                    fName = accountName.Text;
            }
            if (Globals.ThisAddIn.myCredentials != null)
            {
                 searchConstraintURL = Properties.Settings.Default.ServerURL + "SearchConstrainData.searchConstrain?"+
                "access_token=" + Globals.ThisAddIn.myCredentials.AccessToken+
                "&instance_url=https://ap4.salesforce.com&first_name="+ fName +"&last_name="+lName +"&constrain="+ searchType;
                
                string constraintResponse = await WebRequestHelper.getResponseAsync(searchConstraintURL);
                if (constraintResponse != "")
                {
                    if (searchType=="contact")
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

        private void InitializeFields(UserData user)
        {
            if (user.FirstName!=null)
            {
                name = user?.FirstName;
            }
            else
                name = user?.Name;

            phone = user?.Phone;
            // All other fields stay empty
        }

        string name, company, phone, product, reqNumber, potentialLiability, slaViolation;

        private void EmailTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            bool result = ValidatorExtensions.IsValidEmailAddress(EmailTxtBox.Text);
            if (!result)
            {
                emailError.Text = "Please Enter Valid Email";
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (ownerTxtBox.Text=="" || EmailTxtBox.Text==""|| subjectTxtbox.Text=="")
            {
                return ;
            }
            
            string fName="", lName="";
            if (contactName.Text != "")
            {
                fName = contactName.Text.Split(' ')[0];
                lName = contactName.Text.Split(' ')[1];
            }
            
            string saveNewCaseURL = string.Concat(
                            Properties.Settings.Default.ServerURL,
                            "createSfObject.newcase?",
                            "access_token=", Globals.ThisAddIn.myCredentials.AccessToken,
                            $"&instance_url=https://ap4.salesforce.com&owner={ownerTxtBox.Text}",
                            $"&contact={contactName.Text}",
                            $"&account={accountName.Text}",
                            $"&firstname={fName}",
                            $"&lastname={lName}",
                            $"&case_type={cbCaseType.Text}",
                            $"&case_reason={cbReasonType.Text}",
                            $"&status={cbStatusType.Text}",
                            $"&priority={cbPriorityType.Text}",
                            $"&case_origin={cbCaseOrigin.Text}",
                            $"&form_subject={subjectTxtbox.Text}",
                            $"&description={descTxtBox.Text}",
                            $"&contact_email={EmailTxtBox.Text}",
                            $"&searchcontactemail={contactEmail}",
                            $"&searchaccountemail={accountEmail}"
                            );


            
            string SaveCaseResponse = await WebRequestHelper.getResponseAsync(saveNewCaseURL);
            if (SaveCaseResponse != "")
            {
                
                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<CreateCaseApiResponse>(SaveCaseResponse);
                    MessageBox.Show(apiResponse.Message);
                }
                catch (Exception ex ){ MessageBox.Show(ex.Message); }
                finally { onCancelClick?.Invoke(this, EventArgs.Empty); }
            }

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
             onCancelClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
