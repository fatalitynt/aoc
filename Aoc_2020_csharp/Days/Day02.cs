using System;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day02
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/02.txt");
            var result = 0;

            foreach (var line in input)
            {
                var lineParts = line.Split(':');
                var ruleParts = lineParts[0].Split(' ');
                var range = ruleParts[0].Split('-').Select(int.Parse).ToArray();
                var letter = ruleParts[1][0];
                var word = lineParts[1];
                var cc = 0;
                if (word[range[0]] == letter) cc++;
                if (word[range[1]] == letter) cc++;
                if (cc == 1) result++;
            }

            Console.WriteLine(result);
        }
    }
}