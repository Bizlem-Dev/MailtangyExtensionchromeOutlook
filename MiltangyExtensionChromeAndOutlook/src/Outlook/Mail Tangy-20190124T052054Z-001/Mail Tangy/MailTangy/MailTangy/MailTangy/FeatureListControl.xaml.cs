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
    /// Interaction logic for FeatureListControl.xaml
    /// </summary>
    public partial class FeatureListControl : UserControl
    {
        public event SelectionChangedEventHandler  FeatureListSelectedValueChanged;

        public object FeatureListSelectedValue
        {
            get { return lbFeatureList.SelectedValue; }
        }

        public FeatureListControl()
        {
            InitializeComponent();

            List<Feature> featureList = new List<Feature>();
            featureList.Add(new Feature() { Name = "Email to Case Email Based", FeatureImagePath = @"Resources/Features/EmailBased.png"});
            featureList.Add(new Feature() { Name = "Email to Case Lead Based", FeatureImagePath = @"Resources/Features/LeadBased.png" });
            featureList.Add(new Feature() { Name = "Email to Case Content Based", FeatureImagePath = @"Resources/Features/ContentBased.png" });
            featureList.Add(new Feature() { Name = "Calendar Share", FeatureImagePath = @"Resources/Features/CalendarShare.png" });
            featureList.Add(new Feature() { Name = "Mail Merge", FeatureImagePath = @"Resources/Features/MailMerge.png" });
            featureList.Add(new Feature() { Name = "Email Tracking", FeatureImagePath = @"Resources/Features/EmailTracking.png" });
            featureList.Add(new Feature() { Name = "Smartphone Templates", FeatureImagePath = @"Resources/Features/SmartPhones.png" });
            featureList.Add(new Feature() { Name = "Summarizer", FeatureImagePath = @"Resources/Features/Summarizer.png" });
            featureList.Add(new Feature() { Name = "Contact Detail Verification", FeatureImagePath = @"Resources/Features/ContactDetail.png" });
            featureList.Add(new Feature() { Name = "Plug-ins for Gmail,Outlook,Office 365", FeatureImagePath = @"Resources/Features/GmailOutlook.png" });
            featureList.Add(new Feature() { Name = "Productivity Meter", FeatureImagePath = @"Resources/Features/ProductivityMeter.png" });
            featureList.Add(new Feature() { Name = "Email Templates", FeatureImagePath = @"Resources/Features/EmailTemplates.png" });
            featureList.Add(new Feature() { Name = "Create SalesForce Records", FeatureImagePath = @"Resources/Features/SalesForceRecords.png" });
            featureList.Add(new Feature() { Name = "Mailbox Inside SFDC", FeatureImagePath = @"Resources/Features/MailboxSFDC.png" });
            featureList.Add(new Feature() { Name = "Lead to Case Content Based", FeatureImagePath = @"Resources/Features/LeadContentBased.png" });
            featureList.Add(new Feature() { Name = "Content Based Auto Case Type Assignment", FeatureImagePath = @"Resources/Features/AutoCaseAssignment.png" });
            featureList.Add(new Feature() { Name = "Sentiment Analysis", FeatureImagePath = @"Resources/Features/SentimentsAnalysis.png" });
            featureList.Add(new Feature() { Name = "Cognitive Autoreply", FeatureImagePath = @"Resources/Features/Autoreply.png" });
            featureList.Add(new Feature() { Name = "Lead Propensity to Buy Score Productivity Meter", FeatureImagePath = @"Resources/Features/ProductivityMeter.png" });
            featureList.Add(new Feature() { Name = "Social Footprint", FeatureImagePath = @"Resources/Features/SocialFootPrint.png" });

            lbFeatureList.ItemsSource = featureList;
            lbFeatureList.SelectionChanged += LbFeatureList_SelectionChanged;
            lbFeatureList.SelectAll();
        }

        private void LbFeatureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FeatureListSelectedValueChanged?.Invoke(sender, e);
        }
    }
}
