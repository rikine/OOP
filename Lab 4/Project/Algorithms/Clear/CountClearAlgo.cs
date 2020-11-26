using System;
using System.Collections.Generic;

public class CountClearAlgo : IAlgorithmClear
{
    public int Count { get; set; }

    public CountClearAlgo(int count)
    {
        if (count <= 0)
            throw new WrongClearArgumentsException($"Count of clear arguments can't be 0 or lower. Your's: {count}");
        Count = count;
    }

    public void Clear(List<RestorePoint> restorePoints)
    {
        while (restorePoints.Count > Count)
        {
            restorePoints.RemoveAt(0);
        }
    }
}