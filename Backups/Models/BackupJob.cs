using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Algorithms;
using Backups.Repositories;
using Backups.Tools;

namespace Backups.Models
{
    public class BackupJob
    {
        public BackupJob(IAlgorithm algorithm, IRepository repository)
        {
            Algorithm = algorithm;
            Repository = repository;
        }

        public IAlgorithm Algorithm { get; private set; }
        public IRepository Repository { get; private set; }

        public int Id { get; protected set; } = 1;

        public List<File> Files { get; } = new ();
        public List<RestorePoint> Points { get; } = new ();

        public virtual void AddFiles(IEnumerable<File> files)
        {
            Files.AddRange(files);
        }

        public virtual void RemoveFiles(IEnumerable<File> files)
        {
            Files.RemoveAll(x => files.Any(f => f.Path == x.Path));
        }

        public virtual void ChangeAlgorithm(IAlgorithm algorithm)
        {
            Algorithm = algorithm;
        }

        public virtual void ChangeRepositoryType(IRepository repository)
        {
            Repository = repository;
        }

        public virtual RestorePoint CreatePoint()
        {
            if (Files.Count == 0)
            {
                throw new BackupException("U can't create RestorePoint without files!");
            }

            var point = new RestorePoint(Points.LastOrDefault()?.Number + 1 ?? 1, Algorithm, Repository, Id);
            Algorithm.CreateBackup(point, Files, Repository);
            Points.Add(point);
            return point;
        }
    }
}