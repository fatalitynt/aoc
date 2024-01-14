namespace Aoc_2021_csharp;

public class Day22
{
    private int[] Parse(string s, out int on)
    {
        var p = s.Split(" ");
        on = p[0] == "on" ? 1 : 0;
        var nums = p[1].Split(",")
            .SelectMany(l => l.Split("=")[1].Split("..").Select(int.Parse)).ToArray();
        return nums;
    }

    private void Set(Dictionary<P3, int> d, int[] nums, int on)
    {
        var x0 = Math.Max(-50, nums[0]);
        var x1 = Math.Min(50, nums[1]);
        var y0 = Math.Max(-50, nums[2]);
        var y1 = Math.Min(50, nums[3]);
        var z0 = Math.Max(-50, nums[4]);
        var z1 = Math.Min(50, nums[5]);

        for (var x = x0; x <= x1; x++)
        for (var y = y0; y <= y1; y++)
        for (var z = z0; z <= z1; z++)
        {
            var p = P3.C(x, y, z);
            d[p] = on;
        }
    }

    public long SolvePart1()
    {
        var a = 22.DayInput();
        var d = new Dictionary<P3, int>();
        foreach (var l in a) Set(d, Parse(l, out var on), on);
        return d.Values.Count(v => v == 1);
    }

    public static int Mn(int a, int b) => Math.Min(a, b);
    public static int Mx(int a, int b) => Math.Max(a, b);

    class Cube
    {
        public P3 Beg { get; set; }
        public P3 End { get; set; }

        public Cube Clone() => new(Beg.Clone(), End.Clone());

        public Cube(P3 b, P3 e)
        {
            Beg = b;
            End = e;
        }

        public Cube(int[] a)
        {
            var b = P3.C(Mn(a[0], a[1]), Mn(a[2], a[3]), Mn(a[4], a[5]));
            var e = P3.C(Mx(a[0], a[1]), Mx(a[2], a[3]), Mx(a[4], a[5]));
            Beg = b;
            End = e;
        }

        public override string ToString() => $"Cube [{Beg} - {End}]";

        private (Cube, Cube) Split(Cube c, int x, Func<P3, int> get, Action<P3, int> set)
        {
            if (x < get(c.Beg) || x > get(c.End)) throw new Exception();
            var u = c.Clone();
            var d = c.Clone();
            set(u.Beg, x);
            set(d.End, x - 1);
            return (u, d);
        }

        private Cube BreakBy(Cube me, Cube other, Func<P3, int> get, Action<P3, int> set, List<Cube> parts)
        {
            // cut above
            if (get(me.End) > get(other.End) && get(me.Beg) <= get(other.End))
            {
                var (u, d) = Split(me, get(other.End) + 1, get, set);
                parts.Add(u);
                me = d;
            }
            // cut below
            if (get(me.Beg) < get(other.Beg) && get(me.End) >= get(other.Beg))
            {
                var (u, d) = Split(me, get(other.Beg), get, set);
                parts.Add(d);
                me = u;
            }
            return me;
        }

        public List<Cube> BreakBy(Cube other)
        {
            // non intersection case
            if (Beg.X > other.End.X || End.X < other.Beg.X ||
                Beg.Y > other.End.Y || End.Y < other.Beg.Y ||
                Beg.Z > other.End.Z || End.Z < other.Beg.Z)
                return new List<Cube> { this };

            var parts = new List<Cube>();
            var me = this;
            me = BreakBy(me, other, p3 => p3.X, (p3, i) => p3.X = i, parts);
            me = BreakBy(me, other, p3 => p3.Y, (p3, i) => p3.Y = i, parts);
            BreakBy(me, other, p3 => p3.Z, (p3, i) => p3.Z = i, parts);

            return parts;
        }

        public long GetCellsCount() =>
            ((long)End.X - Beg.X + 1) * ((long)End.Y - Beg.Y + 1) * ((long)End.Z - Beg.Z + 1);
    }

    public long SolvePart2()
    {
        var cubes = new List<Cube>();
        var next = new List<Cube>();

        var a = 22.DayInput();
        foreach (var l in a)
        {
            next.Clear();
            var nums = Parse(l, out var on);
            var newCube = new Cube(nums);

            foreach (var c in cubes) next.AddRange(c.BreakBy(newCube));
            if (on > 0) next.Add(newCube);
            (cubes, next) = (next, cubes);
        }

        return cubes.Sum(c => c.GetCellsCount());
    }
}