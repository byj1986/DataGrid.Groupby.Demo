using DataGrid.Groupby.Demo.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Input;

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
    }

    private void GridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
    }
}
