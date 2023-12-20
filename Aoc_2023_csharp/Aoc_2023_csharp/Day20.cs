namespace Aoc_2023_csharp;

public class Day20
{
    private Dictionary<string, (char, string[])> ParseInput(string[] a)
    {
        var d = new Dictionary<string, (char, string[])>();
        foreach (var s in a)
        {
            var p = s.Split(" -> ");
            var dist = p[1].Split(", ").ToArray();
            d[p[0].Substring(1)] = (p[0][0], dist);
        }
        return d;
    }

    private const string B = "roadcaster"; // yes, there is no 'b- in the word.

    private (long, long) HandlePulses(Dictionary<string, (char, string[])> d,
        Dictionary<string, int> ff,
        Dictionary<string, Dictionary<string, int>> cj,
        string trg = null)
    {
        var cntL = 0L;
        var cntH = 0L;
        var q = new Queue<(string, int, string)>();
        var initDist = d[B].Item2;
        foreach (var initD in initDist) q.Enqueue((initD, 0, B));

        while (q.Count > 0)
        {
            var (target, val, from) = q.Dequeue();
            if (trg == null || trg == target)
            {
                if (val == 0) cntL++;
                else cntH++;
            }
            if (!d.TryGetValue(target, out var data)) continue;
            var (type, nextDist) = data;
            if (type == '%')
            {
                if (val > 0) continue;
                ff[target] = 1 - ff[target];
                foreach (var dist in nextDist) q.Enqueue((dist, ff[target], target));
            }
            else if (type == '&')
            {
                var myMap = cj[target];
                myMap[from] = val;
                var toSend = myMap.Values.Sum() == myMap.Count ? 0 : 1;
                foreach (var dist in nextDist) q.Enqueue((dist, toSend, target));
            }
        }
        return (cntL, cntH);
    }

    private Dictionary<string, Dictionary<string, int>> BuildCjMap(
        Dictionary<string, (char, string[])> d)
    {
        var cj = d.Where(x => x.Value.Item1 == '&')
            .ToDictionary(x => x.Key, _ => new Dictionary<string, int>());
        foreach (var kvp in d)
        foreach (var dist in kvp.Value.Item2)
            if (cj.TryGetValue(dist, out var value)) value[kvp.Key] = 0;
        return cj;
    }

    public long SolvePart1()
    {
        var d = ParseInput(20.DayInput());
        var ff = d.Where(x => x.Value.Item1 == '%').ToDictionary(x => x.Key, _ => 0);
        var cj = BuildCjMap(d);

        var cntL = 0L;
        var cntH = 0L;
        for (var i = 1; i <= 1000; i++)
        {
            var (r1, r2) = HandlePulses(d, ff, cj);
            cntL += r1 + 1;
            cntH += r2;
        }
        return cntL * cntH;
    }

    private string[] FindKeys(Dictionary<string, (char, string[])> d, int l, string[] start)
    {
        var keys = d
            .Where(x => x.Value.Item2.Any(start.Contains))
            .Select(x => x.Key)
            .ToArray();

        return l == 0 ? keys : FindKeys(d, l - 1, keys);
    }

    public long SolvePart2()
    {
        var d = ParseInput(20.DayInput());
        var keys = FindKeys(d, 1, new [] {"rx"});
        return keys.Select(k =>
        {
            var ff = d.Where(x => x.Value.Item1 == '%').ToDictionary(x => x.Key, _ => 0);
            var cj = BuildCjMap(d);
            for (var i = 1L;; i++)
            {
                if (HandlePulses(d, ff, cj, k).Item1 == 1) return i;
            }
        }).Aggregate(1L, (cur, next) => cur * next);
    }
}