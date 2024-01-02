namespace Aoc_2021_csharp;

public class Day11
{
    private void Increase(int[][] a)
    {
        foreach (var p in P.GetAllPoints(a)) p.Update(a, x => x + 1);
    }

    private long CountFlashes(int[][] a)
    {
        var q = new HashSet<P>();
        var all = new HashSet<P>();
        foreach (var p in P.GetAllPoints(a).Where(x => x.Read(a) > 9))
        {
            all.Add(p);
            q.Add(p);
        }
        while (q.Count > 0)
        {
            var q2 = new HashSet<P>();
            foreach (var cur in q)
            {
                foreach (var next in cur.Get8ValidNeighbors(a).Where(x => !all.Contains(x)))
                {
                    next.Update(a, x => x + 1);
                    if (next.Read(a) > 9)
                    {
                        all.Add(next);
                        q2.Add(next);
                    }
                }
            }
            q = q2;
        }
        foreach (var cur in all) cur.Write(a, 0);
        return all.Count;
    }

    public long SolvePart1()
    {
        var a = 11.DayInput().Select(x => x.Select(c => c - '0').ToArray()).ToArray();
        var res = 0L;
        for (var i = 0; i < 100; i++)
        {
            Increase(a);
            res += CountFlashes(a);
        }
        return res;
    }

    public long SolvePart2()
    {
        var a = 11.DayInput().Select(x => x.Select(c => c - '0').ToArray()).ToArray();
        var i = 0;
        while (++i > 0)
        {
            Increase(a);
            if (CountFlashes(a) == 100) return i;
        }
        return -1;
    }
}