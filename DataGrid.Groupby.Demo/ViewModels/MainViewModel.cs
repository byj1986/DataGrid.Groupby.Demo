using DataGrid.Groupby.Demo.Attributes;
using DataGrid.Groupby.Demo.Command;
using DataGrid.Groupby.Demo.Models;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Data;
using System.Windows.Input;

namespace DataGrid.Groupby.Demo.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    private ICollectionView? _carsView;
    public ICollectionView? CarsView => _carsView;

    private List<Car> _cars = [];
    private bool _isLoading;
    private string _statusText = "准备加载车辆数据...";

    /// <summary>
    /// 列可见性管理器
    /// </summary>
    public ColumnVisibilityManager<Car> ColumnManager { get; }

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

    public MainViewModel()
    {
        ColumnManager = new ColumnVisibilityManager<Car> { CurrentScenario = "View" };
        ResetVisibilityCommand = new RelayCommand(() => ColumnManager.ResetToDefault());
        HideColumnCommand = new RelayCommand(() => ColumnManager.SetColumnVisibility("Insurance", System.Windows.Visibility.Collapsed));
        ToggleColumnVisibilityCommand = new RelayCommand<string>(headerName =>
        {
            ColumnManager.ToggleColumnVisibility(headerName);
        });
    }

    /// <summary>
    /// 场景切换命令（绑定到RadioButton）
    /// </summary>
    public ICommand ChangeScenarioCommand => new RelayCommand<string>(scenario =>
    {
        ColumnManager.CurrentScenario = scenario;
    });

    /// <summary>
    /// 隐藏 Insurance 列命令
    /// </summary>
    public ICommand HideColumnCommand { get; }

    /// <summary>
    /// 恢复默认可见性命令
    /// </summary>
    public ICommand ResetVisibilityCommand { get; }

    /// <summary>
    /// 切换指定列可见性命令，CommandParameter 传列 Header 字符串
    /// </summary>
    public ICommand ToggleColumnVisibilityCommand { get; }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

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

            _cars = cars;

            var view = new ListCollectionView(_cars);
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Car.Producer)));
            view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Car.CarClass)));
            _carsView = view;

            OnPropertyChanged(nameof(CarsView));
            OnPropertyChanged(nameof(TotalCount));
            StatusText = $"已加载 {TotalCount:N0} 条车辆数据，按厂商和车型分组显示。";
        }
        catch (Exception ex)
        {
            _cars = [];
            _carsView = null;
            OnPropertyChanged(nameof(CarsView));
            OnPropertyChanged(nameof(TotalCount));
            StatusText = $"加载失败：{ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

}
