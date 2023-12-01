using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day13
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/13.txt");

            var deps = input[1].Split(',')
                .Select(x => int.TryParse(x, out var val) ? val : -1)
                .Select((x, idx) => (period: (long)x, delay: idx))
                .Where(x => x.period > 0)
                .OrderByDescending(x => x.period)
                .ToArray();
            
            var mlp = deps.Select(x => x.period).Aggregate((a, b) => a * b);
            Console.WriteLine($"mlp: {mlp}");

            long ts = 100000000000000;

            var take = 3;
            while (deps.Take(take).Any(x => (ts + x.delay) % x.period != 0)) ts++;
            var p = deps.Take(take).Select(x => x.period).Aggregate((a, b) => a * b);
            Console.WriteLine($"ts: {ts}");

            while (true)
            {
                if (deps.All(x => (ts + x.delay) % x.period == 0))
                {
                    Console.WriteLine($"Found: {ts}");
                    break;
                    
                }
                ts += p;
            }
        }
    }
}