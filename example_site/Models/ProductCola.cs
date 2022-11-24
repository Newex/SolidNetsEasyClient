using System;
using SolidNetsEasyClient.Models.DTOs.Enums;

namespace ExampleSite.Models;

/// <summary>
/// A simple product example
/// </summary>
public record class ProductCola
{
    /// <summary>
    /// The product SKU / ID
    /// </summary>
    public Guid ID => new("f32be43c-19f8-4546-bb8b-5fcd273d19a1");

    /// <summary>
    /// The product name
    /// </summary>
    public string Name => "Nuka-Cola";

    /// <summary>
    /// The product description
    /// </summary>
    public string Description => "The unique taste of Nuka-Cola is the result of a combination of 17 fruit essences, balanced to enhance the classic cola flavor.";

    /// <summary>
    /// The price per unit
    /// </summary>
    public int Price => 40_00;

    /// <summary>
    /// The unit designation (each)
    /// </summary>
    public string Unit => "ea";

    /// <summary>
    /// The currency
    /// </summary>
    public Currency Currency => Currency.DKK;
}
