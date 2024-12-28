using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class ClawMachineSolver
{
    public class Machine
    {
        public int AX { get; set; }
        public int AY { get; set; }
        public int BX { get; set; }
        public int BY { get; set; }
        public int PrizeX { get; set; }
        public int PrizeY { get; set; }
    }

    public static void Main(string[] args)
    {
        // Read the input file
        var machines = ParseInput("list.txt");

        // Process each machine and calculate the minimum tokens required
        int totalTokens = 0;
        int prizesWon = 0;

        foreach (var machine in machines)
        {
            int minTokens = CalculateMinimumTokens(machine);

            if (minTokens != int.MaxValue) // Prize can be won
            {
                totalTokens += minTokens;
                prizesWon++;
            }
        }

        Console.WriteLine($"Prizes won: {prizesWon}");
        Console.WriteLine($"Minimum tokens spent: {totalTokens}");
    }

    private static List<Machine> ParseInput(string filePath)
    {
        var machines = new List<Machine>();
        var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray(); // Ignore empty lines

        for (int i = 0; i < lines.Length; i += 3)
        {
            if (i + 2 >= lines.Length)
            {
                Console.WriteLine("Error: Input file is missing lines for a complete machine definition.");
                continue;
            }

            try
            {
                var buttonA = lines[i].Split(new[] { "X+", ", Y+" }, StringSplitOptions.None);
                var buttonB = lines[i + 1].Split(new[] { "X+", ", Y+" }, StringSplitOptions.None);
                var prize = lines[i + 2].Split(new[] { "X=", ", Y=" }, StringSplitOptions.None);

                if (buttonA.Length < 3 || buttonB.Length < 3 || prize.Length < 3)
                {
                    Console.WriteLine($"Error: Malformed input at lines {i + 1}-{i + 3}.");
                    continue;
                }

                machines.Add(new Machine
                {
                    AX = int.Parse(buttonA[1]),
                    AY = int.Parse(buttonA[2]),
                    BX = int.Parse(buttonB[1]),
                    BY = int.Parse(buttonB[2]),
                    PrizeX = int.Parse(prize[1]),
                    PrizeY = int.Parse(prize[2])
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing machine definition at lines {i + 1}-{i + 3}: {ex.Message}");
            }
        }

        return machines;
    }

    private static int CalculateMinimumTokens(Machine machine)
    {
        int maxPresses = 100;
        int minTokens = int.MaxValue;

        for (int aPresses = 0; aPresses <= maxPresses; aPresses++)
        {
            for (int bPresses = 0; bPresses <= maxPresses; bPresses++)
            {
                int x = aPresses * machine.AX + bPresses * machine.BX;
                int y = aPresses * machine.AY + bPresses * machine.BY;

                if (x == machine.PrizeX && y == machine.PrizeY)
                {
                    int cost = aPresses * 3 + bPresses * 1;
                    minTokens = Math.Min(minTokens, cost);
                }
            }
        }

        return minTokens;
    }
}