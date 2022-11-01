using System.Collections.Generic;
using Backups.Models;
using Backups.Repositories;

namespace Backups.Algorithms
{
    public class SingleStorage : IAlgorithm
    {
        public void CreateBackup(RestorePoint point, List<File> files, IRepository repository)
        {
            repository.Save(files, point);
        }
    }
}