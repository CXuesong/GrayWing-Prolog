using System;

namespace Graywing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Will gather information from Warriors Wiki.");
            var collector = new RelativesCollector();
            collector.CollectAsync("wwdataset.pl").GetAwaiter().GetResult();
        }
    }
}
