using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day18
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/18.txt");
            var sum = input.Sum(line => Eval(Parse(line), 0, out var _));
            Console.WriteLine(sum);
        }

        private static long Eval(List<Token> tokens, int i, out int idx)
        {
            idx = i;
            var left = new List<long>();
            TType op;
            if (tokens[idx].Type == TType.Value)
            {
                left.Add(tokens[idx].Value);
                op = tokens[idx + 1].Type;
                idx += 2;
            }
            else if (tokens[idx].Type == TType.Open)
            {
                left.Add(Eval(tokens, idx + 1, out idx));
                op = tokens[idx++].Type;
            }
            else throw new Exception("can't init");

            while (idx < tokens.Count)
            {
                var next = tokens[idx++];
                if (next.Type == TType.Value) left = Perf(left, next.Value, op);
                else if (next.Type == TType.Open) left = Perf(left, Eval(tokens, idx, out idx), op);
                else if (next.Type == TType.Close) return left.Aggregate((a, b) => a * b);
                else if (next.Type == TType.Mult) op = TType.Mult;
                else if (next.Type == TType.Add) op = TType.Add;
            }

            return left.Aggregate((a, b) => a * b);
        }

        private static List<long> Perf(List<long> l, long r, TType op)
        {
            if (op == TType.Add) l[l.Count - 1] += r;
            if (op == TType.Mult) l.Add(r);
            return l;
        }

        private static List<Token> Parse(string line)
        {
            var res = new List<Token>();
            var nextVal = 0;

            var chars = line.ToCharArray();
            foreach (var c in chars)
            {
                if (c == '0' && nextVal == 0)
                {
                    res.Add(Token.C(0));
                }
                if (char.IsDigit(c))
                {
                    nextVal *= 10;
                    nextVal += c - '0';
                }
                else
                {
                    if (nextVal != 0)
                    {
                        res.Add(Token.C(nextVal));
                        nextVal = 0;
                    }

                    if (c == '+') res.Add(Token.A());
                    else if (c == '*') res.Add(Token.M());
                    else if (c == '(') res.Add(Token.O());
                    else if (c == ')') res.Add(Token.C());
                }
            }
            
            if (nextVal != 0)
            {
                res.Add(Token.C(nextVal));
            }

            return res;
        }
        
        class Token
        {
            public TType Type { get; set; }
            public long Value { get; set; }

            public static Token C(long v) => new Token() {Type = TType.Value, Value = v};
            public static Token M() => new Token() {Type = TType.Mult, Value = -1};
            public static Token A() => new Token() {Type = TType.Add, Value = -1};
            public static Token O() => new Token() {Type = TType.Open, Value = -1};
            public static Token C() => new Token() {Type = TType.Close, Value = -1};
        }
        
        enum TType
        {
            Value,
            Open,
            Close,
            Mult,
            Add
        }
    }
}