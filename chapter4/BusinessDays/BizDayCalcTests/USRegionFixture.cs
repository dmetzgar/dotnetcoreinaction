using BizDayCalc;
using Xunit;

namespace BizDayCalcTests
{
    public class USRegionFixture
    {
        public Calculator Calc { get; private set; }

        public USRegionFixture()
        {
            Calc = new Calculator();
            Calc.AddRule(new WeekendRule());
            Calc.AddRule(new HolidayRule());
        }

        [CollectionDefinition("US region collection")]
        public class USRegionCollection : ICollectionFixture<USRegionFixture>
        {
        }
    }
}
