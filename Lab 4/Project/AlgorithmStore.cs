using System;
using System.Collections.Generic;

public interface IAlgorithmStore
{
    void Store(List<RestorePoint> restorePoints);
}