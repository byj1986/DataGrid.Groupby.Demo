using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Converters
{
    public class VisibilityToBoolConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is Visibility v && v == Visibility.Visible;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => value is true ? Visibility.Visible : Visibility.Collapsed;
    }
}
