namespace Aoc_2023_csharp;

public class Day17
{
    private P Move(P p, int dir) => dir switch
    {
        0 => p.Up,
        1 => p.Right,
        2 => p.Down,
        3 => p.Left,
        _ => throw new Exception()
    };

    private static readonly int[][] Turns = {
        new[] { 1, 3 },
        new[] { 0, 2 },
        new[] { 1, 3 },
        new[] { 0, 2 },
        new[] { 0, 1, 2, 3 },
    };

    private int[] Solve(int minLimit, int maxLimit)
    {
        var a = 17.DayInput().Select(x => x.Select(y => y - '0').ToArray()).ToArray();
        var dp = a.Length.MakeArr(() => a[0].Length.MakeArr(() => new int[4]));
        var q = new Queue<(P, int, int)>();

        q.Enqueue((P.C(0, 0), 4, 0));

        while (q.Count > 0)
        {
            var (p, dir, cost) = q.Dequeue();
            foreach (var newDir in Turns[dir])
            {
                var newCost = cost;
                var newP = p;
                for (var i = 1; i <= maxLimit; i++)
                {
                    newP = Move(newP, newDir);
                    if (!newP.TryRead(a, out var price)) break;
                    newCost += price;
                    if (i < minLimit) continue;
                    var nextArr = newP.Read(dp);
                    if (nextArr[newDir] > 0 && nextArr[newDir] <= newCost) continue;
                    nextArr[newDir] = newCost;
                    q.Enqueue((newP, newDir, newCost));
                }
            }
        }
        return dp.Last().Last();
    }

    public long SolvePart1() => Solve(1, 3).Where(x => x > 0).Min();

    public long SolvePart2() => Solve(4, 10).Where(x => x > 0).Min();
}