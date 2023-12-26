namespace Aoc_2021_csharp;

public class Day4
{
    class Board
    {
        public int WinIdx { get; set; } = 0;
        public int[][] A { get; set; }

        public void Mark(int val)
        {
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (A[i][j] == val) A[i][j] = -1;
                }
            }
        }

        public bool IsWinner()
        {
            for (var i = 0; i < 5; i++)
            {
                if (Enumerable.Range(0, 5).All(j => A[i][j] == -1)) return true;
                if (Enumerable.Range(0, 5).All(j => A[j][i] == -1)) return true;
            }
            return false;
        }

        public int GetScore() => A.SelectMany(x => x).Where(x => x >= 0).Sum();
    }


    public int SolvePart1()
    {
        var a = 4.DayInput().ToArray();
        var n = (a.Length - 1) / 6;
        var boards = Enumerable.Range(0, n).Select(i => new Board
        {
            A = Enumerable.Range(2 + i * 6, 5).Select(j =>
                a[j].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray()
        }).ToArray();

        foreach (var s in a[0].Split(","))
        {
            var val = int.Parse(s);
            foreach (var b in boards)
            {
                b.Mark(val);
                if (b.IsWinner()) return b.GetScore() * val;
            }
        }

        return -1;
    }

    public int SolvePart2()
    {
        var a = 4.DayInput().ToArray();
        var n = (a.Length - 1) / 6;
        var boards = Enumerable.Range(0, n).Select(i => new Board
        {
            A = Enumerable.Range(2 + i * 6, 5).Select(j =>
                a[j].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray()
        }).ToArray();

        var winIdx = 1;
        foreach (var s in a[0].Split(","))
        {
            var val = int.Parse(s);
            foreach (var b in boards)
            {
                if (b.WinIdx != 0) continue;
                b.Mark(val);
                if (b.IsWinner()) b.WinIdx = winIdx++;
            }
            if (boards.All(x => x.WinIdx != 0))
                return boards.OrderByDescending(x => x.WinIdx).First().GetScore() * val;
        }

        return -1;
    }
}