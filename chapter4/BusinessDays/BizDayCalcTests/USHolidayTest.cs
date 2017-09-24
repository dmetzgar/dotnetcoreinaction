using System;
using System.Collections.Generic;
using BizDayCalc;
using Xunit;

namespace BizDayCalcTests
{
    public class USHolidayTest
    {
        public static IEnumerable<object[]> Holidays {
            get {
                yield return new object[] { new DateTime(2016, 1, 1) };
                yield return new object[] { new DateTime(2016, 7, 4) };
                yield return new object[] { new DateTime(2016, 12, 24) };
                yield return new object[] { new DateTime(2016, 12, 25) };
            }
        }

        private Calculator calculator;

        public USHolidayTest()
        {
            calculator = new Calculator();
            calculator.AddRule(new HolidayRule());
        }

        [Theory]
        [MemberData(nameof(Holidays))]
        [Trait("Holiday", "true")]
        public void TestHolidays(DateTime date)
        {
            Assert.False(calculator.IsBusinessDay(date));
        }

        [Theory]
        [InlineData("2016-02-28")]
        [InlineData("2016-01-02")]
        [Trait("Holiday", "false")]
        public void TestNonHolidays(string date)
        {
            Assert.True(calculator.IsBusinessDay(DateTime.Parse(date)));
        }
    }
}
