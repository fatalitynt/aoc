namespace Aoc_2021_csharp;

public class Day5
{
    private P[] GetLine(string s)
    {
        var p = s.Replace(" -> ", ",").Split(",").Select(int.Parse).ToArray();
        return new[]
        {
            P.C(p[0], p[1]),
            P.C(p[2], p[3]),
        };
    }

    private void PutLine(P[] ps, int[][] map, bool avoidDiag)
    {
        var dx = Math.Sign(ps[1].X - ps[0].X);
        var dy = Math.Sign(ps[1].Y - ps[0].Y);
        if (avoidDiag && dx != 0 && dy != 0) return;
        var cur = ps[0];
        while (true)
        {
            map[cur.Y][cur.X]++;
            if (cur == ps[1]) break;
            cur = P.C(cur.X + dx, cur.Y + dy);
        }
    }

    private long Solve(bool avoidDiag)
    {
        var lines = 5.DayInput().Select(GetLine).ToArray();

        var maxX = lines.Select(x => Math.Max(x[0].X, x[1].X)).Max();
        var maxY = lines.Select(x => Math.Max(x[0].Y, x[1].Y)).Max();
        var a = Enumerable.Range(0, maxY + 1).Select(_ => new int[maxX + 1]).ToArray();

        foreach (var line in lines) PutLine(line, a, avoidDiag);

        return P.GetAllPoints(a).Count(p => p.Read(a) > 1);
    }

    public long SolvePart1() => Solve(true);
    public long SolvePart2() => Solve(false);
}