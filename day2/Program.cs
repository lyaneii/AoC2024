using System;

namespace day2
{
	public static class Program
	{
		private static bool SafetyCheck(List<int> list)
		{
			if (list.Count < 2)
				return true;
			var decrease = list[1] - list[0] < 0 ? true : false;

			for (int i = 0, j = 1; j < list.Count; i++, j++)
			{
				if (Math.Abs(list[j] - list[i]) > 3
				    || list[j] - list[i] == 0
				    || (decrease && list[j] - list[i] > 0)
				    || (!decrease && list[j] - list[i] < 0))
				{
					return false;
				}
			}

			return true;
		}

		private static bool SafetyCheckDampener(List<int> list, int tolerateLevel)
		{
			var toleratedList = new List<int>(list);
			toleratedList.RemoveAt(tolerateLevel);
			return SafetyCheck(toleratedList);
		}

		private static bool SafetyCheckPartTwo(List<int> list)
		{
			if (list.Count < 2)
				return true;

			if (SafetyCheckDampener(list, 0))
				return true;
			var decrease = list[1] - list[0] < 0 ? true : false;
			bool safe = true;

			for (int i = 0, j = 1; j < list.Count; i++, j++)
			{
				if (Math.Abs(list[j] - list[i]) > 3
				    || list[j] - list[i] == 0
				    || (decrease && list[j] - list[i] > 0)
				    || (!decrease && list[j] - list[i] < 0))
				{
					if (SafetyCheckDampener(list, i))
						return true;
					if (SafetyCheckDampener(list, j))
						return true;
					safe = false;
				}
			}

			return safe;
		}

		private static void PartOne(string[] input)
		{
			long total = 0;

			foreach (var line in input)
			{
				var list = new List<int>();
				var report = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				foreach (var level in report)
				{
					list.Add(Convert.ToInt32(level));
				}

				if (SafetyCheck(list))
					total++;
			}

			Console.WriteLine(total);
		}

		private static void PartTwo(string[] input)
		{
			long total = 0;

			foreach (var line in input)
			{
				var list = new List<int>();
				var report = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

				foreach (var level in report)
				{
					list.Add(Convert.ToInt32(level));
				}

				if (SafetyCheckPartTwo(list))
					total++;
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