namespace Aoc_2023_csharp;

public class Day14
{
    private char[][] UpdateN(char[][] a)
    {
        for (var x = 0; x < a[0].Length; x++)
        for (var y = 0; y < a.Length; y++)
        {
            if (a[y][x] == 'O')
            {
                a[y][x] = '.';
                a[PosN(a, y, x)][x] = 'O';
            }
        }
        return a;
    }

    private char[][] UpdateS(char[][] a)
    {
        for (var x = 0; x < a[0].Length; x++)
        for (var y = a.Length - 1; y >= 0; y--)
        {
            if (a[y][x] == 'O')
            {
                a[y][x] = '.';
                a[PosS(a, y, x)][x] = 'O';
            }
        }
        return a;
    }

    // <-
    private char[][] UpdateW(char[][] a)
    {
        for (var y = 0; y < a.Length; y++)
        for (var x = 0; x < a[0].Length; x++)
        {
            if (a[y][x] == 'O')
            {
                a[y][x] = '.';
                a[y][PosW(a, y, x)] = 'O';
            }
        }
        return a;
    }

    // ->
    private char[][] UpdateE(char[][] a)
    {
        for (var y = 0; y < a.Length; y++)
        for (var x = a[0].Length - 1; x >= 0; x--)
        {
            if (a[y][x] == 'O')
            {
                a[y][x] = '.';
                a[y][PosE(a, y, x)] = 'O';
            }
        }
        return a;
    }

    private int PosN(char[][] a, int y0, int x)
    {
        var res = y0;
        for (var y = y0; y >= 0; y--)
        {
            if (a[y][x] == '.') res = y;
            else break;
        }
        return res;
    }

    private int PosS(char[][] a, int y0, int x)
    {
        var res = y0;
        for (var y = y0; y < a[0].Length; y++)
        {
            if (a[y][x] == '.') res = y;
            else break;
        }
        return res;
    }

    // <-
    private int PosW(char[][] a, int y, int x0)
    {
        var res = x0;
        for (var x = x0; x >= 0; x--)
        {
            if (a[y][x] == '.') res = x;
            else break;
        }
        return res;
    }

    // ->
    private int PosE(char[][] a, int y, int x0)
    {
        var res = x0;
        for (var x = x0; x < a.Length; x++)
        {
            if (a[y][x] == '.') res = x;
            else break;
        }
        return res;
    }

    private char[][] Cycle(char[][] a) => UpdateE(UpdateS(UpdateW(UpdateN(a))));

    private readonly List<long> primes = new List<long> {239017};

    private long GetP(int i)
    {
        while (i >= primes.Count)
            primes.Add(primes[primes.Count - 1] * primes[0]);
        return primes[i];
    }

    private long Hash(char[][] a) => a
        .SelectMany(x => x)
        .Select((x, idx) => (letter: x, idx))
        .Aggregate(0L, (cur, x) => cur + (x.letter != 'O' ? 0 : GetP(x.idx)));

    private char[][] GetMap() => 14
        .DayInput()
        .Select(x => x.ToCharArray())
        .ToArray();

    private long GetLoad(char[][] a) => P
        .GetAllPoints(a)
        .Where(x => x.Read(a) == 'O')
        .Sum(x => (long)a[0].Length - x.Y);

    public long SolvePart1() => GetLoad(UpdateN(GetMap()));

    public long SolvePart2()
    {
        var a = GetMap();
        var dict = new Dictionary<long, (string, int)>();

        var n = 1000000000;
        while (n > 0)
        {
            Cycle(a);
            var h = Hash(a);
            if (!dict.ContainsKey(h))
            {
                var line = new string(a.SelectMany(x => x).ToArray());
                dict[h] = (line, n);
            }
            else
            {
                var line = new string(a.SelectMany(x => x).ToArray());
                var (oldLine, idx) = dict[h];
                if (line.Equals(oldLine))
                {
                    var d = idx - n;
                    n -= d * (n / d);
                }
            }
            n--;
        }
        return GetLoad(a);
    }

    private void PrintMap(char[][] a)
    {
        Console.Clear();
        foreach (var line in a) Console.WriteLine(line);
    }
}