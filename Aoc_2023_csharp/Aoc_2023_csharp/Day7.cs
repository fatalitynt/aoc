namespace Aoc_2023_csharp;

public class Day7
{
    private T[] Arr<T>(params T[] a) => a;

    private const string DictStr = "T-10,Q-12,K-13,A-14";

    private static readonly Dictionary<char, long> Map = DictStr
        .Split(",")
        .Select(x => x.Split("-"))
        .ToDictionary(x => x[0][0], x => long.Parse(x[1]));

    private long GetRank2Value(char c, bool useJokers) =>
        c == 'J'
            ? useJokers ? 1 : 11
            : Map.TryGetValue(c, out var x)
                ? x
                : c - '0';

    private long GetRank2(string s, bool useJokers) => s
        .Aggregate(0L, (cur, ch) => GetRank2Value(ch, useJokers) + cur * 100);

    private int[] GetRank1Rates(string s, bool useJokers) => s
        .GroupBy(x => x)
        .ToDictionary(x => x.Key, x => x.Count())
        .Where(x => !useJokers || x.Key != 'J')
        .Select(x => x.Value)
        .Concat(Arr(0, 0))
        .OrderByDescending(x => x)
        .Select((x, idx) => useJokers && idx == 0 ? x + s.Count(c => c == 'J') : x)
        .ToArray();

    private int GetRank1ByRates(int[] a) =>
        a[0] == 5 ? 1
        : a[0] == 4 ? 2
        : a[0] == 3 && a[1] == 2 ? 3
        : a[0] == 3 ? 4
        : a[0] == 2 && a[1] == 2 ? 5
        : a[0] == 2 ? 6
        : 7;

    private int GetRank1(string s, bool useJokers) =>
        GetRank1ByRates(GetRank1Rates(s, useJokers));

    private long Solve(bool useJokers) => 7.DayInput()
        .Select(l => l.Split(" "))
        .OrderBy(x => GetRank1(x[0], useJokers))
        .ThenByDescending(x => GetRank2(x[0], useJokers))
        .Reverse()
        .Select((x, idx) => long.Parse(x[1]) * (idx + 1))
        .Sum();

    public long SolvePart1() => Solve(useJokers: false);

    public long SolvePart2() => Solve(useJokers: true);
}