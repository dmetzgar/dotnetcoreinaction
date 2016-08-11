using System;
using BusinessDays;
using Xunit;

namespace BusinessDaysTest
{
  public class WeekendRuleTest
  {
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
  }
}
