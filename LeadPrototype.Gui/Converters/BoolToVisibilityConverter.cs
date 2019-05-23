using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReportGenerator.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (parameter is string s && s == "Inverse")
                return value != null && (bool)value ? Visibility.Collapsed : Visibility.Visible;
            return value != null && (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}