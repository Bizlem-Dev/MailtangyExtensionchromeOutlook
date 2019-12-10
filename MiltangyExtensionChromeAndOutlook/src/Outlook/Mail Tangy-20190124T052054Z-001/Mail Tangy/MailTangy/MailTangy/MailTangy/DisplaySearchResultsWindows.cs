using MailTangy.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailTangy
{
    public partial class DisplaySearchResultsWindows : Form
    {
        public System.Windows.Forms.Integration.ElementHost ElHost { get; set; }
        public DisplaySearchResultsWindows(DisplaySearchResultViewModel vm)
        {
            InitializeComponent();
            var elHost = (this.elementHost1.Child as SearchResults);
            ElHost = this.elementHost1;
            elHost.DataContext = vm;
        }

        private void DisplaySearchResultsWindows_Load(object sender, EventArgs e)
        {

        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
