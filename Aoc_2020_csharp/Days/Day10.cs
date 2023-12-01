using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day10
    {
        public static void Main1()
        {
            var a = new List<int> {0};
            a.AddRange(File.ReadAllLines("Inputs/10.txt").Select(int.Parse).OrderBy(x => x));
            a.Add(a.Last() + 3);

            var dp = new long[a.Count];
            dp[0] = 1;

            for (var i = 1; i < a.Count; i++)
            {
                long sum = 0;
                for (var j = 1; j <= 3; j++)
                {
                    if (i - j < 0 || a[i] - a[i - j] > 3) break;
                    sum += dp[i - j];
                }
                dp[i] = sum;
            }

            Console.WriteLine(dp.Last());
        }
    }
}