using System;
using System.Collections.Generic;
using System.Collections.Immutable;

public class RestorePoint
{
    public RestorePoint(List<FileWithInfo> files, long size, DateTime dateTime, bool incriment = false)
    {
        ID = new Random().Next();
        Files = files;
        Size = size;
        DateTime = dateTime;
        Incriment = incriment;
    }

    public ImmutableList<FileWithInfo> GetFiles() => Files.ToImmutableList();

    public int ID { get; }
    public bool Incriment { get; }
    private List<FileWithInfo> Files = new List<FileWithInfo>();
    public long Size { get; }
    public DateTime DateTime { get; }
}