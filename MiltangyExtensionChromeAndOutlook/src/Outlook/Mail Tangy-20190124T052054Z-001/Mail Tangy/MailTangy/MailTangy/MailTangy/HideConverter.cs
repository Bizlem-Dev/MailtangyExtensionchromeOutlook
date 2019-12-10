using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MailTangy
{
    class HideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                    if (value.ToString().Length == 0)
                    {
                        return Visibility.Hidden;
                    }
                    else
                        return Visibility.Visible;

                else
                    return System.Windows.Visibility.Collapsed;
            }
            catch (Exception)
            {

                return System.Windows.Visibility.Collapsed;
            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    class HideLabel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value.ToString().Length > 11)

                    return Visibility.Visible;
                else
                    return System.Windows.Visibility.Collapsed;
            }
            catch (Exception)
            {

                return System.Windows.Visibility.Collapsed;
            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    //List<Feeling> feel = (List < Feeling > )value;
                    //string sentiment = feel[0].Polarity;
                    string sentiment = value.ToString();
                    switch (sentiment)
                    {
                        case "":
                            return @"Resources/smile_confused.png";

                        case "positive":
                            return @"Resources/smile_happy.png";

                        case "negative":
                            return @"Resources/smile_sad.png";

                        default:
                            return @"Resources/smile_confused.png";

                    }
                }
                else
                    return "";
            }
            catch (Exception)
            {
                return "";

            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }
    public class HideNavigationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Case> cases = value as List<Case>;
            if (cases != null)
            {
                if (cases.Count > 0)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                    return System.Windows.Visibility.Hidden;
            }
            else
                return System.Windows.Visibility.Hidden;


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ErrorMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string mail = value as string;
            if (mail != null)
            {
                if (ValidatorExtensions.IsValidEmailAddress(mail))
                {
                    return 1;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
