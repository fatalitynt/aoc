using System;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day23
    {
        public static void Main2()
        {
            var sw = Stopwatch.StartNew();
            var map = new int[1000000 + 1];
            var arr = new[] {2, 1, 9, 3, 4, 7, 8, 6, 5}.ToList();
            arr.AddRange(Enumerable.Range(10, 1000000 - 9));
            for (var i = 0; i < arr.Count - 1; i++) map[arr[i]] = arr[i + 1];
            map[arr.Last()] = arr.First();
            var maxValue = arr.Max();
            var cur = arr.First();
            
            var taken = new int[3];

            for (var i = 0; i < 10000000; i++)
            {
                var idx = cur;
                for (var j = 0; j < 3; j++) taken[j] = idx = map[idx];

                var dest = cur;
                while (true)
                {
                    dest = dest - 1 == 0 ? maxValue : dest - 1;
                    if (taken[0] != dest && taken[1] != dest && taken[2] != dest) break;
                }
                
                cur = map[cur] = map[idx];
                map[taken[2]] = map[dest];
                map[dest] = taken[0];
            }

            Console.WriteLine((long)map[1] * map[map[1]]);
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            return;

            cur = 1;
            for (var i = 0; i < arr.Count - 1; i++) Console.Write(cur = map[cur]);
        }
        public static void Main1()
        {
            var sw = Stopwatch.StartNew();
            var arr = new[] {2, 1, 9, 3, 4, 7, 8, 6, 5}.ToList();
            //var arr = new[] {3, 8, 9, 1, 2, 5, 4, 6, 7}.ToList();
            
            arr.AddRange(Enumerable.Range(10, 1000000 - 9));

            var maxValue = arr.Max();
            var nodes = arr.Select(x => new Node {Val = x}).ToList();
            for (var i = 0; i < nodes.Count - 1; i++) nodes[i].Next = nodes[i + 1];
            nodes.Last().Next = nodes.First();
            var curNode = nodes.First();
            
            nodes.Add(new Node());
            nodes = nodes.OrderBy(x => x.Val).ToList();

            var taken = new Node[3]; 

            //10000000
            for (var i = 0; i < 10000000; i++)
            {
                var curValue = curNode.Val;
                var idxNode = curNode;
                for (var j = 0; j < 3; j++)
                {
                    idxNode = idxNode.Next;
                    taken[j] = idxNode;
                }

                while (true)
                {
                    curValue--;
                    if (curValue == 0) curValue = maxValue;
                    if (taken[0].Val != curValue && taken[1].Val != curValue && taken[2].Val != curValue) break;
                }

                curNode.Next = idxNode.Next;
                curNode = curNode.Next;

                idxNode = nodes[curValue];

                taken[2].Next = idxNode.Next;
                idxNode.Next = taken[0];
            }

            Console.WriteLine((long)nodes[1].Next.Val * nodes[1].Next.Next.Val);
            Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
            return;
            curNode = nodes[1];
            for (var i = 0; i < arr.Count - 1; i++) Console.Write((curNode = curNode.Next).Val);
        }
    }

    public class Node
    {
        public Node Next { get; set; }
        public int Val { get; set; }
    }
}