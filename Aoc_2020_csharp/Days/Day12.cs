using System;
using System.IO;

namespace AdventOfCode2020.Days
{
    public static class Day12
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/12.txt");

            var x = 0;
            var y = 0;

            var wx = 10;
            var wy = -1;

            foreach (var line in input)
            {
                var cmd = line[0];
                var val = int.Parse(line.Substring(1));

                if (cmd == 'F')
                {
                    var dx = wx - x;
                    var dy = wy - y;
                    x += dx * val;
                    y += dy * val;
                    
                    wx += dx * val;
                    wy += dy * val;
                }
                else if (cmd == 'L' || cmd == 'R')
                {
                    var w_dx = wx - x;
                    var w_dy = wy - y;
                    var turns = val / 90;
                    var rotDir = cmd == 'L' ? -1 : 1;
                    if (turns == 3)
                    {
                        turns = 1;
                        rotDir = -rotDir;
                    }

                    if (turns == 2)
                    {
                        w_dx = -w_dx;
                        w_dy = -w_dy;
                    }
                    else
                    {
                        var tmp = w_dx;
                        w_dx = w_dy * -rotDir;
                        w_dy = tmp * rotDir;
                    }

                    wx = x + w_dx;
                    wy = y + w_dy;
                }
                else
                {
                    var dir = GetDxDy(GetDir(cmd));
                    wx += dir.Dx * val;
                    wy += dir.Dy * val;
                }
            }

            Console.WriteLine($"{Math.Abs(x) + Math.Abs(y)}");
        }

        private static int Rotate(int dIdx, int delta, int turns)
        {
            for (var i = 0; i < turns; i++)
            {
                dIdx += delta;
                if (dIdx == Directions.Length) dIdx = 0;
                if (dIdx < 0) dIdx = Directions.Length - 1;
            }

            return dIdx;
        }
        
        private static readonly Dir[] Directions = {Dir.N, Dir.E, Dir.S, Dir.W};

        private static Dir GetDir(char c)
        {
            switch (c)
            {
                case 'N': return Dir.N;
                case 'E': return Dir.E;
                case 'S': return Dir.S;
                case 'W': return Dir.W;
                default: throw new Exception();
            }
        }

        private static (int Dx, int Dy) GetDxDy(Dir dir)
        {
            switch (dir)
            {
                case Dir.N:
                    return (0, -1);
                case Dir.W:
                    return (-1, 0);
                case Dir.S:
                    return (0, 1);
                case Dir.E:
                    return (1, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }

        enum Dir
        {
            N,
            W,
            S,
            E
        }
    }
}

/*
 --------------------------------------------------
 * A good example by https://github.com/xoposhiy  *
 * how can such task can be solved with modern C# *
 --------------------------------------------------
 *
 * Original file: https://github.com/xoposhiy/aoc2020/blob/main/12_ferry.cs
 * 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Linq.Enumerable;
using static System.Math;

public class Day12
{
    public record Command(char dir, int count);
    public record Ferry(char dir, int x, int y);
    public record Point(int x, int y);

    public void Solve()
    {
        var ns = File.ReadLines("12.txt")
            .Select(s => new Command(s[0], int.Parse(s.Substring(1))))
            .ToArray();
        var ferry = new Ferry('E', x: 0, y: 0);
        ferry = ns.Aggregate(ferry, Move);

        Console.WriteLine($"Part One: {Abs(ferry.x) + Abs(ferry.y)}");
        var wp = new Point(x: 10, y: -1);
        var pos = new Point(x: 0, y: 0);
        foreach (var step in ns)
        {
            if (step.dir == 'F')
                pos = new Point(pos.x + wp.x * step.count, pos.y + wp.y * step.count);
            else
                wp = MoveWayPoint(wp, step);
        }
        Console.WriteLine($"Part Two: {Abs(pos.x) + Abs(pos.y)}");
    }

    private Point MoveWayPoint(Point wp, Command cmd)
    {
        return cmd switch
        {
            ('R', var len) => RotateWpRight(wp, (len / 90) % 4),
            ('L', var len) => RotateWpRight(wp, 4-(len / 90) % 4),
            var (dir, len) => new Point(wp.x + deltas[dir].x * len, wp.y + deltas[dir].y * len)
        };
    }

    private Point RotateWpRight(Point wp, int len)
    {
        for (int i = 0; i < len; i++)
            wp = new Point(-wp.y, wp.x);
        return wp;
    }

    private static readonly Dictionary<char, char> nextDir = new()
    {
        {'E', 'S'},
        {'S', 'W'},
        {'W', 'N'},
        {'N', 'E'},
    };

    private static readonly Dictionary<char, (int x, int y)> deltas= new()
    {
        {'E', (1, 0)},
        {'S', (0, 1)},
        {'W', (-1, 0)},
        {'N', (0, -1)},
    };

    private Ferry Move(Ferry ferry, Command cmd)
    {
        return cmd switch
        {
            ('R', var len) => ferry with { dir = RotateRight(ferry.dir, (len / 90) % 4) },
            ('L', var len) => ferry with { dir = RotateRight(ferry.dir, 4 - (len / 90) % 4) },
            ('F', var len) => ferry with { x = ferry.x + deltas[ferry.dir].x * len, y = ferry.y + deltas[ferry.dir].y * len },
            var (dir, len) => new Ferry(ferry.dir, ferry.x + deltas[dir].x * len, ferry.y + deltas[dir].y * len)
        };
    }

    private char RotateRight(char dir, int count)
    {
        for (int i = 0; i < count; i++)
            dir = nextDir[dir];
        return dir;
    }
} 
 */