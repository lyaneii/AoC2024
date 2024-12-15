using System;
using System.IO;

namespace day15
{
    public static class Program
    {
        private static (int x, int y) Up = (0, -1);
        private static (int x, int y) Left = (-1, 0);
        private static (int x, int y) Right = (1, 0);
        private static (int x, int y) Down = (0, 1);
        private static (int x, int y) Invalid = (-1, -1);
        
        private static (int, int) MovementToDirection(char movement)
        {
            switch (movement)
            {
                case '^': return Up;
                case '<': return Left;
                case '>': return Right;
                case 'v': return Down;
                default : return Invalid;
            }
        }

        private static (int, int) ScaleTuple((int x, int y) current, (int x, int y) dir, int scale)
        {
            return ((current.x + dir.x * scale, current.y + dir.y * scale));
        }

        private static (int, int) NextFreeSpace(Dictionary<(int x, int y), char> map,
            (int x, int y) current, (int x, int y) direction)
        {
            var next = map.FirstOrDefault(pair => 
                    pair.Key == ScaleTuple(current, direction, 1), 
                (new KeyValuePair<(int x, int y), char>(Invalid, '#')));

            while (next.Key != Invalid)
            {
                if (next.Value == '#')
                    return Invalid;
                if (next.Value == '.')
                    break ;
                next = map.FirstOrDefault(pair => 
                        pair.Key == ScaleTuple(next.Key, direction, 1), 
                    (new KeyValuePair<(int x, int y), char>(Invalid, '#')));
            }

            return next.Key;
        }

        private static void AddEntireBox(Dictionary<(int x, int y), char> map, 
            List<KeyValuePair<(int, int), char>> connected, (int x, int y) current, char type)
        {
            connected.Add(new KeyValuePair<(int, int), char>(current, type));
            var segmentPos = type == ']' ? ScaleTuple(current, Left, 1) : ScaleTuple(current, Right, 1);
            connected.Add(new KeyValuePair<(int, int), char>(segmentPos, map[segmentPos]));
        }

        private static List<KeyValuePair<(int, int), char>> ConnectedBoxes(Dictionary<(int x, int y), char> map, 
            List<KeyValuePair<(int, int), char>> alreadyConnected, (int x, int y) current, (int x, int y) direction)
        {
            var connected = new List<KeyValuePair<(int, int), char>>();

            if (alreadyConnected.Select(pair => pair.Key).Contains(current) ||
                map[current] == '#' || map[current] == '.')
                return connected;
            AddEntireBox(map, connected, current, map[current]);
            AddEntireBox(map, alreadyConnected, current, map[current]);
            var newConnectedBoxes = new HashSet<List<KeyValuePair<(int, int), char>>>();

            foreach (var box in connected)
            {
                newConnectedBoxes.Add(ConnectedBoxes(map, alreadyConnected, ScaleTuple(box.Key, direction, 1), direction));
            }

            foreach (var newConnectedBox in newConnectedBoxes)
            {
                if (newConnectedBox.Any())
                    connected = connected.Concat(newConnectedBox).ToList();
            }
            
            return connected;
        }
        
        private static bool BoxesArePushable(Dictionary<(int x, int y), char> map,
            (int x, int y) direction, HashSet<(int, int)> boxes)
        {
            var next = boxes.Select(box => map.FirstOrDefault(pair => 
                    pair.Key == ScaleTuple(box, direction, 1), 
                (new KeyValuePair<(int x, int y), char>(Invalid, '#')))).ToList();

            foreach (var nextSpot in next)
            {
                if (nextSpot.Value == '#')
                    return false;
            }
            return true;
        }

        private static void Move(Dictionary<(int x, int y), char> map, (int x, int y) pos, (int x, int y) direction)
        {
            var next = map.FirstOrDefault(pair =>
                    pair.Key == ScaleTuple(pos, direction, 1),
                (new KeyValuePair<(int x, int y), char>(Invalid, '#')));
            if (next.Value == '#')
                return;
            if (next.Value == 'O')
            {
                var freeSpace = NextFreeSpace(map, next.Key, direction);
                if (freeSpace == Invalid)
                    return;
                map[freeSpace] = 'O';
            }
            map[next.Key] = '@';
            map[pos] = '.';
        }

        private static void HighlightBoxes(Dictionary<(int x, int y), char> map, 
            HashSet<(int, int)> boxes)
        {
            var minX = map.MinBy(pair => pair.Key.x).Key.x;
            var minY = map.MinBy(pair => pair.Key.y).Key.y;
            var maxX = map.MaxBy(pair => pair.Key.x).Key.x;
            var maxY = map.MaxBy(pair => pair.Key.y).Key.y;
            Console.WriteLine();
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    if (boxes.Contains((x, y)))
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    if (map.ContainsKey((x, y)))
                        Console.Write(map[(x,y)]);
                    else
                        Console.Write('.');
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        private static void MoveWide(Dictionary<(int x, int y), char> map, (int x, int y) pos, (int x, int y) direction)
        {
            var next = map.FirstOrDefault(pair =>
                    pair.Key == ScaleTuple(pos, direction, 1),
                (new KeyValuePair<(int x, int y), char>(Invalid, '#')));
            if (next.Value == '#')
                return;
            if (next.Value == '[' || next.Value == ']')
            {
                var boxes = ConnectedBoxes(map, new List<KeyValuePair<(int, int), char>>(), next.Key, direction);
                if (!BoxesArePushable(map, direction, boxes.Select(pair => pair.Key).ToHashSet()))
                    return;
                var pushedBoxes = boxes.ToDictionary(group => ScaleTuple(group.Key, direction, 1), group => group.Value);
                foreach (var box in boxes)
                {
                    map[box.Key] = '.';
                }

                foreach (var (box, val) in pushedBoxes)
                {
                    map[box] = val;
                }
            }
            map[next.Key] = '@';
            map[pos] = '.';
        }

        private static void PrintMap(Dictionary<(int x, int y), char> map)
        {
            var minX = map.MinBy(pair => pair.Key.x).Key.x;
            var minY = map.MinBy(pair => pair.Key.y).Key.y;
            var maxX = map.MaxBy(pair => pair.Key.x).Key.x;
            var maxY = map.MaxBy(pair => pair.Key.y).Key.y;
            Console.WriteLine();
            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    if (map.ContainsKey((x, y)))
                        Console.Write(map[(x,y)]);
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        private static Dictionary<(int x, int y), char> WidenMap(Dictionary<(int x, int y), char> map)
        {
            Dictionary<(int x, int y), char> wideMap = new();
            foreach (var (coord, value) in map)
            {
                if (value == 'O')
                {
                    wideMap[(coord.x * 2, coord.y)] = '[';
                    wideMap[(coord.x * 2 + 1, coord.y)] = ']';
                }
                else if (value == '@')
                {
                    wideMap[(coord.x * 2, coord.y)] = value;
                    wideMap[(coord.x * 2 + 1, coord.y)] = '.';
                }
                else
                {
                    wideMap[(coord.x * 2, coord.y)] = value;
                    wideMap[(coord.x * 2 + 1, coord.y)] = value;
                }
            }

            return wideMap;
        }
        
        private static void PartOne(string[] input)
        {
            long total = 0;
            
            var split = string.Join("\n", input)
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line
                    .Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .ToList();
            
            var map = split[0]
                .SelectMany((row, y) => row
                    .Select((value, x) => new { coord = (x, y), value }))
                .GroupBy(group => group.coord)
                .ToDictionary(group => group.Key,
                    group => group.First().value);
            
            var movement = string.Join("", split[1])
                .Select(movement => MovementToDirection(movement))
                .ToList();

            foreach (var direction in movement)
            {
                var pos = map.First(pair => pair.Value == '@').Key;
                Move(map, pos, direction);
            }

            var boxes = map
                .Where(pair => pair.Value == 'O')
                .Select(pair => pair.Key);
            
            total += boxes.Sum(box => box.y * 100 + box.x);

            Console.WriteLine(total);
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            
            var split = string.Join("\n", input)
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line
                    .Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .ToList();
            
            var map = WidenMap(split[0]
                .SelectMany((row, y) => row
                    .Select((value, x) => new { coord = (x, y), value }))
                .GroupBy(group => group.coord)
                .ToDictionary(group => group.Key,
                    group => group.First().value));
            
            var movement = string.Join("", split[1])
                .Select(movement => MovementToDirection(movement))
                .ToList();

            foreach (var direction in movement)
            {
                var pos = map.First(pair => pair.Value == '@').Key;
                MoveWide(map, pos, direction);
            }

            var boxes = map
                .Where(pair => pair.Value == '[')
                .Select(pair => pair.Key);
            
            total += boxes.Sum(box => box.y * 100 + box.x);

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