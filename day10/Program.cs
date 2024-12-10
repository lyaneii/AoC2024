using System;
using System.IO;

namespace day10
{
    public static class Program
    {
        private static Dictionary<int, HashSet<(int, int)>> DictionaryMap(string[] input)
        {
            var dict = input
                .SelectMany((row, y) => row
                    .Select((value, x) => new { value, x, y }))
                .GroupBy(item => (int)item.value)
                .ToDictionary(group => (group.Key - '0'), 
                    group => group
                    .Select(coord => (coord.x, coord.y))
                    .ToHashSet());

            return dict;
        }

        private static void ContinueTrailUp(HashSet<(int height, (int x, int y))> visited, int height, (int x, int y) pos, Dictionary<int, HashSet<(int, int)>> trails)
        {
            visited.Add((height, pos));
            if (height == 9)
                return ;
            if (!trails.TryGetValue(height + 1, out var nextHeight))
                return ;
            foreach (var (x, y) in nextHeight)
            {
                if (Math.Abs(x - pos.x) + Math.Abs(y - pos.y) == 1)
                    ContinueTrailUp(visited, height + 1, (x, y), trails);
            }

        }
        private static void ContinueTrailDown(List<(int height, (int x, int y))> visited, int height, (int x, int y) pos, Dictionary<int, HashSet<(int, int)>> trails)
        {
            visited.Add((height, pos));
            if (height == 0)
                return ;
            if (!trails.TryGetValue(height - 1, out var nextHeight))
                return ;
            foreach (var (x, y) in nextHeight)
            {
                if (Math.Abs(x - pos.x) + Math.Abs(y - pos.y) == 1)
                    ContinueTrailDown(visited, height - 1, (x, y), trails);
            }

        }

        private static void PrintTrail(HashSet<(int height, (int x, int y) coord)> visited, (int x, int y) size)
        {
            Console.WriteLine();
            for (var y = 0; y < size.y; y++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    var visit = visited.FirstOrDefault(v => v.coord == (x, y));
                    if (visit.coord == (x, y))
                        Console.Write(visit.height);
                    else
                        Console.Write('.');

                }
                Console.WriteLine();
            }
        }
        
        private static void PrintTrail(List<(int height, (int x, int y) coord)> visited, (int x, int y) size)
        {
            Console.WriteLine();
            for (var y = 0; y < size.y; y++)
            {
                for (var x = 0; x < size.x; x++)
                {
                    var visit = visited.FirstOrDefault(v => v.coord == (x, y));
                    if (visit.coord == (x, y))
                        Console.Write(visit.height);
                    else
                        Console.Write('.');

                }
                Console.WriteLine();
            }
        }
        
        private static void PartOne(string[] input)
        {
            long total = 0;
            var dict = DictionaryMap(input);

            foreach (var coord in dict[0])
            {
                var visited = new HashSet<(int, (int, int))>();
                ContinueTrailUp(visited, 0, coord, dict);
                // PrintTrail(visited, (input[0].Length, input.Length));
                total += visited.Count(visit => visit.Item1 == 9);
            }
            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            var dict = DictionaryMap(input);

            foreach (var coord in dict[9])
            {
                var visited = new List<(int height, (int, int) coord)>();
                ContinueTrailDown(visited, 9, coord, dict);
                // PrintTrail(visited, (input[0].Length, input.Length));
                total += visited.Count(visit => visit.Item1 == 0);
            }
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