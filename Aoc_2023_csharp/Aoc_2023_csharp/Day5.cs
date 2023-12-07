namespace Aoc_2023_csharp;

public class Day5
{
    private T[] Arr<T>(params T[] a) => a;

    private string[] GetInput() => 5.DayInput().Concat(Arr("")).ToArray();

    record Range(long Start, long End, long Offset);

    private Range GetRange(long[] a) => new(a[1], a[1] + a[2] - 1, a[0] - a[1]);

    private long[] LineToLongs(string s) => s
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray();

    private Range[] GetRangesForSection(string[] a, int i) => a
        .Select((line, idx) => (line, idx))
        .SkipWhile(x => x.idx <= i)
        .TakeWhile(x => !string.IsNullOrWhiteSpace(x.line))
        .Select(x => GetRange(LineToLongs(x.line)))
        .OrderBy(x => x.Start)
        .ToArray();

    private Range[][] GetRanges(string[] a) => a
        .Select((line, idx) => (line, idx))
        .Where(x => x.line.Contains("map:"))
        .Select(x => GetRangesForSection(a, x.idx))
        .ToArray();

    private long[] GetSeeds(string[] a) => a[0]
        .Split(": ")[1]
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray();


    private Range Intersect(Range r0, Range r1) => new(
        Math.Max(r0.Start, r1.Start) + r1.Offset,
        Math.Min(r0.End, r1.End) + r1.Offset,
        0);

    private Range GetLeftPart(Range r0, Range r1) => new(r0.Start, r1.Start - 1, 0);
    private Range GetRightPart(Range r0, Range r1) => new(r1.End + 1, r0.End, 0);

    private Range[] ExpandRangesInLayer(Range[] layer, Range r0)
    {
        var list = new List<Range>();
        var last = r0;
        foreach (var cur in layer)
        {
            if (last.Start > cur.End) continue;
            if (last.End < cur.Start) break;

            if (last.Start < cur.Start)
                list.Add(GetLeftPart(last, cur));

            list.Add(Intersect(last, cur));

            last = last.End <= cur.End
                ? null
                : GetRightPart(last, cur);

            if (last == null) break;
        }
        if (last != null)
            list.Add(last);
        return list.ToArray();
    }

    private Range[] ExpandRanges(Range[][] layers, Range r0) => layers
        .Aggregate(Arr(r0),
            (ranges, layer) => ranges
                .SelectMany(r => ExpandRangesInLayer(layer, r)).ToArray());

    public long SolvePart1()
    {
        var i = GetInput();
        var layers = GetRanges(i);
        var s = GetSeeds(i);
        return s.SelectMany(x => ExpandRanges(layers, new Range(x, x, 0)))
            .OrderBy(x => x.Start)
            .First()
            .Start;
    }

    private Range Rng(long start, long len) => new(start, start + len - 1, 0);

    public long SolvePart2()
    {
        var i = GetInput();
        var layers = GetRanges(i);
        var s = GetSeeds(i);
        return Enumerable
            .Range(0, s.Length / 2)
            .SelectMany(x => ExpandRanges(layers, Rng(s[x * 2], s[x * 2 + 1])))
            .OrderBy(x => x.Start)
            .First()
            .Start;
    }
}