using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace LZ.Compressions.UI.Converters
{
    public class TimeSpanConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan time)
            {
                var strTime = string.Empty;

                if (time.Ticks == 0)
                    return strTime;

                if (time.Minutes != 0)
                    strTime = strTime + time.Minutes + " м.";
                if (time.Seconds != 0)
                    strTime = strTime + time.Seconds + " c.";
                if (time.Milliseconds != 0)
                    strTime = strTime + time.Milliseconds + " мc.";

                if (string.IsNullOrWhiteSpace(strTime))
                    strTime = "Мгновенно";

                return strTime;
            }

            throw new ArgumentException("Value is not TimeSpan");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

        private class TimeInfo
        {

        }
    }
}
