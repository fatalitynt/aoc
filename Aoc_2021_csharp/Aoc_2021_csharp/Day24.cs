namespace Aoc_2021_csharp;

public class Day24
{
    public enum OpType
    {
        Inp,
        Add,
        Mul,
        Div,
        Mod,
        Eq
    }

    public class Operation
    {
        public int A { get; set; }
        public int B { get; set; }
        private bool ValB { get; set; }
        public OpType Type { get; set; }

        public Operation(string s)
        {
            var p = s.Split(" ");
            Type = ops[p[0]];
            A = d[p[1][0]];
            if (Type != OpType.Inp)
            {
                if (d.TryGetValue(p[2][0], out var i))
                {
                    B = i;
                }
                else
                {
                    B = int.Parse(p[2]);
                    ValB = true;
                }
            }
        }

        public override string ToString() => $"{Type} {A} {B}";

        public void Input(long[] a, long val)
        {
            a[A] = val;
        }

        public void Run(long[] a)
        {
            var b = ValB ? B : a[B];
            switch (Type)
            {
                case OpType.Add:
                    a[A] += b;
                    break;
                case OpType.Mul:
                    a[A] *= b;
                    break;
                case OpType.Div:
                    a[A] /= b;
                    break;
                case OpType.Mod:
                    a[A] %= b;
                    break;
                case OpType.Eq:
                    a[A] = a[A] == b ? 1 : 0;
                    break;
                default:
                    throw new Exception("Wrong command on Run");
            }
        }


        private static Dictionary<string, OpType> ops = new()
        {
            { "inp", OpType.Inp },
            { "add", OpType.Add },
            { "mul", OpType.Mul },
            { "div", OpType.Div },
            { "mod", OpType.Mod },
            { "eql", OpType.Eq },
        };

        private static Dictionary<char, int> d = new()
        {
            { 'x', 0 },
            { 'y', 1 },
            { 'w', 2 },
            { 'z', 3 },
        };
    }

    private long[] Test(Operation[] ops, long[] inputs)
    {
        var iIdx = 0;
        var res = new long[4];
        foreach (var op in ops)
        {
            if (op.Type == OpType.Inp)
            {
                op.Input(res, inputs[iIdx++]);
            }
            else op.Run(res);
        }
        return res;
    }

    private long Op(long z, long i, long[] d)
    {
        // this code is specific to an personal input
        long x = z % 26 + d[1] != i ? 1 : 0;
        z /= d[0];
        z *= x * 25 + 1;
        z += x * (i + d[2]);
        return z;
    }

    private long Run(long[][] deltas, int di, long z0, long b0, int[] indexes)
    {
        var b = b0 * 10;

        foreach(var i in indexes)
        {
            var z = Op(z0, i, deltas[di]);
            if (di + 1 == deltas.Length)
            {
                if (z == 0) return b + i;
                continue;
            }
            var ok = Run(deltas, di + 1, z, b + i, indexes);
            if (ok > 0) return ok;
        }
        return -1;
    }

    private long Solve(bool reversedIndexes)
    {
        var ops = 24.DayInput().Select(l => new Operation(l)).ToArray();

        var ids = ops.Select((x, idx) => (x, idx))
            .Where(x => x.x.Type == OpType.Inp).Select(x => x.idx)
            .ToArray();

        var sections = ids
            .Select(idx => ops.Skip(idx + 1).TakeWhile(x => x.Type != OpType.Inp).ToArray())
            .ToArray();

        var deltas = sections.Select(s => new long[] { s[3].B, s[4].B, s[14].B }).ToArray();

        var indexes = reversedIndexes
            ? Enumerable.Range(1, 9).Reverse().ToArray()
            : Enumerable.Range(1, 9).ToArray();

        var res = Run(deltas, 0, 0, 0, indexes);
        Console.WriteLine(res);

        return 0;
    }

    public long SolvePart1() => Solve(true);
    public long SolvePart2() => Solve(false);
}