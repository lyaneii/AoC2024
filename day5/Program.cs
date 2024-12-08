using System;

namespace day5
{
    public struct OrderRules
    {
        public List<int> First;
        public List<int> Second;
    };

    public static class Program
    {
        private static int PageAdheresOrderRules(OrderRules orderRules, List<int> pageUpdates, int current)
        {
            for (int i = 0; i < orderRules.First.Count; i++)
            {
                if (orderRules.First[i] == pageUpdates[current])
                {
                    for (int j = current - 1; j >= 0; j--)
                    {
                        if (orderRules.Second[i] == pageUpdates[j])
                        {
                            return orderRules.Second[i];
                        }
                    }
                }
            }

            return 0;
        }

        private static int UpdateIsInCorrectOrder(OrderRules orderRules, List<int> pageUpdates)
        {
            for (int i = 0; i < pageUpdates.Count; i++)
            {
                if (PageAdheresOrderRules(orderRules, pageUpdates, i) != 0)
                    return -1;
            }

            return pageUpdates[pageUpdates.Count / 2];
        }

        private static void PartOne(string[] input)
        {
            long total = 0;
            bool pageUpdates = false;
            OrderRules orderRules;
            orderRules.First = new List<int>();
            orderRules.Second = new List<int>();
            var updateList = new List<List<int>>();

            foreach (var line in input)
            {
                if (line == "")
                    pageUpdates = true;
                else if (pageUpdates)
                {
                    var update = new List<int>();
                    foreach (var element in line.Split(',',
                                 StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                        update.Add(Convert.ToInt32(element));
                    updateList.Add(update);
                }
                else
                {
                    int count = 0;
                    foreach (var element in line.Split('|',
                                 StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (count % 2 == 0)
                            orderRules.First.Add(Convert.ToInt32(element));
                        else
                            orderRules.Second.Add(Convert.ToInt32(element));

                        count++;
                    }
                }
            }

            foreach (var update in updateList)
            {
                var middlePage = UpdateIsInCorrectOrder(orderRules, update);

                if (middlePage != -1)
                    total += middlePage;
            }

            Console.WriteLine(total);
        }

        private static void SortUpdate(OrderRules orderRules, List<int> update)
        {
            for (int i = 0; i < update.Count; i++)
            {
                int invalidPage = PageAdheresOrderRules(orderRules, update, i);
                while (invalidPage != 0)
                {
                    update.Remove(invalidPage);
                    update.Insert(i, invalidPage);
                    invalidPage = PageAdheresOrderRules(orderRules, update, i);
                }
            }
        }

        private static void PartTwo(string[] input)
        {
            long total = 0;
            bool pageUpdates = false;
            OrderRules orderRules;
            orderRules.First = new List<int>();
            orderRules.Second = new List<int>();
            var updateList = new List<List<int>>();

            foreach (var line in input)
            {
                if (line == "")
                    pageUpdates = true;
                else if (pageUpdates)
                {
                    var update = new List<int>();
                    foreach (var element in line.Split(',',
                                 StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                        update.Add(Convert.ToInt32(element));
                    updateList.Add(update);
                }
                else
                {
                    int count = 0;
                    foreach (var element in line.Split('|',
                                 StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (count % 2 == 0)
                            orderRules.First.Add(Convert.ToInt32(element));
                        else
                            orderRules.Second.Add(Convert.ToInt32(element));
                        count++;
                    }
                }
            }

            foreach (var update in updateList)
            {
                bool sortPage = false;
                var middlePage = UpdateIsInCorrectOrder(orderRules, update);

                while (middlePage == -1)
                {
                    sortPage = true;
                    SortUpdate(orderRules, update);
                    middlePage = UpdateIsInCorrectOrder(orderRules, update);
                }

                if (sortPage)
                    total += update[update.Count / 2];
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