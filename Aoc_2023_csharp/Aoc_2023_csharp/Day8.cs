namespace Aoc_2023_csharp;

public class Day8
{
    private record Data(string Pattern,
        Dictionary<string, string> L,
        Dictionary<string, string> R,
        Dictionary<string, string> Q);

    private long GetCnt(string loc, Func<string, bool> isFin, Data d) =>
        d.Pattern.Length + (d.Q.TryGetValue(loc, out var dest)
            ? isFin(dest) ? 0 : GetCnt(dest, isFin, d)
            : isFin(d.Q[loc] = RunRound(loc, d))
                ? 0
                : GetCnt(d.Q[loc], isFin, d));

    private string RunRound(string loc, Data d) => d.Pattern
        .Aggregate(loc, (cur, c) => (c == 'L' ? d.L : d.R)[cur]);

    private Dictionary<string, string> GetDict(string[] s, int start) => s
        .Skip(2)
        .Select(x => x.Split(" = ("))
        .ToDictionary(x => x[0], x => x[1].Substring(start, 3));

    private Data BuildData(string[] s) => new(
        s[0],
        GetDict(s, 0),
        GetDict(s, 5),
        new Dictionary<string, string>()
    );

    public long SolvePart1() => GetCnt("AAA", s => s == "ZZZ", BuildData(8.DayInput()));

    private long SolvePart2(long[] a) => a.Skip(1).Aggregate(a[0], Lcm);

    private long SolvePart2(Data d) => SolvePart2(d.L.Keys
        .Where(x => x[2] == 'A')
        .Select(x => GetCnt(x, s => s[2] == 'Z', d))
        .ToArray());

    public long SolvePart2() => SolvePart2(BuildData(8.DayInput()));

    private long Gcd(long a, long b) =>
        a % b == 0 ? b :
        b % a == 0 ? a :
        a > b ? Gcd(a % b, b) :
        Gcd(a, b % a);

    private long Lcm(long a, long b) => a * b / Gcd(a, b);
}