using DataGrid.Groupby.Demo.Models;
using DataGrid.Groupby.Demo.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Converters
{
    public class ColumnVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is not BindingProxy bindingProxy)
            {
                return Visibility.Collapsed;
            }
            if (bindingProxy.Data is not MainViewModel viewModel)
            {
                return Visibility.Collapsed;
            }
            return viewModel.ColumnManager.GetColumnVisibility("Insurance");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
