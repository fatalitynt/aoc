namespace Aoc_2023_csharp;

public class Day10
{
    private readonly char[] srcR = { '-', 'L', 'F', 'S' };
    private readonly char[] destR = { '-', '7', 'J' };

    private readonly char[] srcL = { '-', '7', 'J', 'S' };
    private readonly char[] destL = { '-', 'L', 'F' };

    private readonly char[] srcU = { '|', 'L', 'J', 'S' };
    private readonly char[] destU = { '|', 'F', '7' };

    private readonly char[] srcD = { '|', 'F', '7', 'S' };
    private readonly char[] destD = { '|', 'L', 'J' };

    private bool IsConnected(char[][] a, P p0, P p)
    {
        if (!p0.TryRead(a, out var src) || !p.TryRead(a, out var dest) || dest == '.')
            return false;

        var y0 = p0.Y;
        var x0 = p0.X;
        var y = p.Y;
        var x = p.X;

        //->
        if (y == y0 && x > x0 && srcR.Any(c => c == src) && destR.Any(c => c == dest))
            return true;
        //<-
        if (y == y0 && x < x0 && srcL.Any(c => c == src) && destL.Any(c => c == dest))
            return true;
        // V
        if (x == x0 && y > y0 && srcD.Any(c => c == src) && destD.Any(c => c == dest))
            return true;
        // ^
        if (x == x0 && y < y0 && srcU.Any(c => c == src) && destU.Any(c => c == dest))
            return true;

        return false;
    }

    private P FindStart(char[][] a) => P.GetAllPoints(a).First(p => p.Read(a) == 'S');

    private void TrySet(char[][] a, P p, char val, Dictionary<P, P> path)
    {
        if (CanPaint(a, p, path)) p.Write(a, val);
    }

    private bool CanPaint(char[][] a, P p, Dictionary<P, P> path)
    {
        if (!p.TryRead(a, out var c) || c == 'I' || c == 'O') return false;
        return c == '.' || !path.ContainsKey(p);
    }

    private void TryPaint(char[][] a, P cur, P from, Dictionary<P, P> path)
    {
        if (!cur.TryRead(a, out var c)) return;
        if (c == '-')
        {
            if (cur.X > from.X) // ->
            {
                TrySet(a, cur.Down, 'I', path);
                TrySet(a, cur.Up, 'O', path);
            }
            else // <-
            {
                TrySet(a, cur.Down, 'O', path);
                TrySet(a, cur.Up, 'I', path);
            }
        }
        if (c == '|')
        {
            if (cur.Y > from.Y) // V
            {
                TrySet(a, cur.Left, 'I', path);
                TrySet(a, cur.Right, 'O', path);
            }
            else // ^
            {
                TrySet(a, cur.Left, 'O', path);
                TrySet(a, cur.Right, 'I', path);
            }
        }
        if (c == '7')
        {
            if (cur.Y == from.Y)
            {
                TrySet(a, cur.Up, 'O', path);
                TrySet(a, cur.Right, 'O', path);
            }
            else
            {
                TrySet(a, cur.Right, 'I', path);
                TrySet(a, cur.Up, 'I', path);
            }
        }
        if (c == 'L')
        {
            if (cur.Y == from.Y)
            {
                TrySet(a, cur.Down, 'O', path);
                TrySet(a, cur.Left, 'O', path);
            }
            else
            {
                TrySet(a, cur.Down, 'I', path);
                TrySet(a, cur.Left, 'I', path);
            }
        }
        if (c == 'J')
        {
            if (cur.Y == from.Y)
            {
                TrySet(a, cur.Right, 'I', path);
                TrySet(a, cur.Down, 'I', path);
            }
            else
            {
                TrySet(a, cur.Down, 'O', path);
                TrySet(a, cur.Right, 'O', path);
            }
        }
        if (c == 'F')
        {
            if (cur.Y == from.Y)
            {
                TrySet(a, cur.Up, 'I', path);
                TrySet(a, cur.Left, 'I', path);
            }
            else
            {
                TrySet(a, cur.Left, 'O', path);
                TrySet(a, cur.Up, 'O', path);
            }
        }
    }

    private Dictionary<P, P> Walk(char[][] a, P start, Dictionary<P, P> path)
    {
        var s = new Stack<P>();
        var parents = new Dictionary<P, P> { [start] = start };
        s.Push(start);

        while (s.Count > 0)
        {
            var cur = s.Pop();
            var from = parents[cur];
            if (path != null)
                TryPaint(a, cur, from, path);

            foreach (var next in cur.GetNeighbors()
                         .Where(nxt => nxt != from && IsConnected(a, cur, nxt)))
            {
                if (next == start)
                    return parents;
                s.Push(next);
                parents[next] = cur;
            }
        }
        return parents;
    }

    private void Extend(char[][] a, char k, Dictionary<P, P> path)
    {
        var q = P.GetAllPoints(a).Where(p => p.Read(a) == k).ToList();
        while (q.Count > 0)
        {
            var nextGen = q.SelectMany(p => p
                    .GetNeighbors().Where(x => CanPaint(a, x, path)))
                .Distinct()
                .ToList();
            foreach (var nextP in nextGen)
                TrySet(a, nextP, k, path);
            q = nextGen;
        }
    }

    private void PrintStats(char[][] a)
    {
        var ins = a.SelectMany(l => l).Count(c => c == 'I');
        var outs = a.SelectMany(l => l).Count(c => c == 'O');
        Console.WriteLine($"I:{ins}, O:{outs}");
    }

    private void PrintMap(char[][] a)
    {
        Console.Clear();
        foreach (var line in a)
            Console.WriteLine(new string(line));
    }

    public long SolvePart1()
    {
        var a = 10.DayInput().Select(l => l.ToCharArray()).ToArray();
        var start = FindStart(a);
        var path = Walk(a, start, null);
        return path.Count / 2;
    }

    public long SolvePart2()
    {
        var a = 10.DayInput().Select(l => l.ToCharArray()).ToArray();
        var start = FindStart(a);
        var path = Walk(a, start, null);
        Walk(a, start, path);
        Extend(a, 'I', path);
        Extend(a, 'O', path);
        PrintStats(a);
        return 0;
    }
}