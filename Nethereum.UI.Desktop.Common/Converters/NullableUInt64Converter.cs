using System;
using Avalonia.Data.Converters;

namespace Nethereum.UI.Desktop.Common.Converters
{
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
                if (ulong.TryParse((string)value, out number))
                {
                    return number;
                }
            }

            return null;
        }
    }
}
