using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace day12
{
    public static class Program
    {
        private enum Direction {
            Start = -1,
            Up,
            Left,
            Right,
            Down,
        };

        private static int CountSides(List<(int x, int y)> side, Direction dir)
        {
            var total = 0;
            List<(int x, int y)> ordered;

            if (dir == Direction.Up || dir == Direction.Down)
                ordered = side.OrderBy(s => s.y).ThenBy(s => s.x).ToList();
            else
                ordered = side.OrderBy(s => s.x).ThenBy(s => s.y).ToList();
            
            (int x, int y) prev = (-10, -10);
            foreach (var coord in ordered)
            {
                if (Math.Abs(coord.x - prev.x) + Math.Abs(coord.y - prev.y) > 1)
                    total++;
                prev = coord;
            }

            return total;
        }
        
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
            HashSet<(int, int)> visited, HashSet<(int, int)> region, List<List<(int, int)>> sides, Direction dir)
        {
            if (visited.Contains(current))
            {
                if (grid[current] != type)
                {
                    if (dir == Direction.Up || dir == Direction.Down)
                        sides[(int)dir].Add(current);
                    else
                        sides[(int)dir].Add(current);
                }
                return;
            }
            
            if (!grid.ContainsKey(current) ||
                grid[current] != type)
            {
                if (dir == Direction.Up || dir == Direction.Down)
                    sides[(int)dir].Add(current);
                else
                    sides[(int)dir].Add(current);
                return;
            }

            visited.Add(current);
            region.Add(current);
            CurrentRegionAndSide(type, (current.x, current.y - 1), grid, visited, region, sides, Direction.Up); // up
            CurrentRegionAndSide(type, (current.x - 1, current.y), grid, visited, region, sides, Direction.Left); // left
            CurrentRegionAndSide(type, (current.x + 1, current.y), grid, visited, region, sides, Direction.Right); // right
            CurrentRegionAndSide(type, (current.x, current.y + 1), grid, visited, region, sides, Direction.Down); // down
        }
        
        private static long FenceRegionsPerimeter(Dictionary<(int, int), char> grid)
        {
            long total = 0;
            HashSet<(int, int)> visited = new();

            foreach (var (coord, type) in grid)
            {
                if (visited.Contains(coord))
                    continue;
                
                HashSet<(int, int)> region = new();
                long perimeter = 0;
                
                CurrentRegionAndPerimeter(type, coord, grid, visited, region, ref perimeter);

                total += region.Count * perimeter;
            }

            return total;
        }
        
        private static long FenceRegionsSides(Dictionary<(int, int), char> grid)
        {
            long total = 0;
            HashSet<(int, int)> visited = new();

            foreach (var (coord, type) in grid)
            {
                if (visited.Contains(coord))
                    continue;
                
                HashSet<(int, int)> region = new();
                List<List<(int, int)>> sides = new()
                {
                    new List<(int, int)>(), // up
                    new List<(int, int)>(), // left
                    new List<(int, int)>(), // right
                    new List<(int, int)>(), // down
                };
                
                CurrentRegionAndSide(type, coord, grid, visited, region, sides, Direction.Start);
                
                var numSides = 0;
                foreach (var (side, i) in sides.Select((side, i) => (side, i)))
                    numSides += CountSides(side, (Direction)i);

                total += region.Count * numSides;
            }

            return total;
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
            
            total = FenceRegionsPerimeter(dict);
            
            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            var dict = GridToDict(input);

            total = FenceRegionsSides(dict);

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