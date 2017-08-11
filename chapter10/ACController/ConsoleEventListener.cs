using System;
using System.Diagnostics.Tracing;

namespace ACController
{
  public class ConsoleEventListener : EventListener
  {
    protected override void OnEventWritten(
      EventWrittenEventArgs eventData) =>
      Console.WriteLine(eventData.Message, eventData.Payload[0]);
  }
}