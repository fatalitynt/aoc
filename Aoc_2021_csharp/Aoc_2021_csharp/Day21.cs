namespace Aoc_2021_csharp;

public class Day21
{
    public long SolvePart1()
    {
        var a = 21.DayInput().Select(x => int.Parse(x.Split(": ")[1]) - 1).ToArray();
        var pts = new int[2];
        var d = 0;
        var dc = 0;
        var p = 0;
        while (true)
        {
            var s = d + d + d + 6;
            dc += 3;
            d += 3;
            a[p] += s;
            a[p] %= 10;
            pts[p] += a[p] + 1;
            if (pts[p] >= 1000) return dc * pts[1 - p];
            p = 1 - p;
        }
    }

    private IEnumerable<int> GetValues()
    {
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
        for (var k = 0; k < 3; k++)
            yield return i + j + k + 3;
    }

    public long SolvePart2()
    {
        var a = 21.DayInput().Select(x => int.Parse(x.Split(": ")[1]) - 1).ToArray();
        var d = new Dictionary<P4, long>();
        var d2 = new Dictionary<P4, long>();

        // points1 pos1 points2 pos2
        d[P4.Cr(0, a[0], 0, a[1])] = 1;

        var res = new long[2];
        var p = 0;

        while (d.Count > 0)
        {
            d2.Clear();
            foreach (var kvp in d)
            {
                var s = kvp.Key;
                var u = kvp.Value;
                foreach (var diceValue in GetValues())
                {
                    var nextPos = ((p == 0 ? s.B : s.D) + diceValue) % 10;
                    var nextPts = (p == 0 ? s.A : s.C) + nextPos + 1;
                    if (nextPts >= 21)
                    {
                        res[p] += u;
                        continue;
                    }
                    var k = p == 0
                        ? P4.Cr(nextPts, nextPos, s.C, s.D)
                        : P4.Cr(s.A, s.B, nextPts, nextPos);
                    d2.TryAdd(k, 0);
                    d2[k] += u;
                }
            }
            p = 1 - p;
            (d, d2) = (d2, d);
        }
        return res.Max();
    }
}