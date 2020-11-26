using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System;

namespace Tester
{
    [TestClass]
    public class UnitTest1
    {
        private Backup backup;

        [TestInitialize]
        public void TestInitialize()
        {
            backup = new Backup();
            backup.SetCreationAlgo(new FullRestorePointAlgo());
            backup.SetStoragingAlgo(new FullStorageAlgo());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            backup = new Backup();
            backup.SetCreationAlgo(new FullRestorePointAlgo());
            backup.SetStoragingAlgo(new FullStorageAlgo());
        }

        [TestMethod]
        public void Case1()
        {
            Debug.WriteLine("Case 1 Start");
            //Я создаю бекап, в который добавляю 2 файла
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Text.txt");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Lalla.txt");
            //Я запускаю алгоритм создания точки для этого бекапа — создается точка восстановления
            backup.CreateRestorePoint();
            //Я должен убедиться, что в этой точке лежит информация по двум файлам
            var check = backup.ToString().Split('\n', int.MaxValue);
            int counter = 0;
            bool isRestorePoint = false;
            foreach (var str in check)
            {
                if (str == string.Empty)
                {
                    continue;
                }
                if (isRestorePoint)
                {
                    counter++;
                }
                if (str == "Files:")
                {
                    isRestorePoint = true;
                }
            }
            Debug.WriteLine("Checking only 2 files in restore point");
            Assert.AreEqual(2, counter);
            Debug.WriteLine(backup);
            //Я создаю следующую точку восстановления для цепочки
            backup.CreateRestorePoint();
            //Я применяю алгоритм очистки цепочки по принципу ограничения максимального количества указав длину 1
            backup.SetClearingAlgo(new CountClearAlgo(1));
            //Я убеждаюсь, что в ответ получу цепочку длиной 1
            Debug.WriteLine("Checking only 1 restore point");
            Assert.AreEqual(1, new Regex("\nID").Matches(backup.ToString()).Count);
            Debug.WriteLine(backup);
        }

        [TestMethod]
        public void Case2()
        {
            Debug.WriteLine("Case 2 Start");
            //Я создаю бекап, в который добавляю 2 файла размером 24469 байт
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Text.txt");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Lalla.txt");
            //Я создаю точку восстановления для него
            backup.CreateRestorePoint();
            //Я создаю следующую точку, убеждаюсь, что точки две и размер бекапа 48938 байт
            backup.CreateRestorePoint();
            var check = backup.ToString().Split('\n', int.MaxValue);
            bool rightSize = false;
            foreach (var restore in check)
            {
                if (restore == "Size of Backup: 48938")
                    rightSize = true;
            }
            Debug.WriteLine("Size of Backup must be 48938");
            Assert.AreEqual(true, rightSize);
            Debug.WriteLine(backup);
            //Я применяю алгоритм очистки с ограничением по размеру, указываю 40000 байт для цепочки и убеждаюсь, что остается один бекап.
            backup.SetClearingAlgo(new SizeClearAlgo(40000));
            Assert.AreEqual(1, new Regex("\nID").Matches(backup.ToString()).Count);
            Debug.WriteLine(backup);
        }

        [TestMethod]
        public void Case4_1()
        {
            Debug.WriteLine("Case 4_1 Start");
            //Я создаю бекап, в который добавляю 4 файла
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture1.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture2.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Text.txt");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Lalla.txt");
            //Создаю 4 точки восстановления
            DateTime dateTime = new DateTime();
            backup.CreateRestorePoint();
            backup.CreateRestorePoint();
            Thread.Sleep(1000);
            dateTime = DateTime.Now;
            backup.CreateRestorePoint();
            backup.CreateRestorePoint();
            Debug.WriteLine(backup);
            //Создаю гибридную очистку, которая чистит точки, которые вышли за все лимиты и очищаю(должны очиститься 2 точки)
            var gibrid = new GibridClearAlgo(true);
            gibrid.SetDateTime(dateTime);
            gibrid.SetCount(1);
            backup.SetClearingAlgo(gibrid);
            //Проверяю, что осталось 2 точки
            var check = backup.ToString().Split('\n', int.MaxValue);
            bool rightSize = false;
            foreach (var restore in check)
            {
                if (restore == "Size of Backup: 8848796")
                    rightSize = true;
            }
            Debug.WriteLine("Size of Backup must be 8848796");
            Assert.AreEqual(true, rightSize);
            Debug.WriteLine(backup);
        }

        [TestMethod]
        public void Case4_2()
        {
            Debug.WriteLine("Case 4_2 Start");
            //Я создаю бекап, в который добавляю 4 файла
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture1.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture2.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Text.txt");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Lalla.txt");
            //Создаю 4 точки восстановления
            DateTime dateTime = new DateTime();
            backup.CreateRestorePoint();
            backup.CreateRestorePoint();
            Thread.Sleep(1000);
            dateTime = DateTime.Now;
            backup.CreateRestorePoint();
            backup.CreateRestorePoint();
            Debug.WriteLine(backup);
            //Создаю гибридную очистку, которая чистит точки, которые вышли за любые лимиты и очищаю(должна очиститься 1 точка)
            var gibrid = new GibridClearAlgo(false);
            gibrid.SetDateTime(dateTime);
            gibrid.SetCount(1);
            backup.SetClearingAlgo(gibrid);
            //Проверяю, что осталось 1 точка
            var check = backup.ToString().Split('\n', int.MaxValue);
            bool rightSize = false;
            foreach (var restore in check)
            {
                if (restore == "Size of Backup: 4424398")
                    rightSize = true;
            }
            Debug.WriteLine("Size of Backup must be 4424398");
            Assert.AreEqual(true, rightSize);
            Debug.WriteLine(backup);
        }

        [TestMethod]
        public void Case3_1()
        {
            Debug.WriteLine("Case 3_1 Start");
            //Я создаю бекап, в который добавляю 4 файла
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture1.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture2.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Text.txt");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Lalla.txt");
            //Задаю алгоритм общего хранения
            backup.SetStoragingAlgo(new FullStorageAlgo());
            //Создаю полную точку восстановления
            backup.CreateRestorePoint();
            Debug.WriteLine(backup);
            //Создаю инкрементальную точку восстановления и добавляю файл, для ее создания
            backup.SetCreationAlgo(new IncrementRestorePointAlgo());
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/ChetoTam/HRU.txt");
            backup.CreateRestorePoint();
            Debug.WriteLine(backup);
        }

        [TestMethod]
        public void Case3_2()
        {
            Debug.WriteLine("Case 3_2 Start");
            //Я создаю бекап, в который добавляю 4 файла
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture1.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Picture2.png");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Text.txt");
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/Lalla.txt");
            //Задаю алгоритм раздельного хранения
            backup.SetStoragingAlgo(new SeparateStorageAlgo());
            //Создаю полную точку восстановления
            backup.CreateRestorePoint();
            Debug.WriteLine(backup);
            //Создаю инкрементальную точку восстановления и добавляю файл, для ее создания
            backup.SetCreationAlgo(new IncrementRestorePointAlgo());
            backup.AddFile("/Users/rikine/Documents/VS_code/C#_Projects/OOP lab4/Tester/ForTestBackup/ChetoTam/HRU.txt");
            backup.CreateRestorePoint();
            Debug.WriteLine(backup);
        }
    }
}
