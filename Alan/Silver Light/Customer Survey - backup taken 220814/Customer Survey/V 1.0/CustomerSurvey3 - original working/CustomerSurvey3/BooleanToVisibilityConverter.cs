using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace CustomerSurvey3
{
    public sealed class BooleanToVisibilityConverter : IValueConverter 
    { 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        { 
            bool flag = false;
            if (value is bool) 
                { flag = (bool)value; }
            else if (value is bool?)
                { bool? nullable = (bool?)value; flag = nullable.HasValue ? nullable.Value : false; }
            return (flag ? Visibility.Visible : Visibility.Collapsed); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }
    } 


}
