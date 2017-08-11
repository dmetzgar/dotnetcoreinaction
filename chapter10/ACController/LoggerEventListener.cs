using System.Diagnostics.Tracing;
using Microsoft.Extensions.Logging;

namespace ACController
{
  public class LoggerEventListener : EventListener
  {
    private readonly ILogger logger;
    public LoggerEventListener(ILogger logger) =>
      this.logger = logger;

    protected override void OnEventWritten(
      EventWrittenEventArgs eventData) =>
      logger.LogInformation(eventData.Message, eventData.Payload[0]);
  }
}
