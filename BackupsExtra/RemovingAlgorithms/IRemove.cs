using System.Collections.Generic;
using Backups.Models;
using BackupsExtra.Models;

namespace BackupsExtra.RemovingAlgorithms
{
    public interface IRemove
    {
        void Remove(List<RestorePoint> points, BackupJobExtra backupJobExtra);
    }
}