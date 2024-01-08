namespace Aoc_2021_csharp;

public class Day17
{
    private int[] GetArea(string s)
    {
        return s.Split(": ")[1].Split(", ")
            .Select(x => x.Split("=")[1])
            .SelectMany(x => x.Split("..").Select(int.Parse))
            .ToArray();
    }

    public long SolvePart1()
    {
        var a = 17.DayInput();
        var area = GetArea(a[0]);
        var minY = Math.Min(area[2], area[3]);
        var velY = Math.Abs(minY) - 1;
        var hitY = (velY + 1) * velY / 2;
        return hitY;
    }

    public long SolvePart2()
    {
        var a = 17.DayInput();
        var area = GetArea(a[0]);
        var minX = Math.Min(area[0], area[1]);
        var maxX = Math.Max(area[0], area[1]);
        var minY = Math.Min(area[2], area[3]);
        var maxY = Math.Max(area[3], area[3]);

        var maxVelY = Math.Abs(minY) - 1;

        var res = 0L;
        for (var vx0 = 1; vx0 <= maxX; vx0++)
        {
            for (var vy0 = minY; vy0 <= maxVelY; vy0++)
            {
                var x = 0;
                var y = 0;
                var vx = vx0;
                var vy = vy0;
                while (true)
                {
                    x += vx;
                    y += vy;
                    if (vx > 0) vx -= 1;
                    vy -= 1;

                    if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                    {
                        res++;
                        break;
                    }

                    if ((x < minX && vx == 0) || x > maxX || y < minY) break;
                }
            }
        }

        return res;
    }
}