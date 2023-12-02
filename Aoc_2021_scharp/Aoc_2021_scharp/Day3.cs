namespace Aoc_2021_scharp;

public class Day3
{
    public int SolvePart1()
    {
        var a = 3.DayInput().ToArray();
        var n = a[0].Length;
        var b = new int[n];
        var c = new int[n];
        for (var i = 0; i < n; i++)
        {
            var zeros = a.Count(x => x[i] == '0');
            b[i] = zeros > a.Length / 2 ? 0 : 1;
            c[i] = 1 - b[i];
        }
        return Convert.ToInt32(string.Join("", b), 2) *
               Convert.ToInt32(string.Join("", c), 2);
    }

    public int SolvePart2()
    {
        var a = 3.DayInput().ToArray();

        var ox = a.Select(x => x).ToArray();
        var oxIdx = 0;
        while (ox.Length > 1)
        {
            var zeros = ox.Count(x => x[oxIdx] == '0');
            var target = zeros > ox.Length / 2 ? 0 : 1;
            ox = ox.Where(x => x[oxIdx] == target + '0').ToArray();
            oxIdx++;
        }

        var co = a.Select(x => x).ToArray();
        var coIdx = 0;
        while (co.Length > 1)
        {
            var zeros = co.Count(x => x[coIdx] == '0');
            var target = zeros > co.Length / 2 ? 1 : 0;
            co = co.Where(x => x[coIdx] == target + '0').ToArray();
            coIdx++;
        }
        return Convert.ToInt32(string.Join("", ox[0]), 2) *
               Convert.ToInt32(string.Join("", co[0]), 2);
    }
}