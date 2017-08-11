using System.Runtime.InteropServices;

internal partial class Interop
{
  internal partial class WindowsPid
  {
    [DllImport("api-ms-win-core-processthreads-l1-1-0.dll")]
    internal extern static uint GetCurrentProcessId();
  }
}
