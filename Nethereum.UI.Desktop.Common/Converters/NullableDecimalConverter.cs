using System;
using Avalonia.Data.Converters;

namespace Nethereum.UI.Desktop.Common.Converters
{
    public class NullableDecimalConverter : IValueConverter
    {
        private static readonly NullableDecimalConverter defaultInstance = new NullableDecimalConverter();

        public static NullableDecimalConverter Default { get { return defaultInstance; } }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is decimal?)
            {
                decimal? decimalValue = (decimal?)value;
                if (decimalValue.HasValue)
                {
                    return decimalValue.Value.ToString();
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                decimal number;
                if (decimal.TryParse((string)value, out number))
                {
                    return number;
                }
            }

            return null;
        }
    }
}
