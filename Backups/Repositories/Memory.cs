using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Models;
using File = Backups.Models.File;

namespace Backups.Repositories
{
    public class Memory : IRepository
    {
        public string Path { get; private set; } = "-";
        public void SetPath(string path)
        {
        }

        public void Save(IList<File> files, RestorePoint point)
        {
            foreach (File file in files)
            {
                point.AddFile(file);
            }
        }
    }
}