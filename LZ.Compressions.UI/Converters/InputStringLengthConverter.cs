using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace LZ.Compressions.UI.Converters
{
    internal class InputStringLengthConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return str.Length;

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
