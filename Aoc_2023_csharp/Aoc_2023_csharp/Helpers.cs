namespace Aoc_2023_csharp;

public static class Helpers
{
    public static string[] DayInput(this int day) => File.ReadAllLines($"Inputs/day{day}.txt");

    public static long[] ToLongArr(this string line) => line
        .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

    public static int[] ToIntArr(this string line) => line
        .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

    public static T[] MakeArr<T>(this int h, Func<T> f) => Enumerable
        .Range(0, h).Select(_ => f()).ToArray();
}