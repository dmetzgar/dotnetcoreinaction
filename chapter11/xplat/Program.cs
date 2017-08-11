using System;

namespace Xplat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"My PID is {PidUtility.GetProcessId()}");
        }
    }
}
