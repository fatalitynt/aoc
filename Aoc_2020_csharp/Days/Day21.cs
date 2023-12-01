using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day21
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/21.txt");
            var dishes = input.Select(x => new Dish(x)).ToArray();
            var allergens = dishes.Select(x => x.Allergens).Aggregate((a1, a2) => a1.Concat(a2).ToHashSet());
            var names = new Dictionary<string, string>();

            while (allergens.Count > 0)
            {
                foreach (var allergen in allergens.ToArray())
                {
                    var commonIngredients = dishes.Where(x => x.Allergens.Contains(allergen))
                        .Select(x => x.Ingredients)
                        .Aggregate((i1, i2) => i1.Intersect(i2).ToHashSet());

                    if (commonIngredients.Count == 1)
                    {
                        Remove(allergens, dishes, allergen, commonIngredients.Single());
                        names[allergen] = commonIngredients.Single();
                    }
                }
            }

            Console.WriteLine(dishes.Sum(x => x.Ingredients.Count));
            Console.WriteLine(string.Join(",", names.OrderBy(x => x.Key).Select(x => x.Value)));
        }

        private static void Remove(HashSet<string> allergens, Dish[] dishes, string a0, string i0)
        {
            var rm = new Queue<(string, string)>();
            rm.Enqueue((a0, i0));
            while (rm.Count > 0)
            {
                var (a, i) = rm.Dequeue();
                allergens.Remove(a);
                foreach (var dish in dishes)
                {
                    dish.Ingredients.Remove(i);
                    dish.Allergens.Remove(a);
                    if (dish.Allergens.Count == 1 && dish.Ingredients.Count == 1)
                        rm.Enqueue((dish.Allergens.Single(), dish.Ingredients.Single()));
                }
            }
        }
        

        class Dish
        {
            public Dish(string line)
            {
                var parts = line.Substring(0, line.Length - 1).Split(" (contains ");
                Ingredients = parts[0].Split(" ").ToHashSet();
                Allergens = parts[1].Split(", ").ToHashSet();
            }
            public HashSet<string> Ingredients { get; set; }
            public HashSet<string> Allergens { get; set; }
        }
    }
}