using DataGrid.Groupby.Demo.Attributes;
using DataGrid.Groupby.Demo.Constants;
using System.Text.Json.Serialization;
using System.Windows;

namespace DataGrid.Groupby.Demo.Models;

public sealed class Car
{
    public string Producer { get; set; } = string.Empty;

    [JsonPropertyName("Class")]
    public string CarClass { get; set; } = string.Empty;

    public string Usage { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string EnergyType { get; set; } = string.Empty;

    [ColumnVisibility(Scenarios.Order, Visibility = Visibility.Collapsed)]
    public DateTime LaunchDate { get; set; }

    [ColumnVisibility(Scenarios.View, Visibility = Visibility.Collapsed)]
    public decimal BasePrice { get; set; }

    [ColumnVisibility(Scenarios.View, Visibility = Visibility.Collapsed)]
    public decimal Insurance { get; set; }

    [ColumnVisibility(Scenarios.View, Visibility = Visibility.Collapsed)]
    public decimal Tax { get; set; }

    public decimal TotalPrice { get; set; }
}
