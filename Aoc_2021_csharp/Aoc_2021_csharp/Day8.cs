namespace Aoc_2021_csharp;

public class Day8
{
    private bool Inc(string big, string small) => small.All(big.Contains);

    private Dictionary<string, int> Deduct(string[] s)
    {
        s = s.Select(l => new string(l.OrderBy(x => x).ToArray())).ToArray();

        var one = s.First(x => x.Length == 2);
        var four = s.First(x => x.Length == 4);
        var seven = s.First(x => x.Length == 3);
        var eight = s.First(x => x.Length == 7);

        var nine = s.First(x => x.Length == 6 && Inc(x, four));
        var zero = s.First(x => x.Length == 6 && x != nine && Inc(x, one));
        var six = s.First(x => x.Length == 6 && x != nine && x != zero);

        var three = s.First(x => x.Length == 5 && Inc(x, one));
        var five = s.First(x => x.Length == 5 && Inc(six, x));
        var two = s.First(x => x.Length == 5 && x != three && x != five);

        var res = new Dictionary<string, int>
        {
            {zero, 0},
            {one, 1},
            {two, 2},
            {three, 3},
            {four, 4},
            {five, 5},
            {six, 6},
            {seven, 7},
            {eight, 8},
            {nine, 9},
        };
        return res;
    }

    public long SolvePart2()
    {
        var a = 8.DayInput();
        var res = 0L;
        var mlp = new[] { 1000, 100, 10, 1 };
        foreach (var line in a)
        {
            var p = line.Split(" | ");
            var d = Deduct(p[0].Split(" "));
            res += p[1].Split(" ")
                .Select((x, idx) => d[new string(x.OrderBy(y => y).ToArray())] * mlp[idx])
                .Aggregate(0L, (c, v) => c + v);
        }
        return res;
    }

    public long SolvePart1()
    {
        var a = 8.DayInput();
        var hs = new HashSet<int> { 2, 4, 3, 7 };
        return a.Aggregate(0L,
            (cur, str) =>
                cur + str.Split(" | ")[1].Split(" ").Count(x => hs.Contains(x.Length)));
    }
}