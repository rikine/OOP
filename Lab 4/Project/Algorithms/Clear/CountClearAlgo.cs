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
            if (restorePoints[0].Incriment == false && restorePoints.Count > 1 && restorePoints[1].Incriment == false)
            {
                restorePoints.RemoveAt(0);
            }
            else
            {
                break;
            }
        }
        if (restorePoints.Count > Count)
        {
            throw new WarningClearAlgorithmException($"Not all restore points were deleted. Max count = {Count}. Actual count = {restorePoints.Count}");
        }
    }
}