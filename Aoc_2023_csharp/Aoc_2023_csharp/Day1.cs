namespace Aoc_2023_csharp;

public static class Day1
{
    private static readonly Dictionary<string, int> Map = "one,two,three,four,five,six,seven,eight,nine"
        .Split(",")
        .Select((x, i) => (K: x, V: i + 1))
        .ToDictionary(x => x.K, x => x.V);

    private static int? GetAsNumber(string s, int i) =>
        char.IsDigit(s[i]) ? s[i] - '0' : null;

    private static int? GetAsStr(string s, Func<string, string, bool> aHasB) =>
        Map.Keys.Any(k => aHasB(s, k))
            ? Map.First(kvp => aHasB(s, kvp.Key)).Value
            : null;

    private static int? TryGetFirst(string s, int i) =>
        GetAsNumber(s, i) ?? GetAsStr(s[i..], (a, b) => a.StartsWith(b));

    private static int? TryGetLast(string s, int i) =>
        GetAsNumber(s, i) ?? GetAsStr(s[..(i + 1)], (a, b) => a.EndsWith(b));

    private static int GetFirstNumber(string s) => Enumerable
        .Range(0, s.Length)
        .Select(i => TryGetFirst(s, i))
        .First(i => i.HasValue)!.Value;

    private static int GetLastNumber(string s) => Enumerable
        .Range(0, s.Length)
        .Reverse()
        .Select(i => TryGetLast(s, i))
        .First(i => i.HasValue)!.Value;

    public static int SolvePart1() => 1.DayInput()
        .Sum(l => 10 * (l.First(char.IsDigit) - '0') + l.Last(char.IsDigit) - '0');

    public static int SolvePart2() => 1.DayInput()
        .Sum(l => 10 * GetFirstNumber(l) + GetLastNumber(l));
}