using System.Linq;
using Backups.Tools;

namespace Backups.Models
{
    public class File : IObjectable
    {
        public File(string path)
        {
            if (!path.Contains('/') || !path.Contains('.'))
            {
                throw new BackupException("Incorrect file name!");
            }

            Path = path;
            Name = path?.Split('/').Last();
        }

        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}