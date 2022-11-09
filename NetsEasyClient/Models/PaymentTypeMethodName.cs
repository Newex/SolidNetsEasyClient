using System.Text.Json.Serialization;
using SolidNetsEasyClient.Converters;

namespace SolidNetsEasyClient.Models;

/// <summary>
/// The names of both payment methods and types
/// </summary>
[JsonConverter(typeof(PaymentTypeMethodNameConverter))]
public record struct PaymentTypeMethodName
{
    private readonly string name;

    /// <summary>
    /// Instantiate a new <see cref="PaymentTypeMethodName"/>
    /// </summary>
    /// <param name="paymentType">The payment type</param>
    public PaymentTypeMethodName(PaymentTypeEnum paymentType)
    {
        name = paymentType.GetName();
        IsType = true;
        IsMethod = false;
    }

    /// <summary>
    /// Instantiate a new <see cref="PaymentTypeMethodName"/>
    /// </summary>
    /// <param name="paymentMethod">The payment method</param>
    public PaymentTypeMethodName(PaymentMethodEnum paymentMethod)
    {
        name = paymentMethod.ToString();
        IsType = false;
        IsMethod = true;
    }

    /// <summary>
    /// True if the name is a payment type otherwise false
    /// </summary>
    public bool IsType { get; }

    /// <summary>
    /// True if the name is a payment method otherwise false
    /// </summary>
    public bool IsMethod { get; }

    /// <summary>
    /// Convert a <see cref="PaymentTypeMethodName"/> to a string
    /// </summary>
    /// <param name="payment">The payment type or name</param>
    public static implicit operator string(PaymentTypeMethodName payment) => payment.name;

    /// <summary>
    /// Convert a <see cref="PaymentTypeEnum"/> to a <see cref="PaymentTypeMethodName"/>
    /// </summary>
    /// <param name="paymentType">The payment type</param>
    public static implicit operator PaymentTypeMethodName(PaymentTypeEnum paymentType) => new(paymentType);

    /// <summary>
    /// Convert a <see cref="PaymentMethodEnum"/> to a <see cref="PaymentTypeMethodName"/>
    /// </summary>
    /// <param name="paymentMethod">The payment method</param>
    public static implicit operator PaymentTypeMethodName(PaymentMethodEnum paymentMethod) => new(paymentMethod);
}
