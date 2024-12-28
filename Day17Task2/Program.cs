using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Read the input file
        var file = File.ReadAllText("day17.txt").Split("\r\n\r\n");

        // Parse the program instructions
        var program = file[1].Split(" ")[1].Split(',').Select(long.Parse).ToArray();

        long current = 0;
        
        // Iterate through each digit in the program to find the matching output
        for (int digit = program.Length - 1; digit >= 0; digit--)
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                var candidate = current +  (1L << (digit * 3)) * i;
                var output = Run(program, candidate, 0, 0);
                if (output.Skip(digit).SequenceEqual(program.Skip(digit)))
                {
                    current = candidate;
                    Console.WriteLine($"Success finding digit {digit} to match {program[digit]}, i={Convert.ToString(i, 8)} (octal)");
                    Console.WriteLine($"Output: [{String.Join(", ", output)}]");
                    Console.WriteLine($"Current is now {Convert.ToString(current, 8)} (octal)");
                    break;
                }
            }
        }

        // Part 2 output
        Console.WriteLine($"Part 2: {current} / {Convert.ToString(current, 8)} (decimal / octal)");
    }

    // Logic to compute the value of combo (handles the operand)
    static long Combo(long[] registers, long value)
    {
        return value switch
        {
            >= 0 and <= 3 => value,
            var reg => registers[reg - 4],
        };
    }

    // Main program logic where we simulate the program execution
    static IEnumerable<long> Run(long[] program, long a, long b, long c)
    {
        var registers = new long[] { a, b, c };
        long ip = 0;
        
        // Loop through the program instructions
        while (ip < program.Length)
        {
            var opCode = (OpCode)program[ip];
            var operand = program[ip + 1];
            switch (opCode)
            {
                case OpCode.adv:
                    registers[0] = registers[0] / (1L << (int)Combo(registers, operand));
                    break;
                case OpCode.bxl:
                    registers[1] = registers[1] ^ operand;
                    break;
                case OpCode.bst:
                    registers[1] = Combo(registers, operand) % 8;
                    break;
                case OpCode.jnz:
                    if (registers[0] != 0)
                    {
                        ip = operand;
                        ip -= 2; // Adjust the pointer due to the increment at the end of loop
                    }
                    break;
                case OpCode.bxc:
                    registers[1] = registers[1] ^ registers[2];
                    break;
                case OpCode.output:
                    yield return Combo(registers, operand) % 8;
                    break;
                case OpCode.bdv:
                    registers[1] = registers[0] / (1 << (int)Combo(registers, operand));
                    break;
                case OpCode.cdv:
                    registers[2] = registers[0] / (1 << (int)Combo(registers, operand));
                    break;
            }
            ip += 2; // Move to the next instruction
        }
    }

    // Enum defining the operation codes
    enum OpCode
    {
        adv = 0,
        bxl = 1,
        bst = 2,
        jnz = 3,
        bxc = 4,
        output = 5,
        bdv = 6,
        cdv = 7,
    }
}
