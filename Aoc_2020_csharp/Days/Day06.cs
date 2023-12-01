using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day06
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/06.txt");
            
            var total = string.Join(
                    "@", 
                    input.Select(x => string.IsNullOrEmpty(x) ? "#" : x))
                .Split('#', StringSplitOptions.RemoveEmptyEntries)
                .Select(GetCount)
                .Sum();

            Console.WriteLine(total);
        }

        private static int GetCount(string s)
        {
            var parts = s.Split('@', StringSplitOptions.RemoveEmptyEntries);
            return s
                .Where(x => x != '@')
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count())
                .Count(x => x.Value == parts.Length);
        }
    }
}