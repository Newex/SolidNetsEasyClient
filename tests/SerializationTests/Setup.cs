namespace SolidNetsEasyClient.Tests.SerializationTests;

public static class Setup
{
    public static string JsonExampleString()
    {
        return /*lang=json,strict*/ @"
            {
            ""order"": {
                ""items"": [
                    {
                        ""reference"": ""string"",
                        ""name"": ""string"",
                        ""quantity"": 0,
                        ""unit"": ""string"",
                        ""unitPrice"": 0,
                        ""taxRate"": 0,
                        ""taxAmount"": 0,
                        ""grossTotalAmount"": 0,
                        ""netTotalAmount"": 0
                    }
                ],
                ""amount"": 0,
                ""currency"": ""string"",
                ""reference"": ""string""
            },
            ""checkout"": {
                ""url"": ""string"",
                ""integrationType"": ""EmbeddedCheckout"",
                ""returnUrl"": ""string"",
                ""cancelUrl"": ""string"",
                ""consumer"": {
                    ""reference"": ""string"",
                    ""email"": ""string"",
                    ""shippingAddress"": {
                        ""addressLine1"": ""string"",
                        ""addressLine2"": ""string"",
                        ""postalCode"": ""string"",
                        ""city"": ""string"",
                        ""country"": ""string""
                    },
                    ""phoneNumber"": {
                        ""prefix"": ""string"",
                        ""number"": ""string""
                    },
                    ""privatePerson"": {
                        ""firstName"": ""string"",
                        ""lastName"": ""string""
                    },
                    ""company"": {
                        ""name"": ""string"",
                        ""contact"": {
                            ""firstName"": ""string"",
                            ""lastName"": ""string""
                        }
                    }
                },
                ""termsUrl"": ""string"",
                ""merchantTermsUrl"": ""string"",
                ""shippingCountries"": [
                    {
                        ""countryCode"": ""string""
                    }
                ],
                ""shipping"": {
                    ""countries"": [
                        {
                            ""countryCode"": ""string""
                        }
                    ],
                    ""merchantHandlesShippingCost"": true,
                    ""enableBillingAddress"": true
                },
                ""consumerType"": {
                    ""default"": ""B2B"",
                    ""supportedTypes"": [
                        ""B2C""
                    ]
                },
                ""charge"": true,
                ""publicDevice"": true,
                ""merchantHandlesConsumerData"": true,
                ""appearance"": {
                    ""displayOptions"": {
                        ""showMerchantName"": true,
                        ""showOrderSummary"": true
                    },
                    ""textOptions"": {
                        ""completePaymentButtonText"": ""string""
                    }
                },
                ""countryCode"": ""string""
            },
            ""merchantNumber"": ""string"",
            ""notifications"": {
                ""webHooks"": [
                    {
                        ""eventName"": ""string"",
                        ""url"": ""string"",
                        ""authorization"": ""string"",
                        ""headers"": null
                    }
                ]
            },
            ""subscription"": {
                ""subscriptionId"": ""d079718b-ff63-45dd-947b-4950c023750f"",
                ""endDate"": ""2019-08-24T14:15:22Z"",
                ""interval"": 0
            },
            ""unscheduledSubscription"": {
                ""create"": true,
                ""unscheduledSubscriptionId"": ""92143051-9e78-40af-a01f-245ccdcd9c03""
            },
            ""paymentMethodsConfiguration"": [
                {
                    ""name"": ""string"",
                    ""enabled"": true
                }
            ],
            ""paymentMethods"": [
                {
                    ""name"": ""string"",
                    ""fee"": {
                        ""reference"": ""string"",
                        ""name"": ""string"",
                        ""quantity"": 0,
                        ""unit"": ""string"",
                        ""unitPrice"": 0,
                        ""taxRate"": 0,
                        ""taxAmount"": 0,
                        ""grossTotalAmount"": 0,
                        ""netTotalAmount"": 0
                    }
                }
            ],
            ""myReference"": ""string""
        }
        ";
    }
}
