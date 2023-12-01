using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020.Days
{
    public static class Day17
    {
        public static void Main1()
        {   
            var map = new Dictionary<P, bool>();
            var input = File.ReadAllLines("Inputs/17.txt");
            
            var h = input.Length;
            var w = input[0].Length;
            var d = 1;
            var t = 1;
            
            for (var i = 0; i < h; i++)
            for (var j = 0; j < w; j++)
                if (input[i][j] == '#') map.Add(P.C(j, i, 0, 0), true);

            for (var i = 1; i <= 6; i++)
            {
                var next = new Dictionary<P, bool>();
                
                for (var y = 0 - i; y < h + i; y++)
                for (var x = 0 - i; x < w + i; x++)
                for (var z = 0 - i; z < d + i; z++)
                for (var r = 0 - i; r < t + i; r++)
                {
                    var me = P.C(x, y, z, r);
                    var res = GetCheckResult(me, map);
                    if (res == 3 || res == 2 && map.ContainsKey(me)) next[me] = true;
                }

                map = next;
            }

            Console.WriteLine(map.Count);
        }

        private static int GetCheckResult(P me, Dictionary<P,bool> map)
        {
            
            var active = 0;
            for (var dx = -1; dx <= 1; dx++)
            for (var dy = -1; dy <= 1; dy++)
            for (var dz = -1; dz <= 1; dz++)
            for (var dt = -1; dt <= 1; dt++)
            {
                if (dx == 0 && dy == 0 && dz == 0 && dt == 0) continue;
                var ngh = me.Shift(dx, dy, dz, dt);
                if (map.ContainsKey(ngh)) active++;
                if (active > 3) return active;
            }
            return active;
        }

        private class P
        {
            public P Shift(int x, int y, int z, int t) => new P(X + x, Y + y, Z + z, T + t);
            public static P C(int x, int y, int z, int t) => new P(x, y, z, t);
            private P(int x, int y, int z, int t)
            {
                X = x;
                Y = y;
                Z = z;
                T = t;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int T { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                if (!(obj is P)) return false;
                var other = (P) obj;
                return X == other.X && Y == other.Y && Z == other.Z && other.T == T;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = 17;
                    hash = hash * 23 + X.GetHashCode();
                    hash = hash * 23 + Y.GetHashCode();
                    hash = hash * 23 + Z.GetHashCode();
                    hash = hash * 23 + T.GetHashCode();
                    return hash;
                }
            }
        }
    }
}