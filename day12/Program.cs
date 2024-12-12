using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace day12
{
    public static class Program
    {
        
        
        private static void CurrentRegionAndPerimeter(char type, (int x, int y) current, Dictionary<(int, int), char> grid,
            HashSet<(int, int)> visited, HashSet<(int, int)> region, ref long perimeter)
        {
            if (visited.Contains(current))
            {
                if (grid[current] != type)
                    perimeter++;
                return;
            }
            if (!grid.ContainsKey(current) ||
                grid[current] != type)
            {
                perimeter++;
                return;
            }

            visited.Add(current);
            region.Add(current);
            CurrentRegionAndPerimeter(type, (current.x, current.y - 1), grid, visited, region, ref perimeter); // up
            CurrentRegionAndPerimeter(type, (current.x - 1, current.y), grid, visited, region, ref perimeter); // left
            CurrentRegionAndPerimeter(type, (current.x + 1, current.y), grid, visited, region, ref perimeter); // right
            CurrentRegionAndPerimeter(type, (current.x, current.y + 1), grid, visited, region, ref perimeter); // down
        }
        
        private static void CurrentRegionAndSide(char type, (int x, int y) current, Dictionary<(int, int), char> grid,
            HashSet<(int, int)> visited, HashSet<(int, int)> region, List<Dictionary<int, int>> sides)
        {
            if (visited.Contains(current))
            {
                if (grid[current] != type);
                
                return;
            }
            if (!grid.ContainsKey(current) ||
                grid[current] != type)
            {
                return;
            }

            visited.Add(current);
            region.Add(current);
            CurrentRegionAndSide(type, (current.x, current.y - 1), grid, visited, region, sides); // up
            CurrentRegionAndSide(type, (current.x - 1, current.y), grid, visited, region, sides); // left
            CurrentRegionAndSide(type, (current.x + 1, current.y), grid, visited, region, sides); // right
            CurrentRegionAndSide(type, (current.x, current.y + 1), grid, visited, region, sides); // down
        }
        
        private static HashSet<HashSet<(int, int)>> FenceRegionsPerimeter(Dictionary<(int, int), char> grid, ref long total)
        {
            HashSet<HashSet<(int, int)>> regions = new();
            HashSet<(int, int)> visited = new();

            foreach (var (coord, type) in grid)
            {
                if (visited.Contains(coord))
                    continue;
                HashSet<(int, int)> region = new();
                long perimeter = 0;
                CurrentRegionAndPerimeter(type, coord, grid, visited, region, ref perimeter);
                if (region.Any())
                    regions.Add(region);
                total += region.Count * perimeter;
            }

            return regions;
        }
        
        private static HashSet<HashSet<(int, int)>> FenceRegionsSides(Dictionary<(int, int), char> grid, ref long total)
        {
            HashSet<HashSet<(int, int)>> regions = new();
            HashSet<(int, int)> visited = new();

            foreach (var (coord, type) in grid)
            {
                if (visited.Contains(coord))
                    continue;
                HashSet<(int, int)> region = new();
                List<Dictionary<int, int>> sides = new();
                CurrentRegionAndSide(type, coord, grid, visited, region, sides);
                if (region.Any())
                    regions.Add(region);
                total += region.Count;
            }

            return regions;
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
        
        private static void PrintRegion(HashSet<(int, int)> island, (int x, int y) size)
        {
            Console.WriteLine();
            for (var y = 0; y < size.y; y++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    var islandPart = island.FirstOrDefault(part => part == (x, y), (-1, -1));
                    if (islandPart != (-1, -1))
                        Console.Write('X');
                    else
                        Console.Write('.');

                }
                Console.WriteLine();
            }
        }
        
        private static void PartOne(string[] input)
        {
            long total = 0;
            var dict = GridToDict(input);
            var regions = FenceRegions(dict, ref total);

            
            // foreach (var region in regions)
            // {
            //     PrintRegion(region, (input[0].Length, input.Length));
            // }
            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            var dict = GridToDict(input);
            var regions = FenceRegions(dict, ref total);

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