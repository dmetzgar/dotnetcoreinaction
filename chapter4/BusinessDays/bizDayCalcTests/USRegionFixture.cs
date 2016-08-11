using BusinessDays;
using Xunit;

namespace BusinessDaysTest
{
  public class USRegionFixture
  {
    public BizDayCalc Calculator { get; private set; }

    public USRegionFixture()
    {
      Calculator = new BizDayCalc();
      Calculator.AddRule(new WeekendRule());
      Calculator.AddRule(new HolidayRule());
    }
  }

  [CollectionDefinition("US region collection")]
  public class USRegionCollection : ICollectionFixture<USRegionFixture> {}
}
