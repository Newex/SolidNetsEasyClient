using System;

namespace SolidNetsEasyClient.Tests.SerializationTests.WebHooksTests.ActualResponses;

public static class CleanedResponses
{
    public const string PaymentCreated = "{\"id\":\"484107b70c134d7d8b582354e05cd1f9\",\"merchantId\":123456,\"timestamp\":\"2022-11-12T06:33:21.2310+00:00\",\"event\":\"payment.created\",\"data\":{\"order\":{\"amount\":{\"amount\":4000,\"currency\":\"DKK\"},\"reference\":\"282f89f2-d620-4cc0-91bb-9cce1897f0bc\",\"orderItems\":[{\"grossTotalAmount\":4000,\"name\":\"Nuka-Cola\",\"netTotalAmount\":4000,\"quantity\":1.0,\"reference\":\"f32be43c-19f8-4546-bb8b-5fcd273d19a1\",\"taxRate\":0,\"taxAmount\":0,\"unit\":\"ea\",\"unitPrice\":4000}]},\"paymentId\":\"023e0000636f3df7e30174516bf6aa48\"}}";
    public const string CheckoutCompleted = "{\"id\":\"4a094d529b1647eb857453e92e6874b6\",\"merchantId\":123456,\"timestamp\":\"2022-11-12T07:33:24.3795+01:00\",\"event\":\"payment.checkout.completed\",\"data\":{\"order\":{\"amount\":{\"amount\":4000,\"currency\":\"DKK\"},\"reference\":\"282f89f2-d620-4cc0-91bb-9cce1897f0bc\",\"orderItems\":[{\"grossTotalAmount\":4000,\"name\":\"Nuka-Cola\",\"netTotalAmount\":4000,\"quantity\":1.0,\"reference\":\"f32be43c-19f8-4546-bb8b-5fcd273d19a1\",\"taxRate\":0,\"taxAmount\":0,\"unit\":\"ea\",\"unitPrice\":4000}]},\"consumer\":{\"ip\":\"192.168.0.1\"},\"paymentId\":\"023e0000636f3df7e30174516bf6aa48\"}}";
    public const string ReservationCreated = "{\"id\":\"ffb26a376517427da7236b55e06478d9\",\"merchantId\":123456,\"timestamp\":\"2022-11-12T06:33:24.3795+00:00\",\"event\":\"payment.reservation.created\",\"data\":{\"cardDetails\":{\"creditDebitIndicator\":\"D\",\"expiryMonth\":12,\"expiryYear\":26,\"issuerCountry\":\"NO\",\"truncatedPan\":\"492500******0004\",\"threeDSecure\":{\"authenticationEnrollmentStatus\":\"Y\",\"authenticationStatus\":\"Y\",\"eci\":\"05\"}},\"paymentMethod\":\"Visa\",\"paymentType\":\"CARD\",\"consumer\":{\"ip\":\"192.168.0.1\"},\"reservationReference\":\"211569\",\"reserveId\":\"ffb26a376517427da7236b55e06478d9\",\"amount\":{\"amount\":4000,\"currency\":\"DKK\"},\"paymentId\":\"023e0000636f3df7e30174516bf6aa48\"}}";
    public const string ReservationCreatedV2 = "{\"id\":\"ffb26a376517427da7236b55e06478d9\",\"timestamp\":\"2022-11-12T06:33:24.3795+00:00\",\"merchantNumber\":123456,\"event\":\"payment.reservation.created.v2\",\"data\":{\"paymentMethod\":\"Visa\",\"paymentType\":\"CARD\",\"amount\":{\"amount\":4000,\"currency\":\"DKK\"},\"paymentId\":\"023e0000636f3df7e30174516bf6aa48\"}}";
    public const string ChargeCreated = "{\"id\":\"006b0000636f4149e30174516bf6aa5a\",\"timestamp\":\"2022-11-12T07:46:33.7120+01:00\",\"merchantNumber\":123456,\"event\":\"payment.charge.created.v2\",\"data\":{\"chargeId\":\"006b0000636f4149e30174516bf6aa5a\",\"orderItems\":[{\"grossTotalAmount\":4000,\"name\":\"Nuka-Cola\",\"netTotalAmount\":4000,\"quantity\":1.0,\"reference\":\"f32be43c-19f8-4546-bb8b-5fcd273d19a1\",\"taxRate\":0,\"taxAmount\":0,\"unit\":\"ea\",\"unitPrice\":4000}],\"paymentMethod\":\"Visa\",\"paymentType\":\"CARD\",\"amount\":{\"amount\":4000,\"currency\":\"DKK\"},\"paymentId\":\"023e0000636f3df7e30174516bf6aa48\"}}";
}
