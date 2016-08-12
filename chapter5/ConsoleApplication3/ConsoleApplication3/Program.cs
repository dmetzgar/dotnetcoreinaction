using System;
using System.Diagnostics.Eventing;

namespace ConsoleApplication3
{
    class Program
    {
        private static readonly Guid Provider = Guid.Parse("B695E411-F53B-4C72-9F81-2926B2EA233A");

        static void Main(string[] args)
        {
            var eventProvider = new EventProvider(Provider);
            eventProvider.WriteMessageEvent("Program started");

            // Do some work

            eventProvider.WriteMessageEvent("Program completed");
            eventProvider.Dispose();
        }
    }
}
