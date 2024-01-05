namespace Aoc_2021_csharp;

public class Day14
{
    private Dictionary<int, long> Split(Dictionary<int, long> a, Func<int, (int, int)> keys)
    {
        var res = new Dictionary<int, long>();
        foreach (var kvp in a)
        {
            var k = kvp.Key;
            var v = kvp.Value;
            var (k1, k2) = keys(k);
            res.TryAdd(k1, 0);
            res.TryAdd(k2, 0);
            res[k1] += v;
            res[k2] += v;
        }
        return res;
    }

    private int Num(char l, char r) => l * 1000 + r;

    private long Solve(int steps)
    {
        var a = 14.DayInput();
        var str = a[0].ToList();
        var s = new Dictionary<int, long>();
        for (var i = 1; i < str.Count; i++)
        {
            var k = Num(str[i - 1], str[i]);
            s.TryAdd(k, 0);
            s[k]++;
        }
        var d = new Dictionary<int, (int, int)>();
        foreach (var l in a.Skip(2))
        {
            var p = l.Split(" -> ");
            var aa = p[0][0];
            var bb = p[0][1];
            var cc = p[1][0];
            var k = Num(aa, bb);
            d[k] = (Num(aa, cc), Num(cc, bb));
        }

        for (var i = 0; i < steps; i++)
            s = Split(s, k => d[k]);

        var cnt = Split(s, k => (k % 1000, k / 1000));
        cnt[str.First()]++;
        cnt[str.Last()]++;
        return cnt.Values.Max() / 2 - cnt.Values.Min() / 2;
    }

    public long SolvePart1() => Solve(10);

    public long SolvePart2() => Solve(40);
}