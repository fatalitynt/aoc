namespace Aoc_2023_csharp;

public class Day6
{
    private T[] Arr<T>(params T[] a) => a;

    private long[] GetNumbers(string x) => x
        .Split(":")[1]
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray();

    private long[][] GetRaces(string[] a) => GetRaceLines(a
        .Select(GetNumbers)
        .ToArray());

    private long[][] GetRaceLines(long[][] a) => a[0]
        .Select((_, i) => Arr(a[0][i], a[1][i]))
        .ToArray();

    private long[] GetSingleRace(string[] a) => a
        .Select(x => long.Parse(x.Split(":")[1].Replace(" ", "")))
        .ToArray();

    private long GetNumberOfWins(long[] race) => Enumerable
        .Range(0, (int)race[0] + 1)
        .Count(i => i * (race[0] - i) > race[1]);

    public long SolvePart1() => GetRaces(6.DayInput())
        .Select(GetNumberOfWins)
        .Aggregate(1L, (cur, next) => cur * next);

    public long SolvePart2() => GetNumberOfWins(GetSingleRace(6.DayInput()));
}