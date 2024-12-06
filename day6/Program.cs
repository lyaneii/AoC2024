using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic.CompilerServices;

namespace day6
{
	public static class Program
	{
		private class Vec2 : IEquatable<Vec2>
		{
			public int X { get; set; }
			public int Y { get; set; }

			public Vec2(int x, int y)
			{
				X = x;
				Y = y;
			}
	
			public void Invert()
			{
				(X, Y) = (Y, X);
			}
			
			public bool Equals(Vec2? other)
			{
				if (other is null) 
					return false;
				return X == other.X && Y == other.Y;
			}
			
			public override int GetHashCode()
			{
				return HashCode.Combine(X, Y);
			}

			public override string ToString()
			{
				return $"({X}, {Y})";
			}

			public static Vec2 operator +(Vec2 a, Vec2 b)
			{
				return new Vec2(a.X + b.X, a.Y + b.Y);
			}
			
			public static Vec2 operator *(Vec2 a, int d)
			{
				return new Vec2(a.X * d, a.Y * d);
			}
			
			public static Vec2 operator -(Vec2 a, Vec2 b)
			{
				return new Vec2(a.X - b.X, a.Y - b.Y);
			}	
			
			public static bool operator ==(Vec2? a, Vec2? b)
			{
				if (ReferenceEquals(a, b))
					return true;
				if (a is null || b is null)
					return false;
				return a.Equals(b);
			}
			
			public static bool operator !=(Vec2 a, Vec2 b)
			{
				return !(a == b);
			}
		}

		private static readonly Vec2 NoneVec2 = new Vec2(-1, -1);

		private static readonly List<Vec2> Cardinals = new List<Vec2>
		{
			new Vec2(1, 0),
			new Vec2(0, 1),
			new Vec2(-1, 0),
			new Vec2(0, -1),
		};

		private static Vec2 TurnRight90(Vec2 currentDirection)
		{
			bool turn = false;
			foreach (var vec2 in Cardinals)
			{
				if (currentDirection == vec2)
				{
					turn = true;
					continue;
				}
				if (turn)
					return vec2;
			}

			return Cardinals.First();
		}
		
		private static Vec2 TurnLeft90(Vec2 currentDirection)
		{
			bool turn = false;
			for (int i = Cardinals.Count; i >= 0; i--)
			{
				if (currentDirection == Cardinals[i])
				{
					turn = true;
					continue;
				}
				if (turn)
					return Cardinals[i];
			}

			return Cardinals.Last();
		}
		
		private static bool TakeStep(bool[,] obstacles, bool[,] visited, ref Vec2 direction, ref Vec2 currentPosition)
		{
			visited[currentPosition.Y, currentPosition.X] = true;
			var next = currentPosition + direction;
			if (next.X < 0 || next.X >= obstacles.GetLength(0) ||
			    next.Y < 0 || next.Y >= obstacles.GetLength(1))
		    {
			    return false;
		    }
			else if (obstacles[next.Y, next.X])
			{
				direction = TurnRight90(direction);
				next = currentPosition + direction;
			}
			currentPosition = next;
			// PrintMap(visited);
			return true;
		}

		private static bool[,] MapFromStringArray(string[] input, Vec2 start, char movable, char obstacle, char empty)
		{
			int rows = input.Length;
			int cols = input.First().Length;
			bool[,] map = new bool[rows, cols];

			for (int y = 0; y < rows; y++)
			{
				for (int x = 0; x < cols; x++)
				{
					if (input[y][x] == obstacle)
						map[y,x] = true;
					else if (input[y][x] == movable)
					{
						start.X = x;
						start.Y = y;
					}
					else if (input[y][x] == empty)
						map[y, x] = false;
					else
						throw new InvalidDataException();
				}
			}
			return map;
		}

		private static void PrintMap(bool[,] map)
		{
			Console.WriteLine();
			for (int y = 0; y < map.GetLength(1); y++)
			{
				for (int x = 0; x < map.GetLength(0); x++)
				{
					if (map[y, x])
						Console.Write('X');
					else
						Console.Write('.');
				}
				Console.WriteLine();
			}
		}
		
		private static void PrintMap(bool[,] map, List<Vec2> highlights)
		{
			Console.WriteLine();
			for (int y = 0; y < map.GetLength(1); y++)
			{
				for (int x = 0; x < map.GetLength(0); x++)
				{
					bool highlighted = false;
					foreach (var highlight in highlights)
					{
						if (highlight.X == x && highlight.Y == y)
						{
							Console.ForegroundColor = ConsoleColor.Green;
							highlighted = true;
							break;
						}
					}
					if (map[y, x])
					{
						Console.Write('X');
					}
					else if (highlighted)
					{
						Console.ForegroundColor = ConsoleColor.Yellow;
						Console.Write('#');
					}
					else
						Console.Write('.');
					if (highlighted)
						Console.ResetColor();
				}
				Console.WriteLine();
			}
		}
		
		private static void PartOne(string[] input)
		{
			long total = 0;
			var direction = new Vec2(0, -1);
			var currentPosition = new Vec2(0, 0);
			bool[,] obstacles = MapFromStringArray(input, currentPosition, '^', '#', '.');
			bool[,] visited = new bool[input.Length, input.First().Length];

			while (TakeStep(obstacles, visited, ref direction, ref currentPosition)) {}

			foreach (var visit in visited)
			{
				if (visit)
					total++;
			}

			Console.WriteLine(total);
		}
		
		private static bool MapTurns(bool[,] obstacles, HashSet<(Vec2, Vec2)> map, ref Vec2 direction, ref Vec2 currentPosition)
		{
			var next = currentPosition + direction;
			if (next.X < 0 || next.X >= obstacles.GetLength(0) ||
			    next.Y < 0 || next.Y >= obstacles.GetLength(1))
			{
				return false;
			}
			else if (obstacles[next.Y, next.X])
			{
				direction = TurnRight90(direction);
				map.Add((currentPosition, direction));
				next = currentPosition + direction;
			}
			currentPosition = next;
			return true;
		}
		
		private static Vec2 NextObstacleStop(bool[,] obstacles, Vec2 currentPosition, Vec2 direction)
		{
			var next = currentPosition + direction;
			if (next.X < 0 || next.X >= obstacles.GetLength(0) ||
			    next.Y < 0 || next.Y >= obstacles.GetLength(1))
			{
				return NoneVec2;
			}

			while (!obstacles[next.Y, next.X])
			{
				next = next + direction;
				if (next.X < 0 || next.X >= obstacles.GetLength(0) ||
					next.Y < 0 || next.Y >= obstacles.GetLength(1))
				{
					return NoneVec2;
				}
			}
			return next - direction;
		}
		
		private static void TryLoop(bool[,] obstacles, bool[,] obstructions, Vec2 currentPosition, Vec2 direction)
		{
			Vec2 right = TurnRight90(direction);

			Vec2 obstacleNext = NextObstacleStop(obstacles, currentPosition, direction);
			Vec2 obstaclePrevious = NextObstacleStop(obstacles, currentPosition, right);
			Vec2 obstruction;
			
			
			if (obstacleNext != NoneVec2 && obstaclePrevious != NoneVec2)
			{
				Console.WriteLine("next & previous");
				PrintMap(obstacles, new List<Vec2>{obstacleNext + direction, obstaclePrevious + right});
				obstruction = obstaclePrevious + direction * (obstacleNext.Y - currentPosition.Y);
				obstructions[obstruction.X, obstruction.Y] = true;
			}
			else if (obstacleNext != NoneVec2)
			{
				Console.WriteLine("next");
				PrintMap(obstacles, new List<Vec2>{obstacleNext + direction});
				obstruction = NextObstacleStop(obstacles, obstacleNext, TurnRight90(right));
				if (obstruction != NoneVec2)
				{
					PrintMap(obstacles, new List<Vec2>{obstacleNext + direction, obstruction + TurnRight90(right)});
					obstruction = obstruction + TurnRight90(right) * (obstacleNext.Y - currentPosition.Y);
					obstructions[obstruction.X, obstruction.Y] = true;
				}
			}
			else if (obstaclePrevious != NoneVec2)
			{
				Console.WriteLine("previous");
				PrintMap(obstacles, new List<Vec2>{obstaclePrevious + right});
				obstruction = NextObstacleStop(obstacles, obstaclePrevious, TurnLeft90(direction));
				if (obstruction != NoneVec2)
				{
					PrintMap(obstacles, new List<Vec2>{obstaclePrevious + direction, obstruction + TurnLeft90(direction)});
					obstruction = currentPosition + direction * (obstruction.Y - currentPosition.Y);
					obstructions[obstruction.X, obstruction.Y] = true;
				}
			}
		}

		private static void PartTwo(string[] input)
		{
			long total = 0;
			var direction = new Vec2(0, -1);
			var currentPosition = new Vec2(0, 0);
			var obstacleDirectionMap = new HashSet<(Vec2, Vec2)>();
			bool[,] obstacles = MapFromStringArray(input, currentPosition, '^', '#', '.');
			bool[,] obstructions = new bool[input.Length, input.First().Length];

			// PrintMap(obstacles);
			while (MapTurns(obstacles, obstacleDirectionMap, ref direction, ref currentPosition)) {}

			
			foreach (var (pos, dir) in obstacleDirectionMap)
			{
				Console.Write("trying loop");
				PrintMap(obstacles, new List<Vec2>{pos});
				TryLoop(obstacles, obstructions, pos, dir);
			}
			PrintMap(obstructions);
			Console.WriteLine(total);
		}

		public static void Main(string[] args)
		{
			var lines = File.ReadAllLines("../../../example.txt");

			Console.Write("Part One: ");
			PartOne(lines);
			Console.Write("Part Two: ");
			PartTwo(lines);
		}
	}
}