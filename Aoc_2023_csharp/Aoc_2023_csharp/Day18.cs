namespace Aoc_2023_csharp;

public class Day18
{
    private (int, int) Parse2(string[] a)
    {
        var s = a[2];
        var distStr = s.Substring(2, 5);
        var dist = Convert.ToInt32(distStr, 16);
        var dir = s[7] - '0';
        return (dir, dist);
    }

    private (int, int) Parse1(string[] a)
    {
        var s0 = a[0];
        var s1 = a[1];
        var cnt = int.Parse(s1);
        var dir = GetDir(s0[0]);
        return (dir, cnt);
    }

    private int GetDir(char c) =>
        c switch
        {
            'R' => 0,
            'D' => 1,
            'L' => 2,
            'U' => 3,
            _ => -1
        };

    private P Move(P p, int dir) => dir switch
    {
        0 => p.Right,
        1 => p.Down,
        2 => p.Left,
        3 => p.Up,
        _ => throw new Exception()
    };

    private long GetNeighY(long x, long y, Dictionary<long, HashSet<long>> d)
    {
        return d.TryGetValue(y + 1, out var hs1) && hs1.Contains(x) ? y + 1
            :  d.TryGetValue(y - 1, out var hs2) && hs2.Contains(x) ? y - 1
            : throw new Exception("");
    }

    private long Solve(Func<string[], (int, int)> parser)
    {
        var a = 18.DayInput();
        var c = P.C(0, 0);
        var d = new Dictionary<long, HashSet<long>>
        {
            [0] = new() { 0 }
        };

        foreach (var line in a)
        {
            var (dir, cnt) = parser(line.Split(" "));
            for (var i = 0; i < cnt; i++)
            {
                c = Move(c, dir);
                if (!d.ContainsKey(c.Y)) d[c.Y] = new HashSet<long>();
                if (!d[c.Y].Contains(c.X)) d[c.Y].Add(c.X);
            }
        }

        var res = d.Values.Select(x => (long)x.Count).Sum();

        foreach (var y in d.Keys.OrderBy(x => x))
        {
            var line = d[y].OrderBy(x => x).ToArray();
            var inside = 0;
            var localAdd = 0L;
            for (var i = 1; i < line.Length; i++)
            {
                if (line[i] - line[i - 1] == 1)
                {
                    var startY = GetNeighY(line[i - 1], y, d);
                    while (i < line.Length && line[i] - line[i - 1] == 1) i++;
                    if (i >= line.Length) break;
                    var endY = GetNeighY(line[i - 1], y, d);
                    if (startY == endY)
                        inside = 1 - inside;
                }
                if (inside == 0)
                    localAdd += line[i] - line[i - 1] - 1;
                inside = 1 - inside;
            }
            res += localAdd;
        }

        return res;
    }

    public long SolvePart1() => Solve(Parse1);

    public long SolvePart2() => Solve(Parse2);
}