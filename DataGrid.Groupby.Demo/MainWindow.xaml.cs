using DataGrid.Groupby.Demo.ViewModels;
using DevExpress.Xpf.Grid;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DataGrid.Groupby.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Loaded -= MainWindow_Loaded;

        var jsonPath = Path.Combine(AppContext.BaseDirectory, "cars.json");
        await _viewModel.LoadAsync(jsonPath);
    }

    private void GridControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (IsColumnHeaderElement(e.OriginalSource))
        {
            e.Handled = true;
        }
    }

    private void GridControl_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Disable all right-click interactions inside the grid (menus, etc.).
        e.Handled = true;
    }

    private void TableView_ShowGridMenu(object sender, GridMenuEventArgs e)
    {
        // DevExpress popup menus (header/row/group) are not WPF ContextMenu; disable at source.
        e.Handled = true;
    }

    private static bool IsColumnHeaderElement(object? source)
    {
        var current = source as DependencyObject;
        while (current is not null)
        {
            if (current.GetType().Name.Contains("ColumnHeader", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            current = VisualTreeHelper.GetParent(current);
        }

        return false;
    }
}