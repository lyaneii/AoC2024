using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace day12
{
    public static class Program
    {
        private static List<ConsoleColor> _islandColors = new List<ConsoleColor>
        {
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Blue,
            ConsoleColor.Magenta,
            ConsoleColor.Cyan
        };
    
        // Function to print the full map with different colors for each island
        private static void PrintMapWithIslands(Dictionary<(int, int), char> grid, HashSet<HashSet<(int, int)>> islands)
        {
            // Step 1: Map each coordinate to its island index
            var coordinateToIsland = new Dictionary<(int, int), int>();
            int islandIndex = 0;

            // Assign each coordinate to an island index
            foreach (var island in islands)
            {
                foreach (var coordinate in island)
                {
                    coordinateToIsland[coordinate] = islandIndex;
                    Console.WriteLine(string.Join(",", coordinate));
                }
                islandIndex++;
            }
            

            // Step 2: Find the dimensions of the map to print it row by row
            int minX = grid.Keys.Min(k => k.Item1);
            int maxX = grid.Keys.Max(k => k.Item1);
            int minY = grid.Keys.Min(k => k.Item2);
            int maxY = grid.Keys.Max(k => k.Item2);

            // Step 3: Print the map with colors
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (grid.ContainsKey((x, y)))
                    {
                        var coordinate = (x, y);
                        var islandId = coordinateToIsland.ContainsKey(coordinate) ? coordinateToIsland[coordinate] : -1;
                        // Console.WriteLine(string.Join(", ", (x, y)));
                        // Console.Write(islandId);

                        // Assign a default color if no island is assigned
                        if (islandId >= 0)
                        {
                            Console.BackgroundColor = _islandColors[islandId % _islandColors.Count];
                        }
                        else
                        {
                            // Default color for non-island areas (e.g., black background)
                            Console.BackgroundColor = ConsoleColor.Black;
                        }

                        // Print the character
                        Console.Write(grid[coordinate]);
                        Console.ResetColor(); // Reset color after printing
                    }
                    else
                    {
                        Console.Write(" "); // Print an empty space for missing coordinates
                    }
                }
                Console.WriteLine(); // Move to the next line after each row
            }
        }

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