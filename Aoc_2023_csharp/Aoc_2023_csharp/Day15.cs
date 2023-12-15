namespace Aoc_2023_csharp;

public class Day15
{
    private long Hash(string s)
    {
        var val = 0L;
        foreach (var c in s)
        {
            var i = (int)c;
            val += i;
            val *= 17;
            val %= 256;
        }
        return val;
    }

    private void UpdateBox(string line, List<string[]>[] boxes)
    {
        if (line.EndsWith('-'))
        {
            var label = line.Substring(0, line.Length - 1);
            var id = Hash(label);
            boxes[id] = boxes[id].Where(x => x[0] != label).ToList();
        }
        else
        {
            var lens = line.Split("=");
            var id = Hash(lens[0]);
            var replaced = false;
            foreach (var l in boxes[id].Where(l => l[0] == lens[0]))
            {
                l[1] = lens[1];
                replaced = true;
            }
            if (!replaced)
                boxes[id].Add(lens);
        }
    }

    private long GetFocusPower(List<string[]>[] boxes)
    {
        var res = 0L;
        for (var i = 0; i < 256; i++)
        for (var j = 0; j < boxes[i].Count; j++)
            res += (i + 1) * (j + 1) * int.Parse(boxes[i][j][1]);
        return res;
    }

    public long SolvePart1() => 15.DayInput()[0].Split(",").Select(Hash).Sum();

    public long SolvePart2()
    {
        var a = 15.DayInput()[0].Split(",");
        var boxes = Enumerable
            .Range(0, 256)
            .Select(_ => new List<string[]>())
            .ToArray();
        foreach (var line in a) UpdateBox(line, boxes);
        return GetFocusPower(boxes);
    }
}