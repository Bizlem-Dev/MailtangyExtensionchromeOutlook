using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MailTangy
{
    class ShowLeadCaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value.ToString()=="NO CASE NO LEAD")

                    return Visibility.Collapsed;
                else
                {
                    if (value.ToString()=="LEAD"&&parameter.ToString()== "LeadWindow")
                    {
                        return Visibility.Visible;
                    }
                    
                    else if (value.ToString() == "CASE" && parameter.ToString() == "CaseWindow")
                    {
                        return Visibility.Visible;
                    }
                    else
                        return Visibility.Hidden;
                }
                
            }
            catch (Exception)
            {

                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
