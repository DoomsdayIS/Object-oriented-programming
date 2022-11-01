using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Models;
using Backups.Tools;
using File = Backups.Models.File;

namespace Backups.Repositories
{
    public class Storage : IRepository
    {
        public Storage(string path = null)
        {
            if (Directory.Exists(path))
            {
                Path = path;
            }
        }

        public string Path { get; private set; } = "/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Models/";

        public void SetPath(string path)
        {
            if (Directory.Exists(path))
            {
                Path = path;
            }
        }

        public void Save(IList<File> files, RestorePoint point)
        {
            if (!Directory.Exists(Path + $"/BackupJob_{point.JobId}/Restore_Point_{point.Number}"))
            {
                Directory.CreateDirectory(Path + $"/BackupJob_{point.JobId}/Restore_Point_{point.Number}");
            }

            string rawFileName;
            if (System.IO.File.Exists(files.First().Path))
            {
                string fileName = files.First().Name;
                rawFileName = fileName?.Substring(0, fileName.LastIndexOf('.'));
            }
            else
            {
                throw new BackupException("Incorrect file path");
            }

            using ZipArchive archive = ZipFile.Open(
                Path + $"/BackupJob_{point.JobId}/" +
                $"Restore_Point_{point.Number}/{rawFileName}_{point.Number}.zip",
                ZipArchiveMode.Update);
            foreach (File file in files)
            {
                if (System.IO.File.Exists(file.Path))
                {
                    archive.CreateEntryFromFile(file.Path, file.Name);
                }
                else
                {
                    throw new BackupException("Incorrect file path!");
                }
            }

            foreach (File file in files)
            {
                point.AddFile(file);
            }
        }
    }
}