using System.Diagnostics.Tracing;

namespace SampleEventSource
{
  [EventSource(Name = "My Event Source", Guid = "B695E411-F53B-4C72-9F81-2926B2EA233A")]
  public sealed class MyEventSource : EventSource
  {
    public static MyEventSource Instance = new MyEventSource();

    [Event(1,
      Level = EventLevel.Informational,
      Channel = EventChannel.Operational,
      Opcode = EventOpcode.Start,
      Task = Tasks.Program,
      Message = "Program started")]
    public void ProgramStart()
    {
        WriteEvent(1);
    }

    [Event(2,
      Level = EventLevel.Informational,
      Channel = EventChannel.Operational,
      Opcode = EventOpcode.Stop,
      Task = Tasks.Program,
      Message = "Program completed")]
    public void ProgramStop()
    {
        WriteEvent(2);
    }

    public class Tasks
    {
      public const EventTask Program = (EventTask)1;
    }
  }
}
