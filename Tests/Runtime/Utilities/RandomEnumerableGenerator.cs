using System;
using System.Collections.Generic;
using System.Linq;

public class RandomEnumerableGenerator
{
    public IEnumerable<T> GetRandomSubsequence<T>(IEnumerable<T> originalSequence, int minSize, int maxSize)
    {
        // Validate input
        if (originalSequence == null || !originalSequence.Any() || minSize <= 0 || maxSize < minSize)
        {
            return Enumerable.Empty<T>(); // Return an empty sequence if input is invalid
        }

        int sequenceSize = originalSequence.Count();
        int subsequenceSize = new Random().Next(minSize, maxSize + 1); // Randomly select a size within the specified range

        if (subsequenceSize >= sequenceSize)
        {
            return originalSequence.ToList(); // Return a copy of the entire original sequence if the subsequence size is equal to or greater than the original sequence size
        }

        int startIndex = new Random().Next(0, sequenceSize - subsequenceSize + 1); // Randomly select a start index within valid bounds

        return originalSequence.Skip(startIndex).Take(subsequenceSize);
    }
}