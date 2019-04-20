using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace ReportGenerator.Converters
{
    public class CorrelationValueToColorConverter : IMultiValueConverter
    {
     
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var correlationValue = (float)values[0];
            var bounds = (float[])values[1];
            if (bounds == null || bounds.Length < 2)
                return new SolidColorBrush(Colors.LightSkyBlue);

            var boundIndex = bounds.Length-1;
            bounds.ToList().ForEach(b =>
            {
                if (b < correlationValue)
                    boundIndex--;
            });
           

            var color = Color.FromRgb(0, (byte)(150+boundIndex * 20), 255);
            var brush = new SolidColorBrush(color);
            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}