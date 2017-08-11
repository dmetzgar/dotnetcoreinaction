using Microsoft.Extensions.Logging;

namespace ACController
{
  public class RobustLoggerProvider : ILoggerProvider
  {
    public ILogger CreateLogger(string categoryName) =>
      new RobustLogger();

    public void Dispose() { }
  }
}
