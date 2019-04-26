using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReportGenerator.Converters
{
    public class FilterByProductNameConverter:IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var productName = (string)values[0];
            var searchString = (string)values[1];
            if (string.IsNullOrEmpty(searchString))
                return Visibility.Visible;
            return productName.ToLower().Contains(searchString.ToLower()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}