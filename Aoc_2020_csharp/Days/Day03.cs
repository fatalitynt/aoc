using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day03
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/03.txt");
            var dx = new [] {1, 3, 5, 7, 1};
            var dy = new [] {1, 1, 1, 1, 2};

            var min = dx.Select((x, idx) => GetTreeCount(input, x, dy[idx])).ToArray();
            Console.WriteLine(min.Aggregate((a, b) => a * b));
        }

        private static long GetTreeCount(string[] input, int dx, int dy)
        {
            var x = 0;
            var y = 0;
            var count = 0;

            while (y < input.Length)
            {
                if (input[y][x] == '#') count++;
                x += dx;
                y += dy;
                x %= input[0].Length;
            }

            return count;
        }
    }
}