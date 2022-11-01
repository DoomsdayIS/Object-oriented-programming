using System.Collections.Generic;
using Backups.Models;
using BackupsExtra.Models;

namespace BackupsExtra.RestrictiveAlgorithms
{
    public interface IRestrictiveAlgorithm
    {
        List<RestorePoint> Check(BackupJobExtra backupJobExtra);
    }
}