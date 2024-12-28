using System;
using System.Collections.Generic;

class PlantGrid
{
    // Define prices for each plant type
    static readonly Dictionary<char, int> pricePerSide = new Dictionary<char, int>
    {
        {'R', 18},  // Example: R = 18
        {'V', 20},  // V = 20
        {'I', 8},   // I = 8
        {'C', 28},  // C = 28
        {'F', 18}   // F = 18
    };

    // Method to count the exposed sides for a specific plant at position (x, y)
    static int CountExposedSides(int x, int y, char plantType, char[,] grid)
    {
        int exposedSides = 0;
        // Directions: Up, Down, Left, Right
        int[,] directions = { {-1, 0}, {1, 0}, {0, -1}, {0, 1} };

        foreach (var dir in directions)
        {
            int nx = x + dir[0];
            int ny = y + dir[1];

            // Check if out of bounds (boundary condition)
            if (nx < 0 || ny < 0 || nx >= grid.GetLength(0) || ny >= grid.GetLength(1))
            {
                exposedSides++; // Exposed to boundary
            }
            else
            {
                char adjacentPlant = grid[nx, ny];
                // Check if adjacent plant is different
                if (adjacentPlant != plantType)
                {
                    exposedSides++; // Exposed to a different plant type
                }
            }
        }

        return exposedSides;
    }

    // Method to calculate the total area, total exposed sides, and total price for a given plant type
    static (int totalArea, int totalSides, int price) CalculateRegion(char plantType, char[,] grid)
    {
        int totalArea = 0;
        int totalSides = 0;

        // Traverse the grid to count area and exposed sides for the specified plant type
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == plantType)
                {
                    totalArea++;
                    totalSides += CountExposedSides(x, y, plantType, grid);
                }
            }
        }

        // Calculate price based on total exposed sides and price per side for the given plant type
        int price = totalSides * (pricePerSide.ContainsKey(plantType) ? pricePerSide[plantType] : 0);
        return (totalArea, totalSides, price);
    }

    static void Main()
    {
        // Sample grid (you can change this grid to test other scenarios)
        char[,] grid = {
            { 'R', 'V', 'I', 'R' },
            { 'R', 'R', 'V', 'C' },
            { 'F', 'I', 'V', 'V' },
            { 'C', 'F', 'C', 'R' }
        };

        // You can change this to any plant type (e.g., 'R', 'V', 'I', 'C', 'F')
        char plantType = 'R';

        var result = CalculateRegion(plantType, grid);

        Console.WriteLine($"Plant Type: {plantType}");
        Console.WriteLine($"Total Area: {result.totalArea} square meters");
        Console.WriteLine($"Total Exposed Sides: {result.totalSides}");
        Console.WriteLine($"Price: ${result.price}");
    }
}
