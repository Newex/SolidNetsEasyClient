using System;
using System.Collections.Generic;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;

namespace ExampleSite.Models;

public static class PaymentRequestHelper
{
    public static Order MinimalOrderExample(ProductCola product, int quantity) => new()
    {
        Currency = product.Currency,
        Items = new List<Item>
                {
                    new()
                    {
                        Name = product.Name,
                        Quantity = quantity,
                        Unit = product.Unit,
                        UnitPrice = product.Price,
                        Reference = product.ID.ToString()
                    }
                },
        Reference = Guid.NewGuid().ToString()
    };
}