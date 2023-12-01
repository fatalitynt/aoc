using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day24
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/24.txt");
            var map = new Dictionary<Point, bool>();
            foreach (var line in input)
            {
                var k = Parse(line).Aggregate(Add);
                if (!map.ContainsKey(k)) map[k] = false;
                map[k] = !map[k];
            }

            for (var i = 0; i < 100; i++)
            {
                var minX = int.MaxValue;
                var maxX = int.MinValue;
                
                var minY = int.MaxValue;
                var maxY = int.MinValue;
                
                foreach (var key in map.Keys)
                {
                    minX = Math.Min(minX, key.X);
                    maxX = Math.Max(maxX, key.X);

                    minY = Math.Min(minY, key.Y);
                    maxY = Math.Max(maxY, key.Y);
                }

                var next = new Dictionary<Point, bool>();
                var adj = Parse("esenewswnw").ToArray();

                for (var x = minX - 1; x <= maxX + 1; x++)
                {
                    for (var y = minY - 1; y <= maxY + 1; y++)
                    {
                        var me = new Point(x, y);
                        var res = adj.Count(p => map.TryGetValue(Add(p, me), out var bl) && bl);
                        var meIsBlack = map.TryGetValue(me, out var bl1) && bl1;
                        if (res == 2 || res == 1 && meIsBlack) next[me] = true;
                    }
                }

                map = next;
            }

            var c = map.Count(x => x.Value);
            Console.WriteLine(c);
        }

        private static Point Add(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);

        private static IEnumerable<Point> Parse(string line)
        {
            var dy = 0;
            foreach (var c in line)
            {
                if (c == 's') dy = 1;
                else if (c == 'n') dy = -1;
                else if (c == 'e')
                {
                    yield return new Point(dy < 0 ? 0 : 1, dy);
                    dy = 0;
                }
                else if (c == 'w')
                {
                    yield return new Point(dy > 0 ? 0 : -1, dy);
                    dy = 0;
                }
            }
        }
    }
}