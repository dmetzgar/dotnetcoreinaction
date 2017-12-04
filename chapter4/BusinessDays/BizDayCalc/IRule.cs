using System;

namespace BizDayCalc
{
    public interface IRule
    {
        bool CheckIsBusinessDay(DateTime date);
    }
}
