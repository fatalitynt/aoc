namespace Aoc_2023_csharp;

public static class Day3
{
    private static int[] Arr(params int[] a) => a;

    private static readonly int[] Delta = Arr(1, 0, -1);

    private static readonly (int, int)[] DxDy = Delta
        .SelectMany(dx => Delta.Select(dy => (dx, dy)))
        .ToArray();

    private static IEnumerable<int> Iterate(string[] map, int h, int w) => Enumerable
        .Range(w, map[h].Length - w)
        .TakeWhile(i => char.IsDigit(map[h][i]));

    private static int ParseNumber(string[] map, int h, int w) => Iterate(map, h, w)
        .Select(i => map[h][i] - '0')
        .Aggregate(0, (cur, val) => cur * 10 + val);

    private static long? TryGetSId(string[] map, int h, int w, Func<char, bool> f) =>
        h >= 0 && h < map.Length && w >= 0 && w < map[0].Length && f(map[h][w])
            ? (h + 1) * 1000000000 + w + 1
            : null;

    private static long? TryGetSIdAround(string[] map, int h, int w, Func<char, bool> f) => DxDy
        .Select(x => TryGetSId(map, h + x.Item1, w + x.Item2, f))
        .FirstOrDefault(x => x.HasValue);

    private static long? TryGetSIdForNum(string[] map, int h, int w, Func<char, bool> f) => Iterate(map, h, w)
        .Select(w1 => TryGetSIdAround(map, h, w1, f))
        .FirstOrDefault(x => x.HasValue);

    private static bool IsNumberStart(string[] map, int h, int w) =>
        char.IsDigit(map[h][w]) && (w == 0 || !char.IsDigit(map[h][w - 1]));

    private static (int, long)? TryGetPart(string[] map, int h, int w, Func<char, bool> f) =>
        IsNumberStart(map, h, w) && TryGetSIdForNum(map, h, w, f).HasValue
            ? (ParseNumber(map, h, w), TryGetSIdForNum(map, h, w, f)!.Value)
            : null;

    private static (int Id, long SymId)[] GetParts(string[] map, int h, Func<char, bool> f) => Enumerable
        .Range(0, map[h].Length)
        .Select(w => TryGetPart(map, h, w, f))
        .Where(x => x.HasValue)
        .Select(x => x!.Value)
        .ToArray();

    private static int GetSum(string[] map) => Enumerable
        .Range(0, map.Length)
        .SelectMany(h => GetParts(map, h, c => !char.IsDigit(c) && c != '.'))
        .Sum(x => x.Id);

    private static int GetGearsRatioSum(string[] map) => Enumerable
        .Range(0, map.Length)
        .SelectMany(h => GetParts(map, h, c => c == '*'))
        .GroupBy(x => x.SymId)
        .Where(x => x.Count() == 2)
        .Select(x => x.First().Id * x.Last().Id)
        .Sum();

    public static int SolvePart1() => GetSum(3.DayInput());

    public static int SolvePart2() => GetGearsRatioSum(3.DayInput());
}