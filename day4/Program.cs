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

			if (count != 0)
				Console.WriteLine("{0} horizontal: {1}", occurence, count);
			return count;
		}

		private static int DiagonalOccurence(string[] input, string occurence, int lineWidth, int position)
		{
			int count = 0;

			if (input.Length - position < occurence.Length)
				return 0;
			// if (input[position] != "SMSMSASXSS" || occurence != "SAMX")
			// 	return 0;
			for (int i = 0; i < lineWidth; i++)
			{
				if (input[position][i] == occurence[0])
				{
					bool found = true;
					if (i <= lineWidth - occurence.Length)
					{
						Console.WriteLine("positive lookup, {0}", i);
						for (int j = 1; j < occurence.Length; j++)
						{
							if (input[position + j][i + j] != occurence[j])
							{
								Console.WriteLine("not found");
								found = false;
								break;
							}
						}

						if (found)
						{
							Console.WriteLine("found");
							count++;
						}
					}

					found = true;
					if (i >= occurence.Length)
					{
						Console.WriteLine("negative lookup, {0}", i);
						for (int j = 1; j < occurence.Length; j++)
						{
							if (input[position + j][i - j] != occurence[j])
							{
								Console.WriteLine("not found");
								found = false;
								break;
							}
						}

						if (found)
						{
							Console.WriteLine("found");
							count++;
						}
					}
				}
			}
			if (count != 0)
				Console.WriteLine("{0} diagonal: {1}", occurence, count);
			return count;
		}

		private static int VerticalOccurence(string[] input, string occurence, int lineWidth, int position)
		{
			int count = 0;
			
			if (input.Length - position < occurence.Length)
				return 0;
			for (int i = 0; i < lineWidth; i++)
			{
				if (input[position][i] == occurence[0])
				{
					bool found = true;

					for (int j = 1; j < occurence.Length; j++)
					{
						if (input[position + j][i] != occurence[j])
						{
							found = false;
							break;
						}
					}

					if (found)
						count++;
				}
			}

			if (count != 0)
				Console.WriteLine("{0} vertical: {1}", occurence, count);
			return count;
		}
		
		private static void PartOne(string[] input)
		{
			long total = 0;
			var lineWidth = input[0].Length;
			int verticalPosition = 0;

			foreach (var line in input)
			{
				Console.WriteLine("\n" + line);
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

		private static void PartTwo(string[] input)
		{
			long total = 0;

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