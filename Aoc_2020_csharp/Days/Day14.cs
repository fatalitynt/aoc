using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day14
    {
        public static void Main1()
        {
            
            var input = File.ReadAllLines("Inputs/14.txt");
            long mask0 = 0;
            long mask1 = 0;
            long maskX = 0;
            var mem = new Dictionary<long, long>();
            var xs = new int[0];
            foreach (var line in input)
            {
                var parts = line.Split(" = ");
                if (parts[0] == "mask")
                {
                    xs = parts[1].Reverse()
                        .Select((letter, idx) => (letter, idx))
                        .Where(x => x.letter == 'X')
                        .Select(x => x.idx)
                        .ToArray();

                    mask0 = Convert.ToInt64(parts[1].Replace("X", "1"), 2);
                    mask1 = Convert.ToInt64(parts[1].Replace("X", "0"), 2);
                    maskX = Convert.ToInt64(parts[1].Replace("0", "1").Replace("X", "0"), 2);
                }
                else
                {
                    var idx = long.Parse(parts[0].Substring(4, parts[0].Length - 5));
                    var val = long.Parse(parts[1]);

                    idx |= mask1;
                    idx &= maskX;
                    var indexes = new List<long>();
                    GetIndexes(idx, xs, 0, indexes);
                    foreach (var index in indexes)
                    {
                        mem[index] = val;
                    }
                }
            }

            Console.WriteLine(mem.Values.Sum());
        }

        private static void GetIndexes(long idx, int[] xs, int xsIdx, List<long> output)
        {
            if (xsIdx == xs.Length)
            {
                output.Add(idx);
                return;
            }
            
            GetIndexes(idx, xs, xsIdx + 1, output);
            idx |= (long)1 << xs[xsIdx];
            GetIndexes(idx, xs, xsIdx + 1, output);
        }

        private static void SetValue(long[][] mem2, long idx, long val)
        {
            var chunk = idx / int.MaxValue;
            var i = idx % int.MaxValue;
            mem2[chunk][i] = val;
        }
    }
}