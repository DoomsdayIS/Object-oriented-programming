using System.Collections.Generic;
using Backups.Algorithms;
using Backups.Models;
using Backups.Repositories;
using Backups.Tools;
using NUnit.Framework;

namespace Backups.Tests
{
    [TestFixture]
    public class BackupJobTest
    {
        private BackupJob _backupJob;
        private IAlgorithm _splitStorage;
        private IRepository _storage;
        private File _file;
        private File _file2;
        [SetUp]
        public void Setup()
        {
            _splitStorage = new SplitStorage();
            _storage = new Memory();
            _backupJob = new BackupJob(_splitStorage, _storage);
            _file = new Backups.Models.File("/Users/dsivan/RiderProjects/DoomsdayIS/Backups/Storage/NewEntry.jpg");
            _file2 = new Backups.Models.File("/Users/dsivan/Downloads/OS_Lab5.pdf");
        }
        [Test]
        public void AddFilesToBackup_FilesAddedSuccessfully()
        {
            _backupJob.AddFiles(new List<File>() { _file, _file2 });
            Assert.Contains(_file,_backupJob.Files);
            Assert.Contains(_file2,_backupJob.Files);
        }
        [Test]
        public void CreateRestorePointsRemoveFileCreateAnother_PointsHaveDifferentCountOfObjects()
        {
            _backupJob.AddFiles(new List<File>() { _file, _file2 });
            RestorePoint point = _backupJob.CreatePoint();
            _backupJob.RemoveFiles(new List<File>() { _file2 });
            RestorePoint point2 = _backupJob.CreatePoint();
            Assert.AreEqual(2,point.Files.Count);
            Assert.AreEqual(1,point2.Files.Count);
            Assert.AreEqual(2,_backupJob.Points.Count);
        }
        [Test]
        public void ChangeAlgoCreatePoint_PointCreatedSuccessfully()
        {
            _backupJob.AddFiles(new List<File>() { _file, _file2 });
            RestorePoint point = _backupJob.CreatePoint();
            IAlgorithm algorithm2 = new SingleStorage();
            _backupJob.ChangeAlgorithm(algorithm2);
            RestorePoint point2 = _backupJob.CreatePoint();
            Assert.AreEqual(2,point2.Files.Count);
        }
        [Test]
        public void GetIncorrectFileNameException()
        {
            Assert.Catch<BackupException>(() =>
            {
                var file3 = new File("smthStupidAndIncorrect");

            });

        }
    }
}