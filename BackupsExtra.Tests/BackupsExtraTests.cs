using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Algorithms;
using Backups.Models;
using Backups.Repositories;
using BackupsExtra.Loggers;
using BackupsExtra.Models;
using BackupsExtra.RemovingAlgorithms;
using BackupsExtra.RestrictiveAlgorithms;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    [TestFixture]
    public class BackupsExtraTests
    {
        private BackupJobService _backupJobService = BackupJobService.GetInstance();
        private BackupJobExtra _backupJob;
        private IAlgorithm _algorithm;
        private IRepository _storage;
        private Backups.Models.File _file;
        private Backups.Models.File _file2;
        private ILogger _logger;
        private IRestrictiveAlgorithm _restrictiveAlgorithm;
        private IRemove _remove;
        [SetUp]
        public void Setup()
        {
            _algorithm = new SplitStorage();
            _storage = new Memory();
            _logger = new FileLogger();
            _backupJob = _backupJobService.CreateBackupJobExtra(_algorithm, _storage, _logger);
            _file = new Backups.Models.File("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/NewEntry.jpg");
            _file2 = new Backups.Models.File("/Users/dsivan/Downloads/OS_Lab5.pdf");
        }
        [Test]
        public void CreatePoints_CheckHybridRemoveAlgorithmWork()
        {
            _remove = new Delete();
            _restrictiveAlgorithm = new LightHybridRestriction(DateTime.Now.AddHours(-2), 3);
            _backupJob.SetRemoveAlgorithm(_remove);
            _backupJob.SetRestrictiveAlgorithm(_restrictiveAlgorithm);
            _backupJob.AddFiles(new List<File>() { _file2, _file });
            _backupJob.CreatePoint();
            _backupJob.CreatePoint();
            _backupJob.Points[0].CreationTime = DateTime.Now.AddHours(-3);
            _backupJob.RemoveFiles(new List<File>() { _file });
            _backupJob.CreatePoint();
            _backupJob.AddFiles(new List<File>() { _file });
            _backupJob.ChangeAlgorithm(new SingleStorage());
            _backupJob.CreatePoint();
            Assert.AreEqual(3,_backupJob.Points.Count);
            
        }
        [Test]
        public void DeleteExtraPoints_MergeWorksCorrect()
        {
            _remove = new Merge();
            _restrictiveAlgorithm = new CountRestriction(2);
            _backupJob.SetRemoveAlgorithm(_remove);
            _backupJob.SetRestrictiveAlgorithm(_restrictiveAlgorithm);
            _backupJob.AddFiles(new List<File>() { _file2, _file });
            _backupJob.CreatePoint();
            _backupJob.CreatePoint();
            _backupJob.Points[0].CreationTime = DateTime.Now.AddHours(-3);
            _backupJob.RemoveFiles(new List<File>() { _file });
            RestorePoint restorePoint = _backupJob.CreatePoint();
            _backupJob.AddFiles(new List<File>() { _file });
            _backupJob.ChangeAlgorithm(new SingleStorage());
            _backupJob.CreatePoint();
            Assert.AreEqual(2,_backupJob.Points.Count);
            Assert.AreEqual(2,restorePoint.Files.Count);
        }
        // [Test]
        // public void DownloadFromConfigFile_CorrectDownload()
        // {
        //     _backupJobService.DownloadBackupJobs();
        //     Assert.IsTrue(_backupJobService.BackupJobExtras.Count > 0);
        //     Assert.IsTrue(_backupJobService.BackupJobExtras.Last().Points.Count > 0);
        //     Assert.IsTrue(_backupJobService.BackupJobExtras.Last().Points.First().Files.Count > 0);
        //
        // }
        [Test]
        public void LoggerWorkCheck_LoggerWorksCorrect()
        {
            _remove = new Merge();
            _restrictiveAlgorithm = new CountRestriction(2);
            _backupJob.SetRemoveAlgorithm(_remove);
            _backupJob.SetRestrictiveAlgorithm(_restrictiveAlgorithm);
            _backupJob.AddFiles(new List<File>() { _file2, _file });
            _backupJob.CreatePoint();
            _backupJob.CreatePoint();
            _backupJob.Points[0].CreationTime = DateTime.Now.AddHours(-3);
            _backupJob.RemoveFiles(new List<File>() { _file });
            RestorePoint restorePoint = _backupJob.CreatePoint();
            _backupJob.AddFiles(new List<File>() { _file });
            _backupJob.ChangeAlgorithm(new SingleStorage());
            _backupJob.CreatePoint();
            Assert.AreEqual(2, _backupJob.Files.Count); 
        }
    }
}