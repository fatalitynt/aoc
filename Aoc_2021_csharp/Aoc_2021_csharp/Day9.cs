namespace Aoc_2021_csharp;

public class Day9
{
    public long SolvePart1()
    {
        var a = 9.DayInput().Select(x => x.Select(c => c - '0').ToArray()).ToArray();
        return P.GetAllPoints(a)
            .Where(p => p.GetValidNeighbors(a).All(x => p.Read(a) < x.Read(a)))
            .Select(x => x.Read(a))
            .Aggregate(0L, (cur, next) => cur + 1 + (next - '0'));
    }

    private long GetPool(P s, int[][] a)
    {
        var visited = new HashSet<P>();
        var q = new Queue<P>();
        visited.Add(s);
        q.Enqueue(s);
        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            var cVal = cur.Read(a);
            foreach (var neigh in cur.GetValidNeighbors(a))
            {
                if (neigh.TryRead(a, out var nVal) && nVal != 9 && nVal > cVal)
                {
                    q.Enqueue(neigh);
                    visited.Add(neigh);
                }
            }
        }
        return visited.Count;
    }

    public long SolvePart2()
    {
        var a = 9.DayInput().Select(x => x.Select(c => c - '0').ToArray()).ToArray();
        var lowP = P.GetAllPoints(a)
            .Where(p => p.GetValidNeighbors(a).All(x => p.Read(a) < x.Read(a))).ToArray();
        return lowP.Select(p => GetPool(p, a)).OrderByDescending(x => x)
            .Take(3)
            .Aggregate(1L, (cur, next) => cur * next);
    }
}