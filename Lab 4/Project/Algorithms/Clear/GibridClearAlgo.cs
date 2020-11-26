using System;
using System.Collections.Generic;
using System.Linq;

public class GibridClearAlgo : IAlgorithmClear
{
    public int? Count { get; set; }
    public DateTime? DateTime { get; set; }
    public long? Size { get; set; }
    public bool AllParamsMustBeOverLimited { get; set; }

    public GibridClearAlgo(bool allParamsMustBeOverLimited)
    {
        AllParamsMustBeOverLimited = allParamsMustBeOverLimited;
    }

    public void SetCount(int count)
    {
        if (count <= 0)
            throw new WrongClearArgumentsException($"Count of clear arguments can't be 0 or lower than 0. Your's: {count}");
        Count = count;
    }

    public void SetSize(long size)
    {
        if (size <= 0)
            throw new WrongClearArgumentsException($"Size can't be 0 or lower than 0. Your's: {Size}");
        Size = size;
    }

    public void SetDateTime(DateTime dateTime)
    {
        DateTime = dateTime;
    }

    public void SetAllParamsMustBeOverLimited(bool allParamsMustBeOverLimited)
    {
        AllParamsMustBeOverLimited = allParamsMustBeOverLimited;
    }

    public void Clear(List<RestorePoint> restorePoints)
    {
        if (AllParamsMustBeOverLimited)
        {
            if (Count.HasValue)
            {
                while (restorePoints.Count > Count)
                {
                    if (DateTime.HasValue == false && Size.HasValue == false)
                        restorePoints.RemoveAt(0);
                    else if (DateTime.HasValue == true && restorePoints[0].DateTime < DateTime && Size.HasValue == false)
                        restorePoints.RemoveAt(0);
                    else if (DateTime.HasValue == false && Size.HasValue == true && restorePoints.Sum(file => file.Size) > Size)
                        restorePoints.RemoveAt(0);
                    else if (DateTime.HasValue == true && restorePoints[0].DateTime < DateTime && Size.HasValue == true && restorePoints.Sum(file => file.Size) > Size)
                        restorePoints.RemoveAt(0);
                    else
                        break;
                }
            }

            if (DateTime.HasValue)
            {
                foreach (var restorePoint in restorePoints)
                {
                    if (restorePoint.DateTime < DateTime)
                    {
                        if (Count.HasValue == false && Size.HasValue == false)
                            restorePoints.Remove(restorePoint);
                        else if (Count.HasValue == true && restorePoints.Count > Count && Size.HasValue == false)
                            restorePoints.Remove(restorePoint);
                        else if (Count.HasValue == false && Size.HasValue == true && restorePoints.Sum(file => file.Size) > Size)
                            restorePoints.Remove(restorePoint);
                        else if (Count.HasValue == true && restorePoints.Count > Count && Size.HasValue == true && restorePoints.Sum(file => file.Size) > Size)
                            restorePoints.Remove(restorePoint);
                    }
                }
            }

            if (Size.HasValue)
            {
                while (restorePoints.Sum(file => file.Size) > Size)
                {
                    if (DateTime.HasValue == false && Count.HasValue == false)
                        restorePoints.RemoveAt(0);
                    else if (DateTime.HasValue == true && restorePoints[0].DateTime < DateTime && Count.HasValue == false)
                        restorePoints.RemoveAt(0);
                    else if (DateTime.HasValue == false && Count.HasValue == true && restorePoints.Count > Count)
                        restorePoints.RemoveAt(0);
                    else if (DateTime.HasValue == true && restorePoints[0].DateTime < DateTime && Count.HasValue == true && restorePoints.Count > Count)
                        restorePoints.RemoveAt(0);
                    else
                        break;
                }
            }
        }
        else
        {
            if (Count.HasValue)
            {
                while (restorePoints.Count > Count)
                {
                    restorePoints.RemoveAt(0);
                }
            }

            if (DateTime.HasValue)
            {
                foreach (var restorePoint in restorePoints)
                {
                    if (restorePoint.DateTime < DateTime)
                        restorePoints.Remove(restorePoint);
                }
            }

            if (Size.HasValue)
            {
                while (restorePoints.Sum(file => file.Size) > Size)
                {
                    restorePoints.RemoveAt(0);
                }
            }
        }
    }
}