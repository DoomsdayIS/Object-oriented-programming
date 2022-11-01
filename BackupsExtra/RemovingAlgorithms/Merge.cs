using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Models;
using BackupsExtra.Models;
using BackupsExtra.Tools;
using File = Backups.Models.File;

namespace BackupsExtra.RemovingAlgorithms
{
    public class Merge : IRemove
    {
        public void Remove(List<RestorePoint> points, BackupJobExtra backupJobExtra)
        {
            foreach (RestorePoint point in points)
            {
                RestorePoint nextPoint = backupJobExtra.Points.FirstOrDefault(p => p.Number > point.Number);
                if (point.Algorithm.ToString()?.Split('.').Last() != "SingleStorage" && nextPoint != null)
                {
                    foreach (File file in point.Files)
                    {
                        if (nextPoint.Files.FirstOrDefault(f => f.Path == file.Path) == null)
                        {
                            nextPoint.Files.Add(file);
                            string filename = file.Name?.Substring(0, file.Name?.LastIndexOf('.') ?? 0);
                            string fullFileName = $"{point.Repository.Path}BackupJob" +
                                                  $"_{backupJobExtra.Id}/Restore_Point_{point.Number}" +
                                                  $"/{filename}_{point.Number}.zip";
                            string nextFullName = $"{nextPoint.Repository.Path}BackupJob" +
                                                  $"_{backupJobExtra.Id}/Restore_Point_{nextPoint.Number}" +
                                                  $"/{filename}_{nextPoint.Number}.zip";
                            if (System.IO.File.Exists(fullFileName) && Directory.Exists($"{nextPoint.Repository.Path}BackupJob" +
                                $"_{backupJobExtra.Id}/Restore_Point_{nextPoint.Number}"))
                            {
                                System.IO.File.Move(fullFileName, nextFullName, true);
                            }
                        }
                    }
                }

                if (Directory.Exists($"{point.Repository.Path}BackupJob" +
                                     $"_{backupJobExtra.Id}/Restore_Point_{point.Number}"))
                {
                    Directory
                        .Delete($"{point.Repository.Path}BackupJob_{backupJobExtra.Id}/Restore_Point_{point.Number}", true);
                }

                backupJobExtra.Points.Remove(point);
            }
        }
    }
}