using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Algorithms;
using Backups.Models;
using Backups.Repositories;
using BackupsExtra.Loggers;
using BackupsExtra.RemovingAlgorithms;
using BackupsExtra.RestrictiveAlgorithms;
using BackupsExtra.Tools;
using File = Backups.Models.File;

namespace BackupsExtra.Models
{
    public class BackupJobExtra : BackupJob
    {
        public BackupJobExtra(IAlgorithm algorithm, IRepository repository, int id, ILogger logger)
            : base(algorithm, repository)
        {
            Id = id;
            Logger = logger;
        }

        public ILogger Logger { get; private set; }
        public IRestrictiveAlgorithm RestrictiveAlgorithm { get; private set; }
        public IRemove RemovalMethod { get; private set; }

        public override void AddFiles(IEnumerable<File> files)
        {
            base.AddFiles(files);
            Logger.Log($"Job {Id}. Files added successfully: ");
            foreach (File file in files)
            {
                Logger.Log($"   {file.Path}");
            }
        }

        public override void RemoveFiles(IEnumerable<File> files)
        {
            base.RemoveFiles(files);
            Logger.Log($"Job {Id}. Files removed successfully: ");
            foreach (File file in files)
            {
                Logger.Log($"   {file.Path}");
            }
        }

        public override void ChangeAlgorithm(IAlgorithm algorithm)
        {
            base.ChangeAlgorithm(algorithm);
            Logger.Log($"Job {Id}. Algorithm changed! " +
                       $"New algorithm is {algorithm.ToString()?.Split('.').Last()}");
        }

        public override void ChangeRepositoryType(IRepository repository)
        {
            base.ChangeRepositoryType(repository);
            Logger.Log($"Job {Id}. Algorithm changed! " +
                       $"New algorithm is {repository.ToString()?.Split('.').Last()}");
        }

        public void ChangeLoggerType(ILogger logger)
        {
            Logger = logger;
            Logger.Log($"Job {Id}. Logger type has been changed! New logger type is" +
                       $" {Logger.ToString()?.Split('.').Last()}");
        }

        public override RestorePoint CreatePoint()
        {
            if (Files.Count == 0)
            {
                throw new BackupExtraException("U can't create RestorePoint without files!");
            }

            var point = new RestorePoint(Points.LastOrDefault()?.Number + 1 ?? 1, Algorithm, Repository, Id);
            Algorithm.CreateBackup(point, Files, Repository);
            Points.Add(point);
            Logger.Log($"Job {Id}. Restore point {point.Number} created!");
            RemoveExtraPoints();
            return point;
        }

        public void SetRestrictiveAlgorithm(IRestrictiveAlgorithm restrictiveAlgorithm)
        {
            RestrictiveAlgorithm = restrictiveAlgorithm;
            Logger.Log($"Set restrictive algorithm {restrictiveAlgorithm.ToString()?.Split('.').Last()}");
        }

        public void SetRemoveAlgorithm(IRemove remove)
        {
            RemovalMethod = remove;
            Logger.Log($"Set removal algorithm {RemovalMethod.ToString()?.Split('.').Last()}");
        }

        public void RemoveExtraPoints()
        {
            if (RemovalMethod == null || RestrictiveAlgorithm == null) return;
            List<RestorePoint> removingPoint = RestrictiveAlgorithm?.Check(this);
            foreach (RestorePoint point in removingPoint)
            {
                Logger.Log($"Job {Id}. Point {point.Number} removed");
            }

            RemovalMethod?.Remove(removingPoint, this);
        }
    }
}