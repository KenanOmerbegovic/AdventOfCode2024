using System;
using System.Collections.Generic;
using System.IO;

class GardenFenceCalculator
{
    static void Main()
    {
        string filePath = "fencemap.txt";

        // Check if the file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File '{filePath}' not found.");
            return;
        }

        // Read the input map from the file
        string[] input = File.ReadAllLines(filePath);

        int rows = input.Length;
        int cols = input[0].Length;
        char[,] map = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                map[i, j] = input[i][j];
            }
        }

        bool[,] visited = new bool[rows, cols];
        int totalPrice = 0;

        // Directions for adjacent cells
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (!visited[i, j])
                {
                    char plantType = map[i, j];
                    int area = 0;
                    int perimeter = 0;

                    // Perform flood-fill to calculate area and perimeter
                    Queue<(int, int)> queue = new Queue<(int, int)>();
                    queue.Enqueue((i, j));
                    visited[i, j] = true;

                    while (queue.Count > 0)
                    {
                        var (x, y) = queue.Dequeue();
                        area++;

                        // Check all four directions
                        for (int d = 0; d < 4; d++)
                        {
                            int nx = x + dx[d];
                            int ny = y + dy[d];

                            if (nx >= 0 && nx < rows && ny >= 0 && ny < cols)
                            {
                                if (map[nx, ny] == plantType && !visited[nx, ny])
                                {
                                    visited[nx, ny] = true;
                                    queue.Enqueue((nx, ny));
                                }
                                else if (map[nx, ny] != plantType)
                                {
                                    perimeter++;
                                }
                            }
                            else
                            {
                                // Edge of the map contributes to perimeter
                                perimeter++;
                            }
                        }
                    }

                    // Calculate price for this region
                    int price = area * perimeter;
                    totalPrice += price;
                }
            }
        }

        Console.WriteLine("Total Price: " + totalPrice);
    }
}
