using System;
using System.Text.Json;
using SolidNetsEasyClient.Models.Status;

namespace SolidNetsEasyClient.Tests.SerializationTests;

[UnitTest, Feature("SerializationTests")]
public class DateTimeSerializationTests
{
    [Fact]
    public void Date_time_string_should_return_valid_date_time_object()
    {
        // Arrange
        const string jsonDateTime = @"
        {
            ""dateOfBirth"": ""2019-08-24T14:15:22Z""
        }";

        // Act
        var actual = JsonSerializer.Deserialize<PrivatePersonInfo>(jsonDateTime);

        // Assert 2019-08-24 @ 14:15:22 in UTC (Zulu)
        Assert.Equal(2019, actual!.DateOfBirth!.Value.Year);
        Assert.Equal(8, actual.DateOfBirth.Value.Month);
        Assert.Equal(24, actual.DateOfBirth.Value.Day);
        Assert.Equal(14, actual.DateOfBirth.Value.Hour);
        Assert.Equal(15, actual.DateOfBirth.Value.Minute);
        Assert.Equal(22, actual.DateOfBirth.Value.Second);
        Assert.Equal(0, actual.DateOfBirth.Value.Offset.Hours);
    }

    [Fact]
    public void Date_only_string_yymm_should_return_valid_date_only_object()
    {
        // Arrange
        const string jsonDate = @"
        {
            ""expiryDate"": ""2312""
        }
        ";

        // Act
        var actual = JsonSerializer.Deserialize<CardDetailsInfo>(jsonDate);
        var expected = new DateOnly(2023, 12, 31);

        // Assert
        Assert.Equal(expected, actual!.ExpiryDate);
    }
}