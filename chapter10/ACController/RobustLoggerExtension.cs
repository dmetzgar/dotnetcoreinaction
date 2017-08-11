using Microsoft.Extensions.Logging;

namespace ACController
{
  public static class RobustLoggerExtension
  {
    public static LoggerFactory AddRobust(this LoggerFactory factory)
    {
      factory.AddProvider(new RobustLoggerProvider());
      return factory;
    }
  }
}
