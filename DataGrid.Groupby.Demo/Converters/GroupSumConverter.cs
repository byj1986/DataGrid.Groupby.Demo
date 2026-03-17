using DataGrid.Groupby.Demo.Models;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Converters;

/// <summary>
/// Converts CollectionViewGroup.Items to a formatted currency sum of a specified Car property.
/// ConverterParameter: "BasePrice" | "Insurance" | "Tax" | "TotalPrice"
/// </summary>
public sealed class GroupSumConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not IEnumerable items || parameter is not string propName)
            return string.Empty;

        return ComputeSum(items, propName).ToString("C2", culture);
    }

    private static decimal ComputeSum(IEnumerable items, string propName)
    {
        decimal sum = 0;
        foreach (var item in items)
        {
            if (item is Car car)
            {
                sum += propName switch
                {
                    nameof(Car.BasePrice)   => car.BasePrice,
                    nameof(Car.Insurance)   => car.Insurance,
                    nameof(Car.Tax)         => car.Tax,
                    nameof(Car.TotalPrice)  => car.TotalPrice,
                    _                       => 0m
                };
            }
            else if (item is CollectionViewGroup group)
            {
                sum += ComputeSum(group.Items, propName);
            }
        }
        return sum;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
