using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace DataGrid.Groupby.Demo.Attributes
{
    public class ColumnVisibilityManager<T> : INotifyPropertyChanged
    {
        // 存储各列在不同场景下的默认可见性
        private readonly Dictionary<string, Dictionary<string, Visibility>> _defaultVisibilities;
        // 当前场景
        private string? _currentScenario;
        // 当前列可见性状态
        private readonly Dictionary<string, Visibility> _currentVisibilities;

        public string CurrentScenario
        {
            get => _currentScenario!;
            set
            {
                if (_currentScenario != value)
                {
                    _currentScenario = value;
                    UpdateColumnVisibilities();
                    OnPropertyChanged(nameof(CurrentScenario));
                }
            }
        }

        public ColumnVisibilityManager()
        {
            _defaultVisibilities = [];
            _currentVisibilities = [];

            // 反射获取所有属性的列可见性特性
            LoadDefaultVisibilities();
        }

        /// <summary>
        /// 添加索引器属性，用于WPF绑定
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Visibility this[string columnName]
        {
            get => GetColumnVisibility(columnName);
            set => SetColumnVisibility(columnName, value);
        }

        /// <summary>
        /// 加载默认可见性配置
        /// </summary>
        private void LoadDefaultVisibilities()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                var attributes = prop.GetCustomAttributes<ColumnVisibilityAttribute>();
                foreach (var attr in attributes)
                {
                    if (!_defaultVisibilities.ContainsKey(attr.Scenario))
                    {
                        _defaultVisibilities[attr.Scenario] = [];
                    }
                    _defaultVisibilities[attr.Scenario][prop.Name] = attr.Visibility;
                }

                if (!_currentVisibilities.ContainsKey(prop.Name))
                {
                    _currentVisibilities[prop.Name] = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// 根据当前场景更新列可见性
        /// </summary>
        private void UpdateColumnVisibilities()
        {
            // 先将所有列恢复默认值 Visible
            foreach (var key in _currentVisibilities.Keys)
                _currentVisibilities[key] = Visibility.Visible;
            // 再应用当前场景的差异（仅需标注 Collapsed 的列）
            if (_defaultVisibilities.TryGetValue(_currentScenario!, out var scenarioVisibilities))
            {
                foreach (var propName in scenarioVisibilities.Keys)
                    SetColumnVisibility(propName, scenarioVisibilities[propName]);
            }
            OnPropertyChanged("Item[]");
        }

        /// <summary>
        /// 获取指定列的可见性（供绑定使用）
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Visibility GetColumnVisibility(string columnName)
        {
            if (_currentVisibilities.TryGetValue(columnName, out var visibility))
            {
                return visibility;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// 设置指定列的可见性
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="visibility"></param>
        public void SetColumnVisibility(string columnName, Visibility visibility)
        {
            if (_currentVisibilities.TryGetValue(columnName, out var current) && current != visibility)
            {
                _currentVisibilities[columnName] = visibility;
                OnPropertyChanged("Item[]");
            }
        }

        public void ToggleColumnVisibility(string columnName)
        {
            if (_currentVisibilities.TryGetValue(columnName, out var current))
            {
                SetColumnVisibility(columnName, current == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
            }
        }

        /// <summary>
        /// 恢复默认可见性
        /// </summary>
        public void ResetToDefault()
        {
            UpdateColumnVisibilities();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
