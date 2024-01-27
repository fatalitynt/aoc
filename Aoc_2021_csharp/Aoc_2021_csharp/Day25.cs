namespace Aoc_2021_csharp;

public class Day25
{
    private P GetNext(P p, char[][] a, bool r)
    {
        if (r)
        {
            var n1 = p.Right;
            return n1.X == a[0].Length ? P.C(0, n1.Y) : n1;
        }
        var n2 = p.Down;
        return n2.Y == a.Length ? P.C(n2.X, 0) : n2;
    }

    private bool CanMove(P p, char[][] a, bool r) => GetNext(p, a, r).Read(a) == '.';

    private void Move(P p, char[][] a, bool r)
    {
        var next = GetNext(p, a, r);
        next.Write(a, p.Read(a));
        p.Write(a, '.');
        p.X = next.X;
        p.Y = next.Y;
    }

    public long SolvePart1()
    {
        var a = 25.DayInput().Select(x => x.ToCharArray()).ToArray();
        var r = P.GetAllPoints(a).Where(p => p.Read(a) == '>').ToArray();
        var d = P.GetAllPoints(a).Where(p => p.Read(a) == 'v').ToArray();

        var cnt = 0;
        while (true)
        {
            var mvR = r.Where(x => CanMove(x, a, true)).ToArray();
            foreach (var rr in mvR) Move(rr, a, true);

            var mvD = d.Where(x => CanMove(x, a, false)).ToArray();
            foreach (var dd in mvD) Move(dd, a, false);

            cnt++;

            if (mvR.Length + mvD.Length == 0) break;
        }
        return cnt;
    }
}