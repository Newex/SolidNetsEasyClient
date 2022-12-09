using Microsoft.Net.Http.Headers;
using SolidNetsEasyClient.Encryption;
using SolidNetsEasyClient.Models.DTOs.Responses.Payments;
using SolidNetsEasyClient.Models.DTOs.Responses.Webhooks;
using SolidNetsEasyClient.Tests.Tools;

namespace SolidNetsEasyClient.Tests.Encryption;

[UnitTest]
public class HmacSigningTests
{
    [Theory]
    [InlineData("me-and-the-super-secrets-key", "of-references")]
    [InlineData("another_small", "one")]
    [InlineData("aoeuaoeuaoeu", "snatoeuhasoeusaoeuaoeusnthjkxqjkxs")]
    public void Signing_must_result_in_a_signature_that_is_less_than_32_characters(string key, string reference)
    {
        // Arrange
        var order = Fakes.MinimalOrderExample with
        {
            Reference = reference
        };

        // Act
        var signature = order.SignOrder(key);
        var length = signature.Length;

        // Assert
        Assert.True(length < 32);
    }

    [Theory]
    [InlineData("me-and-the-super-secrets-key", "of-references", "Gdwc1UcUHWfYgFDqGZH26pJx7d")]
    [InlineData("another_small", "one", "EdgiDYo8kiDrWa14E0Fw3B8UkR0")]
    [InlineData("aoeuaoeuaoeu", "snatoeuhasoeusaoeuaoeusnthjkxqjkxs", "Z6r02b7mdU5AKNiDeWzWnEqrCXB")]
    public void Validate_signed_order(string key, string reference, string signature)
    {
        // Arrange
        var request = Mocks.HttpRequest("POST", (HeaderNames.Authorization, signature));
        var created = new PaymentCreated
        {
            Data = new()
            {
                Order = new()
                {
                    Reference = reference
                }
            }
        };

        // Act
        var validSignature = request.ValidateOrderReference(created, key);

        // Assert
        validSignature.Should().BeTrue();
    }

    [Fact]
    public void Encoded_string_can_be_validated_using_the_same_key()
    {
        // Arrange
        const string input = "encode_me";
        const string key = "super_secret_keyaoetnhsuathoneusnaoeushaothnseusnhaoeuhtnaoeuhsntaoheuglr.,ysnthhjnkx";
        var encoded = PaymentRequestHmac.Encode(input, key);

        // Act
        var valid = PaymentRequestHmac.Validate(encoded, input, key);

        // Assert
        valid.Should().BeTrue();
    }
}
