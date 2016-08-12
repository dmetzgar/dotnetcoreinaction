using System;

namespace SampleEventSource
{
  public class Program
  {
    public static void Main(string[] args)
    {
      MyEventSource.Instance.ProgramStart();
      
      // Do some work
      
      MyEventSource.Instance.ProgramStop();
    }
  }
}
