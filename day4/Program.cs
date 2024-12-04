using System;
using System.Text.RegularExpressions;

namespace day4
{
	public static class Program
	{
		private static int HorizontalOccurence(string s, string occurence)
		{
			int count = 0;
			var searchPosition = s.IndexOf(occurence);
			
			while (searchPosition >= 0)
			{
				count++;
				searchPosition = s.IndexOf(occurence, searchPosition + occurence.Length);
			}

			return count;
		}

		private static int DiagonalOccurence(string[] input, string occurence, int lineWidth, int verticalPosition)
		{
			int count = 0;

			if (input.Length - verticalPosition < occurence.Length)
				return 0;
			for (int i = 0; i < lineWidth; i++)
			{
				if (input[verticalPosition][i] == occurence[0])
				{
					bool found = true;
					if (i <= lineWidth - occurence.Length)
					{
						for (int j = 1; j < occurence.Length; j++)
						{
							if (input[verticalPosition + j][i + j] != occurence[j])
							{
								found = false;
								break;
							}
						}

						if (found)
							count++;
					}

					found = true;
					if (i >= occurence.Length - 1)
					{
						for (int j = 1; j < occurence.Length; j++)
						{
							if (input[verticalPosition + j][i - j] != occurence[j])
							{
								found = false;
								break;
							}
						}

						if (found)
							count++;
					}
				}
			}
			return count;
		}

		private static int VerticalOccurence(string[] input, string occurence, int lineWidth, int verticalPosition)
		{
			int count = 0;
			
			if (input.Length - verticalPosition < occurence.Length)
				return 0;
			for (int i = 0; i < lineWidth; i++)
			{
				if (input[verticalPosition][i] == occurence[0])
				{
					bool found = true;
					for (int j = 1; j < occurence.Length; j++)
					{
						if (input[verticalPosition + j][i] != occurence[j])
						{
							found = false;
							break;
						}
					}

					if (found)
						count++;
				}
			}
			return count;
		}
		
		private static void PartOne(string[] input)
		{
			long total = 0;
			var lineWidth = input[0].Length;
			int verticalPosition = 0;

			foreach (var line in input)
			{
				total += HorizontalOccurence(line, "XMAS");
				total += HorizontalOccurence(line, "SAMX");
				total += DiagonalOccurence(input, "XMAS", lineWidth, verticalPosition);
				total += DiagonalOccurence(input, "SAMX", lineWidth, verticalPosition);
				total += VerticalOccurence(input, "XMAS", lineWidth, verticalPosition);
				total += VerticalOccurence(input, "SAMX", lineWidth, verticalPosition);
				verticalPosition++;
			}

			Console.WriteLine(total);
		}

		private static int DiagonalCenterOccurence(string[] input, string occurence, int lineWidth,
			int verticalPosition, int horizontalPosition)
		{
			int count = 0;

			if (input[verticalPosition][horizontalPosition] == occurence[occurence.Length / 2])
			{
				bool found = true;
				for (int j = 0; j < occurence.Length; j++)
				{
					if (input[verticalPosition - occurence.Length / 2 + j][horizontalPosition - occurence.Length / 2 + j] !=
					    occurence[j])
					{
						found = false;
						break;
					}
				}

				if (found)
					count++;

				found = true;
				for (int j = 0; j < occurence.Length; j++)
				{
					if (input[verticalPosition - occurence.Length / 2 + j][horizontalPosition + occurence.Length / 2 - j] !=
					    occurence[j])
					{
						found = false;
						break;
					}
				}

				if (found)
					count++;
			}

			return count;
		}

		private static int CrossOccurence(string[] input, string occurence, int lineWidth, int verticalPosition)
		{
			int count = 0;
			string reverse = new string(occurence.Reverse().ToArray());

			if (input.Length - verticalPosition <= occurence.Length / 2
				|| verticalPosition < occurence.Length / 2)
				return 0;

			for (int horizontalPosition = occurence.Length / 2;
			     horizontalPosition < lineWidth - occurence.Length / 2;
			     horizontalPosition++)
			{
				if (DiagonalCenterOccurence(input, occurence, lineWidth, verticalPosition, horizontalPosition) +
				    DiagonalCenterOccurence(input, reverse, lineWidth, verticalPosition, horizontalPosition) == 2)
					count++;
			}
			
			return count;
		}

		private static void PartTwo(string[] input)
		{
			long total = 0;
			var lineWidth = input[0].Length;

			for (int verticalPosition = 0; verticalPosition < input.Length; verticalPosition++)
			{
				total += CrossOccurence(input, "MAS", lineWidth, verticalPosition);
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