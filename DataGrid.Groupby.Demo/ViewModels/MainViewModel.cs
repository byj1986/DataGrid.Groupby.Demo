using DataGrid.Groupby.Demo.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.ComponentModel;

namespace DataGrid.Groupby.Demo.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private ObservableCollection<Car> _cars = [];
    private bool _isLoading;
    private string _statusText = "准备加载车辆数据...";

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<Car> Cars => _cars;

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
            OnPropertyChanged(nameof(Cars));
            OnPropertyChanged(nameof(TotalCount));
            StatusText = $"已加载 {TotalCount:N0} 条车辆数据，按厂商和车型分组显示。";
        }
        catch (Exception ex)
        {
            _cars = [];
            OnPropertyChanged(nameof(Cars));
            OnPropertyChanged(nameof(TotalCount));
            StatusText = $"加载失败：{ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
