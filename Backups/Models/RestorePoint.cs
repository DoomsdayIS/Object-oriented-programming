using System;
using System.Collections.Generic;
using Backups.Algorithms;
using Backups.Repositories;

namespace Backups.Models
{
    public class RestorePoint
    {
        public RestorePoint(int number, IAlgorithm algorithm, IRepository repository, int jobId)
        {
            CreationTime = DateTime.Now;
            Number = number;
            Algorithm = algorithm;
            Repository = repository;
            JobId = jobId;
        }

        public DateTime CreationTime { get; set; }
        public int Number { get; protected set; }
        public int JobId { get; private set; }

        public IAlgorithm Algorithm { get; private set; }
        public IRepository Repository { get; private set; }
        public List<File> Files { get; private set; } = new List<File>();

        public void AddFile(File file)
        {
            Files.Add(file);
        }
    }
}