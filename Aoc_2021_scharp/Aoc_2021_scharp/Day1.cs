namespace Aoc_2021_scharp;

public class Day1
{
    public int SolvePart1()
    {
        var a = 1.DayInput().Select(int.Parse).ToArray();
        var r = 0;
        for (var i = 1; i < a.Length; i++)
        {
            if (a[i] > a[i - 1]) r++;
        }
        return r;
    }

    public int SolvePart2()
    {
        var a = 1.DayInput().Select(int.Parse).ToArray();
        var r = 0;
        for (var i = 3; i < a.Length; i++)
        {
            if (a[i] > a[i - 3]) r++;
        }
        return r;
    }
}