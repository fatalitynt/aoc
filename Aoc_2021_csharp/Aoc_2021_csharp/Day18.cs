namespace Aoc_2021_csharp;

public class Day18
{
    private T[] Arr<T>(params T[] t) => t;

    private record Node(Node L, Node R, int? V);

    private Node Parse(int[] a, int p, out int p1)
    {
        p1 = p + 1;
        if (a[p] >= 0) return new Node(null, null, a[p]);
        var res = new Node(Parse(a, p + 1, out p), Parse(a, p + 1, out p), null);
        p1 = p + 1;
        return res;
    }

    private long Mag(Node p) => p.V ?? Mag(p.L) * 3 + Mag(p.R) * 2;

    private int[] ToMagicIntArr(string s)
    {
        var a = new int[s.Length];
        for (var i = 0; i < s.Length; i++)
        {
            if (char.IsDigit(s[i])) a[i] = s[i] - '0';
            else if (s[i] == '[') a[i] = -1;
            else if (s[i] == ',') a[i] = -2;
            else if (s[i] == ']') a[i] = -3;
        }
        return a;
    }

    private int[] Add(int[] a, int[] b)
    {
        return Arr(-1)
            .Concat(a.ToArray())
            .Concat(Arr(-2))
            .Concat(b.ToArray())
            .Concat(Arr(-3))
            .ToArray();
    }

    private int[] Reduce(int[] a)
    {
        var runAgain = true;
        while (runAgain)
        {
            a = Explode(a, out runAgain);
            if (!runAgain) a = Split(a, out runAgain);
        }
        return a;
    }

    private int[] Explode(int[] a, out bool explode)
    {
        explode = false;
        var cnt = 0;
        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] == -3) cnt--;
            if (a[i] != -1) continue;
            cnt++;
            if (cnt < 5) continue;

            explode = true;

            for (var j = i - 1; j >= 0; j--)
            {
                if (a[j] < 0) continue;
                a[j] += a[i + 1];
                break;
            }

            for (var j = i + 5; j < a.Length; j++)
            {
                if (a[j] < 0) continue;
                a[j] += a[i + 3];
                break;
            }

            for (var j = 0; j < 5; j++) a[i + j] = -4;
            a[i] = 0;
            break;
        }
        return !explode ? a : a.Where(x => x != -4).ToArray();
    }

    private int[] Split(int[] a, out bool split)
    {
        split = false;
        for (var i = 0; i < a.Length; i++)
        {
            if (a[i] > 9)
            {
                split = true;
                var l = a[i] / 2;
                var r = (a[i] + 1) / 2;
                return a.Take(i)
                    .Concat(Arr(-1, l, -2, r, -3))
                    .Concat(a.Skip(i + 1))
                    .ToArray();
            }
        }
        return a;
    }

    public long SolvePart1()
    {
        var a = 18.DayInput().Select(ToMagicIntArr).ToArray();
        var r = a[0];
        for (var i = 1; i < a.Length; i++)
        {
            r = Add(r, a[i]);
            r = Reduce(r);
        }
        return Mag(Parse(r, 0, out var _));
    }

    public long SolvePart2()
    {
        var a = 18.DayInput().Select(ToMagicIntArr).ToArray();
        var maxMag = long.MinValue;
        for (var i = 0; i < a.Length; i++)
        for (var j = 0; j < a.Length; j++)
        {
            if (i == j) continue;
            var r = Reduce(Add(a[i], a[j]));
            maxMag = Math.Max(maxMag, Mag(Parse(r, 0, out var _)));
        }
        return maxMag;
    }
}