using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // File path (replace with your actual file path)
        string filePath = ".txt";
        
        // Create two lists to store values from each column
        List<long> column1 = new List<long>();
        List<long> column2 = new List<long>();
        
        try
        {
            // Read the file and process each line
            foreach (string line in File.ReadLines(filePath))
            {
                // Split the line into two parts based on space or tab
                string[] values = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (values.Length >= 2)
                {
                    // Convert the values to long and add them to the respective lists
                    column1.Add(long.Parse(values[0]));
                    column2.Add(long.Parse(values[1]));
                }
            }

            // Sort both lists from highest to lowest
            column1.Sort((a, b) => b.CompareTo(a));
            column2.Sort((a, b) => b.CompareTo(a));

            // Now compare each element of column1 to column2 and sum the differences
            long totalDifference = 0;

            // Ensure both lists have the same number of elements
            int minCount = Math.Min(column1.Count, column2.Count);

            for (int i = 0; i < minCount; i++)
            {
                long diff = Math.Abs(column1[i] - column2[i]); // Absolute difference between elements
                totalDifference += diff; // Sum the differences
            }

            // Output the total sum of differences
            Console.WriteLine("Total sum of differences: " + totalDifference);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
