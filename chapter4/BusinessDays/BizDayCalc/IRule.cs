using System;

namespace BizDayCalc
{
    public interface IRule
    {
        bool CheckDate(DateTime date);
    }
}
