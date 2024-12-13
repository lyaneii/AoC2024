using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace day13
{
    public static class Program
    {
        private enum Claw {
            A,
            B,
            Prize,
        }
        
        private static (int, int) SeparateXY(string input)
        {
            var x = new string(input.Skip(input.IndexOf('X') + 2).TakeWhile(Char.IsDigit).ToArray());
            var y = new string(input.Skip(input.IndexOf('Y') + 2).TakeWhile(Char.IsDigit).ToArray());
            return (int.Parse(x), int.Parse(y));
        }
        
        private static (long, long) SeparateXYLong(string input)
        {
            var x = new string(input.Skip(input.IndexOf('X') + 2).TakeWhile(Char.IsDigit).ToArray());
            var y = new string(input.Skip(input.IndexOf('Y') + 2).TakeWhile(Char.IsDigit).ToArray());
            return (long.Parse(x), long.Parse(y));
        }

        // private static List<(int, int)> GCF(int n)
        // {
        //     List<(int, int)> gcf = new();
        //     var multiplicant = 1;
        //
        //     while (multiplicant < n)
        //     {
        //         var res = n / multiplicant;
        //         if (res * multiplicant == n)
        //             gcf.Add((multiplicant, res));
        //         multiplicant++;
        //     }
        //     return gcf;
        // }

        private static Dictionary<(int, int), int> CalculatePresses((int x, int y) a, (int x, int y) b, (int x, int y) prize)
        {
            List<(int x, int y)> resA = Enumerable.Range(1, 100).Select(count => (count * a.x, count * a.y)).ToList();
            List<(int x, int y)> resB = Enumerable.Range(1, 100).Select(count => (count * b.x, count * b.y)).ToList();
            Dictionary<(int x, int y), int> dict = new();

            // Console.WriteLine();
            // Console.WriteLine("a: " + string.Join(", ", a));
            // Console.WriteLine("b: " + string.Join(", ", b));
            // Console.WriteLine("prize: " + string.Join(", ", prize));
            
            int ia = 1;
            foreach (var ra in resA)
            {
                int ib = 1;
                foreach (var rb in resB)
                {
                    if (ra.x + rb.x == prize.x && ra.y + rb.y == prize.y)
                        dict.Add((ia, ib), ia * 3 + ib);
                    ib++;
                }
                ia++;
            }

            foreach (var (key, value) in dict)
            {
                Console.WriteLine(string.Join(",", key) + "= {0}", value);
            }

            return dict;
        }
        
        private static Dictionary<(long, long), int> CalculatePressesLong((long x, long y) a, (long x, long y) b, (long x, long y) prize)
        {
            Dictionary<(long x, long y), int> dict = new();

            // Console.WriteLine();
            // Console.WriteLine("a: " + string.Join(", ", a));
            // Console.WriteLine("b: " + string.Join(", ", b));
            // Console.WriteLine("prize: " + string.Join(", ", prize));
            
            (long x, long y) res = (a.x + b.x, a.y + b.y);
            int ia = 1;
            while (ia < prize.x / a.x)
            {
                int ib = 1;
                while (ib < prize.y / prize.y)
                {
                    res = (a.x * ia + b.x * ib, a.y * ia + b.y * ib);
                    if (res.x == prize.x && res.y == prize.y)
                        dict.Add((ia, ib), ia * 3 + ib);
                    ib++;
                }
                ia++;
            }

            foreach (var (key, value) in dict)
            {
                Console.WriteLine(string.Join(",", key) + "= {0}", value);
            }

            return dict;
        }
        
        private static void PartOne(string[] input)
        {
            long total = 0;
            List<List<(int x, int y)>> clawMachines = input
                .SelectMany(lines => lines
                    .Split("\n\n", StringSplitOptions.RemoveEmptyEntries))
                .Select((line, index) => new { line, index })
                .GroupBy(group => group.index / 3)
                .Select(group => group.Select(s => SeparateXY(s.line))
                    .ToList())
                .ToList();

            foreach (var clawMachine in clawMachines)
            {
                var presses = CalculatePresses(
                    clawMachine[(int)Claw.A], 
                    clawMachine[(int)Claw.B], 
                    clawMachine[(int)Claw.Prize]
                    );
                if (presses.Any())
                    total += presses.Values.Min();
            }
            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            List<List<(long x, long y)>> clawMachines = input
                .SelectMany(lines => lines
                    .Split("\n\n", StringSplitOptions.RemoveEmptyEntries))
                .Select((line, index) => new { line, index })
                .GroupBy(group => group.index / 3)
                .Select(group => group.Select(s => SeparateXYLong(s.line))
                    .ToList())
                .ToList();

            foreach (var clawMachine in clawMachines)
            {
                var presses = CalculatePressesLong(
                    clawMachine[(int)Claw.A], 
                    clawMachine[(int)Claw.B], 
                    (clawMachine[(int)Claw.Prize].x + 10000000000000, clawMachine[(int)Claw.Prize].y + 10000000000000)
                );
                if (presses.Any())
                    total += presses.Values.Min();
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