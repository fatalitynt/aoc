using System;

namespace AdventOfCode2020.Days
{
    public static class Day25
    {
        public static void Main()
        {
            var t1 = 19774466;// 5764801;
            var t2 = 7290641;// 17807724;
            var indexes = MoveTo(7, t1, t2);
            Console.WriteLine(Move(indexes.i2, t1));
            Console.WriteLine(Move(indexes.i1, t2));
        }

        private static long Move(int size, long sn)
        {
            long last = 1;
            for (var i = 0; i < size; i++) last = last * sn % 20201227;
            return last;
        }
        
        private static (int i1, int i2) MoveTo(long sn, long t1, long t2)
        {
            var i1 = -1;
            var i2 = -1;
            long last = 1;
            var idx = 1;
            while (i1 == -1 || i2 == -1)
            {
                var next = last * sn % 20201227;
                if (next == t1) i1 = idx;
                if (next == t2) i2 = idx;
                last = next;
                idx++;
            }

            return (i1, i2);
        }
    }
}