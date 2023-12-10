namespace Aoc_2023_csharp;

public static class Helpers
{
    public static string[] DayInput(this int day) => File.ReadAllLines($"Inputs/day{day}.txt");

    public static long[] ToLongArr(this string line) => line
        .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
}