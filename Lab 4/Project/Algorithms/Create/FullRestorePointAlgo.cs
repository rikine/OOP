using System;
using System.Collections.Generic;
using System.Linq;

public class FullRestorePointAlgo : IAlgorithmCreate
{
    public RestorePoint CreateRestore(List<FileWithInfo> files, List<RestorePoint> restorePoints)
    {
        if (files.Count == 0)
            throw new ListIsEmptyException($"Empty list. Can't create Restore Point");
        if (restorePoints.Count != 0)
        {
            while (restorePoints.Last().Incriment)
            {
                restorePoints.Remove(restorePoints.Last());
            }
        }
        return new RestorePoint(files, files.Sum(file => file.Size), DateTime.Now);
    }
}