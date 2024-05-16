using SolidNetsEasyClient.Models.DTOs.Enums;

namespace MinimalAPI.Models;

public record Product
{
    /// <summary>
    /// The product SKU / ID
    /// </summary>
    public Guid ID { get; init; }

    /// <summary>
    /// The product name
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// The product description
    /// </summary>
    public string Description { get; init; } = "";

    /// <summary>
    /// The price per unit
    /// </summary>
    public int Price { get; init; }

    /// <summary>
    /// The unit designation (each)
    /// </summary>
    public string Unit { get; init; } = "ea";

    /// <summary>
    /// The currency
    /// </summary>
    public Currency Currency { get; init; }
}
