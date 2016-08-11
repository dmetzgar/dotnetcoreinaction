using System;
using System.Collections.Generic;
using BusinessDays;
using Xunit;

namespace BusinessDaysTest
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

    private BizDayCalc bizDayCalc;

    public USHolidayTest()
    {
      bizDayCalc = new BizDayCalc();
      bizDayCalc.AddRule(new HolidayRule());
    }

    [Theory]
    [MemberData(nameof(Holidays))]
    public void TestHolidays(DateTime date)
    {
      Assert.False(bizDayCalc.IsBusinessDay(date));
    }

    [Theory]
    [InlineData("2016-02-28")]
    [InlineData("2016-01-02")]
    public void TestNonHolidays(string date)
    {
        Assert.True(bizDayCalc.IsBusinessDay(DateTime.Parse(date)));
    }
  }
}
