namespace Aoc_2023_csharp;

public class Day9
{
    long[] GetNextLine(long[] a) => a
        .Skip(1)
        .Select((x, idx) => x - a[idx])
        .ToArray();

    private long[][] SingleArr(long[] a) => Enumerable
        .Range(0, 1)
        .Select(_ => a)
        .ToArray();

    long[][] GetAllLayers(long[] a) => GetAllLayersByLastLine(a, GetNextLine(a));

    long[][] GetAllLayersByLastLine(long[] a, long[] lastLine) => SingleArr(a)
        .Concat(
            lastLine.All(x => x == 0)
                ? SingleArr(lastLine)
                : GetAllLayers(lastLine))
        .ToArray();

    (long, long) GetNextValue(long[] a) => GetNextValueByLayers(GetAllLayers(a));

    (long, long) GetNextValueByLayers(long[][] a) => (
        a.Sum(x => x.Last()),
        a.Select(x => x.First())
            .Reverse()
            .Aggregate(0L, (cur, next) => next - cur)
    );

    private (long, long)[] GetPredictions() => 9
        .DayInput()
        .Select(x => x.Split(" ").Select(long.Parse).ToArray())
        .Select(GetNextValue)
        .ToArray();

    public long SolvePart1() => GetPredictions().Sum(x => x.Item1);
    public long SolvePart2() => GetPredictions().Sum(x => x.Item2);
}