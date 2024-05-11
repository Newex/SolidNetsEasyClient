using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SolidNetsEasyClient.Models.DTOs;

/// <summary>
/// An item in an order
/// </summary>
public record Item
{
    /// <summary>
    /// The reference number of the item
    /// </summary>
    /// <remarks>
    /// A reference to recognize the product, usually the SKU (stock keeping unit) of the product. For convenience in the case of refunds or modifications of placed orders, the reference should be unique for each variation of a product item (size, color, etc).
    /// </remarks>
    [Required]
    [JsonPropertyName("reference")]
    public string Reference { get; init; } = string.Empty;

    /// <summary>
    /// The name of the product
    /// </summary>
    [Required]
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The quantity of the product
    /// </summary>
    [Required]
    [JsonPropertyName("quantity")]
    public double Quantity { get; init; }

    /// <summary>
    /// The defined unit of measurement for the product
    /// </summary>
    /// <remarks>
    /// For example, pcs, liters or kg
    /// </remarks>
    [Required]
    [JsonPropertyName("unit")]
    public string Unit { get; init; } = string.Empty;

    /// <summary>
    /// The price per unit excluding VAT
    /// </summary>
    [Required]
    [JsonPropertyName("unitPrice")]
    public int UnitPrice { get; init; }

    /// <summary>
    /// The tax/VAT rate in percentage times 100
    /// </summary>
    /// <remarks>
    /// Example the value 2500 corresponds to 25%
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("taxRate")]
    public int? TaxRate { get; init; }

    /// <summary>
    /// The tax/VAT amount
    /// </summary>
    /// <remarks>
    /// Calculated by <see cref="UnitPrice"/> * <see cref="Quantity"/> * <see cref="TaxRate"/> / 10000
    /// </remarks>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("taxAmount")]
    public int? TaxAmount => NetTotalAmount * TaxRate / 10_000;

    /// <summary>
    /// The total amount including VAT
    /// </summary>
    /// <remarks>
    /// Calculated by <see cref="NetTotalAmount"/> + <see cref="TaxAmount"/>
    /// </remarks>
    [Required]
    [JsonPropertyName("grossTotalAmount")]
    public int GrossTotalAmount => NetTotalAmount + (TaxAmount ?? 0);

    /// <summary>
    /// The total amount excluding VAT
    /// </summary>
    /// <remarks>
    /// Calculated by <see cref="UnitPrice"/> * <see cref="Quantity"/>
    /// </remarks>
    [Required]
    [JsonPropertyName("netTotalAmount")]
    public int NetTotalAmount => Convert.ToInt32(Math.Ceiling(UnitPrice * Quantity));

    /// <summary>
    /// Url to image of the product. Meant to be configured before checkout is
    /// completed. Ignored on later operations like charging, refunding etc.
    /// Currently affecting: Riverty Invoice. Supported size: width and height
    /// between 100 pixels and 1280 pixels. Supported formats: gif, jpeg(jpg),
    /// png, webp.
    /// </summary>
    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; init; }
}
