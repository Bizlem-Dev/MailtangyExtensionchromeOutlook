using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailTangy
{
    public partial class CreateNewItemWindow : Form
    {
        public CreateNewItemWindow()
        {
            InitializeComponent();
            this.newItemFrame1 = new MailTangy.CreateItemsControls.NewItemFrame();
            this.elementHost1.Child = this.newItemFrame1;
        }

        public CreateNewItemWindow(string WindowName)
        {
            InitializeComponent();
            this.newItemFrame1 = new MailTangy.CreateItemsControls.NewItemFrame(WindowName);
            this.elementHost1.Child = this.newItemFrame1;
            CreateItemsControls.NewCaseControl.onCancelClick += NewCaseControl_onCancelClick;
            CreateItemsControls.NewLeadControl.onCancelClick += NewCaseControl_onCancelClick;
            CreateItemsControls.NewTaskControl.onCancelClick += NewCaseControl_onCancelClick;

        }

        private void NewCaseControl_onCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
