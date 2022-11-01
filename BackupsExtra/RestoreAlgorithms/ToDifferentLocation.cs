using System.IO;
using System.IO.Compression;
using Backups.Models;
using BackupsExtra.Models;
using BackupsExtra.Tools;
using File = System.IO.File;

namespace BackupsExtra.RestoreAlgorithms
{
    public class ToDifferentLocation : IRestore
    {
        public ToDifferentLocation(string path = null)
        {
            if (Directory.Exists(path))
            {
                Path = path;
            }
        }

        public string Path { get; private set; } = "/Users/dsivan/RiderProjects/DoomsdayIS/BackupsExtra/Restore/";

        public void SetPath(string path)
        {
            if (Directory.Exists(path))
            {
                Path = path;
            }
        }

        public void Restore(BackupJobExtra backupJobExtra, RestorePoint restorePoint)
        {
            foreach (Backups.Models.File file in restorePoint.Files)
            {
                string filename = file.Name.Substring(0, file.Name.LastIndexOf('.'));
                string fullFileName = $"{restorePoint.Repository.Path}BackupJob" +
                                      $"_{backupJobExtra.Id}/Restore_Point_{restorePoint.Number}" +
                                      $"/{filename}_{restorePoint.Number}.zip";
                if (!File.Exists(fullFileName)) continue;
                using ZipArchive archive = ZipFile.Open(fullFileName, ZipArchiveMode.Update);
                archive.ExtractToDirectory(Path, true);
            }
        }
    }
}