namespace Aoc_2021_csharp;

public class Day20
{
    private char[][] Extend(char[][] a, int cnt, char c)
    {
        return Enumerable.Range(0, cnt).Select(_ => Empty(a[0].Length + cnt + cnt))
            .Concat(a.Select(ExtendLine))
            .Concat(Enumerable.Range(0, cnt).Select(_ => Empty(a[0].Length + cnt + cnt)))
            .ToArray();

        char[] Empty(int size) => Enumerable.Range(0, size).Select(_ => c).ToArray();

        char[] ExtendLine(char[] b) =>
            Enumerable.Range(0, cnt).Select(_ => c)
                .Concat(b)
                .Concat(Enumerable.Range(0, cnt).Select(_ => c)).ToArray();
    }

    private int Idx(int dy, int dx, char[][] a)
    {
        var val = 0;
        for (var y = -1; y < 2; y++)
        for (var x = -1; x < 2; x++)
        {
            val <<= 1;
            val |= a[y + dy][x + dx] == '#' ? 1 : 0;
        }
        return val;
    }

    private char[][] Transform(char[][] a, string rep)
    {
        var b = Enumerable.Range(0, a.Length)
            .Select(_ => Enumerable.Range(0, a[0].Length).Select(_ => '.').ToArray())
            .ToArray();

        for (var y = 1; y < b.Length - 1; y++)
        for (var x = 1; x < b[0].Length - 1; x++)
        {
            b[y][x] = rep[Idx(y, x, a)];
        }

        for (var y = 0; y < b.Length; y++)
        {
            b[y][0] = b[y][b[y].Length - 1] = a[y][0] == '.' ? rep[0] : rep[511];
        }
        for (var x = 0; x < b[0].Length; x++)
        {
            b[0][x] = b[b[0].Length - 1][x] = a[0][x] == '.' ? rep[0] : rep[511];
        }
        return b;
    }

    private long Solve(int iterations)
    {
        var a = 20.DayInput();
        var rep = a[0];
        var map = a.Skip(2).Select(x => x.ToCharArray()).ToArray();
        var borderC = '.';

        for (var i = 0; i < iterations; i++)
        {
            map = Transform(Extend(map, 2, borderC), rep);
            borderC = borderC == '.' ? rep[0] : rep[511];
        }
        return map.Sum(l => l.Count(c => c == '#'));
    }

    public long SolvePart1() => Solve(2);
    public long SolvePart2() => Solve(50);
}