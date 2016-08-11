using System;

namespace BusinessDays
{
  public interface IRule
  {
    bool CheckDate(DateTime date);
  }
}
