namespace DataGrid.Groupby.Demo.ViewModels;

public sealed class GroupSummaryInfo
{
    public GroupSummaryInfo(
        string name,
        int itemCount,
        int level,
        decimal basePrice,
        decimal insurance,
        decimal tax,
        decimal totalPrice)
    {
        Name = name;
        ItemCount = itemCount;
        Level = level;
        BasePrice = basePrice;
        Insurance = insurance;
        Tax = tax;
        TotalPrice = totalPrice;
    }

    public string Name { get; }

    public int ItemCount { get; }

    public int Level { get; }

    public decimal BasePrice { get; }

    public decimal Insurance { get; }

    public decimal Tax { get; }

    public decimal TotalPrice { get; }

    public double IndentWidth => Level * 22d;
}
