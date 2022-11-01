using System.Collections.Generic;
using System.IO;
using Backups.Models;
using BackupsExtra.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.RemovingAlgorithms
{
    public class Delete : IRemove
    {
        public void Remove(List<RestorePoint> points, BackupJobExtra backupJobExtra)
        {
            foreach (RestorePoint point in points)
            {
                if (Directory.Exists($"{point.Repository.Path}BackupJob_{backupJobExtra.Id}/Restore_Point_{point.Number}"))
                {
                    Directory
                        .Delete($"{point.Repository.Path}BackupJob_{backupJobExtra.Id}/Restore_Point_{point.Number}", true);
                }

                backupJobExtra.Points.Remove(point);
            }
        }
    }
}