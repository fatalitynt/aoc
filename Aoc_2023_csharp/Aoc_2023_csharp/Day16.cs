namespace Aoc_2023_csharp;

public class Day16
{
    private (P, int) GetNext(P p, int dir) =>
        dir switch
        {
            0 => (p.Up, 0),
            1 => (p.Right, 1),
            2 => (p.Down, 2),
            3 => (p.Left, 3),
            _ => throw new Exception()
        };

    // -- \
    private (P, int) GetNextA(P p, int dir) =>
        dir switch
        {
            0 => (p.Left, 3),
            1 => (p.Down, 2),
            2 => (p.Right, 1),
            3 => (p.Up, 0),
            _ => throw new Exception()
        };

    // -- /
    private (P, int) GetNextB(P p, int dir) =>
        dir switch
        {
            0 => (p.Right, 1),
            1 => (p.Up, 0),
            2 => (p.Left, 3),
            3 => (p.Down, 2),
            _ => throw new Exception()
        };

    private long GetTilesCount(char[][] a, P p0, int dir0)
    {
        var d = new Dictionary<P, bool[]>();
        var q = new Queue<(P, int)>();
        q.Enqueue((p0, dir0));
        while (q.Count > 0)
        {
            var (p, dir) = q.Dequeue();
            if (d.TryGetValue(p, out var dirs) && dirs[dir]) continue;

            if (!p.TryRead(a, out var ch)) continue;

            if (!d.ContainsKey(p)) d[p] = new bool[4];
            d[p][dir] = true;

            if (ch == '.' || (ch == '-' && dir % 2 == 1) || (ch == '|' && dir % 2 == 0))
                q.Enqueue(GetNext(p, dir));

            if (ch == '\\') q.Enqueue(GetNextA(p, dir));

            if (ch == '/') q.Enqueue(GetNextB(p, dir));

            if (ch == '|' && dir % 2 == 1)
            {
                q.Enqueue((p.Up, 0));
                q.Enqueue((p.Down, 2));
            }

            if (ch == '-' && dir % 2 == 0)
            {
                q.Enqueue((p.Right, 1));
                q.Enqueue((p.Left, 3));
            }
        }

        return d.Count;
    }

    public long SolvePart1()
    {
        var a = 16.DayInput().Select(x => x.ToCharArray()).ToArray();
        return GetTilesCount(a, P.C(0, 0), 1);
    }

    public long SolvePart2()
    {
        var a = 16.DayInput().Select(x => x.ToCharArray()).ToArray();
        var res = 0L;
        for (var ver = 0; ver < a.Length; ver++)
        {
            res = Math.Max(res, GetTilesCount(a, P.C(0, ver), 1));
            res = Math.Max(res, GetTilesCount(a, P.C(a[0].Length - 1, ver), 3));
        }
        for (var hor = 0; hor < a[0].Length; hor++)
        {
            res = Math.Max(res, GetTilesCount(a, P.C(hor, 0), 2));
            res = Math.Max(res, GetTilesCount(a, P.C(hor, a.Length - 1), 0));
        }
        return res;
    }
}