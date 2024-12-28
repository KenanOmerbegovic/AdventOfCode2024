using System;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles._2024._22.Part1
{
    public class Part1
    {
        public async Task<string> SolveAsync(string filePath)
        {
            ulong sum = 0;
            var lines = await File.ReadAllLinesAsync(filePath); // Read all lines from the file
            foreach (var line in lines)
            {
                ulong secretNumber = ulong.Parse(line);
                for (int i = 0; i < 2000; i++)
                {
                    secretNumber = Evolve(secretNumber);
                }

                sum += secretNumber;
            }

            return sum.ToString();
        }

        private ulong Mix(ulong a, ulong b) => a ^ b;

        private ulong Prune(ulong a) => a % 16777216;

        private ulong Evolve(ulong secretNumber)
        {
            var multiple = secretNumber * 64UL;
            secretNumber = Mix(secretNumber, multiple);
            secretNumber = Prune(secretNumber);

            var divide = secretNumber / 32UL;
            secretNumber = Mix(secretNumber, divide);
            secretNumber = Prune(secretNumber);

            multiple = secretNumber * 2048;
            secretNumber = Mix(secretNumber, multiple);
            secretNumber = Prune(secretNumber);

            return secretNumber;
        }
    }

    // Entry point for the program
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var part1 = new Part1();

            // Specify the input file path
            string filePath = "day22.txt"; // Change this to the actual file path

            // Solve the puzzle and print the result
            var result = await part1.SolveAsync(filePath);
            Console.WriteLine($"Result: {result}");
        }
    }
}
