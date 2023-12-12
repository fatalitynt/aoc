namespace Aoc_2023_csharp;

public class Day12
{
    private long Solve(bool unfold) => 12
        .DayInput()
        .Select(x => x.Split(" "))
        .Sum(p => CountGroupsRaw(
            unfold ? Repeat5(p[0], '?') : p[0],
            unfold ? Repeat5(p[1], ',') : p[1]
        ));

    private string Repeat5(string s, char c) =>
        string.Join(c, Enumerable.Range(0, 5).Select(_ => s));

    private long CountGroupsRaw(string line, string a) =>
        CountGroups(line.ToCharArray(), a.Split(",").Select(int.Parse).ToArray());

    private long CountGroups(char[] line, int[] a) =>
        CountGroups(line, 0, a, 0, new Dictionary<P, long>());

    private long CountGroups(char[] line, int i, int[] a, int j, Dictionary<P, long> cache) =>
        cache.TryGetValue(P.C(i, j), out var res)
            ? res
            : cache[P.C(i, j)] = GoCountGroups(line, i, a, j, cache);

    private long GoCountGroups(char[] line, int i, int[] a, int j, Dictionary<P, long> cache)
    {
        if (j == a.Length) return line[i..].Any(x => x == '#') ? 0 : 1;
        if (i == line.Length) return 0;
        if (line[i] == '.') return CountGroups(line, i + 1, a, j, cache);
        if (line[i] == '?')
        {
            line[i] = '#';
            var r1 = GoCountGroups(line, i, a, j, cache);
            line[i] = '.';
            var r2 = GoCountGroups(line, i, a, j, cache);
            line[i] = '?';
            return r1 + r2;
        }
        if (!ConsumeSection(line[i..], a[j])) return 0;
        if (i + a[j] == line.Length) return j == a.Length - 1 ? 1 : 0;
        if (line[i + a[j]] == '#') return 0;

        return CountGroups(line, i + a[j] + 1, a, j + 1, cache);
    }

    private bool ConsumeSection(char[] line, int len) =>
        line.Length >= len && line[..len].All(x => x != '.');

    public long SolvePart1() => Solve(false);
    public long SolvePart2() => Solve(true);
}