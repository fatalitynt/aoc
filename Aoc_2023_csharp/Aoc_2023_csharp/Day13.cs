namespace Aoc_2023_csharp;

public class Day13
{
    private bool HasVerticalReflection(char[][] a, int y, int l, int r)
    {
        while (l >= 0 && r < a[y].Length)
            if (a[y][l--] != a[y][r++])
                return false;
        return true;
    }

    private bool HasHorizontalReflection(char[][] a, int x, int l, int r)
    {
        while (l >= 0 && r < a.Length)
            if (a[l--][x] != a[r++][x])
                return false;
        return true;
    }

    private bool CheckVerticalLine(char[][] a, int x) => Enumerable
        .Range(0, a.Length)
        .All(y => HasVerticalReflection(a, y, x, x + 1));

    private bool CheckHorizontalLine(char[][] a, int y) => Enumerable
        .Range(0, a[y].Length)
        .All(x => HasHorizontalReflection(a, x, y, y + 1));

    private IEnumerable<int> GetAllReflections(char[][] a)
    {
        for (var y = 0; y < a.Length - 1; y++)
            if (CheckHorizontalLine(a, y))
                yield return (y + 1) * 100;
        for (var x = 0; x < a[0].Length - 1; x++)
            if (CheckVerticalLine(a, x))
                yield return x + 1;
    }

    private IEnumerable<char[][]> GetMirrors(IEnumerable<string> a)
    {
        var r = new List<char[]>();
        foreach (var line in a)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                yield return r.ToArray();
                r.Clear();
            }
            else r.Add(line.ToCharArray());
        }
        yield return r.ToArray();
    }

    private long GetMutatedReflection(char[][] a)
    {
        var old = GetAllReflections(a).First();
        foreach (var p in P.GetAllPoints(a))
        {
            p.Write(a, p.Read(a) == '#' ? '.' : '#');
            foreach (var r in GetAllReflections(a).Where(x => x != old)) return r;
            p.Write(a, p.Read(a) == '#' ? '.' : '#');
        }
        throw new Exception();
    }

    public long SolvePart1() => GetMirrors(13.DayInput())
        .Sum(x => GetAllReflections(x).First());

    public long SolvePart2() => GetMirrors(13.DayInput())
        .Sum(GetMutatedReflection);
}