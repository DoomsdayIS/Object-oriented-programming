using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using Backups.Algorithms;
using Backups.Models;
using Backups.Repositories;
using File = Backups.Models.File;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            IAlgorithm splitStorage = new SplitStorage();
            Console.WriteLine(splitStorage.GetType());
            IRepository storage = new Storage();
            Console.WriteLine(storage.GetType());
            Console.WriteLine(storage.ToString()?.Split('.').Last());
            var file = new Backups.Models.File("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/NewEntry.jpg");
            var file2 = new Backups.Models.File("/Users/dsivan/Downloads/OS_Lab5.pdf");
            var backup = new BackupJob(splitStorage, storage);
            backup.AddFiles(new List<File>() { file, file2 });
            backup.CreatePoint();
            backup.CreatePoint();
            backup.RemoveFiles(new List<File>() { file2 });
            backup.CreatePoint();
            backup.AddFiles(new List<File>() { file2 });
            IAlgorithm singleStorage = new SingleStorage();
            backup.ChangeAlgorithm(singleStorage);
            backup.CreatePoint();

            // string fileName = "genji_oni_by_mozg_art_darkzmf-pre.jpg";
            // string sourcePath = "/Users/dsivan/Downloads";
            // string targetPath = "/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage";
            // int counter = 1;
            // string fileName2 = "smth_" + Convert.ToString(counter);
            // string sourceFile = Path.Combine(sourcePath, fileName);
            // string destFile = Path.Combine(targetPath, fileName2);
            // Directory.CreateDirectory(targetPath);
            // try
            // {
            //     File.Copy(sourceFile, "/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/1.jpg", true);
            // }
            // catch (IOException iox)
            // {
            //     Console.WriteLine(iox.Message);
            // }

            // using FileStream fs = new FileStream("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/1.zip", FileMode.Create);
            // using ZipArchive arch = new ZipArchive(fs, ZipArchiveMode.Create);
            // arch.CreateEntryFromFile("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/1.jpg", "2.jpg");
            // arch.ExtractToDirectory("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage");
            // using (ZipArchive archive = ZipFile.Open("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/1.zip", ZipArchiveMode.Update))
            // {
            //     archive.CreateEntryFromFile("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/1.jpg", "NewEntry.jpg");
            //     archive.CreateEntryFromFile("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/Типовик.pdf", "NewP.pdf");
            //     archive.ExtractToDirectory("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage");
            // }

            // string startPath = "/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage";
            // string zipPath = "/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage2.zip";
            // string extractPath = "/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage3";
            // ZipFile.CreateFromDirectory(startPath, zipPath);
            //
            // ZipFile.ExtractToDirectory(zipPath, extractPath);

            // if (Directory.Exists(sourcePath))
            // {
            //     string[] files = Directory.GetFiles(sourcePath);
            //
            //     // Copy the files and overwrite destination files if they already exist.
            //     foreach (string s in files)
            //     {
            //         // Use static Path methods to extract only the file name from the path.
            //         fileName = Path.GetFileName(s);
            //         destFile = Path.Combine(targetPath, fileName);
            //         File.Copy(s, destFile, true);
            //     }
            // }
            // else
            // {
            //     Console.WriteLine("Source path does not exist!");
            // }
            //
            // // Keep console window open in debug mode.
            // Console.WriteLine("Press any key to exit.");
            // Console.ReadKey();
        }
    }
}
