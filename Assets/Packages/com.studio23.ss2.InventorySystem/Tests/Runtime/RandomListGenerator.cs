using System;
using System.Collections.Generic;
using Random = UnityEngine.Random; // If you're using Unity, you might need UnityEngine.Random

public class RandomListGenerator
{
    public List<T> GetRandomSublist<T>(List<T> originalList, int minSize, int maxSize)
    {
        // Validate input
        if (originalList == null || originalList.Count == 0 || minSize <= 0 || maxSize < minSize)
        {
            return new List<T>(); // Return an empty list if input is invalid
        }

        int listSize = originalList.Count;
        int sublistSize = Random.Range(minSize, maxSize + 1); // Randomly select a size within the specified range

        if (sublistSize >= listSize)
        {
            return new List<T>(originalList); // Return a copy of the original list if the sublist size is equal to or greater than the original list size
        }

        int startIndex = Random.Range(0, listSize - sublistSize + 1); // Randomly select a start index within valid bounds

        List<T> randomSublist = new List<T>(sublistSize);

        for (int i = startIndex; i < startIndex + sublistSize; i++)
        {
            randomSublist.Add(originalList[i]); // Add elements to the random sublist
        }

        return randomSublist;
    }
}