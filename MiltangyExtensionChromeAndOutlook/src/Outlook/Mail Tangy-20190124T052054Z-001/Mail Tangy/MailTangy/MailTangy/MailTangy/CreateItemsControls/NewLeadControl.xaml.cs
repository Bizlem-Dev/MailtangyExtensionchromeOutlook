using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MailTangy.CreateItemsControls
{
    /// <summary>
    /// Interaction logic for NewLeadControl.xaml
    /// </summary>
    public partial class NewLeadControl : UserControl
    {
        public static event EventHandler onCancelClick;

        public NewLeadControl()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            onCancelClick?.Invoke(this, EventArgs.Empty);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AssignedToTextBox.Text=="" || FirstNameTextBox.Text=="" || LastNameTextBox.Text==""||CompanyTextBox.Text==""||
                TitleTextBox.Text==""||EmailTextBox.Text==""||cbStatus.Text=="--NONE--"||AnnualRevenueTextBox.Text=="")
            {
                return;
            }
            string saveNewLeadURL = string.Concat(
                Properties.Settings.Default.ServerURL,
                "createSfObject.newlead?",
                "access_token=", Globals.ThisAddIn.myCredentials.AccessToken,
                $"&instance_url=https://ap4.salesforce.com&assigned_to={AssignedToTextBox.Text}",
                $"&salutaion={cbSalutation.Text}",
                $"&first_name={FirstNameTextBox.Text}",
                $"&last_name={LastNameTextBox.Text}",
                $"&company={CompanyTextBox.Text}",
                $"&title={TitleTextBox.Text}",
                $"&lead_source={cbLeadSource.Text}",
                $"&industry={cbIndustry.Text}",
                $"&annual_revenue={AnnualRevenueTextBox.Text}",
                $"&phone={PhoneTextBox.Text}",
                $"&mobile_phone={MobilePhoneTextBox.Text}",
                $"&fax={FaxTextBox.Text}",
                $"&email={EmailTextBox.Text}",
                $"&website={WebsiteTextBox.Text}",
                $"&status={cbStatus.Text}",
                $"&rating={cbRating.Text}",
                $"&description={DescriptionTextBox.Text}"
                );

            string saveLeadResponse = await WebRequestHelper.getResponseAsync(saveNewLeadURL);

            if (!string.IsNullOrEmpty(saveLeadResponse))
            {
                // Process response
                try
                {
                    var apiResponse = JsonConvert.DeserializeObject<CreateLeadApiResponse>(saveLeadResponse);
                    MessageBox.Show(apiResponse.Message);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally { onCancelClick?.Invoke(this, EventArgs.Empty); }
                
            }

            /*
             *access_token=00D6F000002VXkq!AQQAQOZhO7gwh1wsaRCxnWnEYnVydGn_3jgPvFDgXNfybiE.x_WrGtlTNEvM6AOZwnaS1d_ONMviCh_7eu_dPP4v6LPcOgGS
             * &instance_url=https://ap4.salesforce.com&assigned_to=isha.patel9116@gmail.com
             * &salutaion=
             * &first_name=vikas
             * &last_name=pandit
             * &company=XXXX
             * &title=new lead is created
             * &lead_source=
             * &industry=
             * &annual_revenue=0
             * &phone=
             * &mobile_phone=
             * &fax=
             * &email=vikas.pandit0090@gmail.com
             * &website=
             * &status=Open Not Contacted
             * &rating=
             * &description=testing 
             */
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool result = ValidatorExtensions.IsValidEmailAddress(EmailTextBox.Text);
            if (!result)
            {
                emailError.Text = "Please Enter Valid Email";
            }
        }
    }
}
