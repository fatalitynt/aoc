using System.Diagnostics;

namespace Aoc_2023_csharp;

public class Day23
{
    private void Fill(P start, char[][] a, int[][] d)
    {
        var q = new Queue<P>();
        var parents = new Dictionary<P, P>();

        q.Enqueue(start);
        start.Write(d, 0);
        parents[start] = start;

        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            var dist = cur.Read(d);
            var par = parents[cur];
            foreach (var next in GetNeigh(cur, cur.Read(a)))
            {
                if (next == par) continue;
                if (!next.TryRead(a, out var nextVal) || nextVal == '#') continue;
                if (next.Read(d) != 0) continue;

                q.Enqueue(next);
                next.Write(d, dist + 1);
                parents[next] = cur;
            }
        }
    }

    private IEnumerable<P> GetNeigh(P cur, char curChar)
    {
        if (curChar == '>') return new[] { cur.Right };
        if (curChar == '<') return new[] { cur.Left };
        if (curChar == 'v') return new[] { cur.Down };
        if (curChar == '^') return new[] { cur.Up };
        return cur.GetNeighbors();
    }

    public long SolvePart1()
    {
        var a = 23.DayInput().Select(x => x.ToCharArray()).ToArray();
        var b = Enumerable.Range(0, a.Length).Select(_ => new int[a[0].Length]).ToArray();
        var startX = a[0].Select((x, idx) => (x, idx)).First(x => x.x == '.').idx;
        var start = P.C(startX, 0);
        Fill(start, a, b);
        return b.Last().Sum();
    }

    private long SlowDfs(P s, int val, char[][] a, int[][] d)
    {
        var stack = new Stack<(P, int)>();

        s.Write(d, val);
        stack.Push((s, val));

        var max = 0L;

        while (stack.Count > 0)
        {
            var (cur, dist) = stack.Pop();
            if (dist == -1)
            {
                cur.Write(d, 0);
                continue;
            }
            stack.Push((cur, -1));

            if (cur.Y == a.Length - 1)
            {
                if (dist > max)
                    Console.WriteLine($"max upd {max} - {dist}");
                max = Math.Max(max, dist);
            }

            foreach (var next in cur.GetNeighbors())
            {
                if (!next.TryRead(a, out var c) || c == '#') continue;

                if (next.Read(d) != 0) continue;

                stack.Push((next, dist + 1));
                next.Write(d, dist + 1);
            }
        }
        Console.WriteLine("fin");
        return max;
    }


    // 13+ min to get an answer.
    public long SolvePart2_SlowVersion()
    {
        var a = 23.DayInput().Select(x => x.ToCharArray()).ToArray();
        var d = Enumerable.Range(0, a.Length).Select(_ => new int[a[0].Length]).ToArray();
        var startX = a[0].Select((x, idx) => (x, idx)).First(x => x.x == '.').idx;
        var s = P.C(startX, 0);
        var res = SlowDfs(s, 1, a, d) - 1;
        return res;
    }

    private class Node
    {
        public int Id { get; set; }
        public P P { get; set; }
        public Dictionary<int, (Node, int)> Others { get; set; } = new();

        public void Connect(Node other, int dist)
        {
            Others[other.Id] = (other, dist);
            other.Others[Id] = (this, dist);
        }
    }

    private bool CanGo(P p, char[][] a) => p.TryRead(a, out var c) && c != '#';

    private void Connect(Node n, char[][] a, Dictionary<P, Node> map)
    {
        var q = new Queue<(P, int)>();
        var v = new HashSet<P> { n.P };
        q.Enqueue((n.P, 0));
        while (q.Count > 0)
        {
            var (cur, dist) = q.Dequeue();

            foreach (var next in cur.GetNeighbors().Where(x => CanGo(x, a) && !v.Contains(x)))
            {
                if (map.TryGetValue(next, out var nextNode))
                {
                    n.Connect(nextNode, dist + 1);
                    continue;
                }
                v.Add(next);
                q.Enqueue((next, dist + 1));
            }
        }
    }

    private long VisitNode(Node n, P e, HashSet<int> visited)
    {
        if (n.P == e) return 0;
        var res = long.MinValue;
        visited.Add(n.Id);

        foreach (var (next, dist) in n.Others.Values)
        {
            if (visited.Contains(next.Id)) continue;
            res = Math.Max(res, dist + VisitNode(next, e, visited));
        }

        visited.Remove(n.Id);
        return res;
    }

    public long SolvePart2()
    {
        var sw = Stopwatch.StartNew();
        var a = 23.DayInput().Select(x => x.ToCharArray()).ToArray();
        var s = P.C(Array.IndexOf(a.First(), '.'), 0);
        var e = P.C(Array.IndexOf(a.Last(), '.'), a.Length - 1);

        var nodes = P.GetAllPoints(a)
            .Where(p => p.Read(a) != '#')
            .Where(p => p.GetNeighbors().Count(x => CanGo(x, a)) > 2)
            .Concat(new[] { s, e })
            .Select((p, idx) => new Node { Id = idx, P = p })
            .ToArray();

        var map = nodes.ToDictionary(x => x.P, x => x);

        foreach (var node in nodes) Connect(node, a, map);

        var res = VisitNode(map[s], e, new HashSet<int>());
        Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
        return res;
    }
}