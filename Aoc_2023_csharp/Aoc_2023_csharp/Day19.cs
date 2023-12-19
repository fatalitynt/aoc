namespace Aoc_2023_csharp;

public class Day19
{
    private static T[] Arr<T>(params T[] a) => a;

    class Rules
    {
        private readonly Dictionary<char, int> actionMap = new()
        {
            { 'x', 0 },
            { 'm', 1 },
            { 'a', 2 },
            { 's', 3 },
        };

        private readonly (int, char, int, string)[] expressions;

        public Rules(string s)
        {
            s = s.Substring(0, s.Length - 1);
            var parts = s.Split(",");
            expressions = parts.Select(GetEx).ToArray();
        }

        private (int, char, int, string) GetEx(string s)
        {
            var p = s.Split(":");
            if (p.Length == 1) return (-1, 'x', -1, p[0]);
            var val = p[0].Substring(2);
            return (actionMap[p[0][0]], p[0][1], int.Parse(val), p[1]);
        }

        private int[][] RunExpression(int i, Dictionary<string, List<int[][]>> data, int[][] range)
        {
            var (idx, op, val, res) = expressions[i];
            if (!data.ContainsKey(res)) data[res] = new List<int[][]>();
            if (idx < 0)
            {
                data[res].Add(range);
                return null;
            }
            if (op == '>')
            {
                var section = range[idx];
                if (section[0] > val)
                {
                    data[res].Add(range);
                    return null;
                }
                if (section[1] > val)
                {
                    data[res].Add(ReRange(range, idx, Arr(val + 1, section[1])));
                    return ReRange(range, idx, Arr(section[0], val));
                }
            }
            if (op == '<')
            {
                var section = range[idx];
                if (section[1] < val)
                {
                    data[res].Add(range);
                    return null;
                }
                if (section[0] < val)
                {
                    data[res].Add(ReRange(range, idx, Arr(section[0], val - 1)));
                    return ReRange(range, idx, Arr(val, section[1]));
                }
            }
            return null;
        }

        private void RangeEvalSingle(int[][] range, Dictionary<string, List<int[][]>> data)
        {
            for (var i = 0; i < expressions.Length && range != null; i++)
                range = RunExpression(i, data, range);
        }

        public void RangeEval(List<int[][]> ranges, Dictionary<string, List<int[][]>> data)
        {
            foreach (var range in ranges) RangeEvalSingle(range, data);
        }

        private int[][] ReRange(int[][] range, int i, int[] section) => range
            .Select((x, idx) => idx == i ? section : x)
            .ToArray();
    }

    private int[] Parse(string s)
    {
        s = s.Substring(1, s.Length - 1);
        s = s.Substring(0, s.Length - 1);
        return s.Split(",").Select(x => int.Parse(x.Split("=")[1])).ToArray();
    }

    private bool KeyHasData(string x, Dictionary<string, List<int[][]>> data) =>
        x != "R" && x != "A" && data[x] != null && data[x].Count > 0;

    private List<int[][]> MoveRanges(Dictionary<string, Rules> rules, List<int[][]> inRanges)
    {
        var data = new Dictionary<string, List<int[][]>> { ["in"] = inRanges };
        while (data.Keys.Any(k => KeyHasData(k, data)))
        {
            var keys = data.Keys.Where(k => KeyHasData(k, data)).ToArray();
            foreach (var key in keys)
            {
                var rule = rules[key];
                var input = data[key];
                data[key] = null;
                rule.RangeEval(input, data);
            }
        }
        return data["A"];
    }

    private Dictionary<string, Rules> BuildRules(string[] a)
    {
        return a.TakeWhile(x => !string.IsNullOrEmpty(x))
            .Select(x => x.Split("{"))
            .ToDictionary(x => x[0], x => new Rules(x[1]));
    }

    public long SolvePart1()
    {
        var a = 19.DayInput();
        var rules = BuildRules(a);
        var inRanges = a.Skip(rules.Count + 1)
            .Select(Parse)
            .Select(x => Arr(Arr(x[0], x[0]), Arr(x[1], x[1]), Arr(x[2], x[2]), Arr(x[3], x[3])))
            .ToList();
        var acc = MoveRanges(rules, inRanges);
        return acc.Sum(x => (long)x.Sum(y => y[0]));
    }

    public long SolvePart2()
    {
        var a = 19.DayInput();
        var rules = BuildRules(a);
        var inRange = Arr(Arr(1, 4000), Arr(1, 4000), Arr(1, 4000), Arr(1, 4000));
        var acc = MoveRanges(rules, new List<int[][]> { inRange });
        return acc
            .Select(r => r.Aggregate(1L, (cur, s) => cur * (s[1] - s[0] + 1)))
            .Sum();
    }
}