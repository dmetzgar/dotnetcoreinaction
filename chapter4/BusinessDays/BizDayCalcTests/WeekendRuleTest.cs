using System;
using System.Collections.Generic;
using BizDayCalc;
using Xunit;

namespace BizDayCalcTests
{
    public class WeekendRuleTest
    {
            public static IEnumerable<object[]> Days {
                get {
                    yield return new object[] {true,  new DateTime(2016,  6, 27)};
                    yield return new object[] {true,  new DateTime(2016,  3,  1)};
                    yield return new object[] {false, new DateTime(2016,  6, 26)};
                    yield return new object[] {false, new DateTime(2016, 11, 12)};
                }
            }

        [Fact]
        public void TestCheckDate()
        {
            var rule = new WeekendRule();
            Assert.True(rule.CheckDate(new DateTime(2016, 6, 27)));
            Assert.False(rule.CheckDate(new DateTime(2016, 6, 26)));
        }

        [Theory]
        [InlineData("2016-06-27")]                          
        [InlineData("2016-03-01")]
        public void IsBusinessDay(string date)              
        {
            var rule = new WeekendRule();
            Assert.True(rule.CheckDate(DateTime.Parse(date)));
        }

        [Theory]
        [InlineData("2016-06-26")]
        [InlineData("2016-11-12")]
        public void IsNotBusinessDay(string date)
        {
            var rule = new WeekendRule();
            Assert.False(rule.CheckDate(DateTime.Parse(date)));
        }

        [Theory]
        [InlineData(true,  "2016-06-27")]
        [InlineData(true,  "2016-03-01")]
        [InlineData(false, "2016-06-26")]
        [InlineData(false, "2016-11-12")]
        public void IsBusinessDayExpected(bool expected, string date)
        {
            var rule = new WeekendRule();
            Assert.Equal(expected, rule.CheckDate(DateTime.Parse(date)));
        }

        [Theory]
        [MemberData(nameof(Days))]
        public void TestCheckDateMemberData(bool expected, DateTime date)
        {
            var rule = new WeekendRule();
            Assert.Equal(expected, rule.CheckDate(date));
        }
    }
}
