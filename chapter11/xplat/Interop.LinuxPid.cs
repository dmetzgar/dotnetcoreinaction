using System.Runtime.InteropServices;

internal static partial class Interop
{
  internal static partial class LinuxPid
  {
    [DllImport("System.Native",
       EntryPoint="SystemNative_GetPid")]
    internal static extern int GetPid();
  }
}
