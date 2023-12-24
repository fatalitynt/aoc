namespace Aoc_2023_csharp;

public class Day24
{
    private const decimal Eps = 0.0000000001M;
    private decimal Det(decimal a, decimal b, decimal c, decimal d) => a * d - b * c;

    private bool Intersect(Line m, Line n, out Pd res)
    {
        res = new Pd(0, 0, 0);
        var zn = Det(m.A, m.B, n.A, n.B);
        if (Math.Abs(zn) < Eps)
            return false;
        res.X = -Det(m.C, m.B, n.C, n.B) / zn;
        res.Y = -Det(m.A, m.C, n.A, n.C) / zn;
        return true;
    }

    private bool FullIntersect(Line m, Line n, out Pd res) =>
        Intersect(m, n, out res) && m.IsInFuture(res) && n.IsInFuture(res);

    class Pd
    {
        public Pd(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public override string ToString() => $"({X:f3}, {Y:f3}, {Z:f3})";

        public Pd Add(Pd o) => new(X + o.X, Y + o.Y, Z + o.Z);
    }

    class Line
    {
        public Line(string s)
        {
            var ps = s.Split(" @ ");
            var p0 = ps[0].Split(", ").Select(decimal.Parse).ToArray();
            var p1 = ps[1].Split(", ").Select(decimal.Parse).ToArray();

            P = new Pd(p0[0], p0[1], p0[2]);
            Vec = new Pd(p1[0], p1[1], p1[2]);

            var p2 = P.Add(Vec);

            A = P.Y - p2.Y;
            B = p2.X - P.X;
            C = -A * P.X - B * P.Y;

            LineStr = s;
        }

        public decimal A { get; set; }
        public decimal B { get; set; }
        public decimal C { get; set; }
        private string LineStr { get; set; }
        public Pd P { get; set; }
        public Pd Vec { get; set; }

        public override string ToString() => LineStr;

        public bool IsInFuture(Pd p)
        {
            var dx = p.X - P.X;
            var dy = p.Y - P.Y;
            var res = Math.Sign(dx) == Math.Sign(Vec.X)
                      && Math.Sign(dy) == Math.Sign(Vec.Y);
            return res;
        }
    }

    public long SolvePart1()
    {
        var lines = 24.DayInput().Select(x => new Line(x)).ToArray();

        decimal min = 200000000000000;
        decimal max = 400000000000000;
        var res = 0;

        for (var i = 0; i < lines.Length; i++)
        for (var j = i + 1; j < lines.Length; j++)
        {
            var l1 = lines[i];
            var l2 = lines[j];

            if (!FullIntersect(l1, l2, out var p)) continue;
            if (p.X >= min && p.X <= max && p.Y >= min && p.Y <= max)
            {
                Console.WriteLine($"{l1} + {l2} = {p}");
                res++;
            }
        }
        return res;
    }

    public long SolvePart2()
    {
        //https://github.com/jonathanpaulson/AdventOfCode/blob/master/2023/24.py
        return 0;
    }
}