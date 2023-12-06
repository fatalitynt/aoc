namespace Aoc_2023_csharp;

public class Day2
{
    private int[][] GetGameSets(string s) => s
        .Split(":")[1]
        .Split(";")
        .Select(GetSet)
        .ToArray();

    private int[] GetSet(string s) => s
        .Split(",")
        .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        .Select(x => (Cnt: int.Parse(x[0]), Idx: "rgb".IndexOf(x[1][0])))
        .Aggregate(new int[3], (cur, x) => MutateArr(cur, x.Idx, x.Cnt));

    private int[] MutateArr(int[] ar, int idx, int val) => ar
        .Select((x, i) => idx == i ? x + val : x)
        .ToArray();

    private bool IsGamePossible(int[][] sets, int[] limit) => sets
        .All(set => Enumerable.Range(0, 3).All(i => set[i] <= limit[i]));

    private int[] Arr(params int[] a) => a;

    private int MaxBy(int[][] sets, int i) => sets.Max(x => x[i]);

    private int GetGamePower(int[][] sets) => Enumerable
        .Range(0, 3)
        .Aggregate(1, (cur, i) => MaxBy(sets, i) * cur);

    public int SolvePart1() => 2
        .DayInput()
        .Select(GetGameSets)
        .Select((x, i) => (Sets: x, Id: i + 1))
        .Where(x => IsGamePossible(x.Sets, Arr(12, 13, 14)))
        .Sum(x => x.Id);

    public int SolvePart2() => 2
        .DayInput()
        .Select(GetGameSets)
        .Sum(GetGamePower);
}