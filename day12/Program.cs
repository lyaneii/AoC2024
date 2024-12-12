using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace day12
{
    public static class Program
    {
        private static void CurrentIsland(char type, (int x, int y) current, Dictionary<(int, int), char> grid,
            HashSet<(int, int)> visited, HashSet<(int, int)> island)
        {
            if (!grid.ContainsKey(current))
                return;
            if (visited.Contains(current))
                return;
            if (grid[current] != type)
                return;
            visited.Add(current);
            island.Add(current);
            CurrentIsland(type, (current.x, current.y - 1), grid, visited, island); // up
            CurrentIsland(type, (current.x - 1, current.y), grid, visited, island); // left
            CurrentIsland(type, (current.x + 1, current.y), grid, visited, island); // right
            CurrentIsland(type, (current.x, current.y + 1), grid, visited, island); // down
        }
        
        private static HashSet<HashSet<(int, int)>> SeparateIslands(Dictionary<(int, int), char> grid)
        {
            HashSet<HashSet<(int, int)>> islands = new();
            HashSet<(int, int)> visited = new();

            HashSet<(int, int)> island = new();
            foreach (var (coord, type) in grid)
            {
                if (visited.Contains(coord))
                    continue;
                CurrentIsland(type, coord, grid, visited, island);
                if (island.Any())
                    islands.Add(island);
                island.Clear();
            }

            return islands;
        }

        private static Dictionary<(int, int), char> GridToDict(string[] input)
        {
            return input
                .SelectMany((row, y) => row
                    .Select((value, x) => new { coord = (x, y), value }))
                .GroupBy(group => group.coord)
                .ToDictionary(group => group.Key,
                    group => group.First().value);
        }
        
        private static void PartOne(string[] input)
        {
            long total = 0;
            var dict = GridToDict(input);
            var islands = SeparateIslands(dict);

            PrintMapWithIslands(dict, islands);

            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;

            Console.WriteLine(total);
        }

        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");

            Console.Write("Part One: ");
            PartOne(lines);
            Console.Write("Part Two: ");
            PartTwo(lines);
        }
    }
}