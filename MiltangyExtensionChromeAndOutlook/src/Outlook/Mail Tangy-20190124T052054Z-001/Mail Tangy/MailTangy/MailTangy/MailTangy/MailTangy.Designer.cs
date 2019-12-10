namespace MailTangy
{
    partial class MailTangyRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MailTangyRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailTangyRibbon));
            this.tabMailTangy = this.Factory.CreateRibbonTab();
            this.mailTangyGroup = this.Factory.CreateRibbonGroup();
            this.btnCases = this.Factory.CreateRibbonButton();
            this.btnSignIn = this.Factory.CreateRibbonButton();
            this.tabMailTangy.SuspendLayout();
            this.mailTangyGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMailTangy
            // 
            this.tabMailTangy.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tabMailTangy.ControlId.OfficeId = "TabMail";
            this.tabMailTangy.Groups.Add(this.mailTangyGroup);
            this.tabMailTangy.Label = "TabMail";
            this.tabMailTangy.Name = "tabMailTangy";
            this.tabMailTangy.Position = this.Factory.RibbonPosition.AfterOfficeId("GroupMailNew");
            // 
            // mailTangyGroup
            // 
            this.mailTangyGroup.Items.Add(this.btnSignIn);
            this.mailTangyGroup.Items.Add(this.btnCases);
            this.mailTangyGroup.Label = "SalesForce";
            this.mailTangyGroup.Name = "mailTangyGroup";
            // 
            // btnCases
            // 
            this.btnCases.Label = "Cases";
            this.btnCases.Name = "btnCases";
            this.btnCases.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnCases_Click);
            // 
            // btnSignIn
            // 
            this.btnSignIn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnSignIn.Enabled = false;
            this.btnSignIn.Image = ((System.Drawing.Image)(resources.GetObject("btnSignIn.Image")));
            this.btnSignIn.Label = "SignIn";
            this.btnSignIn.Name = "btnSignIn";
            this.btnSignIn.ScreenTip = "Click on this button to Signin into Salesforce Account";
            this.btnSignIn.ShowImage = true;
            this.btnSignIn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnSignIn_Click);
            // 
            // MailTangyRibbon
            // 
            this.Name = "MailTangyRibbon";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tabMailTangy);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MailTangy_Load);
            this.tabMailTangy.ResumeLayout(false);
            this.tabMailTangy.PerformLayout();
            this.mailTangyGroup.ResumeLayout(false);
            this.mailTangyGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabMailTangy;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup mailTangyGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSignIn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCases;
    }

    partial class ThisRibbonCollection
    {
        internal MailTangyRibbon MailTangy
        {
            get { return this.GetRibbon<MailTangyRibbon>(); }
        }
    }
}
