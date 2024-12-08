using System;
using System.Text.RegularExpressions;

namespace day3
{
	public static class Program
	{
		private static void PartOne(string input)
		{
			long total = 0;

			const string pattern = @"(?<=mul\()(\d+,\d+)(?=\))";

			MatchCollection matches = Regex.Matches(input, pattern);

			foreach (Match match in matches)
			{
				var pair = match.Groups[0].Value.Split(',',
					StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
				total += Convert.ToInt64(pair[0]) * Convert.ToInt64(pair[1]);
			}

			Console.WriteLine(total);
		}

		private static void PartTwo(string input)
		{
			long total = 0;

			const string pattern = @"do\(\)|(?<=mul\()(\d+,\d+)(?=\))|don't\(\)";

			MatchCollection matches = Regex.Matches(input, pattern);

			bool doMul = true;
			foreach (Match match in matches)
			{
				if (match.Groups[0].Value == "do()")
					doMul = true;
				else if (match.Groups[0].Value == "don't()")
					doMul = false;
				else if (doMul)
				{
					var pair = match.Groups[0].Value.Split(',',
						StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
					total += Convert.ToInt64(pair[0]) * Convert.ToInt64(pair[1]);
				}
			}

			Console.WriteLine(total);
		}

		public static void Main(string[] args)
		{
			var lines = File.ReadAllLines("../../../input.txt");
			var input = string.Concat(lines);

			const string pattern = @"do\(\)|(?<=mul\()(\d+,\d+)(?=\))|don't\(\)";

			Console.Write("Part One: ");
			PartOne(input);
			Console.Write("Part Two: ");
			PartTwo(input);
		}
	}
}