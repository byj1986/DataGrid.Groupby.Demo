using DataGrid.Groupby.Demo.Attributes;
using DataGrid.Groupby.Demo.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Converters
{
    public class HeaderVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is ColumnVisibilityManager<Car> manager && values[1] is string header)
                return manager[header];
            return Visibility.Collapsed;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
