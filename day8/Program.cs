using System;
using System.IO;
using System.Reflection;

namespace day8
{
	public static class Program
	{
		private static void PrintAntinodes(List<(int, int)> antennas, List<(int, int)> antinodes, (int, int) size)
		{
			for (int x = 0; x < size.Item1; x++)
			{
				for (int y = 0; y < size.Item2; y++)
				{
					if (antennas.Any(node => node.Item1 == x && node.Item2 == y))
						Console.Write('O');
					else if (antinodes.Any(node => node.Item1 == x && node.Item2 == y))
						Console.Write('#');
					else
						Console.Write('.');
				}
				Console.WriteLine();
			}
		}
		
		private static Dictionary<char, List<(int, int)>> ParseAntennas(string[] input)
		{
			var antennas = new Dictionary<char, List<(int, int)>>();
			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					if (input[y][x] == '.')
						continue;
					if (!antennas.TryGetValue(input[y][x], out var list))
					{
						list = new List<(int, int)>();
						antennas[input[y][x]] = list;
					}
					list.Add((y, x));
				}
			}

			return antennas;
		}

		private static void PlaceAntinodes(
			List<(int, int)> antennas, 
			List<(int, int)> antinodes,
			(int, int) size)
		{
			for (int i = 0; i < antennas.Count; i++)
			{
				for (int j = i + 1; j < antennas.Count; j++)
				{
					var diff = (antennas[j].Item1 - antennas[i].Item1, antennas[j].Item2 - antennas[i].Item2);
					var t1 = (antennas[i].Item1 - diff.Item1, antennas[i].Item2 - diff.Item2);
					var t2 = (antennas[j].Item1 + diff.Item1, antennas[j].Item2 + diff.Item2);

					if (t1.Item1 >= 0 && t1.Item1 < size.Item1 &&
					    t1.Item2 >= 0 && t1.Item2 < size.Item2 &&
					    !antinodes.Contains(t1))
					{
						antinodes.Add(t1);
					}

					if (t2.Item1 >= 0 && t2.Item1 < size.Item1 &&
					    t2.Item2 >= 0 && t2.Item2 < size.Item2 &&
						!antinodes.Contains(t2))
					{
						antinodes.Add(t2);
					}
				}
			}
		}
		
		private static void PlaceHarmonicAntinodes(
			List<(int, int)> antennas, 
			List<(int, int)> antinodes,
			(int, int) size)
		{
			for (int i = 0; i < antennas.Count; i++)
			{
				for (int j = i + 1; j < antennas.Count; j++)
				{
					if (!antinodes.Contains(antennas[i]))
						antinodes.Add(antennas[i]);
					if (!antinodes.Contains(antennas[j]))
						antinodes.Add(antennas[j]);
					
					var diff = (antennas[j].Item1 - antennas[i].Item1, antennas[j].Item2 - antennas[i].Item2);
					var t1 = (antennas[i].Item1 - diff.Item1, antennas[i].Item2 - diff.Item2);
					var t2 = (antennas[j].Item1 + diff.Item1, antennas[j].Item2 + diff.Item2);

					while (t1.Item1 >= 0 && t1.Item1 < size.Item1 && 
					       t1.Item2 >= 0 && t1.Item2 < size.Item2)
					{
						if (!antinodes.Contains(t1))
							antinodes.Add(t1);
						t1 = (t1.Item1 - diff.Item1, t1.Item2 - diff.Item2);
					}

					while (t2.Item1 >= 0 && t2.Item1 < size.Item1 && 
					       t2.Item2 >= 0 && t2.Item2 < size.Item2)
					{
						if (!antinodes.Contains(t2))
							antinodes.Add(t2);
						t2 = (t2.Item1 + diff.Item1, t2.Item2 + diff.Item2);
					}
				}
			}
		}
		
		private static void PartOne(string[] input)
		{
			var antennaDict = ParseAntennas(input);
			var antinodes = new List<(int, int)>();
			
			foreach (var antennas in antennaDict.Values)
				PlaceAntinodes(antennas, antinodes, (input.First().Length, input.Length));
			Console.WriteLine(antinodes.Count);
		}

		private static void PartTwo(string[] input)
		{
			var antennaDict = ParseAntennas(input);
			var antinodes = new List<(int, int)>();

			foreach (var antennas in antennaDict.Values)
				PlaceHarmonicAntinodes(antennas, antinodes, (input.First().Length, input.Length));
			Console.WriteLine(antinodes.Count);
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