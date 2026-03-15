using DataGrid.Groupby.Demo.Models;
using DataGrid.Groupby.Demo.ViewModels;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace DataGrid.Groupby.Demo.Converters;

public sealed class CollectionViewGroupSummaryConverter : IValueConverter
{
    private readonly ConditionalWeakTable<CollectionViewGroup, GroupSummaryInfo> _cache = [];

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not CollectionViewGroup group)
        {
            return Binding.DoNothing;
        }

        return _cache.GetValue(group, CreateSummary);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static GroupSummaryInfo CreateSummary(CollectionViewGroup group)
    {
        var totals = Accumulate(group);
        var name = System.Convert.ToString(group.Name, CultureInfo.InvariantCulture) ?? string.Empty;

        return new GroupSummaryInfo(
            name,
            group.ItemCount,
            DetermineLevel(group),
            totals.BasePrice,
            totals.Insurance,
            totals.Tax,
            totals.TotalPrice);
    }

    private static int DetermineLevel(CollectionViewGroup group)
    {
        if (group.Items.Count == 0)
        {
            return 0;
        }

        return group.Items[0] is CollectionViewGroup ? 0 : 1;
    }

    private static GroupTotals Accumulate(CollectionViewGroup group)
    {
        var totals = new GroupTotals();

        foreach (var item in group.Items)
        {
            switch (item)
            {
                case Car car:
                    totals.BasePrice += car.BasePrice;
                    totals.Insurance += car.Insurance;
                    totals.Tax += car.Tax;
                    totals.TotalPrice += car.TotalPrice;
                    break;

                case CollectionViewGroup childGroup:
                    totals.Add(Accumulate(childGroup));
                    break;
            }
        }

        return totals;
    }

    private sealed class GroupTotals
    {
        public decimal BasePrice { get; set; }

        public decimal Insurance { get; set; }

        public decimal Tax { get; set; }

        public decimal TotalPrice { get; set; }

        public void Add(GroupTotals other)
        {
            BasePrice += other.BasePrice;
            Insurance += other.Insurance;
            Tax += other.Tax;
            TotalPrice += other.TotalPrice;
        }
    }
}
