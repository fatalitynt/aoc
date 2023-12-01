using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day01
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/01.txt").Select(int.Parse).ToArray();
            var pairs = input.SelectMany(x => input.Select(y => (x + y, y)));
            var dict = new Dictionary<int, int>();
            foreach (var pair in pairs)
            {
                if (!dict.ContainsKey(pair.Item1)) dict[pair.Item1] = pair.Item2;
            }

            foreach (var third in input)
            {
                var sum = 2020 - third;
                if (!dict.TryGetValue(sum, out var first)) continue;
                var second = sum - first;
                Console.WriteLine((long)first * second * third);
                break;
            }

            Console.WriteLine("Finished");
        }
    }
}