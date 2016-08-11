using System;
using System.Collections.Generic;

namespace BusinessDays
{
  public class BizDayCalc
  {
    private List<IRule> rules = new List<IRule>();

    public void AddRule(IRule rule)
    {
      rules.Add(rule);
    }

    public bool IsBusinessDay(DateTime date)
    {
      foreach (var rule in rules)
        if (!rule.CheckDate(date))
          return false;

      return true;
    }
  }
}
