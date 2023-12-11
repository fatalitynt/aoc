namespace Aoc_2023_csharp;

public class Day11
{
    private HashSet<int> GetSkipCoordinates(int[][] a, Func<P, int> getXorY) => P
        .GetAllPoints(a)
        .Where(x => x.Read(a) < 0)
        .Select(getXorY)
        .Distinct()
        .ToHashSet();

    private long Solve(int k)
    {
        var a = 11.DayInput()
            .Select(x => x.Select(l => l == '#' ? -1 : 1).ToArray())
            .ToArray();

        var skipX = GetSkipCoordinates(a, p => p.X);
        var skipY = GetSkipCoordinates(a, p => p.Y);

        var rewrite = P.GetAllPoints(a)
            .Where(p => !skipX.Contains(p.X) || !skipY.Contains(p.Y));

        foreach (var p in rewrite) p.Write(a, k);

        var ps = P.GetAllPoints(a).Where(p => p.Read(a) < 0).ToArray();

        return ps
            .Select((p, idx) => ps.Skip(idx).Sum(p1 => Distance(p, p1, a)))
            .Sum();
    }

    private long Distance(P p1, P p2, int[][] a) =>
        Enumerable
            .Range(Math.Min(p1.X, p2.X), Math.Abs(p1.X - p2.X))
            .Select(x => (long)a[p1.Y][x])
            .Sum(x => x < 0 ? 1 : x)
        + Enumerable
            .Range(Math.Min(p1.Y, p2.Y), Math.Abs(p1.Y - p2.Y))
            .Select(y => (long)a[y][p1.X])
            .Sum(x => x < 0 ? 1 : x);

    public long SolvePart1() => Solve(2);

    public long SolvePart2() => Solve(1000000);
}