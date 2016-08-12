using System;

namespace Xplat
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine($"My PID is {PidUtility.GetProcessId()}");
    }
  }
}
