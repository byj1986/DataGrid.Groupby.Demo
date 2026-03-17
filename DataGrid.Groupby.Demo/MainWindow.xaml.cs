using DataGrid.Groupby.Demo.Models;
using DataGrid.Groupby.Demo.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
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

        // Wait for data binding to apply ItemsSource, then configure grouping and sorting
        await Dispatcher.InvokeAsync(() =>
        {
            var view = CollectionViewSource.GetDefaultView(CarDataGrid.ItemsSource);
            if (view == null) return;

            using (view.DeferRefresh())
            {
                view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Car.Producer)));
                view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Car.CarClass)));
                view.SortDescriptions.Add(new SortDescription(nameof(Car.Producer), ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription(nameof(Car.CarClass), ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription(nameof(Car.Model),    ListSortDirection.Ascending));
            }
        }, DispatcherPriority.DataBind);
    }

    private void GridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
    }
}
