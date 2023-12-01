using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day15
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/15.txt");
            var arr = input[0].Split(',').Select(int.Parse).ToArray();
            var map = new Dictionary<int, int>();
            for (var i = 0; i < arr.Length - 1; i++) map[arr[i]] = i + 1;

            var last = arr.Last();
            for (var i = map.Count + 1; i <= 30000000; i++)
            {
                if (map.ContainsKey(last))
                {
                    var next = i - map[last];
                    map[last] = i;
                    last = next;
                }
                else
                {
                    map[last] = i;
                    last = 0;
                }
                
                if (i == 30000000 - 1)
                    Console.WriteLine(last);
            }
        }
    }
}