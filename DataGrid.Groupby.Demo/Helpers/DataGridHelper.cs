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
                if (column.Header is string headerName)
                {
                    BindingOperations.SetBinding(column, DataGridColumn.VisibilityProperty, new Binding($"ColumnManager[{headerName}]") { Source = dataContext });
                }
            }
        }
    }
}
