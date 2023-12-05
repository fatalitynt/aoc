namespace Aoc_2023_csharp;

public class Day5
{
    private T[] Arr<T>(params T[] a) => a;

    private string[] GetInput() => 5.DayInput().Concat(Arr("")).ToArray();

    record Store(long[] Seeds, long[][] Data, int[][] Offsets);

    private long? TryConvertId(long[] section, long id) =>
        id >= section[1] && id < section[1] + section[2]
            ? section[0] - section[1] + id
            : null;

    // SLOW
    private long ConvertId(long[][] sections, int[] offs, long id) => Enumerable
        .Range(offs[0], offs[1])
        .Select(i => TryConvertId(sections[i], id))
        .FirstOrDefault(x => x.HasValue) ?? id;

    // FASTER by x8-x10
    private long ConvertIdOptimized(long[][] sections, int[] offs, long id)
    {
        var off = offs[0];
        var cnt = offs[1];
        for (var i = 0; i < cnt; i++)
        {
            var section = sections[i + off];
            if (id >= section[1] && id < section[1] + section[2])
                return section[0] - section[1] + id;
        }
        return id;
    }

    private long GetLocation(long id, Store s) => s
        .Offsets
        .Aggregate(id, (curId, offs) => ConvertIdOptimized(s.Data, offs, curId));

    private long[] GetSeeds(string[] a) => a[0]
        .Split(": ")[1]
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray();

    private long[][] GetData(string[] a) => a
        .Where(x => x.Length > 0 && char.IsDigit(x[0]))
        .Select(x => x.Split(" ").Select(long.Parse).ToArray())
        .ToArray();

    private int GetMapSize(string[] a, string mapName) => a
        .SkipWhile(x => x != mapName)
        .TakeWhile(x => !string.IsNullOrWhiteSpace(x))
        .Count() - 1;

    private int GetLastOffset(int[][] a) => a.Length == 0 ? 0 : a.Last().Sum();

    private int[][] AppendArray(int[][] a, int nextSize) => a
        .Concat(Arr<int[]>(Arr(GetLastOffset(a), nextSize)))
        .ToArray();

    private int[][] GetOffsets(string[] a) => a
        .Where(x => x.Contains("map:"))
        .Select(x => GetMapSize(a, x))
        .Aggregate(Array.Empty<int[]>(), AppendArray);

    private Store BuildStore(string[] a) => new(GetSeeds(a), GetData(a), GetOffsets(a));

    private long GetMinForRange(long st, long cnt, Store s) => Enumerable
        .Range(0, (int)cnt)
        .Select(x => st + x)
        .AsParallel()
        .Select(id => GetLocation(id, s))
        .Min();

    private long SolvePart1(Store s) => s
        .Seeds
        .Select(x => GetLocation(x, s))
        .Min();

    public long SolvePart1() => SolvePart1(BuildStore(GetInput()));

    private long SolvePart2(Store s) => Enumerable
        .Range(0, s.Seeds.Length / 2)
        .AsParallel()
        .Select(i => GetMinForRange(s.Seeds[i * 2], s.Seeds[i * 2 + 1], s))
        .Min();

    public long SolvePart2() => SolvePart2(BuildStore(GetInput()));
}