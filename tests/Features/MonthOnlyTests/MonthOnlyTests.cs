using System;
using SolidNetsEasyClient.Models.DTOs;

namespace SolidNetsEasyClient.Tests.Features.MonthOnlyTests;

[UnitTest]
public class MonthOnlyTests
{
    [Fact]
    public void Default_MonthOnly_is_year_1_and_month_1()
    {
        // Arrange + act
        var actual = default(MonthOnly);

        // Assert
        actual.Should().Match<MonthOnly>(x => x.Year == 1 && x.Month == 1);
    }
}
