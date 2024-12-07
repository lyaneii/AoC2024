using System;
using System.IO;
using System.Reflection;

namespace day7
{
	public static class Program
	{
		private static long ConcatenateLongInt(long l, long r)
		{
			return l * (long)Math.Pow(10, r.ToString().Length) + r;
		}

		private static void Evaluate(List<long> results, List<long> equation, long current, int index)
		{
			if (index < equation.Count)
			{
				Evaluate(results, equation, current * equation[index], index + 1);
				Evaluate(results, equation, current + equation[index], index + 1);
			}
			else
				results.Add(current);
		}

		private static void EvaluateElephants(List<long> results, List<long> equation, long current, int index)
		{
			if (index < equation.Count)
			{
				EvaluateElephants(results, equation, current * equation[index], index + 1);
				EvaluateElephants(results, equation, current + equation[index], index + 1);
				EvaluateElephants(results, equation, ConcatenateLongInt(current, equation[index]), index + 1);
			}
			else
				results.Add(current);
		}

		private static void PartOne(string[] input)
		{
			long total = 0;
			List<List<long>> equations = input
				.Select(entry => entry
					.Split([' ', ':'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
					.Select(long.Parse)
					.ToList())
				.ToList();
			var results = new List<long>();

			foreach (var equation in equations)
			{
				results.Clear();
				Evaluate(results, equation, equation[1], 2);
				if (results.Contains(equation[0]))
					total += equation[0];
			}

			Console.WriteLine(total);
		}

		private static void PartTwo(string[] input)
		{
			long total = 0;
			List<List<long>> equations = input
				.Select(entry => entry
					.Split([' ', ':'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
					.Select(long.Parse)
					.ToList())
				.ToList();
			var results = new List<long>();

			foreach (var equation in equations)
			{
				results.Clear();
				EvaluateElephants(results, equation, equation[1], 2);
				if (results.Contains(equation[0]))
					total += equation[0];
			}

			Console.WriteLine(total);
		}

		public static void Main(string[] args)
		{
			var lines = File.ReadAllLines("../../../input.txt");

			Console.Write("Part One: ");
			PartOne(lines);
			Console.Write("Part Two: ");
			// PartTwo(lines);
		}
	}
}