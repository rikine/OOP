using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var backup = new Backup("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup");
                backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/Preved");
                backup.RemoveFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/Preved");
                backup.SetCreationAlgo(new FullRestorePointAlgo());
                backup.SetStoragingAlgo(new FullStorageAlgo());
                backup.CreateRestorePoint();
                backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/Preved");
                backup.CreateRestorePoint();
                backup.SetClearingAlgo(new CountClearAlgo(1));
                Console.WriteLine(backup);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
