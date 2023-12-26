namespace Aoc_2021_csharp;

public static class Helpers
{
    public static string[] DayInput(this int day) => File.ReadAllLines($"Inputs/day{day}.txt");
}