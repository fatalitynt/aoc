namespace Aoc_2021_csharp;

public class Day7
{
    private long GetDistCost(long a, long b, bool part1)
    {
        var dist = Math.Abs(a - b);
        if (part1) return dist;
        return (dist + 1) * dist / 2;
    }
    private long Solve(bool part1)
    {
        var a = 7.DayInput()[0].AsLongs().ToArray();
        var sum = long.MaxValue;
        var min = a.Min();
        var max = a.Max();
        for (var i = min; i <= max; i++)
        {
            sum = Math.Min(sum, a.Select(v => GetDistCost(v, i, part1)).Sum());
        }
        return sum;
    }

    public long SolvePart1() => Solve(false);
    public long SolvePart2() => Solve(true);
}