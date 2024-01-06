namespace Aoc_2021_csharp;

public class Day15
{
    private static Dictionary<P, long> Bfs(P p, int[][] a)
    {
        var q = new Queue<P>();
        var dist = new Dictionary<P, long>();
        q.Enqueue(p);
        dist[p] = 0;

        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            var d0 = dist[cur];
            foreach (var next in cur.Get4ValidNeighbors(a))
            {
                var d1 = d0 + next.Read(a);
                if (dist.TryGetValue(next, out var d2) && d2 <= d1) continue;
                q.Enqueue(next);
                dist[next] = d1;
            }
        }
        return dist;
    }

    private void CopySection(int[][] a, int x0, int y0, int x1, int y1, int w, int h)
    {
        for (var y = 0; y < h; y++)
        for (var x = 0; x < w; x++)
        {
            var v = a[y0 + y][x0 + x] + 1;
            if (v == 10) v = 1;
            a[y1 + y][x1 + x] = v;
        }
    }

    private int[][] IncreaseMap(int[][] a, int k)
    {
        var h = a.Length;
        var w = a[0].Length;
        var res = Enumerable.Range(0, h * k).Select(_ => new int[w * k]).ToArray();
        foreach (var p in P.GetAllPoints(a)) p.Write(res, p.Read(a));
        for (var i = 0; i < k; i++)
        for (var j = 0; j < k; j++)
        {
            if (i + j == 0) continue;
            CopySection(res,
                (i == 0 ? i : i - 1) * w,
                (i == 0 ? j - 1 : j) * h,
                i * w,
                j * h,
                w,
                h);
        }
        return res;
    }

    private long Solve(bool increaseMap)
    {
        var a = 15.DayInput().Select(x => x.Select(c => c - '0').ToArray()).ToArray();
        if (increaseMap) a = IncreaseMap(a, 5);
        var d = Bfs(P.C(0, 0), a);
        return d[P.C(a[0].Length - 1, a.Length - 1)];
    }

    public long SolvePart1() => Solve(false);
    public long SolvePart2() => Solve(true);
}