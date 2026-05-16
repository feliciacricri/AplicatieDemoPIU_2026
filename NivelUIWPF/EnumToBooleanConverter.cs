using System;
using System.Globalization;
using System.Windows.Data;

namespace NivelUIWPF
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bifat = (value as bool?) ?? false;
            if (bifat)
                return parameter;
            return Binding.DoNothing;
        }
    }
}
