namespace Aoc_2021_csharp;

public class Day6
{
    private void Incr(Dictionary<long, long> d, long k, long v)
    {
        d.TryAdd(k, 0);
        d[k] += v;
    }

    private long Solve(int totalDays)
    {
        var a = 6.DayInput()[0].AsLongs();
        var d1 = new Dictionary<long, long>();
        var d2 = new Dictionary<long, long>();
        foreach (var k in a) Incr(d1, k, 1);

        for (var days = 0; days < totalDays; days++)
        {
            d2.Clear();
            foreach (var kvp in d1)
            {
                Incr(d2, kvp.Key == 0 ? 6 : kvp.Key - 1, kvp.Value);
                if (kvp.Key == 0)
                    Incr(d2, 8, kvp.Value);
            }
            (d1, d2) = (d2, d1);
        }
        return d1.Values.Sum();
    }

    public long SolvePart1() => Solve(80);
    public long SolvePart2() => Solve(256);
}