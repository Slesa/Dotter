using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Dotter.Core.Converters
{
    public class CalculateWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var availableWidth = (double)values[0];

            for (int i = 1; i < values.Count(); i++)
            {
                var width = (double) values[i];
                availableWidth -= width;
            }
            return availableWidth-20;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}