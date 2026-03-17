using DataGrid.Groupby.Demo.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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

        // Cars 已设置到 ItemsSource，等待 DataBind(8) + Render(7) 全部完成后（ContextIdle=3 最低）再操作
        await Dispatcher.InvokeAsync(() =>
        {
            CarGridControl.ShowLoadingPanel = false;
            CarGridControl.CollapseAllGroups();
        }, DispatcherPriority.ContextIdle);
    }

    private void GridControl_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Disable all right-click interactions inside the grid (menus, etc.).
        e.Handled = true;
    }

    private void TableView_ShowGridMenu(object sender, DevExpress.Xpf.Grid.GridMenuEventArgs e)
    {
        // DevExpress popup menus (header/row/group) are not WPF ContextMenu; disable at source.
        e.Handled = true;
    }

    private void GridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
    }
}
