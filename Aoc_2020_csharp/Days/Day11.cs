using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day11
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/11.txt");
            var H = input.Length;
            var W = input[0].Length;

            var map1 = new char[H][];
            var map2 = new char[H][];
            for (var i = 0; i < H; i++)
            {
                map1[i] = input[i].ToCharArray();
                map2[i] = input[i].ToCharArray();
            }

            var count = 0;
            while (true)
            {
                count = 0;
                var same = true;

                for (var h = 0; h < H; h++)
                {
                    for (var w = 0; w < W; w++)
                    {
                        map2[h][w] = Mutate(map1, h, w, H, W);
                        if (map2[h][w] != map1[h][w]) same = false;
                        if (map2[h][w] == '#') count++;
                    }
                }

                if (same)
                    break;

                var tmp = map1;
                map1 = map2;
                map2 = tmp;
            }

            Console.WriteLine(count);
        }

        private static void Print(char[][] map)
        {
            Console.WriteLine(string.Join("\n", map.Select(x => new string(x))));
        }

        private static char Mutate(char[][] map, int h, int w, int H, int W)
        {
            if (map[h][w] == '.') return '.';
            var occupedCount = GetOccupedCount(map, h, w, H, W);
            if (h == 8 &&  w == 0) Console.WriteLine(occupedCount);
            if (map[h][w] == 'L' && occupedCount == 0) return '#';
            if (map[h][w] == '#' && occupedCount >= 5) return 'L';
            return map[h][w];
        }

        private static int GetOccupedCount(char[][] map, int h0, int w0, int H, int W)
        {
            var count = 0;
            for (var i = 0; i < 8; i++)
            {
                var h = h0;
                var w = w0;
                while (true)
                {
                    h += dh[i];
                    w += dw[i];
                    if (h < 0 || w < 0) break;
                    if (h >= H || w >= W) break;
                    if (map[h][w] == '.') continue;

                    if (map[h][w] == '#') count++;
                    break;
                }
            }

            return count;
        }

        private static int[] dh = new[] { -1, -1, -1, 0, 1, 1,  1,  0};
        private static int[] dw = new[] { -1,  0,  1, 1, 1, 0, -1, -1};
    }
}