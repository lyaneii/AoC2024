using System;
using System.IO;
using System.Drawing;

namespace day14
{
    public static class Program
    {
        private enum Robot {
            Position,
            Velocity,
        }
        
        private static List<(int, int)> ParseRobotEntry(IEnumerable<string> robotEntry)
        {
            var robot = new List<(int, int)>();
            
            foreach (var item in robotEntry)
            {
                var split = item.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (split.Length < 2)
                    continue;
                robot.Add((int.Parse(split[0]), int.Parse(split[1].Trim(['v', ' ']))));
            }

            return robot;
        }

        private static (int, int) ScaleTuple((int x, int y) pos, (int x, int y) dir, int scalar)
        {
            return (pos.x + dir.x * scalar, pos.y + dir.y * scalar);
        }

        private static (int, int) ApplyLoopAround((int x, int y) coord, (int width, int height) space)
        {
            int afterX = coord.x % space.width;
            int afterY = coord.y % space.height;

            if (afterX < 0)
                afterX += space.width;
            if (afterY < 0)
                afterY += space.height;
            return (afterX, afterY);
        }

        private static void PrintRobotMap(List<(int x, int y)> robots, (int width, int height) space)
        {
            Console.WriteLine();
            for (var y = 0; y < space.height; y++)
            {
                for (var x = 0; x < space.width; x++)
                {
                    var pos = robots.FirstOrDefault(r => r == (x, y), (space.width, space.height));
                    if (pos != (space.width, space.height))
                        Console.Write('X');
                    else
                        Console.Write('.');
                }

                Console.WriteLine();
            }
        }

        private static int RobotSafetyFactor(List<(int x, int y)> robots, (int width, int height) space)
        {
            var topLeft = robots.Where(robot => robot.x < space.width / 2 && robot.y < space.height / 2).Count();
            var topRight = robots.Where(robot => robot.x > space.width / 2 && robot.y < space.height / 2).Count();
            var bottomLeft = robots.Where(robot => robot.x < space.width / 2 && robot.y > space.height / 2).Count();
            var bottomRight = robots.Where(robot => robot.x > space.width / 2 && robot.y > space.height / 2).Count();
            
            return topLeft * topRight * bottomLeft * bottomRight;
        }
        
        private static void PartOne(string[] input)
        { 
            long total = 0;
            const int seconds = 100;
            (int width, int height) space = (101, 103);
            List<List<(int x, int y)>> robots = input
                .Select(line => line
                    .Split('=', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(line => line.Where(entry => Char.IsDigit(entry.First()) || entry.First() == '-'))
                .Select(entry => ParseRobotEntry(entry))
                .ToList();

            var futureRobotPositions = robots
                .Select(robot => ScaleTuple(robot[(int)Robot.Position], robot[(int)Robot.Velocity], seconds))

            var loopAround = futureRobotPositions
                .Select(pos => ApplyLoopAround(pos, space))
                .ToList();

            total += RobotSafetyFactor(loopAround, space);

            Console.WriteLine(total);
        }

        private static void DrawRobotMap(List<(int, int)> robotPositions, (int width, int height) space,
            string outputPath)
        {
            var width = space.width;
            var height = space.height;

            using (Bitmap bmp = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Black);

                    foreach (var (x, y) in robotPositions)
                    {
                        bmp.SetPixel(x, y, Color.White);
                    }
                }
                bmp.Save(outputPath);
            }
        }

        private static void PartTwo(string[] input)
        {
            const int seconds = 20000;
            (int width, int height) space = (101, 103);
            List<List<(int x, int y)>> robots = input
                .Select(line => line
                    .Split('=', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(line => line.Where(entry => Char.IsDigit(entry.First()) || entry.First() == '-'))
                .Select(entry => ParseRobotEntry(entry))
                .ToList();

            var outputDirectory = "../../../output/";
            Directory.CreateDirectory(outputDirectory);
            for (var i = 0; i < seconds; i++)
            {
                var futureRobotPositions = robots
                    .Select(robot => ScaleTuple(robot[(int)Robot.Position], robot[(int)Robot.Velocity], i))

                var loopAround = futureRobotPositions
                    .Select(pos => ApplyLoopAround(pos, space))
                    .ToList();
                
                var fileName = $"{outputDirectory}{i}.png";
                DrawRobotMap(loopAround, space, fileName);
            }
        }

        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");

            Console.Write("Part One: ");
            PartOne(lines);
            // Console.Write("Part Two: ");
            PartTwo(lines);
        }
    }
}