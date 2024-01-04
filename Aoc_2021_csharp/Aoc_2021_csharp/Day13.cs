namespace Aoc_2021_csharp;

public class Day13
{
    private void Print(P[] ps)
    {
        var maxX = ps.MaxBy(x => x.X)!.X;
        var maxY = ps.MaxBy(x => x.Y)!.Y;
        var a = Enumerable.Range(0, maxY + 1)
            .Select(_ => Enumerable.Range(0, maxX + 1).Select(_ => '.').ToArray())
            .ToArray();
        foreach (var p in ps) p.Write(a, '#');
        foreach (var line in a) Console.WriteLine(line);
    }

    private P[] FoldY(int y, P[] ps) => ps
        .Select(p => p.Y < y ? p : P.C(p.X, y + y - p.Y))
        .Distinct()
        .ToArray();

    private P[] FoldX(int x, P[] ps) => ps
        .Select(p => p.X < x ? p : P.C(x + x - p.X, p.Y))
        .Distinct()
        .ToArray();

    public long SolvePart1()
    {
        var a = 13.DayInput();
        var ps = a.TakeWhile(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Split(","))
            .Select(x => P.C(int.Parse(x[0]), int.Parse(x[1])))
            .ToArray();

        var folds = a.Skip(ps.Length + 1).Select(x => x.Split(" ").Last().Split("="));
        foreach (var fold in folds)
        {
            if (fold[0] == "y") ps = FoldY(int.Parse(fold[1]), ps);
            if (fold[0] == "x") ps = FoldX(int.Parse(fold[1]), ps);
            Console.WriteLine(ps.Length);
        }

        Print(ps);

        return ps.Length;
    }

    // Just read the code from printed map.
    public long SolvePart2() => SolvePart1();
}