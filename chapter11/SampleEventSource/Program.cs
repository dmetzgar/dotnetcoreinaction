using System;

namespace SampleEventSource
{
  class Program
  {
    static void Main(string[] args)
    {
      MyEventSource.Instance.ProgramStart();

      // Do some work

      MyEventSource.Instance.ProgramStop();
    }
  }
}
