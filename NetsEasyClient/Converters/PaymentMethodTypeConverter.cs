using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolidNetsEasyClient.Models;
using Types = SolidNetsEasyClient.Models.PaymentMethodConfigurationType.Types;
using static SolidNetsEasyClient.Models.PaymentMethodConfigurationType.Methods;

namespace SolidNetsEasyClient.Converters;

/// <summary>
/// Converter for <see cref="PaymentMethodConfigurationType"/>
/// </summary>
public class PaymentMethodTypeConverter : JsonConverter<PaymentMethodConfigurationType>
{
    /// <inheritdoc />
    public override PaymentMethodConfigurationType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var method = reader.GetString();
        var result = method?.ToLowerInvariant() switch
        {
            "visa" => Visa,
            "mastercard" => MasterCard,
            "dankort" => Dankort,
            "americanexpress" => AmericanExpress,
            "paypal" => PayPal,
            "vipps" => Vipps,
            "mobilepay" => MobilePay,
            "swish" => Swish,
            "arvato" => Arvato,
            "easyinvoice" => EasyInvoice,
            "easycampaign" => EasyCampaign,
            "ratepayinvoice" => RatePayInvoice,
            "ratepayinstallment" => RatePayInstallment,
            "ratepaysepa" => RatePaySepa,
            "sofort" => Sofort,
            "trustly" => Trustly,

            "card" => Types.Card,
            "invoice" => Types.Invoice,
            "installment" => Types.Installment,
            "a2a" => Types.A2A,
            "wallet" => Types.Wallet,
            "prepaid-invoice" => Types.PrepaidInvoice,
            var x when x is not null => new(x, PaymentInstance.Method),
            _ => new(string.Empty, PaymentInstance.Method),
        };

        return result;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, PaymentMethodConfigurationType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
