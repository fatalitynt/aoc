namespace Aoc_2023_csharp;

public class Day4
{
    private int PowIt(int p) => p == 0
        ? 0
        : (int)Math.Pow(2, p - 1);

    private int GetNbMatches(string s) => s
        .Split(": ")[1]
        .Replace(" | ", " ")
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .GroupBy(x => x, x => x)
        .Count(x => x.Count() > 1);

    private int[] Arr1(int n) => Enumerable
        .Range(0, n)
        .Select(_ => 1)
        .ToArray();

    private int[] MutateArr(int[] a, int cnt, int start) => a
        .Select((x, idx) => idx > start && idx <= start + cnt ? x + a[start] : x)
        .ToArray();

    private int GetWinningCardsCount(string[] s) => s
        .Select((x, idx) => (Cnt: GetNbMatches(x), Idx: idx))
        .Aggregate(Arr1(s.Length), (cur, i) => MutateArr(cur, i.Cnt, i.Idx))
        .Sum();

    private int GetWinningPointsSum(string[] s) => s
        .Select(GetNbMatches)
        .Sum(PowIt);

    public int SolvePart1() => GetWinningPointsSum(4.DayInput());

    public int SolvePart2() => GetWinningCardsCount(4.DayInput());
}