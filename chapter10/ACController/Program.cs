using System.Diagnostics.Tracing;
using System.Globalization;
using System.Threading;

namespace ACController
{
  class Program
  {
    static void Main()
    {
      var culture = CultureInfo.CreateSpecificCulture("ar-SA");
      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;
      using (var listener = new ConsoleEventListener())
      {
        listener.EnableEvents(Controller.Events, EventLevel.Verbose);
        var controller = new Controller();
        controller.Test();
      }
    }
  }
}
