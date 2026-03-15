using System.Text.Json.Serialization;

namespace DataGrid.Groupby.Demo.Models;

public sealed class Car
{
    public string Producer { get; set; } = string.Empty;

    [JsonPropertyName("Class")]
    public string CarClass { get; set; } = string.Empty;

    public string Usage { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public string EnergyType { get; set; } = string.Empty;

    public DateTime LaunchDate { get; set; }

    public decimal BasePrice { get; set; }

    public decimal Insurance { get; set; }

    public decimal Tax { get; set; }

    public decimal TotalPrice { get; set; }
}
