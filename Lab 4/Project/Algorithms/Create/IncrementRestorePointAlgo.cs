using System;
using System.Collections.Generic;
using System.Linq;

public class IncrementRestorePointAlgo : IAlgorithmCreate
{
    public RestorePoint CreateRestore(List<FileWithInfo> files, List<RestorePoint> restorePoints)
    {
        if (files.Count == 0)
            throw new ListIsEmptyException($"Empty list. Can't create Restore Point");
        if (restorePoints.Count == 0)
            throw new IncrementRestorePointException("No restore points. Can't create an increment restore point");
        var lastFullRestore = restorePoints.FindLast(point => point.Incriment == false);
        if (lastFullRestore == null)
            throw new IncrementRestorePointException("No full restore point. Can't create an increment restore point");

        var filesForRestorePoint = new List<FileWithInfo>();
        var lstOfFilesFullRestorePoint = lastFullRestore.GetFiles();
        foreach (var file in files)
        {
            var fileInFull = lstOfFilesFullRestorePoint.Find(file1 => file1.Path == file.Path);
            if (fileInFull == null || fileInFull.Size != file.Size)
                filesForRestorePoint.Add(file);
        }

        if (filesForRestorePoint.Count == 0)
            return null;

        return new RestorePoint(filesForRestorePoint, filesForRestorePoint.Sum(file => file.Size), DateTime.Now, true);
    }
}
