using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MailTangy
{
    public class EnableDisableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double noOfItems = 1;
            double curPage = 1;
            double lastPage = 1;
            if (values[0]!=null)
            {
                if (values[0].ToString() != "")
                {
                    curPage = double.Parse(values[0].ToString());
                }
            }
            
            if (values[1] != null)
            {
                if (values[1].ToString() != "")
                {
                    noOfItems = double.Parse(values[1].ToString().Split(':')[1].Trim());
                    lastPage = Math.Ceiling(noOfItems / 7);
                }
            }

            if (lastPage == curPage)
            {
                return false;
            }
            else
                return true;


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }

    public class EnableDisablePreConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //double noOfItems = double.Parse(parameter.ToString().Split(':')[1]);
            double curPage = 1;
            if (values[0] != null)
            {
                if (values[0].ToString() != "")
                {
                    curPage = double.Parse(values[0].ToString());
                }
            }

                if (curPage == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
           
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
