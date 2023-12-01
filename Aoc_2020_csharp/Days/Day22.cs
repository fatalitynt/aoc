using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day22
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/22.txt");
            var p1 = input.Skip(1).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
            var p2 = input.Skip(p1.Length + 3).Select(int.Parse).ToArray();

            var q1 = MakeQ(p1);
            var q2 = MakeQ(p2);

            P1WinSubGame(q1, q2);

            var arr = q1.Concat(q2).ToArray();
            var res = arr.Select((x, idx) => x *  (arr.Length - idx)).Aggregate((a, b) => a + b);
            Console.WriteLine(res);
        }

        private static bool P1WinSubGame(Queue<int> q1, Queue<int> q2)
        {
            var log = new Dictionary<long, List<List<int>>>();
            while (q1.Count > 0 && q2.Count > 0)
            {
                var h = GetHash(q1, q2, out var cards);
                if (log.TryGetValue(h, out var cases))
                {
                    if (cases.Any(c => Eq(c, cards))) return true;
                    cases.Add(cards);
                }
                else
                {
                    log.Add(h, new List<List<int>> {cards});
                }

                var c1 = q1.Dequeue();
                var c2 = q2.Dequeue();

                var p1Win = q1.Count >= c1 && q2.Count >= c2
                    ? P1WinSubGame(MakeQ(q1.Take(c1)), MakeQ(q2.Take(c2)))
                    : c1 > c2;

                if (p1Win)
                {
                    q1.Enqueue(c1);
                    q1.Enqueue(c2);
                } 
                else
                {
                    q2.Enqueue(c2);
                    q2.Enqueue(c1);
                }
            }
            return q1.Count != 0;
        }

        private static bool Eq(List<int> a, List<int> b)
        {
            if (a.Count != b.Count) return false;
            for (var i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
        

        private static Queue<int> MakeQ(IEnumerable<int> cards)
        {
            var q = new Queue<int>();
            foreach (var card in cards) q.Enqueue(card);
            return q;
        }

        private static long GetHash(Queue<int> q1, Queue<int> q2, out List<int> values)
        {
            var list = new List<int>{q1.Count, q2.Count};
            list.AddRange(q1);
            list.AddRange(q2);
            
            long res = 0;
            var b = 31;
            foreach (var item in list)
            {
                b *= 31;
                res += b * item;
            }

            values = list;
            return res;
        }
    }
}