using System;

namespace BizDayCalc
{
    public class WeekendRule : IRule
    {
        public bool CheckIsBusinessDay(DateTime date)
        {
            return 
                date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
