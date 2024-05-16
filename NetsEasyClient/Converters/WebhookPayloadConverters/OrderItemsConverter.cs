using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Converters.WebhookPayloadConverters;

/// <summary>
/// Custom order items converter
/// </summary>
public class OrderItemsConverter : JsonConverter<IList<Item>>
{
    /// <inheritdoc />
    public override IList<Item>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        List<Item> result = [];

        // current item
        string? reference = null;
        string? name = null;
        double? quantity = null;
        string? unit = null;
        int? unitPrice = null;
        int? taxRate = null;
        string? imageUrl = null;

        // calculated properties
        int? taxAmount = null;
        int? grossTotalAmount = null;
        int? netTotalAmount = null;

        var propertyName = "";

        var parsing = true;
        while (parsing)
        {
            parsing = reader.Read();
            var token = reader.TokenType;
            switch (token)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.EndObject:
                    if (reference is null || name is null || !quantity.HasValue || unit is null || !unitPrice.HasValue || !taxRate.HasValue)
                    {
                        if (result.Count == 0)
                        {
                            parsing = false;
                            break;
                        }
                        else throw new JsonException("Missing properties to deserialize order items");
                    }

                    var item = new Item()
                    {
                        Reference = reference,
                        Name = name,
                        Quantity = quantity.GetValueOrDefault(),
                        Unit = unit,
                        UnitPrice = unitPrice.GetValueOrDefault(),
                        TaxRate = taxRate.GetValueOrDefault(),
                        ImageUrl = imageUrl
                    };
                    var isValid = item.TaxAmount == taxAmount
                                  && item.GrossTotalAmount == grossTotalAmount.GetValueOrDefault()
                                  && item.NetTotalAmount == netTotalAmount.GetValueOrDefault();

                    if (!isValid)
                    {
                        throw new JsonException("Malformed order item. Mismatching calculated properties for the TaxAmount, GrossTotalAmount or NetTotalAmount");
                    }

                    result.Add(item);
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("reference")) reference = text;
                    else if (propertyName.Equals("name")) name = text;
                    else if (propertyName.Equals("unit")) unit = text;
                    else if (propertyName.Equals("imageUrl")) imageUrl = text;
                    break;
                case JsonTokenType.Number:
                    if (propertyName.Equals("quantity"))
                    {
                        quantity = reader.GetDouble();
                    }
                    else if (propertyName.Equals("unitPrice")) unitPrice = reader.GetInt32();
                    else if (propertyName.Equals("taxRate")) taxRate = reader.GetInt32();
                    else if (propertyName.Equals("taxAmount")) taxAmount = reader.GetInt32();
                    else if (propertyName.Equals("grossTotalAmount")) grossTotalAmount = reader.GetInt32();
                    else if (propertyName.Equals("netTotalAmount")) netTotalAmount = reader.GetInt32();
                    break;
                case JsonTokenType.EndArray:
                    if (result.Count > 0) parsing = false;
                    else throw new JsonException("Could not parse webhook order items.");
                    break;
            }
        }

        return result;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, IList<Item> value, JsonSerializerOptions options)
    {
        var omitNull = options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull);
        writer.WriteStartArray();

        foreach (var item in value)
        {
            writer.WriteStartObject();

            writer.WriteString("reference", item.Reference);
            writer.WriteString("name", item.Name);
            writer.WriteNumber("quantity", item.Quantity);
            writer.WriteString("unit", item.Unit);
            writer.WriteNumber("unitPrice", item.UnitPrice);

            if (item.TaxRate.HasValue)
                writer.WriteNumber("taxRate", item.TaxRate.GetValueOrDefault());
            else if (!omitNull)
            {
                writer.WriteNull("taxRate");
            }

            if (item.TaxAmount.HasValue)
            {
                writer.WriteNumber("taxAmount", item.TaxAmount.GetValueOrDefault());
            }
            else if (!omitNull)
            {
                writer.WriteNull("taxAmount");
            }

            writer.WriteNumber("grossTotalAmount", item.GrossTotalAmount);
            writer.WriteNumber("netTotalAmount", item.NetTotalAmount);
            if (!string.IsNullOrWhiteSpace(item.ImageUrl))
            {
                writer.WriteString("imageUrl", item.ImageUrl);
            }
            else if (!omitNull)
            {
                writer.WriteNull("imageUrl");
            }

            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}
