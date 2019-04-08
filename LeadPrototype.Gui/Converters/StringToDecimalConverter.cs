using System;
using System.Globalization;
using System.Windows.Data;

namespace ReportGenerator.Converters
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal.TryParse(value?.ToString().Replace('.',','),out var price);
            return price;
        }
    }
}
