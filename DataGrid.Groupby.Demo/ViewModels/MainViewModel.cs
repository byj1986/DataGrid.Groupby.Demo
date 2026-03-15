using DataGrid.Groupby.Demo.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private ObservableCollection<Car> _cars = [];
    private ICollectionView? _carsView;
    private bool _isLoading;
    private string _statusText = "准备加载车辆数据...";

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICollectionView? CarsView
    {
        get => _carsView;
        private set
        {
            if (_carsView == value)
            {
                return;
            }

            _carsView = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading == value)
            {
                return;
            }

            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public string StatusText
    {
        get => _statusText;
        private set
        {
            if (_statusText == value)
            {
                return;
            }

            _statusText = value;
            OnPropertyChanged();
        }
    }

    public int TotalCount => _cars.Count;

    public async Task LoadAsync(string jsonPath)
    {
        IsLoading = true;
        StatusText = "正在加载车辆数据...";

        try
        {
            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException("未找到车辆数据文件。", jsonPath);
            }

            var cars = await Task.Run(async () =>
            {
                await using var stream = File.OpenRead(jsonPath);
                var items = await JsonSerializer.DeserializeAsync<List<Car>>(stream, JsonOptions);
                return items ?? [];
            });

            _cars = new ObservableCollection<Car>(cars);
            OnPropertyChanged(nameof(TotalCount));
            BuildCarsView();
            StatusText = $"已加载 {TotalCount:N0} 条车辆数据，按厂商和车型分组显示。";
        }
        catch (Exception ex)
        {
            _cars = [];
            OnPropertyChanged(nameof(TotalCount));
            CarsView = null;
            StatusText = $"加载失败：{ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void BuildCarsView()
    {
        var view = CollectionViewSource.GetDefaultView(_cars);
        view.GroupDescriptions.Clear();
        view.SortDescriptions.Clear();

        view.SortDescriptions.Add(new SortDescription(nameof(Car.Producer), ListSortDirection.Ascending));
        view.SortDescriptions.Add(new SortDescription(nameof(Car.CarClass), ListSortDirection.Ascending));
        view.SortDescriptions.Add(new SortDescription(nameof(Car.Model), ListSortDirection.Ascending));

        view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Car.Producer)));
        view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Car.CarClass)));

        CarsView = view;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
