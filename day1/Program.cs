using System;

namespace day1
{
    public static class Program
    {
        private static void PartOne(string[] input)
        {
            long total = 0;
            var left = new List<long>();
            var right = new List<long>();

            foreach (var line in input)
            {
                var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (tokens.Length != 2)
                    continue;
                left.Add(Convert.ToInt64(tokens[0]));
                right.Add(Convert.ToInt64(tokens[1]));
            }

            left.Sort();
            right.Sort();
            for (var i = 0; i < left.Count; i++)
            {
                total += Math.Abs(left[i] - right[i]);
            }

            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            var left = new List<long>();
            var right = new List<long>();

            foreach (var line in input)
            {
                var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (tokens.Length != 2)
                    continue;
                left.Add(Convert.ToInt64(tokens[0]));
                right.Add(Convert.ToInt64(tokens[1]));
            }

            foreach (var value in left)
            {
                long current = 0;
                foreach (var occurence in right)
                {
                    if (occurence == value)
                        current += value;
                }

                total += current;
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