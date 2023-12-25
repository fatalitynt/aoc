namespace Aoc_2023_csharp;

public class Day25
{
    public class Graph
    {
        public Dictionary<int, HashSet<int>> Map = new();

        public void AddEdge(int u, int v, bool biDirect = true)
        {
            GetHs(u).Add(v);
            if (biDirect) GetHs(v).Add(u);
        }
        private HashSet<int> GetHs(int k) => Map.TryGetValue(k, out var hs) ? hs : Map[k] = new HashSet<int>();
    }

    private int GetId(string name, Dictionary<string, int> names)
    {
        if (names.TryGetValue(name, out var id)) return id;
        return names[name] = names.Count;
    }

    //MaxFlow-MinCut https://codeforces.com/blog/entry/78927
    private static long MaxFlow(int s, int t, Graph g)
    {
        if (s == t) return -1;
        var maxFlow = 0L;
        var map = g.Map;
        //
        var flows = Enumerable.Range(0, map.Count).Select(_ => new int[map.Count]).ToArray();
        foreach (var kvp in map)
        foreach (var i in kvp.Value)
            flows[kvp.Key][i] = 1;

        while (true)
        {
            var parent = new Dictionary<int, int>();
            var used = new HashSet<int>();
            var q = new Queue<int>();

            used.Add(s);
            q.Enqueue(s);

            while (q.Count > 0)
            {
                var v = q.Dequeue();
                foreach (var i in g.Map[v])
                {
                    if (!used.Contains(i) && flows[v][i] > 0)
                    {
                        parent[i] = v;
                        used.Add(i);
                        q.Enqueue(i);
                    }
                }
            }

            if (!used.Contains(t))
                break;

            var augFlow = int.MaxValue;

            var ptr = t;
            while (ptr != s)
            {
                augFlow = Math.Min(augFlow, flows[parent[ptr]][ptr]);
                ptr = parent[ptr];
            }
            ptr = t;
            while (ptr != s)
            {
                flows[parent[ptr]][ptr] -= augFlow;
                flows[ptr][parent[ptr]] += augFlow;
                ptr = parent[ptr];
            }
            maxFlow += augFlow;
        }

        return maxFlow;
    }

    public long SolvePart1()
    {
        var lines = 25.DayInput();
        var names = new Dictionary<string, int>();
        var g = new Graph();
        foreach (var line in lines)
        {
            var parts = line.Split(": ");
            var children = parts[1].Split(" ");
            var id = GetId(parts[0], names);
            foreach (var ch in children) g.AddEdge(id, GetId(ch, names));
        }

        var n = g.Map.Count;
        var r = g.Map.Keys.Count(x => MaxFlow(x, 0, g) == 3);
        return r * (n - r);
    }
}