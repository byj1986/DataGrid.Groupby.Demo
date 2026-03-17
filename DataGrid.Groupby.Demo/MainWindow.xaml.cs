using DataGrid.Groupby.Demo.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataGrid.Groupby.Demo;

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

        // Cars 已设置到 ItemsSource，等待 DataBind + Render 全部完成后（ContextIdle=3 最低）再操作
        await Dispatcher.InvokeAsync(() =>
        {
            CarDataGrid.CollapseAllGroup();
        }, DispatcherPriority.ContextIdle);
    }

    private void GridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
    }
}
