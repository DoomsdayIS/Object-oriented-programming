using System.Collections.Generic;
using Backups.Models;

namespace Backups.Repositories
{
    public interface IRepository
    {
        string Path { get; }
        void SetPath(string path);
        public void Save(IList<File> files, RestorePoint point);
    }
}