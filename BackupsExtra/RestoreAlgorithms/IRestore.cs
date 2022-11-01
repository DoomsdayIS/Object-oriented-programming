using Backups.Models;
using BackupsExtra.Models;

namespace BackupsExtra.RestoreAlgorithms
{
    public interface IRestore
    {
        void Restore(BackupJobExtra backupJobExtra, RestorePoint restorePoint);
    }
}