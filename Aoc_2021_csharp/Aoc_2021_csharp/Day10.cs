namespace Aoc_2021_csharp;

public class Day10
{
    private readonly Dictionary<char, long> closeFailPoints
        = new() { [')'] = 3, [']'] = 57, ['}'] = 1197, ['>'] = 25137 };

    private readonly Dictionary<char, long> closeCompletePoints
        = new() { [')'] = 1, [']'] = 2, ['}'] = 3, ['>'] = 4 };

    private readonly Dictionary<char, char> openByClose
        = new() { [')'] = '(', [']'] = '[', ['}'] = '{', ['>'] = '<' };

    private readonly Dictionary<char, char> closeByOpen
        = new() { ['('] = ')', ['['] = ']', ['{'] = '}', ['<'] = '>' };

    private long GetCompletionPoints(string line)
    {
        var s = new Stack<char>();
        foreach (var cur in line)
        {
            if (closeFailPoints.ContainsKey(cur))
            {
                if (s.Count > 0 && s.Pop() == openByClose[cur]) continue;
                return 0;
            }
            s.Push(cur);
        }
        var res = 0L;
        while (s.Count > 0)
        {
            res *= 5;
            res += closeCompletePoints[closeByOpen[s.Pop()]];
        }
        return res;
    }

    public long SolvePart1()
    {
        var a = 10.DayInput();
        var res = 0L;
        foreach (var line in a)
        {
            var s = new Stack<char>();
            foreach (var cur in line)
            {
                if (closeFailPoints.TryGetValue(cur, out var pts))
                {
                    if (s.Count > 0 && s.Pop() == openByClose[cur]) continue;
                    res += pts;
                    break;
                }
                s.Push(cur);
            }
        }
        return res;
    }

    public long SolvePart2()
    {
        var a = 10.DayInput().Select(GetCompletionPoints)
            .Where(x => x > 0)
            .OrderBy(x => x)
            .ToArray();
        return a[a.Length / 2];
    }
}