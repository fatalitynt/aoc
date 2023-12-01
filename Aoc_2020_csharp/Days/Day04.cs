using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day04
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/04.txt");
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i].Length == 0) input[i] = "###";
                else input[i] = input[i] + " ";
            }

            var items = string.Join("", input).Split("###").ToArray();
            var res = 0;
            foreach (var item in items)
            {
                var parts = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var checkSum = 0;
                foreach (var part in parts)
                {
                    var validator = Validators[part.Substring(0, 3)];
                    if (validator(part.Split(':')[1])) checkSum++;
                    else Console.WriteLine($"{part} failed");
                }
                if (checkSum == 7) res++;
            }

            Console.WriteLine(res);
        }

        private static readonly Dictionary<string, Func<string, bool>> Validators
            = new Dictionary<string, Func<string, bool>>
        {
            {"byr", ValidateByr},
            {"iyr", ValidateIyr},
            {"eyr", ValidateEyr},
            {"hgt", ValidateHgt},
            {"hcl", ValidateHcl},
            {"ecl", ValidateEcl},
            {"pid", ValidatePid},
            {"cid", s => false},
        };

        private static bool ValidatePid(string s) => s.Length == 9 && int.TryParse(s, out var _);

        private static bool ValidateEcl(string s) => Ecl.Contains(s);

        private static readonly HashSet<string> Ecl = new HashSet<string>
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };

        private static bool ValidateHcl(string s)
        {
            if (s.Length != 7) return false;
            if (s[0] != '#') return false;
            for (var i = 1; i < 7; i++)
            {
                if (char.IsDigit(s[i]) || char.IsLower(s[i]) && char.IsLetter(s[i]))
                    continue;
                return false;
            }
            return true;
        }

        private static bool ValidateHgt(string s)
        {
            if (!s.EndsWith("in") && !s.EndsWith("cm")) return false;
            var val = s.Substring(0, s.Length - 2);
            return s.EndsWith("cm")
                ? ValidateRange(val, 150, 193)
                : ValidateRange(val, 59, 76);
        }

        private static bool ValidateEyr(string s) => ValidateRange(s, 2020, 2030);

        private static bool ValidateIyr(string s) => ValidateRange(s, 2010, 2020);

        private static bool ValidateByr(string s) => ValidateRange(s, 1920, 2002);

        private static bool ValidateRange(string s, int l, int r)
        {
            var val = int.Parse(s);
            return l <= val && val <= r;
        }
    }
}