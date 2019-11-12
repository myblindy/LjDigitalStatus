using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LjDigitalStatus
{
    class DIBackgroundBrushConverter : IValueConverter
    {
        static readonly SolidColorBrush OnBrush = new SolidColorBrush(Colors.LimeGreen);
        static readonly SolidColorBrush OffBrush = new SolidColorBrush(Colors.DarkRed);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b ? b ? OnBrush : OffBrush : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    class DIForegroundConverter : IValueConverter
    {
        static readonly SolidColorBrush OnBrush = new SolidColorBrush(Colors.Black);
        static readonly SolidColorBrush OffBrush = new SolidColorBrush(Colors.White);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is bool b ? b ? OnBrush : OffBrush : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }

    class BoolRadioConverter : IValueConverter
    {
        public bool Inverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            return Inverse ? !boolValue : boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = (bool)value;

            if (!boolValue)
            {
                // We only care when the user clicks a radio button to select it.
                return null;
            }

            return !Inverse;
        }
    }
}
