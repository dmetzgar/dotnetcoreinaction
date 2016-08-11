using System;
using BusinessDays;
using Xunit;

namespace BusinessDaysTest
{
  public class WeekendRuleTest
  {
    [Fact]
    public void TestCheckDate()
    {
      var rule = new WeekendRule();
      Assert.True(rule.CheckDate(new DateTime(2016, 6, 27)));
      Assert.False(rule.CheckDate(new DateTime(2016, 6, 26)));
    }
  }
}
