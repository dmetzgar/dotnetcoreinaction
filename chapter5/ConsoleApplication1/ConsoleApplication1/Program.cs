using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://www.bing.com";
            var firstRequest = MeasureRequest(uri);
            var secondRequest = MeasureRequest(uri);
            if (firstRequest.Item1 != HttpStatusCode.OK &&
                secondRequest.Item1 != HttpStatusCode.OK)
            {
                Console.WriteLine("Unexpected status code");
            }
            else
            {
                Console.WriteLine($"First request took {firstRequest.Item2}ms");
                Console.WriteLine($"Second request took {secondRequest.Item2}ms");
            }
            Console.ReadLine();
        }

        static Tuple<HttpStatusCode, long> MeasureRequest(string uri)
        {
            var stopwatch = new Stopwatch();
            var request = WebRequest.Create(uri);
            request.Method = "GET";
            stopwatch.Start();
            using (var response = request.GetResponse()
                   as HttpWebResponse)
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    reader.ReadToEnd();
                    stopwatch.Stop();
                }

                return new Tuple<HttpStatusCode, long>(
                    response.StatusCode,
                    stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
