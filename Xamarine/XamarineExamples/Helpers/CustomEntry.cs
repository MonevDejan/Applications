using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SafeSportChat
{
    public class CustomEntry: Entry
    {
    }

    public class isValidBtnBackgroundColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
            {
                return Color.FromRgb(255, 179, 15);
            }
            else
            {
                return Color.FromRgb(210, 212, 215);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class isValidBtnBackgroundColorBlue : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if ((bool)value)
                {
                    return Color.FromRgb(60, 103, 128);
                }
                else
                {
                    return Color.FromRgb(210, 212, 215);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

           
    }





