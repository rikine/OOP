using System;
using System.IO;

public class FileWithInfo : INode
{
    public FileWithInfo(string path)
    {
        var fI = new FileInfo(path);
        Path = fI.FullName;
        Size = fI.Length;
    }

    public string Path { get; }
    public long Size { get; }

    public override string ToString() => new FileInfo(Path).Name;
}