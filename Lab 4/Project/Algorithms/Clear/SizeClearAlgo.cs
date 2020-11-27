using System;
using System.Collections.Generic;
using System.Linq;

public class SizeClearAlgo : IAlgorithmClear
{
    public long Size { get; set; }

    public SizeClearAlgo(long size)
    {
        if (size <= 0)
            throw new WrongClearArgumentsException($"Size can't be 0 or lower. Your's: {Size}");
        Size = size;
    }
    public void Clear(List<RestorePoint> restorePoints)
    {
        while (restorePoints.Sum(file => file.Size) > Size)
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
        if (restorePoints.Sum(file => file.Size) > Size)
        {
            throw new WarningClearAlgorithmException($"Not all restore points were deleted. Max size = {Size}. Actual size = {restorePoints.Sum(file => file.Size)}");
        }
    }
}