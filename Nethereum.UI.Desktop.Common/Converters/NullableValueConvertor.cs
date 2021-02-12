using System;
using Avalonia.Data.Converters;
using Avalonia.Markup;

namespace Nethereum.UI.Desktop.Common.Converters
{
    public class NullableIntConverter : IValueConverter
    {
        private static readonly NullableIntConverter defaultInstance = new NullableIntConverter();

        public static NullableIntConverter Default { get { return defaultInstance; } }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int?)
            {
                int? intValue = (int?)value;
                if (intValue.HasValue)
                {
                    return intValue.Value.ToString();
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                int number;
                if (int.TryParse((string)value, out number))
                {
                    return number;
                }
            }

            return null;
        }
    }
}
