using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day05
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/05.txt");
            var ids = new List<int>();
            
            foreach (var str in input)
            {
                var row = BinSearch(0, 127, str, 'B', 0);
                var col = BinSearch(0, 7, str, 'R', 7);
                var id = row * 8 + col;
                ids.Add(id);
            }

            var sorted = ids.OrderBy(x => x).ToArray();
            for (var i = 0; i < sorted.Length - 1; i++)
            {
                if (sorted[i] + 1 != sorted[i + 1])
                {
                    Console.WriteLine(sorted[i] + 1);
                }
            }
        }

        private static int BinSearch(int l, int r, string s, char goRightKey, int idx)
        {
            while (l < r)
            {
                var c = (l + r) / 2;
                if (s[idx++] == goRightKey) l = c + 1;
                else r = c;
            }

            return l;
        }
    }
}