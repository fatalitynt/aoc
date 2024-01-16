using System.Text;

namespace Aoc_2021_csharp;

public class Day23
{
    private class State
    {
        public readonly char[] Buf;
        public readonly char[] Stacks;

        private readonly int[] costs = { 1, 10, 100, 1000 };
        private readonly int[] realPos = { 0, 1, 3, 5, 7, 9, 10 };
        private readonly int stackSize;

        public State(string data, int stackSize)
        {
            this.stackSize = stackSize;
            Buf = new char[7];
            Stacks = new char[stackSize * 4];
            for (var i = 0; i < 7; i++) Buf[i] = data[i];
            for (var i = 0; i < stackSize * 4; i++) Stacks[i] = data[7 + i];
        }

        public int Cost(char c) => costs[c - 'A'];
        public string Key() => $"{new string(Buf)}{new string(Stacks)}";

        private IEnumerable<char> Stack(int i)
        {
            for (var j = 0; j < stackSize; j++)
                yield return Stacks[i * stackSize + j];
        }

        public int Dist(int bIdx, int sIdx, bool startShouldBeEmpty)
        {
            if (startShouldBeEmpty && Buf[bIdx] != '.') return -1;

            var start = realPos[bIdx];
            var end = (sIdx + 1) * 2;
            var steps = 0;

            for (var i = start; i != end;)
            {
                var di = Math.Sign(end - start);
                var nxt = i + di;
                if (nxt % 2 == 0 || Buf[(nxt + 1) / 2] == '.')
                {
                    steps++;
                    i = nxt;
                    continue;
                }
                return -1;
            }
            return steps;
        }

        public bool TryInsert(int bIdx, out int cost, out int idx)
        {
            cost = -1;
            idx = -1;
            var letter = Buf[bIdx];
            if (letter == '.') return false;
            var sIdx = letter - 'A';

            var ok = Stack(sIdx).All(c => c == letter || c == '.');
            if (!ok) return false;

            var steps = Dist(bIdx, sIdx, false);
            if (steps < 0) return false;

            for (var i = stackSize - 1; i >= 0; i--)
            {
                if (Stacks[sIdx * stackSize + i] == '.')
                {
                    cost = (steps + i + 1) * costs[sIdx];
                    idx = sIdx * stackSize + i;
                    return true;
                }
            }
            return false;
        }
    }

    private string Parse(string[] a, int stackSize)
    {
        var data = new char[7 + 4 * stackSize];
        for (var i = 0; i < 7; i++) data[i] = a[1][1 + i];

        for (var sIdx = 0; sIdx < 4; sIdx++)
        {
            for (var sDeep = 0; sDeep < stackSize; sDeep++)
            {
                data[7 + sDeep + sIdx * stackSize] = a[2 + sDeep][3 + sIdx * 2];
            }
        }
        return new string(data);
    }

    private IEnumerable<(string, int)> GetOptions(string str, int ss)
    {
        var s = new State(str, ss);

        // try insert someone at i in Buff
        for (var i = 0; i < 7; i++)
        {
            if (s.Buf[i] == '.' || !s.TryInsert(i, out var cost, out var sIdx))
                continue;
            (s.Buf[i], s.Stacks[sIdx]) = (s.Stacks[sIdx], s.Buf[i]);
            yield return (s.Key(), cost);
            (s.Buf[i], s.Stacks[sIdx]) = (s.Stacks[sIdx], s.Buf[i]);
        }

        // try extract someone from sIdx
        for (var sIdx = 0; sIdx < 4; sIdx++)
        {
            var d = 0;
            var c = '.';
            for (; d < ss; d++)
            {
                c = s.Stacks[sIdx * ss + d];
                if (c != '.') break;
            }
            if (c == '.') continue;
            for (var bIdx = 0; bIdx < 7; bIdx++)
            {
                var dist = s.Dist(bIdx, sIdx, true);
                if (dist < 0) continue;
                (s.Buf[bIdx], s.Stacks[sIdx * ss + d]) = (s.Stacks[sIdx * ss + d], s.Buf[bIdx]);
                yield return (s.Key(), (d + 1 + dist) * s.Cost(c));
                (s.Buf[bIdx], s.Stacks[sIdx * ss + d]) = (s.Stacks[sIdx * ss + d], s.Buf[bIdx]);
            }
        }
    }

    private string GetEnd(int stackSize)
    {
        const string letters = "ABCD";

        var sb = new StringBuilder();
        for (var i = 0; i < 7; i++) sb.Append('.');
        foreach(var l in letters) for (var i = 0; i < stackSize; i++) sb.Append(l);
        return sb.ToString();
    }

    private long Solve(bool extend)
    {
        var a = 23.DayInput();
        var stackSize = extend ? 4 : 2;
        if (extend)
        {
            a = a.Take(3)
                .Concat(new[] { "  #D#C#B#A#", "  #D#B#A#C#" })
                .Concat(a.Skip(3)).ToArray();
        }
        var start = Parse(a, stackSize);
        var end = GetEnd(stackSize);
        var d = new Dictionary<string, int>();
        var q = new Queue<string>();
        d[start] = 0;
        q.Enqueue(start);

        while (q.Count > 0)
        {
            var s = q.Dequeue();
            var spent = d[s];
            foreach (var (next, cost) in GetOptions(s, stackSize))
            {
                if (!d.TryGetValue(next, out var otherSpent) || spent + cost < otherSpent)
                {
                    d[next] = spent + cost;
                    if (next == end) continue;
                    q.Enqueue(next);
                }
            }
        }
        return d[end];
    }

    public long SolvePart1() => Solve(false);
    public long SolvePart2() => Solve(true);
}