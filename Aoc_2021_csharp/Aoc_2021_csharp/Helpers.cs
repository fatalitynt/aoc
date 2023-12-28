namespace Aoc_2021_csharp;

public static class Helpers
{
    public static string[] DayInput(this int day) => File.ReadAllLines($"Inputs/day{day}.txt");
    public static long[] AsLongs(this string a) => a.Split(",").Select(long.Parse).ToArray();
}