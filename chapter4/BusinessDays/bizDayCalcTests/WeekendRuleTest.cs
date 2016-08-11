using System;
using System.Collections.Generic;
using BusinessDays;
using Xunit;

namespace BusinessDaysTest
{
  public class WeekendRuleTest2
  {
    public static IEnumerable<object[]> Days {
      get {
        yield return new object[] {true, new DateTime(2016, 6, 27)};
        yield return new object[] {true, new DateTime(2016, 3, 1)};
        yield return new object[] {false, new DateTime(2016, 6, 26)};
        yield return new object[] {false, new DateTime(2016, 11, 12)};
      }
    }

    [Theory]
    [MemberData(nameof(Days))]
    public void TestCheckDate(bool expected, DateTime date)
    {
      var rule = new WeekendRule();
      Assert.Equal(expected, rule.CheckDate(date));
    }
  }
}
