using System;
using System.Diagnostics.Tracing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ACController
{
  public class Controller : IDisposable
  {
    internal static readonly ACControllerEventSource Events = new ACControllerEventSource();
    readonly LoggerFactory loggerFactory;
    readonly ResourceManagerStringLocalizerFactory resourceFactory;
    readonly Telemetry telemetry;
    readonly LoggerEventListener listener;

    public Controller()
    {
      loggerFactory = new LoggerFactory()
        .AddRobust();
      var locOptions = new LocalizationOptions() { ResourcesPath = "resources" };
      var options = Options.Create<LocalizationOptions>(locOptions);
      resourceFactory = new ResourceManagerStringLocalizerFactory(options, loggerFactory);
      listener = new LoggerEventListener(
        loggerFactory.CreateLogger<Telemetry>());
      listener.EnableEvents(Controller.Events, EventLevel.Verbose);
      telemetry = new Telemetry(); 
    }

    public void Test()
    {
      TempControl.ExhaustAirTemp = 17;
      TempControl.CoolantTemp = 2;
      TempControl.OutsideAirTemp = 26;
      telemetry.LogStatus();
    }

    public void Dispose() =>
      listener.Dispose();
  }
}
