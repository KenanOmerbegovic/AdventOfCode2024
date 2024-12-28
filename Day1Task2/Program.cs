using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // File path (replace with your actual file path)
        string filePath = "yourFile.txt";
        
        // Create two lists to store values from each column
        List<int> leftList = new List<int>();
        List<int> rightList = new List<int>();

        try
        {
            // Read the file and process each line
            foreach (string line in File.ReadLines(filePath))
            {
                // Split the line into two parts based on space or tab
                string[] values = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                
                if (values.Length >= 2)
                {
                    // Convert the values to integers and add them to the respective lists
                    leftList.Add(int.Parse(values[0]));
                    rightList.Add(int.Parse(values[1]));
                }
            }

            // Calculate the similarity score
            int similarityScore = CalculateSimilarityScore(leftList, rightList);

            // Output the similarity score
            Console.WriteLine("Similarity Score: " + similarityScore);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    // Method to calculate the similarity score
    static int CalculateSimilarityScore(List<int> leftList, List<int> rightList)
    {
        int similarityScore = 0;

        // Create a dictionary to store the count of each element in the right list
        Dictionary<int, int> rightListCount = new Dictionary<int, int>();

        // Count the occurrences of each number in the right list
        foreach (int num in rightList)
        {
            if (rightListCount.ContainsKey(num))
            {
                rightListCount[num]++;
            }
            else
            {
                rightListCount[num] = 1;
            }
        }

        // Iterate through the left list and calculate the similarity score
        foreach (int num in leftList)
        {
            if (rightListCount.ContainsKey(num))
            {
                similarityScore += num * rightListCount[num];
            }
        }

        return similarityScore;
    }
}
