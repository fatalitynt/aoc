namespace Aoc_2023_csharp;

public class Day22
{
    class Brick
    {
        public int Id { get; set; }
        public P P0 { get; set; }
        public P P1 { get; set; }
        public int H { get; set; }
        public int Z { get; set; }
        public Brick[] Below { get; set; }
        public Brick[] Above { get; set; }

        public Brick(string s, int id)
        {
            Id = id;
            var ps = s.Split("~");
            var a = ps[0].Split(",").Select(int.Parse).ToArray();
            var b = ps[1].Split(",").Select(int.Parse).ToArray();
            var x0 = a[0];
            var x1 = b[0];
            var y0 = a[1];
            var y1 = b[1];

            var p0 = P.C(Math.Min(x0, x1), Math.Min(y0, y1));
            var p1 = P.C(Math.Max(x0, x1), Math.Max(y0, y1));

            var h = Math.Abs(a[2] - b[2]) + 1;
            var z = Math.Min(a[2], b[2]);

            P0 = p0;
            P1 = p1;
            H = h;
            Z = z;
        }

        private bool IsCrossing(Brick b)
        {
            if (b.P0.Y > P1.Y || b.P1.Y < P0.Y) return false;
            if (b.P0.X > P1.X || b.P1.X < P0.X) return false;
            return true;
        }

        public void Fall(Brick[] others)
        {
            var newZ = 1;
            foreach (var o in others)
            {
                if (o == this || !IsCrossing(o) || o.Z > Z) continue;
                newZ = Math.Max(newZ, o.Z + o.H);
            }
            Z = newZ;
        }

        public void SetBelow(Brick[] others)
        {
            Below = others.Where(b => b != this && IsCrossing(b))
                .Where(b => b.Z + b.H == Z)
                .ToArray();
        }

        public void SetAbove(Brick[] others)
        {
            Above = others.Where(b => b != this && IsCrossing(b))
                .Where(b => Z + H == b.Z)
                .ToArray();
        }

        public long CountFalling(HashSet<int> falling = null)
        {
            falling ??= new HashSet<int> { Id };
            foreach (var above in Above)
            {
                if (above.Below.All(ss => falling.Contains(ss.Id)))
                {
                    falling.Add(above.Id);
                    above.CountFalling(falling);
                }
            }
            return falling.Count - 1;
        }

        public bool IsStable() => Above.All(ba => ba.Below.Length > 1);
    }

    private Brick[] PrepareBricks()
    {
        var a = 22.DayInput().Select((x, idx) => new Brick(x, idx))
            .OrderBy(x => x.Z)
            .ThenBy(x => x.H)
            .ToArray();
        foreach (var b in a) b.Fall(a);
        foreach (var b in a) b.SetBelow(a);
        foreach (var b in a) b.SetAbove(a);
        return a;
    }

    public long SolvePart1() => PrepareBricks()
        .Count(b => b.IsStable());

    public long SolvePart2() => PrepareBricks()
        .Where(x => !x.IsStable())
        .Sum(x => x.CountFalling());
}