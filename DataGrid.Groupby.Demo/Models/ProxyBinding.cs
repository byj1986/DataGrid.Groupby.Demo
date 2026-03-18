using System.Windows;

namespace DataGrid.Groupby.Demo.Models
{
    /// <summary>
    /// 绑定代理：将ViewModel传递给非可视化树的元素（如DataGridColumn）
    /// </summary>
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        /// <summary>
        /// 要传递的数据源（绑定到ViewModel）
        /// </summary>
        public object Data
        {
            get => GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
    }
}
