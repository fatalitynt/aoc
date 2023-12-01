using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day19
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/19.txt");
            var rulesCount = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x)).Count();


            var words = input.Skip(rulesCount + 1).ToArray();
            
            // var rules0 = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x))
            //     .Select(x => x == "8: 42" ? Patch8(3) : x)
            //     .Select(x => x == "11: 42 31" ? Patch11(1) : x)
            //     .Select(x => new Rule(x))
            //     .ToArray();
            // var word0 = "bbbbbbbaaaabbbbaaabbabaaa";
            // Console.WriteLine(TestWord(word0, rules0));

            
            const int maxRepeat = 100;
            
            var max = 0;

            for (var i = 1; i <= maxRepeat; i++)
            {
                var rules = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x == "8: 42" ? Patch8(i) : x)
                    .Select(x => x == "11: 42 31" ? Patch11(i) : x)
                    .Select(x => new Rule(x))
                    .ToArray();

                var passedWordsCount = words.Count(x => TestWord(x, rules));
                max = Math.Max(max, passedWordsCount);
                Console.WriteLine($"({i}) > {passedWordsCount}");
            }

            Console.WriteLine($"MAX: {max}");
        }

        private static string Patch8(int maxRepeat)
        {
            var rule = string.Join(" | ", Enumerable.Range(1, maxRepeat)
                .Select(
                    repeatCount => string.Join(" ", Rpt(repeatCount, 42))
                )
            );
            return $"8: {rule}";
        }

        private static string Patch11(int maxRepeat)
        {
            var rule = string.Join(" | ", Enumerable.Range(1, maxRepeat)
                .Select(
                    repeatCount => string.Join(" ", Rpt(repeatCount, 42)) + " " + string.Join(" ", Rpt(repeatCount, 31))
                )
            );
            return $"11: {rule}";
        } 

        private static bool TestWord(string word, Rule[] rules)
        {
            foreach (var rule in rules) rule.Applied = false;
            var map = BuildMap(word, rules);
            return map[0][0].Contains(word.Length);
        }

        private static List<int>[][] BuildMap(string word, Rule[] rules)
        {
            var map = Enumerable
                .Range(0, Math.Max(rules.Length, 100))
                .Select(x => Enumerable.Range(0, word.Length).Select(z => new List<int>()).ToArray())
                .ToArray();
            var applied = new HashSet<int>();
            while (applied.Count < rules.Length)
            {
                foreach (var rule in rules.Where(x => !x.Applied))
                {
                    if (!rule.CanApply(applied)) continue;
                    rule.Applied = true;
                    applied.Add(rule.Idx);

                    for (var i = 0; i < word.Length; i++)
                    {
                        map[rule.Idx][i] = rule.Apply(word, i, map);
                    }
                }
            }

            return map;
        }

        class Rule
        {
            public int Idx { get; set; }
            public char? Letter { get; set; }
            //public int[] Rules { get; set; }
            //public int[] OrRules { get; set; }
            
            public int[][] AllRules { get; set; }
            public bool Applied { get; set; }

            public bool CanApply(HashSet<int> applied)
            {
                if (Letter.HasValue) return true;
                
                foreach (var rules in AllRules)
                {
                    if (rules.Where(x => x != Idx).Any(x => !applied.Contains(x))) return false;
                }

                return true;
            }

            public List<int> Apply(string s, int i, List<int>[][] map)
            {
                if (i >= s.Length) return new List<int>();
                
                if (Letter != null)
                {
                    if (s[i] != Letter.Value) return new List<int>();
                    return new List<int> {i + 1};
                }

                return AllRules.SelectMany(ruleIndexes => MatchRange(s, i, ruleIndexes, map)).ToList();
                //
                // var res = MatchRange(s, i, Rules, map);
                // if (OrRules != null)
                //     res.AddRange(MatchRange(s, i, OrRules, map));
                //
                // return res;
            }

            private static List<int> MatchRange(string s, int i, int[] ruleIndexes, List<int>[][] map)
            {
                var list = new List<int> {i};

                foreach (var ruleIdx in ruleIndexes)
                {
                    list = list.Where(idx => idx < s.Length).SelectMany(idx => map[ruleIdx][idx]).ToList();
                }

                return list;
            }

            public Rule(string allInput)
            {
                var lr = allInput.Split(": ");
                Idx = int.Parse(lr[0]);
                var input = lr[1];
                if (input.StartsWith("\""))
                {
                    Letter = input[1];
                    return;
                }

                var parts = input.Split(" | ");

                AllRules = parts.Select(prt => prt.Split(" ").Select(int.Parse).ToArray()).ToArray();
            }

            public override string ToString()
            {
                var res = $"{Idx}:: ";
                if (Letter.HasValue) res += $"l:{Letter.Value} ";
                res += "rls: " + string.Join(" $ ", AllRules.Select(x => string.Join(" ", x)));
                return res;
            }
        }
        
        private static IEnumerable<T> Rpt<T>(int count, T x) => Enumerable.Range(0, count).Select(_ => x);
    }
}