using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2020.Days
{
    public static class Day07
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/07.txt");
            var ruleMap = new Dictionary<int, List<Tuple<int, int>>>();
            var parentsMap = new Dictionary<int, List<int>>();
            var colorMap = new Dictionary<string, int>();

            foreach (var line in input)
            {
                var lr = line.Split("contain");
                if (lr[1].StartsWith(" no other bags")) continue;

                var target = lr[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var colorId = GetOrAddColorId(target[0] + target[1], colorMap);

                if (!ruleMap.ContainsKey(colorId)) ruleMap[colorId] = new List<Tuple<int, int>>();

                var rules = lr[1].Split(',');
                foreach (var rawRule in rules)
                {
                    var parsedRule = rawRule.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    AddRule(parsedRule, ruleMap[colorId], colorMap, parentsMap, colorId);
                }
            }

            var myColor = "shinygold";
            var myColorId = GetOrAddColorId(myColor, colorMap);

            //var cache = new Dictionary<int, HashSet<int>>();
            //var answer = GetAllParents(myColorId, parentsMap, cache);
            //Console.WriteLine(answer.Count);

            var cache = new Dictionary<int, int>();
            var answer = GetNestedBagsCount(myColorId, ruleMap, cache);
            Console.WriteLine(answer);
        }

        private static int GetNestedBagsCount(int colorId,
            Dictionary<int, List<Tuple<int, int>>> ruleMap,
            Dictionary<int, int> cache)
        {
            if (cache.TryGetValue(colorId, out var cacheValue)) return cacheValue;
            if (!ruleMap.ContainsKey(colorId)) return 0;

            var res = 0;
            foreach (var rule in ruleMap[colorId])
            {
                var nestedCount = GetNestedBagsCount(rule.Item1, ruleMap, cache);
                var localRes = (nestedCount + 1) * rule.Item2;
                res += localRes;
            }

            cache[colorId] = res;
            return res;
        }

        private static HashSet<int> GetAllParents(int colorId,
            Dictionary<int, List<int>> parentsMap,
            Dictionary<int, HashSet<int>> cache)
        {
            if (cache.TryGetValue(colorId, out var cacheValue)) return cacheValue;

            var ans = new HashSet<int>();
            if (!parentsMap.ContainsKey(colorId)) return new HashSet<int>();
            var parents = parentsMap[colorId];
            foreach (var parent in parents)
            {
                ans.Add(parent);
                var grandParents = GetAllParents(parent, parentsMap, cache);
                foreach (var grandParent in grandParents) ans.Add(grandParent);
            }

            cache[colorId] = ans;
            return ans;
        }

        private static void AddRule(string[] ruleParts,
            List<Tuple<int, int>> rulesList,
            Dictionary<string, int> colorMap,
            Dictionary<int, List<int>> parentMap,
            int parentId)
        {
            var count = int.Parse(ruleParts[0]);
            var ruleColor = ruleParts[1] + ruleParts[2];
            var colorId = GetOrAddColorId(ruleColor, colorMap);

            rulesList.Add(Tuple.Create(colorId, count));

            if (!parentMap.ContainsKey(colorId)) parentMap[colorId] = new List<int>();
            parentMap[colorId].Add(parentId);
        }

        private static int GetOrAddColorId(string color, Dictionary<string, int> colorMap)
        {
            if (colorMap.TryGetValue(color, out var id)) return id;
            var newId = colorMap.Count;
            colorMap[color] = newId;
            return newId;
        }
    }
}