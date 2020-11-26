using System;
using System.Collections.Generic;

public interface IAlgorithmCreate
{
    RestorePoint CreateRestore(List<FileWithInfo> files, List<RestorePoint> restorePoints);
}