using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day08
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/08.txt");

            var changes = input
                .Select((x, idx) => (cmd: x, idx: idx))
                .Where(x => !x.cmd.StartsWith("a"))
                .ToArray();

            var upd = changes.Select(ch =>
            {
                var ex = new Ex();
                var inputCopy = input.Select(x => x).ToArray();

                var newLine =
                    (ch.cmd.StartsWith("n") ? "jmp" : "nop")
                    + ch.cmd.Substring(3);
                inputCopy[ch.idx] = newLine;

                ex.Run(inputCopy);

                return (acc: ex.Acc, st: ex.State);

            }).Where(x => x.st != ExState.InfLoop).ToArray();

            Console.WriteLine(upd.Single().acc);
        }
    }

    public class Ex
    {
        public int Acc { get; private set; }
        public ExState State { get; private set; } = ExState.NotStarted;

        public void Run(string[] program)
        {
            State = ExState.Running;

            var set = new HashSet<int>();

            var i = 0;
            while (true)
            {
                if (set.Contains(i))
                {
                    State = ExState.InfLoop;
                    break;
                }

                if (i == program.Length)
                {
                    State = ExState.Finished;
                    break;
                }

                set.Add(i);
                var line = program[i++];
                switch (line[0])
                {
                    case 'a':
                        Acc += int.Parse(line.Substring(4));
                        break;
                    case 'j':
                        i--;
                        i += int.Parse(line.Substring(4));
                        break;
                    case 'n':
                        break;
                }
            }
        }
    }

    public enum ExState
    {
        NotStarted,
        Running,
        InfLoop,
        Finished
    }
}