using System;
using System.Collections.Generic;
using SolidNetsEasyClient.Models;
using SolidNetsEasyClient.Models.Requests;

namespace ExampleSite.Models;

public static class PaymentRequestHelper
{
    public static PaymentRequest MinimalPaymentExample(ProductCola product) => new()
    {
        Order = new Order
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
        },
        Checkout = new()
        {
            Url = "https://my.domain/checkout",
            TermsUrl = "https://my.terms.url",
            ReturnUrl = "https://return.to.me"
        },
    };
}
