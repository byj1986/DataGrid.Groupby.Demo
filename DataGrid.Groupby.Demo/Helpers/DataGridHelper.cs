using System.Windows.Controls;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Helpers
{
    public static class DataGridHelper
    {
        public static void BindingColumnsVisibility(this System.Windows.Controls.DataGrid dataGrid, object dataContext)
        {
            if (dataGrid == null) return;

            foreach (var column in dataGrid.Columns)
            {
                if (column is DataGridTextColumn textColumn
                    && textColumn.Binding is Binding binding)
                {
                    string? path = binding.Path?.Path;
                    string? bindingKey = string.IsNullOrEmpty(path) ? column.Header as string : path;
                    if (string.IsNullOrEmpty(bindingKey))
                    {
                        continue;
                    }
                    BindingOperations.SetBinding(column, DataGridColumn.VisibilityProperty, new Binding($"ColumnManager[{bindingKey}]") { Source = dataContext });
                }
            }
        }
    }
}
