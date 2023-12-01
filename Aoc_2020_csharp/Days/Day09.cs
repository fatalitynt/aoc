using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day09
    {
        private const int WinSize = 25;
        
        public static void Main1()
        {
            var arr = File.ReadAllLines("Inputs/09.txt").Select(long.Parse).ToArray();
            var invalid = SolvePart1(arr);

            var pref = new long[arr.Length];
            for (var i = 0; i < arr.Length; i++) pref[i] = arr[i] + (i == 0 ? 0 : pref[i - 1]);

            for (var start = 0; start < arr.Length - 1; start++)
            {
                for (var end = start + 1; end < arr.Length; end++)
                {
                    var sum = pref[end];
                    var excl = start == 0 ? 0 : pref[start - 1];
                    var total = sum - excl;

                    if (total == invalid)
                    {
                        var subset = arr.Skip(start).Take(end - start + 1).OrderBy(x => x).ToArray();
                        Console.WriteLine($"{subset.First()} + {subset.Last()}=");
                        Console.WriteLine($"{subset.First() + subset.Last()}");
                        
                    }
                }
            }
        }

        private static long SolvePart1(long[] arr)
        {
            var dict = new Dictionary<long, int>();
            for (var i = 0; i < WinSize; i++) UpdateByKey(dict, arr[i], 1);

            for (var i = WinSize; i < arr.Length; i++)
            {
                var value = arr[i];
                var isValid = false;

                for (var j = 1; j <= WinSize; j++)
                {
                    var part = arr[i - j];
                    var otherPart = value - part;
                    if (dict.ContainsKey(otherPart))
                    {
                        isValid = dict[otherPart] > (part == otherPart ? 1 : 0);
                        if (isValid) break;
                    }
                }

                if (!isValid)
                    return value;

                UpdateByKey(dict, value, 1);
                UpdateByKey(dict, arr[i - WinSize], -1);
            }
            return -1;
        }

        private static void UpdateByKey(Dictionary<long, int> dict, long key, int delta)
        {
            if (!dict.ContainsKey(key)) dict[key] = 0;
            dict[key] += delta;
        }
    }
}