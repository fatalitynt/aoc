using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day16
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/16.txt");
            var rawRules = input.TakeWhile(x => !string.IsNullOrEmpty(x));

            var rules = rawRules.Select((r, idx) =>
                    {
                        var parts = r.Split(": ");
                        var rawRanges = parts[1].Split(" or ");
                        return new Rule
                        {
                            FinalIndex = -1,
                            Name = parts[0],
                            Ranges = rawRanges.Select(rr =>
                            {
                                var rrr = rr.Split("-").Select(int.Parse).ToArray();
                                return Tuple.Create(rrr[0], rrr[1]);
                            }).ToArray()
                        };
                    }
                )
                .ToArray();

            var invalidRate = 0;

            var parsedTickets = input
                .Skip(rules.Length + 5)
                .TakeWhile(x => !string.IsNullOrEmpty(x))
                .Select(x => x.Split(",").Select(int.Parse).ToArray()).ToArray();

            var fieldsCount = parsedTickets[0].Length;
            var validTicketsIds = new HashSet<int>();

            for (var i = 0; i < parsedTickets.Length; i++)
            {
                var ticket = parsedTickets[i];
                var isValid = true;
                foreach(var value in ticket)
                {
                    if (rules.Any(r => r.CanInclude(value))) continue;
                    invalidRate += value;
                    isValid = false;
                }
                if (isValid) validTicketsIds.Add(i);
            }

            var validTickets = parsedTickets.Where((x, idx) => validTicketsIds.Contains(idx)).ToArray();

            foreach (var rule in rules)
            {
                for (var fieldIdx = 0; fieldIdx < fieldsCount; fieldIdx++)
                {
                    var allValues = validTickets.Select(t => t[fieldIdx]).ToArray();
                    if (allValues.All(rule.CanInclude))
                    {
                        rule.PossibleIndexes.Add(fieldIdx);
                    }
                }
            }

            foreach (var rule in rules) rule.PossibleIndexes = rule.PossibleIndexes.Distinct().ToList();

            var usedIndexes = new HashSet<int>();

            while (rules.Any(r => r.FinalIndex < 0))
            {
                foreach (var rule in rules.Where(x => x.FinalIndex < 0))
                {
                    if (rule.TryFinalize(usedIndexes))
                    {
                        usedIndexes.Add(rule.FinalIndex);
                    }
                }
            }

            var myTicket = input.Skip(rules.Length + 2).Take(1).Single().Split(",").Select(int.Parse).ToArray();
            var targetFields = rules.Where(x => x.Name.StartsWith("departure")).Select(x => myTicket[x.FinalIndex])
                .ToArray();

            long res = 1;
            foreach (var targetField in targetFields) res *= targetField;

            Console.WriteLine(invalidRate);
            Console.WriteLine(res);
        }

        private class Rule
        {
            public string Name { get; set; }
            public Tuple<int,int>[] Ranges { get; set; }
            
            public List<int> PossibleIndexes = new List<int>();
            
            public int FinalIndex { get; set; }

            public bool CanInclude(int value)
            {
                return Ranges.Any(r => r.Item1 <= value && value <= r.Item2);
            }

            public bool TryFinalize(HashSet<int> usedFields)
            {
                var newPossibleIndexes = PossibleIndexes.Where(x => !usedFields.Contains(x)).ToArray();
                if (newPossibleIndexes.Length != 1) return false;
                FinalIndex = newPossibleIndexes[0];
                return true;
            }
        }
    }
}