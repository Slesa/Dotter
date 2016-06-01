using System;
using System.Globalization;
using System.Windows.Data;

namespace Dotter.Core.Converters
{
    public class MinusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double toSubtract = 0.0;
            if (parameter is double)
                toSubtract = (double) parameter;
            else
            {
                if ( !(double.TryParse((string) parameter, out toSubtract)) )
                    toSubtract = 0.0;
            }
            return (double) value - toSubtract;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}