using System;
using BusinessDays;
using Xunit;

namespace BusinessDaysTest
{
  public class WeekendRuleTest
  {
    [Theory]
    [InlineData(true, "2016-06-27")]
    [InlineData(true, "2016-03-01")]
    [InlineData(false, "2016-06-26")]
    [InlineData(false, "2016-11-12")]
    public void IsBusinessDay(bool expected, string date)
    {
      var rule = new WeekendRule();
      Assert.Equal(expected, rule.CheckDate(DateTime.Parse(date)));
    }
  }
}
