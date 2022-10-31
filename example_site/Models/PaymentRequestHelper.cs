using System;
using System.Collections.Generic;
using SolidNetsEasyClient.Models;

namespace ExampleSite.Models;

public static class PaymentRequestHelper
{
    public static Order MinimalOrderExample(ProductCola product) => new()
    {
        Currency = product.Currency,
        Items = new List<Item>
                {
                    new()
                    {
                        Name = product.Name,
                        Quantity = 1,
                        Unit = product.Unit,
                        UnitPrice = product.Price,
                        Reference = product.ID.ToString()
                    }
                },
        Reference = Guid.NewGuid().ToString()
    };
}