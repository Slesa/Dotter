using System;
using System.ComponentModel;
using System.Globalization;

namespace Dotter.Core.Helpers
{
    internal static class TypeConverterHelper
    {
        internal static object DoConversionFrom(TypeConverter converter, object value)
        {
            object obj = value;
            try
            {
                if (converter != null)
                {
                    if (value != null)
                    {
                        if (converter.CanConvertFrom(value.GetType()))
                            obj = converter.ConvertFrom((ITypeDescriptorContext)null, CultureInfo.InvariantCulture, value);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!TypeConverterHelper.ShouldEatException(ex))
                    throw;
            }
            return obj;
        }

        private static bool ShouldEatException(Exception e)
        {
            bool flag = false;
            if (e.InnerException != null)
                flag = flag | TypeConverterHelper.ShouldEatException(e.InnerException);
            return flag | e is FormatException;
        }

        internal static TypeConverter GetTypeConverter(Type type)
        {
            return TypeDescriptor.GetConverter(type);
        }
    }
}