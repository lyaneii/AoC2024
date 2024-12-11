using System;
using System.IO;

namespace day11
{
    public static class Program
    {
        private static Dictionary<(long, long), long> _cache = new();
        
        private static int DigitCount(long n)
        {
            var count = 0;
            while (n > 0)
            {
                count++;
                n /= 10;
            }
            return count;
        }
        
        private static long Blink(long stone, long blinksLeft)
        {
            if (_cache.ContainsKey((stone, blinksLeft)))
                return _cache[(stone, blinksLeft)];
            
            long count;
            if (blinksLeft == 0)
                count = 1;
            else if (stone == 0)
                count = Blink(1, blinksLeft - 1);
            else if (DigitCount(stone) % 2 == 0)
            {
                var left = stone / (long)Math.Pow(10, DigitCount(stone) / 2);
                var right = stone % (long)Math.Pow(10, DigitCount(stone) / 2);
                count = Blink(left, blinksLeft - 1) + Blink(right, blinksLeft - 1);
            }
            else
                count = Blink(stone * 2024, blinksLeft - 1);
            
            _cache[(stone, blinksLeft)] = count;
            return count;
        }
        
        private static void PartOne(string[] input)
        {
            long total = 0;
            var stones = input
                .SelectMany(line => line
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(entry => long.Parse(entry)))
                .ToList();

            foreach (var stone in stones)
                total += Blink(stone, 25);
            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            var stones = input
                .SelectMany(line => line
                    .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(entry => long.Parse(entry)))
                .ToList();

            foreach (var stone in stones)
                total += Blink(stone, 75);
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