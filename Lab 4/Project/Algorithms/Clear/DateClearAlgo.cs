using System;
using System.Collections.Generic;

public class DateClearAlgo : IAlgorithmClear
{
    public DateTime DateTime { get; set; }

    public DateClearAlgo(DateTime dateTime)
    {
        DateTime = dateTime;
    }

    public void Clear(List<RestorePoint> restorePoints)
    {
        foreach (var restorePoint in restorePoints)
        {
            if (restorePoint.DateTime < DateTime)
                restorePoints.Remove(restorePoint);
        }
    }
}