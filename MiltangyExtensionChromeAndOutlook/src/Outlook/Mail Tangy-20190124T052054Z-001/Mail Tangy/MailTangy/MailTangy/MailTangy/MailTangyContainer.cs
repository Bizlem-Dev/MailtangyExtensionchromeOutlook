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
    public partial class MailTangyContainer : UserControl
    {
        public MailTangyContainer()
        {
            InitializeComponent();
            this.mainPane1 = new MailTangy.MainPane();
            this.elementHost2.Child = this.mainPane1;
        }

        public MailTangyContainer(PaneType paneType)
        {
            InitializeComponent();
            this.mainPane1 = new MailTangy.MainPane(paneType);
            this.elementHost2.Child = this.mainPane1;
        }

        //public MailTangyContainer(PaneType paneType,Microsoft.Office.Interop.Outlook.MailItem mailItem)
        //{
        //    InitializeComponent();
        //    this.mainPane1 = new MailTangy.MainPane(paneType,mailItem);
        //    this.elementHost2.Child = this.mainPane1;
        //}
    }

    public enum PaneType
    {
        UserSpecificPane,
        FeaturesPane,
        CasesPane,
        ReplyPane
    }
}
