using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Hardcoded file path
        string filePath = "17aj.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found: " + filePath);
            return;
        }

        // Initialize registers and program
        int registerA = 0, registerB = 0, registerC = 0;
        int[] program = Array.Empty<int>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (line.StartsWith("Register A:", StringComparison.OrdinalIgnoreCase))
                {
                    registerA = int.Parse(line.Split(':')[1].Trim());
                }
                else if (line.StartsWith("Register B:", StringComparison.OrdinalIgnoreCase))
                {
                    registerB = int.Parse(line.Split(':')[1].Trim());
                }
                else if (line.StartsWith("Register C:", StringComparison.OrdinalIgnoreCase))
                {
                    registerC = int.Parse(line.Split(':')[1].Trim());
                }
                else if (line.StartsWith("Program:", StringComparison.OrdinalIgnoreCase))
                {
                    program = Array.ConvertAll(line.Split(':')[1].Trim().Split(','), int.Parse);
                }
                else
                {
                    // Skip unexpected or empty lines
                    Console.WriteLine($"Warning: Skipping unrecognized line: {line}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error parsing input file: " + ex.Message);
            return;
        }

        // Output collection
        List<int> output = new List<int>();

        // Instruction pointer
        int instructionPointer = 0;

        while (instructionPointer < program.Length)
        {
            int opcode = program[instructionPointer];
            int operand = program[instructionPointer + 1];

            switch (opcode)
            {
                case 0: // adv
                    registerA /= (int)Math.Pow(2, GetComboOperandValue(operand, registerA, registerB, registerC));
                    break;
                case 1: // bxl
                    registerB ^= operand;
                    break;
                case 2: // bst
                    registerB = GetComboOperandValue(operand, registerA, registerB, registerC) % 8;
                    break;
                case 3: // jnz
                    if (registerA != 0)
                    {
                        instructionPointer = operand;
                        continue; // Skip the normal pointer increment
                    }
                    break;
                case 4: // bxc
                    registerB ^= registerC;
                    break;
                case 5: // out
                    output.Add(GetComboOperandValue(operand, registerA, registerB, registerC) % 8);
                    break;
                case 6: // bdv
                    registerB = registerA / (int)Math.Pow(2, GetComboOperandValue(operand, registerA, registerB, registerC));
                    break;
                case 7: // cdv
                    registerC = registerA / (int)Math.Pow(2, GetComboOperandValue(operand, registerA, registerB, registerC));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown opcode: {opcode}");
            }

            // Move the instruction pointer forward
            instructionPointer += 2;
        }

        // Join and print the output
        Console.WriteLine(string.Join(",", output));
    }

    static int GetComboOperandValue(int operand, int registerA, int registerB, int registerC)
    {
        return operand switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            3 => 3,
            4 => registerA,
            5 => registerB,
            6 => registerC,
            _ => throw new InvalidOperationException($"Invalid combo operand: {operand}")
        };
    }
}
