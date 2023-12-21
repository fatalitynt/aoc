namespace Aoc_2023_csharp;

public class Day21
{
    public long SolvePart1()
    {
        var a = 21.DayInput().Select(x => x.ToCharArray()).ToArray();
        var start = P.GetAllPoints(a).First(p => p.Read(a) == 'S');

        var q = new Queue<(P, int)>();
        q.Enqueue((start, 64));
        start.Write(a, 'O');

        while (q.Count > 0)
        {
            var (cur, steps) = q.Dequeue();
            if (steps == 0) continue;
            foreach (var next in cur.GetNeighbors()
                         .Where(x => x.TryRead(a, out var c) && c == '.'))
            {
                var ch = steps % 2 == 0 ? 'V' : 'O';
                next.Write(a, ch);
                q.Enqueue((next, steps - 1));
            }
        }
        return P.GetAllPoints(a).Count(p => p.Read(a) == 'O');
    }

    private int[][] ExpandMap(char[][] a) => a
        .Select(l => l.Concat(l).Select(c => c == '#' ? -2 : -1).ToArray())
        .Concat(a.Select(l => l.Concat(l).Select(c => c == '#' ? -2 : -1).ToArray()))
        .ToArray();

    private T[][] Clone<T>(T[][] a) => a.Select(l => l.Select(x => x).ToArray()).ToArray();

    private void Fill(P s, int[][] a) => Fill(new [] {(s, 0)}, a);

    private void Fill((P, int)[] points, int[][] a)
    {
        var q = new Queue<P>();
        foreach (var (s, val) in points)
        {
            q.Enqueue(s);
            s.Write(a, val);
        }
        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            var dist = cur.Read(a);
            foreach (var next in cur.GetNeighbors())
            {
                if (!next.TryRead(a, out var nextVal) || nextVal == -2) continue;
                if (nextVal == -1 || nextVal > dist + 1)
                {
                    next.Write(a, dist + 1);
                    q.Enqueue(next);
                }
            }
        }
    }

    private (long, long, List<long>) CountReps(P s, long steps, int h, int targetRest, int[][] a)
    {
        var plate = Clone(a);
        Fill(s, plate);

        var fullPlateCount = (long)plate.SelectMany(x => x.Select(y => y))
            .Count(x => x >= 0 && x % 2 == targetRest);

        var stepsToCover = plate.SelectMany(x => x.Select(y => y))
            .Where(x => x >= 0 && x % 2 == targetRest)
            .Max();

        var skips = Math.Max(0, steps - stepsToCover) / h;
        var restSteps = steps - skips * h;

        var tails = new List<long>();

        while (restSteps >= 0)
        {
            var counted = plate.SelectMany(x => x.Select(y => y))
                .Count(x => x >= 0 && x % 2 == targetRest && x <= restSteps);
            if (counted == fullPlateCount) skips++;
            else tails.Add(counted);
            restSteps -= h;
        }

        return (skips, fullPlateCount, tails);
    }

    private long SolvePart(P s, int[][] a, long steps0)
    {
        var h = a.Length;
        var w = a[0].Length;
        var core = Clone(a);
        Fill(s, core);

        var angleR = P.C(w - 1, 0);
        var angleDist = angleR.Read(core);
        var targetRest = angleDist % 2 == steps0 % 2 ? 0 : 1;

        var stepsA = steps0 - P.C(w - 1, 0).Read(core) - 2;
        var (skips, fullCount, tails) = CountReps(P.C(0, h - 1), stepsA, h, targetRest, a);
        var platesCount = (skips + 1) * skips / 2;

        var res = platesCount * fullCount + tails.Sum(t => t * ++skips);
        return res + RunUp(core[0], steps0, h, a);
    }

    private long RunUp(int[] topLine, long steps, int h, int[][] a)
    {
        var res = 0L;
        var targetRest = steps % 2;
        while (true)
        {
            var b = Clone(a);
            var nextLine = topLine.Select((val, x) => (P.C(x, h - 1), val + 1)).ToArray();
            Fill(nextLine, b);

            var counted = b.SelectMany(x => x.Select(y => y))
                .Count(x => x >= 0 && x % 2 == targetRest && x <= steps);

            res += counted;

            if (counted == 0) break;

            var nextTopLine = b[0];

            if (SameDiff(topLine, nextTopLine) && nextTopLine.All(x => x < steps))
            {
                var diff = nextTopLine[0] - topLine[0];
                var skips = nextTopLine.Select(x => (steps - x) / diff).Min();
                if (skips > 0)
                {
                    res += counted * skips;
                    nextTopLine = nextTopLine.Select(x => x + (int)skips * diff).ToArray();
                }
            }

            topLine = nextTopLine;
        }

        return res;
    }

    private bool SameDiff(int[] a, int[] b)
    {
        var dif = a[0] - b[0];
        for (var i = 1; i < a.Length; i++)
        {
            if (a[i] - b[i] != dif)
                return false;
        }
        return true;
    }

    public long SolvePart2()
    {
        var map = 21.DayInput().Select(x => x.ToCharArray()).ToArray();
        var start = P.GetAllPoints(map).First(p => p.Read(map) == 'S');
        start.Write(map, '.');
        var a = ExpandMap(map);
        var b = Clone(a);
        Fill(start, b);

        var steps = 5000;

        var res = (long)b.SelectMany(x => x.Select(y => y))
            .Count(x => x >= 0 && x <= steps && x % 2 == steps % 2);

        for (var i = 0; i < 4; i++)
        {
            res += SolvePart(start, a, steps);
            (a, start) = Rotate(a, start);
        }

        return res;
    }

    private (int[][], P) Rotate(int[][] a, P s)
    {
        var h = a.Length;
        var w = a[0].Length;
        var p = P.C(-1, -1);
        var res = new List<int[]>();
        for (var x = w - 1; x >= 0; x--)
        {
            var line = new List<int>();
            for (var y = 0; y < h; y++)
            {
                if (x == s.X && y == s.Y)
                {
                    p = P.C(line.Count, res.Count);
                }
                line.Add(a[y][x]);
            }
            res.Add(line.ToArray());
        }
        return (res.ToArray(), p);
    }
}