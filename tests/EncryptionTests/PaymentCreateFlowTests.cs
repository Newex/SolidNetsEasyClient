using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using SolidNetsEasyClient.Helpers.Encryption;
using SolidNetsEasyClient.Helpers.Encryption.Encodings;
using SolidNetsEasyClient.Helpers.Encryption.Flows;
using SolidNetsEasyClient.Helpers.Invariants;
using SolidNetsEasyClient.Models.DTOs;
using SolidNetsEasyClient.Tests.Tools;

namespace SolidNetsEasyClient.Tests.EncryptionTests;

[UnitTest]
public class PaymentCreateFlowTests
{
    [Fact]
    public void Creating_webhook_auth_for_exactly_same_input_and_key_returns_same_authorization_values()
    {
        var hasher = new MyHasher();
        var rnd = new Random();
        var key = new byte[10];
        rnd.NextBytes(key);

        var payment = new PaymentCreatedInvariant
        {
            Amount = 10,
            OrderItems = new List<Item>
            {
                Fakes.RandomItem() with
                {
                    Quantity = 1,
                    UnitPrice = 10,
                    TaxRate = null
                }
            },
            OrderReference = "ref:#1",
            Nonce = CustomBase62Converter.Encode(RandomNumberGenerator.GetBytes(10))
        };

        var auth1 = PaymentCreatedFlow.CreateAuthorization(hasher, key, payment);
        var auth2 = PaymentCreatedFlow.CreateAuthorization(hasher, key, payment);

        auth1.Should().BeEquivalentTo(auth2);
    }

    [Fact]
    public void Creating_webhook_auth_for_same_payment_but_different_keys_returns_different_authorization_values()
    {
        var hasher = new MyHasher();
        var rnd = new Random();
        var key1 = new byte[10];
        var key2 = new byte[10];
        rnd.NextBytes(key1);
        rnd.NextBytes(key2);

        var payment = new PaymentCreatedInvariant
        {
            Amount = 10,
            OrderItems = new List<Item>
            {
                Fakes.RandomItem() with
                {
                    Quantity = 1,
                    UnitPrice = 10,
                    TaxRate = null
                }
            },
            OrderReference = "ref:#1",
            Nonce = CustomBase62Converter.Encode(RandomNumberGenerator.GetBytes(10))
        };

        var auth1 = PaymentCreatedFlow.CreateAuthorization(hasher, key1, payment);
        var auth2 = PaymentCreatedFlow.CreateAuthorization(hasher, key2, payment);

        auth1.Should().NotBeSameAs(auth2);
    }

    [Fact]
    public void Webhook_authorization_should_have_maximum_length_of_32()
    {
        var hasher = new MyHasher();
        var rnd = new Random();
        var key = new byte[10];
        rnd.NextBytes(key);

        var payment = new PaymentCreatedInvariant
        {
            Amount = 10,
            OrderItems = new List<Item>
            {
                Fakes.RandomItem() with
                {
                    Quantity = 1,
                    UnitPrice = 10,
                    TaxRate = null
                }
            },
            OrderReference = "ref:#1",
            Nonce = CustomBase62Converter.Encode(RandomNumberGenerator.GetBytes(10))
        };
        var (authorization, _) = PaymentCreatedFlow.CreateAuthorization(hasher, key, payment);

        authorization.Length.Should().BeLessThanOrEqualTo(32);
    }

    public class MyHasher : IHasher
    {
        public KeyedHashAlgorithm GetHashAlgorithm()
        {
            return new HMACSHA512();
        }
    }
}
