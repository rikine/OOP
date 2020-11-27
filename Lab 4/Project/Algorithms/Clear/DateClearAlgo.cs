using System;
using System.Collections.Generic;
using System.Linq;

public class DateClearAlgo : IAlgorithmClear
{
    public DateTime DateTime { get; set; }

    public DateClearAlgo(DateTime dateTime)
    {
        DateTime = dateTime;
    }

    public void Clear(List<RestorePoint> restorePoints)
    {
        bool wasOk = true;
        foreach (var restorePoint in restorePoints)
        {
            var index = restorePoints.IndexOf(restorePoint);
            if (restorePoints[index].Incriment == false && restorePoints.Count > index + 1 && restorePoints[index + 1].Incriment == false || restorePoint.ID == restorePoints.Last().ID)
            {
                if (restorePoint.DateTime < DateTime)
                    restorePoints.Remove(restorePoint);
            }
            else
            {
                wasOk = false;
                break;
            }
        }
        if (!wasOk)
        {
            throw new WarningClearAlgorithmException($"Not all restore points were deleted by date.");
        }
    }
}