using SolidNetsEasyClient.Encryption;
using SolidNetsEasyClient.Models.WebHooks;
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
    [InlineData("me-and-the-super-secrets-key", "of-references", "AeGoMYhzhBtxTgnfw88nf/CTFZ0=")]
    [InlineData("another_small", "one", "Zpr3EYdbNDWTgy6Huia9DRXMWDI=")]
    [InlineData("aoeuaoeuaoeu", "snatoeuhasoeusaoeuaoeusnthjkxqjkxs", "9hJ034fekfoL7hQ5peDRVjiv1FE=")]
    public void Validate_signed_order(string key, string reference, string signature)
    {
        // Arrange
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
        var validSignature = created.ValidateOrderReference(key, signature);

        // Assert
        validSignature.Should().BeTrue();
    }
}
