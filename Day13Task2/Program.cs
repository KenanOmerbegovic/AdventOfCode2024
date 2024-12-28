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
        public long PrizeX { get; set; } // Updated to long to handle larger values
        public long PrizeY { get; set; } // Updated to long to handle larger values
    }

    public static void Main(string[] args)
    {
        var machines = ParseInput("list.txt");

        long totalTokens = 0;  // Use long to prevent overflow
        int prizesWon = 0;

        foreach (var machine in machines)
        {
            long minTokens = CalculateMinimumTokens(machine);

            if (minTokens != long.MaxValue)
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
        var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();

        for (int i = 0; i < lines.Length; i += 3)
        {
            try
            {
                var buttonA = lines[i].Split(new[] { "X+", ", Y+" }, StringSplitOptions.None);
                var buttonB = lines[i + 1].Split(new[] { "X+", ", Y+" }, StringSplitOptions.None);
                var prize = lines[i + 2].Split(new[] { "X=", ", Y=" }, StringSplitOptions.None);

                machines.Add(new Machine
                {
                    AX = int.Parse(buttonA[1]),
                    AY = int.Parse(buttonA[2]),
                    BX = int.Parse(buttonB[1]),
                    BY = int.Parse(buttonB[2]),
                    PrizeX = long.Parse(prize[1]) + 10000000000000,
                    PrizeY = long.Parse(prize[2]) + 10000000000000
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing machine definition: {ex.Message}");
            }
        }

        return machines;
    }

    private static long CalculateMinimumTokens(Machine machine)
    {
        long aX = machine.AX, aY = machine.AY;
        long bX = machine.BX, bY = machine.BY;
        long pX = machine.PrizeX, pY = machine.PrizeY;

        // Calculate the determinant of the matrix formed by the button vectors
        long determinant = aX * bY - aY * bX;

        if (determinant == 0)
        {
            // Buttons are linearly dependent, cannot span 2D space
            return long.MaxValue; // Return long.MaxValue instead of int.MaxValue
        }

        // Solve for X-axis using Extended GCD
        long gcdX = ExtendedGCD(aX, bX, out long x0, out long y0);
        if (pX % gcdX != 0)
        {
            return long.MaxValue; // No solution exists for X-axis
        }

        // Scale the solution for X-axis
        long scaleX = pX / gcdX;
        x0 *= scaleX;
        y0 *= scaleX;

        // Solve for Y-axis using Extended GCD
        long gcdY = ExtendedGCD(aY, bY, out long x1, out long y1);
        if (pY % gcdY != 0)
        {
            return long.MaxValue; // No solution exists for Y-axis
        }

        // Scale the solution for Y-axis
        long scaleY = pY / gcdY;
        x1 *= scaleY;
        y1 *= scaleY;

        // Align solutions for both axes
        long minTokens = long.MaxValue;

        // Increase the search space for `k` by expanding the range
        long startK = -1000000;  // Increased range for k
        long endK = 1000000;     // Increased range for k

        for (long k = startK; k <= endK; k++)
        {
            long alignedX = x0 + k * (bX / gcdX);
            long alignedY = y0 + k * (bY / gcdY);

            // Debug output to see if alignedX, alignedY are valid
            if (alignedX >= 0 && alignedY >= 0)
            {
                long cost = alignedX * 3 + alignedY * 1;

                // Debug output for minTokens comparison
                if (cost < minTokens)
                {
                    Console.WriteLine($"Found new minTokens: {cost} (k = {k})");
                }

                minTokens = Math.Min(minTokens, cost);
            }
        }

        return minTokens == long.MaxValue ? long.MaxValue : minTokens; // Return long value
    }

    private static long ExtendedGCD(long a, long b, out long x, out long y)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            return a;
        }

        long gcd = ExtendedGCD(b, a % b, out long x1, out long y1);
        x = y1;
        y = x1 - (a / b) * y1;
        return gcd;
    }
}