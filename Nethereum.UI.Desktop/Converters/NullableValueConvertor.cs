using System;
using Avalonia.Data.Converters;
using Avalonia.Markup;

namespace Nethereum.UI.Desktop.Converters
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
                if (Int32.TryParse((string)value, out number))
                {
                    return number;
                }
            }

            return null;
        }
    }

    public class NullableUInt64Converter : IValueConverter
    {
        private static readonly NullableUInt64Converter defaultInstance = new NullableUInt64Converter();

        public static NullableUInt64Converter Default { get { return defaultInstance; } }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ulong?)
            {
                ulong? typedValue = (ulong?)value;
                if (typedValue.HasValue)
                {
                    return typedValue.Value.ToString();
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                ulong number;
                if (UInt64.TryParse((string)value, out number))
                {
                    return number;
                }
            }

            return null;
        }
    }


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
                if (Decimal.TryParse((string)value, out number))
                {
                    return number;
                }
            }

            return null;
        }
    }
}
