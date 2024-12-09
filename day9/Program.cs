using System;
using System.IO;

namespace day9
{
    public static class Program
    {
        private const int FreeSpace = -1;

        private static void PrintDiskMap(List<int> diskMap)
        {
            foreach (var file in diskMap)
            {
                Console.Write(file == -1 ? '.' : file.ToString());
            }
            Console.WriteLine();
        }
        
        private static List<int> ReadDiskMap(string input)
        {
            var list = new List<int>();
            var id = 0;
            var freeSpace = false;

            foreach (var c in input)
            {
                for (int i = '0'; i < c; i++)
                    list.Add(freeSpace ? FreeSpace : (id));
                if (freeSpace)
                    id++;
                freeSpace = !freeSpace;
            }

            return list;
        }

        private static void MoveFileBlocks(List<int> diskMap)
        {
            var rev = diskMap.Count - 1;
            for (var file = 0; file <= rev; file++)
            {
                if (diskMap[file] != FreeSpace)
                    continue;
                while (rev >= 0 && diskMap[rev] == FreeSpace)
                    rev--;
                (diskMap[file], diskMap[rev]) = (diskMap[rev], diskMap[file]);
                rev--;
            }
        }
        
        private static void MoveWholeFileBlocks(List<int> diskMap)
        {
            var fileBlocks = new List<List<int>>();
            var freeSpaceBlocks = new List<List<int>>();
            
            for (var id = diskMap.Max(); id >= 0; id--)
            {
                var fileIndices = diskMap
                    .Select((file, index) => new { file, index })
                    .Where(block => block.file == id)
                    .Select(block => block.index)
                    .ToList();
                fileBlocks.Add(fileIndices);
            }
            
            var freeSpaceIndices = diskMap
                .Select((file, index) => new { file, index })
                .Where(block => block.file == FreeSpace)
                .Select(block => block.index)
                .ToList();

            var currentBlock = new List<int>();
            for (int i = 0, j = 1; j < freeSpaceIndices.Count; i++, j++)
            {
                if (freeSpaceIndices[j] - freeSpaceIndices[i] > 1)
                {
                    currentBlock.Add(freeSpaceIndices[i]);
                    freeSpaceBlocks.Add(currentBlock);
                    currentBlock = new List<int>();
                }
                else
                    currentBlock.Add(freeSpaceIndices[i]);
            }
            if (currentBlock.Count != 0)
                freeSpaceBlocks.Add(currentBlock);

            var ids = diskMap.Max();
            foreach (var fileBlock in fileBlocks)
            {
                foreach (var freeSpaceBlock in freeSpaceBlocks)
                {
                    if (freeSpaceBlock.Count < fileBlock.Count || freeSpaceBlock[0] >= fileBlock[0])
                        continue;
                    for (var i = 0; i < fileBlock.Count; i++)
                    {
                        diskMap[freeSpaceBlock[i]] = ids;
                        diskMap[fileBlock[i]] = FreeSpace;
                    }
                    freeSpaceBlock.RemoveRange(0, fileBlock.Count);
                    break;
                }
                ids--;
            }
        }
        
        private static void PartOne(string input)
        {
            long total = 0;
            List<int> diskMap = ReadDiskMap(input);
            
            MoveFileBlocks(diskMap);

            var index = 0;
            foreach (var file in diskMap.Where(file => file != FreeSpace))
            {
                total += file * index;
                index++;
            }

            Console.WriteLine(total);
        }

        private static void PartTwo(string input)
        {
            long total = 0;
            List<int> diskMap = ReadDiskMap(input);
            
            MoveWholeFileBlocks(diskMap);

            var index = 0;
            foreach (var file in diskMap.Where(file => file != FreeSpace))
            {
                total += file * index;
                index++;
            }

            Console.WriteLine(total);
        }

        public static void Main(string[] args)
        {
            var lines = File.ReadAllLines("../../../input.txt");

            Console.Write("Part One: ");
            PartOne(lines.First());
            Console.Write("Part Two: ");
            PartTwo(lines.First());
        }
    }
}