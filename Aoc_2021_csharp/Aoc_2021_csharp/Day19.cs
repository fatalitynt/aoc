namespace Aoc_2021_csharp;

public class Day19
{
    private P3 RotateFixedX(P3 p) => P3.C(p.X, p.Z, -p.Y);
    private P3 RotateFixedY(P3 p) => P3.C(p.Z, p.Y, -p.X);
    private P3 RotateFixedZ(P3 p) => P3.C(p.Y, -p.X, p.Z);

    private List<P3> RotateFixedX(List<P3> ps) => ps.Select(RotateFixedX).ToList();
    private List<P3> RotateFixedY(List<P3> ps) => ps.Select(RotateFixedY).ToList();
    private List<P3> RotateFixedZ(List<P3> ps) => ps.Select(RotateFixedZ).ToList();

    private IEnumerable<List<P3>> Get24Rotations(List<P3> ps)
    {
        for (var i = 0; i < 4; i++) yield return ps = RotateFixedZ(ps);
        ps = RotateFixedX(ps);
        for (var i = 0; i < 4; i++) yield return ps = RotateFixedY(ps);
        ps = RotateFixedX(ps);
        for (var i = 0; i < 4; i++) yield return ps = RotateFixedZ(ps);
        ps = RotateFixedX(ps);
        for (var i = 0; i < 4; i++) yield return ps = RotateFixedY(ps);
        ps = RotateFixedX(ps);
        ps = RotateFixedY(ps);
        for (var i = 0; i < 4; i++) yield return ps = RotateFixedX(ps);
        ps = RotateFixedY(ps);
        ps = RotateFixedY(ps);
        for (var i = 0; i < 4; i++) yield return ps = RotateFixedX(ps);
    }

    private List<P3> Shift(List<P3> ps, P3 p0) => ps
        .Select(p => P3.C(p.X + p0.X, p.Y + p0.Y, p.Z + p0.Z)).ToList();

    private P3 CreateP3(int[] a) => P3.C(a[0], a[1], a[2]);

    private P3 TryJoin(HashSet<P3> allPoints, List<P3> currentLayer)
    {
        foreach (var layer in Get24Rotations(currentLayer))
        foreach (var p0 in allPoints)
        foreach (var p1 in layer)
        {
            var delta = P3.C(p0.X - p1.X, p0.Y - p1.Y, p0.Z - p1.Z);
            var shifted = Shift(layer, delta);
            var cnt = shifted.Count(allPoints.Contains);
            if (cnt < 12) continue;
            foreach (var p in shifted) allPoints.Add(p);
            return delta;
        }
        return null;
    }

    private (long, long) Solve()
    {
        var a = 19.DayInput();
        var layers = new List<List<P3>>();
        foreach (var l in a)
        {
            if (l.StartsWith("---")) layers.Add(new List<P3>());
            else if (!string.IsNullOrEmpty(l))
                layers.Last().Add(CreateP3(l.Split(",").Select(int.Parse).ToArray()));
        }
        var ps = new HashSet<P3>();
        foreach (var p in layers[0]) ps.Add(p);

        var pos = new P3[layers.Count];
        pos[0] = P3.C(0, 0, 0);

        var joined = new HashSet<int>();
        while (joined.Count + 1 != layers.Count)
        {
            for (var i = 1; i < layers.Count; i++)
            {
                if (joined.Contains(i)) continue;
                var delta = TryJoin(ps, layers[i]);
                if (delta == null) continue;
                joined.Add(i);
                pos[i] = delta;
                break;
            }
        }
        var maxMd = pos.SelectMany(p1 => pos.Select(p2 => MhDist(p1, p2))).Max();
        return (ps.Count, maxMd);
    }

    private int MhDist(P3 p1, P3 p2) =>
        Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) + Math.Abs(p1.Z - p2.Z);

    public long SolvePart1() => Solve().Item1;

    public long SolvePart2() => Solve().Item2;
}