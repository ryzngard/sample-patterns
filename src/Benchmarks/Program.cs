using BenchmarkDotNet.Running;

namespace Benchmarks
{
    /// <summary>
    /// See https://benchmarkdotnet.org/
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<Benchmarks>();
        }
    }
}