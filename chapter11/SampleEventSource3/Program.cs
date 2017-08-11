using System;

#if NET45                                             
using System.Diagnostics.Eventing;
#endif

namespace SampleEventSource
{
  public class Program
  {
#if NET45
    private static readonly Guid Provider =
      Guid.Parse("B695E411-F53B-4C72-9F81-2926B2EA233A");
#endif

    public static void Main(string[] args)
    {
#if NET45
      var eventProvider = new EventProvider(Provider);
      eventProvider.WriteMessageEvent("Program started");
#else
      MyEventSource.Instance.ProgramStart();
#endif

      // Do some work

#if NET45
      eventProvider.WriteMessageEvent("Program completed");
      eventProvider.Dispose();
#else
      MyEventSource.Instance.ProgramStop();
#endif
    }
  }
}