using DataGrid.Groupby.Demo.Converters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Helpers
{
    public static class ColumnMenuItemHelper
    {
        private static readonly VisibilityToBoolConverter Converter = new();

        public static readonly DependencyProperty AutoBindCheckedProperty =
            DependencyProperty.RegisterAttached(
                "AutoBindChecked",
                typeof(bool),
                typeof(ColumnMenuItemHelper),
                new PropertyMetadata(false, OnAutoBindCheckedChanged));

        public static bool GetAutoBindChecked(DependencyObject obj) => (bool)obj.GetValue(AutoBindCheckedProperty);
        public static void SetAutoBindChecked(DependencyObject obj, bool value) => obj.SetValue(AutoBindCheckedProperty, value);

        private static void OnAutoBindCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MenuItem menuItem && e.NewValue is true)
                menuItem.Loaded += MenuItem_Loaded;
        }

        private static void MenuItem_Loaded(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            menuItem.Loaded -= MenuItem_Loaded;

            if (menuItem.Header is not string header)
                return;

            var binding = new Binding($"ColumnManager[{header}]")
            {
                Converter = Converter,
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(menuItem, MenuItem.IsCheckedProperty, binding);
        }
    }
}
