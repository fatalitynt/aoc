namespace Aoc_2023_csharp;

public class Day1
{
    private readonly Dictionary<string, int> map = "one,two,three,four,five,six,seven,eight,nine"
        .Split(",")
        .Select((x, i) => (K: x, V: i + 1))
        .ToDictionary(x => x.K, x => x.V);

    private int? GetAsNumber(string s, int i) =>
        char.IsDigit(s[i]) ? s[i] - '0' : null;

    private int? GetAsStr(string s, Func<string, string, bool> aHasB) =>
        map.Keys.Any(k => aHasB(s, k))
            ? map.First(kvp => aHasB(s, kvp.Key)).Value
            : null;

    private int? TryGetFirst(string s, int i) =>
        GetAsNumber(s, i) ?? GetAsStr(s[i..], (a, b) => a.StartsWith(b));

    private int? TryGetLast(string s, int i) =>
        GetAsNumber(s, i) ?? GetAsStr(s[..(i + 1)], (a, b) => a.EndsWith(b));

    private int GetFirstNumber(string s) => Enumerable
        .Range(0, s.Length)
        .Select(i => TryGetFirst(s, i))
        .First(i => i.HasValue)!.Value;

    private int GetLastNumber(string s) => Enumerable
        .Range(0, s.Length)
        .Reverse()
        .Select(i => TryGetLast(s, i))
        .First(i => i.HasValue)!.Value;

    public int SolvePart1() => 1.DayInput()
        .Sum(l => 10 * (l.First(char.IsDigit) - '0') + l.Last(char.IsDigit) - '0');

    public int SolvePart2() => 1.DayInput()
        .Sum(l => 10 * GetFirstNumber(l) + GetLastNumber(l));
}