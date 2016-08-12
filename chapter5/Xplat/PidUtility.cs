using System;
using System.Runtime.InteropServices;

namespace Xplat
{
  public static class PidUtility
  {
    public static int GetProcessId()
    {
      var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
      var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

      if (isWindows)
        return (int)Interop.WindowsPid.GetCurrentProcessId();
      else if (isLinux)
        return Interop.LinuxPid.GetPid();
      else
        throw new PlatformNotSupportedException("Unsupported platform");
    }
  }
}
