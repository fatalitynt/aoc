namespace Aoc_2021_csharp;

public class Day12
{
    public class Graph
    {
        public Dictionary<string, HashSet<string>> Map = new Dictionary<string, HashSet<string>>();
        public void AddEdge(string[] e, bool biDirect = true) => AddEdge(e[0], e[1], biDirect);

        public void AddEdge(string u, string v, bool biDirect = true)
        {
            GetHs(u).Add(v);
            if (biDirect) GetHs(v).Add(u);
        }

        private HashSet<string> GetHs(string k) => Map.TryGetValue(k, out var hs) ? hs : Map[k] = new HashSet<string>();
    }

    public static long Dfs(Dictionary<string, HashSet<string>> map, string from, int maxVisit)
    {
        var visited = new Dictionary<string, int>();
        foreach (var k in map.Keys) visited[k] = 0;

        var res = 0L;
        var stack = new Stack<string>();
        stack.Push(from);

        while (stack.Count > 0)
        {
            var cur = stack.Pop();
            if (cur.All(char.IsLower)) visited[cur]++;
            if (cur.StartsWith("!"))
            {
                visited[cur[1..]]--;
                continue;
            }

            foreach (var next in map[cur])
            {
                if (visited[next] != 0 && visited.Values.Any(x => x == maxVisit)) continue;
                if (next == "start") continue;
                if (next == "end" && ++res > 0) continue;
                if (next.All(char.IsLower)) stack.Push($"!{next}");
                stack.Push(next);
            }
        }
        return res;
    }

    private long Solve(int maxLimit)
    {
        var a = 12.DayInput();
        var g = new Graph();
        foreach (var line in a) g.AddEdge(line.Split("-"));
        return Dfs(g.Map, "start", maxLimit);
    }

    public long SolvePart1() => Solve(1);
    public long SolvePart2() => Solve(2);
}