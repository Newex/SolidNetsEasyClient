using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks.Common;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Webhook order payload json converter
/// </summary>
public class WebhookOrderConverter : JsonConverter<WebhookOrder>
{
    /// <inheritdoc />
    public override WebhookOrder? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonToken = reader.TokenType;
        if (jsonToken == JsonTokenType.Null || jsonToken == JsonTokenType.None)
        {
            return null;
        }

        // properties
        WebhookAmount? amount = null;
        string? reference = null;
        List<Item> orderItems = [];

        var propertyName = "";

        var parsing = true;
        while (parsing)
        {
            parsing = reader.Read();
            jsonToken = reader.TokenType;
            switch (jsonToken)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("reference")) reference = text;
                    break;
                case JsonTokenType.StartObject:
                    if (propertyName.Equals("amount"))
                    {
                        amount = GetWebhookAmount(ref reader);
                    }
                    break;
                case JsonTokenType.StartArray:
                    if (propertyName.Equals("orderItems"))
                    {
                        orderItems = GetOrderItems(ref reader);

                        // Stop if amount and reference have been parsed
                        parsing = !(amount is not null && reference is not null);
                    }
                    break;
            }
        }

        if (reference is null || amount is null)
        {
            throw new JsonException("Missing properties to deserialize WebhookOrder");
        }

        return new()
        {
            Reference = reference,
            Amount = amount,
            OrderItems = orderItems
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, WebhookOrder value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    private static WebhookAmount? GetWebhookAmount(ref Utf8JsonReader reader)
    {
        // properties
        int? amount = null;
        string? currencyText = null;
        var propertyName = "";

        var parsing = true;

        while (parsing)
        {
            parsing = reader.Read();
            switch (reader.TokenType)
            {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString()!;
                    break;
                case JsonTokenType.Number:
                    var number = reader.GetInt32();
                    if (propertyName.Equals("amount")) amount = number;
                    break;
                case JsonTokenType.String:
                    var text = reader.GetString()!;
                    if (propertyName.Equals("currency")) currencyText = text;
                    break;
                case JsonTokenType.EndObject:
                    if (!amount.HasValue || currencyText is null)
                    {
                        throw new JsonException("Missing properties to deserialize to WebhookAmount");
                    }

                    parsing = false;
                    break;
            }
        }

        var hasCurrency = Enum.TryParse<Currency>(currencyText, out var currency);
        if (!amount.HasValue || currencyText is null || !hasCurrency)
        {
            throw new JsonException("Missing properties to deserialize to WebhookAmount");
        }

        return new()
        {
            Amount = amount.GetValueOrDefault(),
            Currency = currency
        };
    }

    private static List<Item> GetOrderItems(ref Utf8JsonReader reader)
    {
        List<Item> result = [];

        // current item
        string? reference = null;
        string? name = null;
        double? quantity = null;
        string? unit = null;
        int? unitPrice = null;
        int? taxRate = null;

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
}
