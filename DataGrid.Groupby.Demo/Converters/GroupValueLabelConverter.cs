using System.Globalization;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Converters;

/// <summary>
/// 从 DevExpress GroupValue.Value 字符串（如 "Producer: Audi"）中提取冒号后的原始值 "Audi"。
/// </summary>
public sealed class GroupValueLabelConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            var colonIndex = s.IndexOf(": ", StringComparison.Ordinal);
            if (colonIndex >= 0)
                return s[(colonIndex + 2)..];
        }
        return value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
