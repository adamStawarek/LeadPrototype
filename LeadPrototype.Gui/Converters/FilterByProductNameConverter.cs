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
            var encryptedProductName = (string)values[1];
            var isEncrypted = (bool)values[2];
            var searchString = (string)values[3];
            if (string.IsNullOrEmpty(searchString))
                return Visibility.Visible;
            var name = isEncrypted ? encryptedProductName : productName;
            return name.ToLower().Contains(searchString.ToLower()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}