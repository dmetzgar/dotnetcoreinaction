using System;
using BizDayCalc;
using Xunit;
using Xunit.Abstractions;

namespace BizDayCalcTests
{
    [Collection("US region collection")]
    public class USRegionTest 
    {
        private readonly USRegionFixture fixture;
        private readonly ITestOutputHelper output;

        public USRegionTest(USRegionFixture fixture, ITestOutputHelper output)
        {
            this.fixture = fixture;
            this.output = output;
        }

        [Theory]
        [InlineData("2016-01-01")]
        [InlineData("2016-12-25")]
        [Trait("Holiday", "true")]
        public void TestHolidays(string date)
        {
            output.WriteLine($@"TestHolidays(""{date}"")"); 
            Assert.True(fixture.Calc.IsBusinessDay(DateTime.Parse(date)));
        }

        [Theory]
        [InlineData("2016-02-29")]
        [InlineData("2016-01-04")]
        [Trait("Holiday", "false")]
        public void TestNonHolidays(string date)
        {
            output.WriteLine($@"TestNonHolidays(""{date}"")");
            Assert.True(fixture.Calc.IsBusinessDay(DateTime.Parse(date)));
        }
    }
}
