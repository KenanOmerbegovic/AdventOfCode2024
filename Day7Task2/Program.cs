using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Path to the input file
        string filePath = "EquationCheck.txt";

        // Read equations from the file
        var equations = ReadEquationsFromFile(filePath);

        long totalCalibration = 0;

        // Loop through each equation
        foreach (var equation in equations)
        {
            long testValue = equation.testValue;
            long[] numbers = equation.numbers;

            // Generate all possible operator combinations
            if (CanMakeTrue(testValue, numbers))
            {
                totalCalibration += testValue;
            }
        }

        Console.WriteLine("Total Calibration Result: " + totalCalibration);
    }

    // Function to read equations from a file
    static List<(long testValue, long[] numbers)> ReadEquationsFromFile(string filePath)
    {
        var equations = new List<(long testValue, long[] numbers)>();

        try
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                // Split the line into the test value and numbers
                var parts = line.Split(':');
                if (parts.Length != 2) continue;

                long testValue = long.Parse(parts[0].Trim());
                var numbers = Array.ConvertAll(parts[1].Trim().Split(' '), long.Parse);

                equations.Add((testValue, numbers));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading the file: " + ex.Message);
        }

        return equations;
    }

    // Function to check if an equation can match the test value
    static bool CanMakeTrue(long testValue, long[] numbers)
    {
        var operators = new char[] { '+', '*', 'C' }; // Add 'C' for concatenation
        int operatorCount = numbers.Length - 1;

        // Generate all combinations of '+' and '*', and 'C'
        foreach (var operatorCombo in GenerateOperatorCombinations(operators, operatorCount))
        {
            // Evaluate the equation with this operator combination
            long result = Evaluate(numbers, operatorCombo);

            if (result == testValue)
            {
                return true;
            }
        }

        return false;
    }

    // Function to evaluate the equation left-to-right given a sequence of operators
    static long Evaluate(long[] numbers, char[] operators)
    {
        long result = numbers[0];

        for (int i = 0; i < operators.Length; i++)
        {
            if (operators[i] == '+')
            {
                result += numbers[i + 1];
            }
            else if (operators[i] == '*')
            {
                result *= numbers[i + 1];
            }
            else if (operators[i] == 'C') // Handle concatenation
            {
                string concatenated = result.ToString() + numbers[i + 1].ToString();
                result = long.Parse(concatenated); // Convert back to a number
            }
        }

        return result;
    }

    // Function to generate all combinations of operators
    static IEnumerable<char[]> GenerateOperatorCombinations(char[] operators, int length)
    {
        int totalCombinations = (int)Math.Pow(operators.Length, length);

        for (int i = 0; i < totalCombinations; i++)
        {
            var combo = new char[length];
            int temp = i;

            for (int j = 0; j < length; j++)
            {
                combo[j] = operators[temp % operators.Length];
                temp /= operators.Length;
            }

            yield return combo;
        }
    }
}
