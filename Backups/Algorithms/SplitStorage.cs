using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using Backups.Models;
using Backups.Repositories;

namespace Backups.Algorithms
{
    public class SplitStorage : IAlgorithm
    {
        public void CreateBackup(RestorePoint point, List<File> files, IRepository repository)
        {
            foreach (File file in files)
            {
                repository.Save(new List<File>() { file }, point);
            }
        }
    }
}