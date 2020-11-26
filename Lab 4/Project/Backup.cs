using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Backup
{
    private IAlgorithmCreate _algorithmCreate;
    private IAlgorithmStore _algorithmStore;
    private IAlgorithmClear _algorithmClear;

    private List<RestorePoint> _restorePoints = new List<RestorePoint>();
    private List<FileWithInfo> _files = new List<FileWithInfo>();

    public Backup() { }

    public Backup(string path)
    {
        if (File.Exists(path))
        {
            AddFile(path);
        }
        else if (Directory.Exists(path))
        {
            MakeListOfFiles(path);
        }
        else
        {
            throw new FileNotFoundException($"No file or dir were found at {path}");
        }
    }

    public void AddFile(string pathToFile)
    {
        var fileInfo = new FileInfo(pathToFile);
        if (!fileInfo.Exists)
            throw new FileNotFoundException($"No file were found at {pathToFile}");
        foreach (var file in _files)
        {
            if (file.Path == fileInfo.FullName)
                throw new DoubleFileException($"File were found in backup file list: {fileInfo.FullName}");
        }
        _files.Add(new FileWithInfo(pathToFile));
    }

    public void RemoveFile(string fullNameOfFIle)
    {
        foreach (var file in _files)
        {
            if (file.Path == fullNameOfFIle)
            {
                _files.Remove(file);
                break;
            }
        }
    }

    private void MakeListOfFiles(string pathOfDir)
    {
        var dirInfo = new DirectoryInfo(pathOfDir);
        if (!dirInfo.Exists)
            throw new FileNotFoundException($"No dir were found at {pathOfDir}");

        foreach (var file in dirInfo.GetFiles("*", SearchOption.AllDirectories))
        {
            _files.Add(new FileWithInfo(file.FullName));
        }
    }

    public void SetCreationAlgo(IAlgorithmCreate algorithmCreate)
    {
        _algorithmCreate = algorithmCreate;
    }

    public void CreateRestorePoint()
    {
        if (_algorithmCreate == null)
            throw new NoAlgorithmException("No creating algorithm were found");
        var restorePoint = _algorithmCreate.CreateRestore(new List<FileWithInfo>(_files), _restorePoints);
        if (restorePoint == null)
            throw new NoRestorePointReturnedException("No restore point can't be made");
        _restorePoints.Add(restorePoint);
        if (_algorithmStore == null)
            throw new NoAlgorithmException("No storaging algorithm were found");
        _algorithmStore.Store(_restorePoints);
    }

    public void SetStoragingAlgo(IAlgorithmStore algorithmStore)
    {
        _algorithmStore = algorithmStore;
    }

    public void SetClearingAlgo(IAlgorithmClear algorithmClear)
    {
        _algorithmClear = algorithmClear;
        _algorithmClear.Clear(_restorePoints);
    }

    public void Clear()
    {
        if (_algorithmClear == null)
            throw new NoAlgorithmException("No clearing algorithm were found");
        _algorithmClear.Clear(_restorePoints);
    }

    public long GetSizeOfBackup() => _restorePoints.Sum(restore => restore.Size);

    public override string ToString()
    {
        string s = $"Size of Backup: {GetSizeOfBackup()}\n";
        s += "All Files in Backup:\n";
        foreach (var file in _files)
        {
            s += file.Path + '\n';
        }
        s += "RestorePoints\n";
        foreach (var restorePoint in _restorePoints)
        {
            s += "ID\t\tIncrement?\tDate\t\tSize\n";
            s += restorePoint.ID.ToString() + "\t\t" + restorePoint.Incriment.ToString() + '\t' + restorePoint.DateTime.ToString() + "\t\t" + restorePoint.Size + '\n';
            s += "Files:\n";
            foreach (var file in restorePoint.GetFiles())
            {
                s += file.Path + '\n';
            }
            s += '\n';
        }
        return s;
    }
}