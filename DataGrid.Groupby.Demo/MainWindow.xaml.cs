using DataGrid.Groupby.Demo.ViewModels;
using System.IO;
using System.Windows;

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
}