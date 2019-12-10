using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy.ViewModels
{
    public class DisplaySearchResultViewModel
    {
        public string SearchResultHeader { get; set; }
        public string SearchResultSubHeader { get; set; }
        public UserData SelectedUser { get; set; }
        public ObservableCollection<UserData> searchData { get; set; } = new ObservableCollection<UserData>();
    }
}
