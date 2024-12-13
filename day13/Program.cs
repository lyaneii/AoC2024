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

        private static Dictionary<(int, int), int> CalculatePresses((int x, int y) a, (int x, int y) b, (int x, int y) prize)
        {
            List<(int x, int y)> resA = Enumerable
                .Range(1, 100)
                .Select(count => (count * a.x, count * a.y))
                .ToList();
            List<(int x, int y)> resB = Enumerable
                .Range(1, 100)
                .Select(count => (count * b.x, count * b.y))
                .ToList();
            Dictionary<(int x, int y), int> dict = new();

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

            return dict;
        }

        private static void Inverse2DMatrix(ref (long x, long y) a, ref (long x, long y) b)
        {
            (long x, long y) tmp = a;
            a = (b.y, -b.x);
            b = (-tmp.y, tmp.x);
        }
        
        private static long CalculatePressesLong((long x, long y) a, (long x, long y) b, (long x, long y) prize)
        {
            Console.WriteLine();
            Console.WriteLine("a: " + string.Join(", ", a));
            Console.WriteLine("b: " + string.Join(", ", b));
            Console.WriteLine("prize: " + string.Join(", ", prize));

            var determinant = a.x * b.y - b.x * a.y;
            Inverse2DMatrix(ref a, ref b);
            (double x, double y) inverseA = ((double)a.x / determinant, (double)a.y / determinant);
            (double x, double y) inverseB = ((double)b.x / determinant, (double)b.y / determinant);
            (double x, double y) res = (prize.x * inverseA.x + prize.y * inverseA.y, prize.x * inverseB.x + prize.y * inverseB.y);

            const double epsilon = 0.001;
            if (Math.Abs(Math.Round(res.x) - res.x) < epsilon && Math.Abs(Math.Round(res.y) - res.y) < epsilon)
                return (long)(Math.Round(res.x) * 3) + (long)Math.Round(res.y);
            return 0;
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

            const long add = 10000000000000;
            foreach (var clawMachine in clawMachines)
            {
                var value = CalculatePressesLong(
                    clawMachine[(int)Claw.A], 
                    clawMachine[(int)Claw.B], 
                    (clawMachine[(int)Claw.Prize].x + add, clawMachine[(int)Claw.Prize].y + add)
                );
                total += value;
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