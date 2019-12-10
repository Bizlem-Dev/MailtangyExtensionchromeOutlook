using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailTangy.ViewModels
{
    public class MainPaneViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> mAllItems = new ObservableCollection<string>()
        {
            "New", "Case", "Lead", "Task"
        };
        private string mSelectedItem = "New";

        public ObservableCollection<string> AllItems
        {
            get { return mAllItems; }
            set
            {
                mAllItems = value;
                OnPropertyChanged(nameof(AllItems));
            }
        }
        private int mSelectedInd = 0;
        public int SelectedInd
        {
            get { return mSelectedInd; }
            set
            {
                mSelectedInd =  value;
                OnPropertyChanged(nameof(SelectedInd));
            }
        }

        public string SelectedItem
        {
            get { return mSelectedItem; }
            set
            {
                if(value != "New" && mSelectedItem != value)
                {
                    CreateNewItemWindow newItemWin = new CreateNewItemWindow(value);
                    newItemWin.ShowDialog();
                    OnPropertyChanged(nameof(SelectedItem));
                    SelectedInd = 0;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
