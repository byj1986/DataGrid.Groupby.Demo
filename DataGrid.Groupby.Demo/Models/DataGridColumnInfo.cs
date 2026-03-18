using System.ComponentModel;
using System.Windows;

namespace DataGrid.Groupby.Demo.Models
{
    // 列信息辅助类（用于绑定DataGrid列的可见性）
    public class DataGridColumnInfo : INotifyPropertyChanged
    {
        // 属性名（对应Car的属性）
        public string PropertyName { get; set; } = string.Empty;

        // 列头
        public string Header { get; set; } = string.Empty;

        private Visibility _visibility = Visibility.Visible;
        // 当前可见性
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    OnPropertyChanged(nameof(Visibility));
                }
            }
        }

        // 存储不同场景下的可见性配置
        public Dictionary<string, Visibility> ScenarioVisibilities { get; } = [];

        #region INotifyPropertyChanged 实现
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
