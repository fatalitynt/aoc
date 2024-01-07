namespace Aoc_2021_csharp;

public class Day16
{
    private Dictionary<char, string> map = new()
    {
        ['0'] = "0000",
        ['1'] = "0001",
        ['2'] = "0010",
        ['3'] = "0011",
        ['4'] = "0100",
        ['5'] = "0101",
        ['6'] = "0110",
        ['7'] = "0111",
        ['8'] = "1000",
        ['9'] = "1001",
        ['A'] = "1010",
        ['B'] = "1011",
        ['C'] = "1100",
        ['D'] = "1101",
        ['E'] = "1110",
        ['F'] = "1111",
    };
    class Packet
    {
        public int Version { get; private set; }
        public int Type { get; private set; }
        public List<Packet> Subs { get; private set; }
        public long Val { get; private set; }

        private static long ParseLong(string binStr, int of, out int end)
        {
            end = 0;
            var c = new List<char>();
            while (true)
            {
                end = of + 5;
                c.AddRange(Enumerable.Range(0, 4).Select(x => binStr[of + 1 + x]));
                if (binStr[of] == '0') break;
                of += 5;
            }
            return Convert.ToInt64(new string(c.ToArray()), 2);
        }

        public int GetSumOfVersion()
        {
            return Version + Subs.Sum(x => x.GetSumOfVersion());
        }

        private IEnumerable<long> SubVal() => Subs.Select(x => x.Evaluate());

        private int BinOperator(Func<long, long, bool> op)
        {
            var sub = SubVal().ToArray();
            return op(sub[0], sub[1]) ? 1 : 0;
        }

        public long Evaluate()
        {
            return Type switch
            {
                0 => SubVal().Sum(),
                1 => SubVal().Aggregate(1L, (cur, next) => cur * next),
                2 => SubVal().Min(),
                3 => SubVal().Max(),
                4 => Val,
                5 => BinOperator((a, b) => a > b),
                6 => BinOperator((a, b) => a < b),
                7 => BinOperator((a, b) => a == b),
                _ => throw new Exception("unknown type")
            };
        }

        public static Packet Parse(string binStr, int of, out int end)
        {
            end = 0;
            var version = Convert.ToInt32(binStr.Substring(of, 3), 2);
            of += 3;
            var type = Convert.ToInt32(binStr.Substring(of, 3), 2);
            of += 3;
            var val = 0L;
            var subs = new List<Packet>();

            if (type == 4)
            {
                val = ParseLong(binStr, of, out end);
            }
            else
            {
                var lenType = binStr[of] - '0';
                of += 1;
                if (lenType == 0)
                {
                    var totalSubsLen = Convert.ToInt32(binStr.Substring(of, 15), 2);
                    of += 15;
                    var expectedEnd = of + totalSubsLen;
                    while (end != expectedEnd)
                    {
                        subs.Add(Parse(binStr, of, out end));
                        of = end;
                    }
                }
                if (lenType == 1)
                {
                    var subsCnt = Convert.ToInt32(binStr.Substring(of, 11), 2);
                    of += 11;
                    for (var i = 0; i < subsCnt; i++)
                    {
                        subs.Add(Parse(binStr, of, out end));
                        of = end;
                    }
                }
            }

            return new Packet
            {
                Version = version,
                Type = type,
                Val = val,
                Subs = subs
            };
        }
    }

    private Packet GetPacket()
    {
        var a = 16.DayInput();
        var binStr = new String(a[0].SelectMany(c => map[c].ToCharArray()).ToArray());
        return Packet.Parse(binStr, 0, out var end);
    }

    public long SolvePart1() => GetPacket().GetSumOfVersion();
    public long SolvePart2() => GetPacket().Evaluate();
}