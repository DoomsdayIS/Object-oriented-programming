using System;
using System.IO;
using System.IO.Compression;
using Backups.Models;
using BackupsExtra.Models;
using File = System.IO.File;

namespace BackupsExtra.RestoreAlgorithms
{
    public class ToOriginalLocation : IRestore
    {
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
                archive.ExtractToDirectory($"{restorePoint.Repository.Path}BackupJob_{backupJobExtra.Id}/Restore_Point_{restorePoint.Number}", true);
            }

            foreach (Backups.Models.File file in restorePoint.Files)
            {
                string filename = $"{restorePoint.Repository.Path}BackupJob" +
                                  $"_{backupJobExtra.Id}/Restore_Point_{restorePoint.Number}" +
                                  $"/{file.Name}";
                if (!File.Exists(filename)) continue;
                if (!Directory.Exists(file.Path.Substring(0, file.Path.LastIndexOf('/')))) continue;
                File.Move(filename, file.Path, true);
                backupJobExtra.Logger.Log($"File {file.Name} successfully restored to original location");
            }
        }
    }
}