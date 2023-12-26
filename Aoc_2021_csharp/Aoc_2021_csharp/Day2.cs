namespace Aoc_2021_csharp;

public class Day2
{
    public int SolvePart1()
    {
        var a = 2.DayInput().Select(l =>
        {
            var parts = l.Split(" ");
            return (parts[0][0], int.Parse(parts[1]));
        }).ToArray();
        var h = a.Where(x => x.Item1 == 'f').Sum(x => x.Item2);
        var v = a.Where(x => x.Item1 != 'f')
            .Sum(x => x.Item1 == 'd' ? x.Item2 : -x.Item2);
        return h * v;
    }

    public int SolvePart2()
    {
        var a = 2.DayInput().Select(l =>
        {
            var parts = l.Split(" ");
            return (parts[0][0], int.Parse(parts[1]));
        }).ToArray();

        var pos = 0;
        var dep = 0;
        var aim = 0;

        foreach (var (ch, val) in a)
        {
            switch (ch)
            {
                case 'f':
                    pos += val;
                    dep += aim * val;
                    continue;
                case 'd':
                    aim += val;
                    continue;
                case 'u':
                    aim -= val;
                    continue;
            }
        }
        return pos * dep;
    }
}