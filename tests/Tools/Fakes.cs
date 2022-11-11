using System;
using System.Collections.Generic;
using Bogus;
using Bogus.DataSets;
using ISO3166;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Models.DTOs.Contacts;
using SolidNetsEasyClient.Models.DTOs.Enums;
using SolidNetsEasyClient.Models.DTOs.Requests.Customers;
using SolidNetsEasyClient.Models.DTOs.Requests.Customers.Addresses;
using SolidNetsEasyClient.Models.DTOs.Requests.Orders;
using SolidNetsEasyClient.Models.DTOs.Requests.Payments;
using SolidNetsEasyClient.Models.DTOs.Requests.Styles;
using SolidNetsEasyClient.Models.DTOs.Requests.Webhooks;
using Company = SolidNetsEasyClient.Models.DTOs.Requests.Customers.Company;
using Person = SolidNetsEasyClient.Models.DTOs.Contacts.Person;

namespace SolidNetsEasyClient.Tests.Tools;

public static class Fakes
{
    public static string[] Units { get; set; } = new[]
    {
        "pcs", "liters", "kg", "hours", "months", "years", "km/h"
    };

    /// <summary>
    /// Payment is NOT necessarily a well defined payment! Due to MerchantHandlesConsumerData is random and the Consumer is random
    /// </summary>
    /// <param name="company">With company</param>
    /// <param name="termsUrl">With terms URL</param>
    /// <param name="webHooks">With web hooks</param>
    /// <param name="paymentConfigurations">With number of payment configurations</param>
    /// <param name="paymentMethods">With number of payment methods</param>
    /// <param name="orderItems">With number of order items</param>
    /// <returns>Returns a random payment</returns>
    internal static PaymentRequest RandomPayment(bool company, bool termsUrl, int webHooks, int paymentConfigurations, int paymentMethods, int orderItems)
    {
        var faker = new Faker<PaymentRequest>();

        faker.RuleFor(f => f.Order, RandomOrder(orderItems));
        faker.RuleFor(f => f.Checkout, f => RandomCheckout(company, termsUrl));
        faker.RuleFor(f => f.MerchantNumber, f => f.Finance.Iban().OrNull(f, 0.80f));
        faker.RuleFor(f => f.Notifications, f => RandomNotification(webHooks).OrNull(f));
        faker.RuleFor(f => f.Subscription, f => RandomSubscription().OrNull(f));
        faker.RuleFor(f => f.UnscheduledSubscription, f => RandomUnscheduledSubscription().OrNull(f));

        faker.RuleFor(f => f.PaymentMethodsConfiguration, f => f.Make(paymentConfigurations, () => RandomPaymentConfiguration()).OrNull(f));
        faker.RuleFor(f => f.PaymentMethods, f => f.Make(paymentMethods, () => RandomPaymentMethod()).OrNull(f));

        faker.RuleFor(f => f.MyReference, f => f.Hacker.Phrase());

        return faker.Generate();
    }

    public static Checkout RandomCheckout(bool company, bool withTerms)
    {
        var faker = new Faker<Checkout>();

        faker.RuleFor(f => f.Url, f => f.Internet.Url().OrNull(f));
        faker.RuleFor(f => f.IntegrationType, f => f.PickRandomWithout<Integration>().OrNull(f));
        faker.RuleFor(f => f.ReturnUrl, f => f.Internet.UrlWithPath("https"));
        faker.RuleFor(f => f.CancelUrl, f => f.Internet.Url().OrNull(f));
        faker.RuleFor(f => f.Consumer, f => RandomConsumer(company).OrNull(f));
        faker.RuleFor(f => f.TermsUrl, f => withTerms ? f.Internet.UrlWithPath("https") : f.Internet.UrlWithPath("https"));
        faker.RuleFor(f => f.MerchantTermsUrl, f => f.Internet.Url().OrNull(f));
        faker.RuleFor(f => f.ShippingCountries, f => f.MakeLazy(2, () => RandomShippingCountry()).OrNull(f));
        faker.RuleFor(f => f.Shipping, f => RandomShipping().OrNull(f));
        faker.RuleFor(f => f.ConsumerType, f => RandomConsumerType().OrNull(f));
        faker.RuleFor(f => f.Charge, f => f.Random.Bool().OrNull(f));
        faker.RuleFor(f => f.PublicDevice, f => f.Random.Bool());
        faker.RuleFor(f => f.MerchantHandlesConsumerData, f => f.Random.Bool().OrNull(f));
        faker.RuleFor(f => f.Appearance, f => RandomCheckoutAppearance().OrNull(f));
        faker.RuleFor(f => f.CountryCode, f => f.Address.CountryCode(Iso3166Format.Alpha3));

        return faker.Generate();
    }

    public static Item RandomItem()
    {
        var faker = new Faker<Item>();

        faker.RuleFor(f => f.Reference, f => f.Commerce.Ean13());
        faker.RuleFor(f => f.Name, f => f.Commerce.ProductName());
        faker.RuleFor(f => f.Quantity, f => f.Random.Double(1, 10));
        faker.RuleFor(f => f.Unit, f => f.PickRandom(Units));
        faker.RuleFor(f => f.UnitPrice, f => f.Random.Int(1, 1000));
        faker.RuleFor(f => f.TaxRate, f => f.Random.Int(0, 10000).OrNull(f));

        return faker.Generate();
    }

    public static Order RandomOrder(int itemCount)
    {
        var faker = new Faker<Order>();

        faker.RuleFor(f => f.Items, f => f.MakeLazy(itemCount, () => RandomItem()));
        faker.RuleFor(f => f.Currency, f => f.Finance.Currency().Code);
        faker.RuleFor(f => f.Reference, f => f.Commerce.Ean8());
        return faker.Generate();
    }

    public static ShippingCountry RandomShippingCountry()
    {
        return new()
        {
            CountryCode = RandomCountryCode()
        };
    }

    public static Shipping RandomShipping()
    {
        var faker = new Faker<Shipping>();

        faker.RuleFor(f => f.Countries, f => f.MakeLazy(5, () => RandomShippingCountry()));
        faker.RuleFor(f => f.MerchantHandlesShippingCost, f => f.Random.Bool());
        faker.RuleFor(f => f.EnableBillingAddress, f => f.Random.Bool());

        return faker.Generate();
    }

    public static ConsumerType RandomConsumerType()
    {
        var faker = new Faker<ConsumerType>();
        // var consumerType = faker.PickRandomParam(ConsumerType.B2B, ConsumerType.B2C);
        faker.RuleFor(f => f.Default, f => f.PickRandom<ConsumerTypeEnum>().OrNull(f));
        faker.RuleFor(f => f.SupportedTypes, f => new List<ConsumerTypeEnum> { ConsumerTypeEnum.B2B }.OrNull(f));
        return faker.Generate();
    }

    public static CheckoutAppearance RandomCheckoutAppearance()
    {
        var faker = new Faker();
        return new()
        {
            DisplayOptions = RandomDisplayOptions().OrNull(faker),
            TextOptions = RandomTextOptions().OrNull(faker)
        };
    }

    public static DisplayOptions RandomDisplayOptions()
    {
        var faker = new Faker<DisplayOptions>();

        faker.RuleFor(f => f.ShowMerchantName, f => f.Random.Bool());
        faker.RuleFor(f => f.ShowOrderSummary, f => f.Random.Bool());

        return faker.Generate();
    }

    public static TextOptions RandomTextOptions()
    {
        var faker = new Faker();
        var text = faker.PickRandomParam(PaymentText.Pay, PaymentText.Purchase, PaymentText.Order, PaymentText.Book, PaymentText.Reserve, PaymentText.Signup, PaymentText.Subscribe, PaymentText.Accept).OrNull(faker);
        return new()
        {
            CompletePaymentButtonText = text
        };
    }

    public static Consumer RandomConsumer(bool company)
    {
        var faker = new Faker<Consumer>();

        faker.RuleFor(f => f.Reference, f => f.Finance.RoutingNumber().OrNull(f));
        faker.RuleFor(f => f.Email, f => f.Internet.Email().OrNull(f));
        faker.RuleFor(f => f.ShippingAddress, f => RandomShippingAddress().OrNull(f));
        faker.RuleFor(f => f.PrivatePerson, f =>
        {
            if (!company)
            {
                return RandomPerson();
            }

            return null;
        });

        faker.RuleFor(f => f.Company, f =>
        {
            if (company)
            {
                return RandomCompany();
            }

            return null;
        });

        return faker.Generate();
    }

    public static ShippingAddress RandomShippingAddress()
    {
        var faker = new Faker<ShippingAddress>();

        faker.RuleFor(f => f.AddressLine1, f => f.Address.StreetAddress().OrNull(f));
        faker.RuleFor(f => f.AddressLine2, f => f.Address.SecondaryAddress().OrNull(f));
        faker.RuleFor(f => f.PostalCode, f => f.Address.ZipCode().OrNull(f));
        faker.RuleFor(f => f.City, f => f.Address.City().OrNull(f));
        faker.RuleFor(f => f.Country, f => f.Address.CountryCode(Iso3166Format.Alpha3).OrNull(f));

        return faker.Generate();
    }

    public static PhoneNumber RandomPhoneNumber()
    {
        var faker = new Faker<PhoneNumber>();

        faker.RuleFor(f => f.Prefix, f => f.Random.Number(999).ToString().OrNull(f));
        faker.RuleFor(f => f.Number, f => f.Phone.PhoneNumber().OrNull(f));

        return faker.Generate();
    }

    public static Person RandomPerson()
    {
        var faker = new Faker<Person>();

        faker.RuleFor(f => f.FirstName, f => f.Person.FirstName.OrNull(f));
        faker.RuleFor(f => f.LastName, f => f.Person.LastName.OrNull(f));

        return faker.Generate();
    }

    public static Company RandomCompany()
    {
        var faker = new Faker<Company>();

        faker.RuleFor(f => f.Name, f => f.Company.CompanyName().OrNull(f));
        faker.RuleFor(f => f.Contact, f => RandomPerson().OrNull(f));

        return faker.Generate();
    }

    public static string RandomCountryCode()
    {
        var faker = new Faker();
        var country = faker.PickRandom(Country.List);
        return country.ThreeLetterCode;
    }

    public static Notification RandomNotification(int hooks)
    {
        var faker = new Faker<Notification>();
        faker.RuleFor(f => f.WebHooks, f => f.MakeLazy(hooks, () => RandomWebHook()));
        return faker.Generate();
    }

    public static WebHook RandomWebHook()
    {
        var faker = new Faker<WebHook>();

        faker.RuleFor(f => f.EventName, f => f.PickRandom(
            EventName.PaymentCreated,
            EventName.ReservationCreated,
            EventName.ReservationFailed,
            EventName.CheckoutCompleted,
            EventName.ChargeCreated,
            EventName.ChargeFailed,
            EventName.RefundInitiated,
            EventName.RefundFailed,
            EventName.RefundCompleted,
            EventName.ReservationCancelled,
            EventName.ReservationCancellationFailed
        ).OrNull(f));

        faker.RuleFor(f => f.Url, f => f.Internet.UrlWithPath("https").OrNull(f));
        faker.RuleFor(f => f.Authorization, f => f.Random.AlphaNumeric(31).OrNull(f));

        return faker.Generate();
    }

    public static Subscription RandomSubscription()
    {
        var faker = new Faker<Subscription>();

        faker.RuleFor(f => f.SubscriptionId, f => f.Random.Guid().OrNull(f));
        faker.RuleFor(f => f.EndDate, f => f.Date.FutureOffset().OrNull(f));
        faker.RuleFor(f => f.Interval, f => f.Random.Int().OrNull(f));

        return faker.Generate();
    }

    public static UnscheduledSubscription RandomUnscheduledSubscription()
    {
        var faker = new Faker<UnscheduledSubscription>();

        faker.RuleFor(f => f.Create, f => f.Random.Bool().OrNull(f));
        faker.RuleFor(f => f.UnscheduledSubscriptionId, f => f.Random.Guid().OrNull(f));

        return faker.Generate();
    }

    public static PaymentTypeMethodName RandomPaymentMethodConfigurationType()
    {
        var faker = new Faker();

        var paymentType = faker.PickRandom<PaymentTypeEnum>();
        var paymentMethod = faker.PickRandom<PaymentMethodEnum>();
        return faker.PickRandomParam<PaymentTypeMethodName>(paymentType, paymentMethod);
    }

    public static PaymentMethodConfiguration RandomPaymentConfiguration()
    {
        var faker = new Faker<PaymentMethodConfiguration>();

        faker.RuleFor(f => f.Name, f => RandomPaymentMethodConfigurationType());
        faker.RuleFor(f => f.Enabled, f => f.Random.Bool());

        return faker.Generate();
    }

    public static PaymentMethod RandomPaymentMethod()
    {
        var faker = new Faker<PaymentMethod>();

        faker.RuleFor(f => f.Fee, _ => RandomItem());

        return faker.Generate();
    }

    public static Order MinimalOrderExample => new()
    {
        Currency = "DKK",
        Items = new List<Item>
                {
                    new()
                    {
                        Name = "Base service",
                        Quantity = 12,
                        Unit = "month",
                        UnitPrice = 80000,
                        Reference = Guid.NewGuid().ToString()
                    }
                },
        Reference = Guid.NewGuid().ToString()
    };
}
