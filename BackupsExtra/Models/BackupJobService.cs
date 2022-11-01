using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Algorithms;
using Backups.Models;
using Backups.Repositories;
using BackupsExtra.Loggers;
using BackupsExtra.Tools;
using File = System.IO.File;

namespace BackupsExtra.Models
{
    public class BackupJobService
    {
        private static BackupJobService _instance;

        private BackupJobService() { }
        public List<BackupJobExtra> BackupJobExtras { get; } = new ();
        public string ConfigFile { get; } = "/Users/dsivan/RiderProjects/DoomsdayIS/BackupsExtra/config.txt";

        public static BackupJobService GetInstance()
        {
            return _instance ??= new BackupJobService();
        }

        public BackupJobExtra CreateBackupJobExtra(IAlgorithm algorithm, IRepository repository, ILogger logger)
        {
            var backupJobExtra =
                new BackupJobExtra(algorithm, repository, BackupJobExtras.LastOrDefault()?.Id + 1 ?? 1, logger);
            if (Directory.Exists($"{repository.Path}/BackupJob_{backupJobExtra.Id}"))
            {
                Directory.Delete($"{repository.Path}/BackupJob_{backupJobExtra.Id}", true);
            }

            BackupJobExtras.Add(backupJobExtra);
            return backupJobExtra;
        }

        public void DeleteBackupJobExtra(List<BackupJobExtra> backupJobExtras)
        {
            BackupJobExtras.RemoveAll(b => backupJobExtras.Any(j => b == j));
        }

        public void DownloadBackupJobs()
        {
            if (!File.Exists(ConfigFile)) return;
            try
            {
                using (var sr = new StreamReader(ConfigFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] list = line.Split(' ');
                        if (list[0] != "Job" || list.Length != 8) continue;
                        if (!int.TryParse(list[1], out int id))
                        {
                            throw new BackupExtraException("Incorrect job id!");
                        }

                        IAlgorithm algorithm = list[2] switch
                        {
                            "SingleStorage" => new SingleStorage(),
                            "SplitStorage" => new SplitStorage(),
                            _ => throw new BackupExtraException("Incorrect algorithm type!")
                        };
                        IRepository repository = list[3] switch
                        {
                            "Storage" => new Storage(),
                            "Memory" => new Memory(),
                            _ => throw new BackupExtraException("Incorrect repository type!")
                        };
                        repository.SetPath(list[7]);
                        ILogger logger = list[4] switch
                        {
                            "FileLogger" => new FileLogger(),
                            "ConsoleLogger" => new ConsoleLogger(),
                            _ => throw new BackupExtraException("Incorrect logger type!")
                        };
                        var backupJobExtra = new BackupJobExtra(algorithm, repository, id, logger);
                        if (!int.TryParse(list[5], out int jobFilesCount))
                        {
                            throw new BackupExtraException($"Incorrect job {id} files count!");
                        }

                        if (!int.TryParse(list[6], out int pointsCount))
                        {
                            throw new BackupExtraException($"Incorrect job {id} points count!");
                        }

                        for (int i = 0; i < jobFilesCount; i += 1)
                        {
                            string filePath = sr.ReadLine();
                            var file = new Backups.Models.File(filePath);
                            backupJobExtra.Files.Add(file);
                        }

                        for (int k = 0; k < pointsCount; k += 1)
                        {
                            string pointInfo = sr.ReadLine();
                            string[] pointList = pointInfo?.Split(' ');
                            if (pointList?.Length != 9)
                            {
                                continue;
                            }

                            if (!int.TryParse(pointList[1], out int pointId))
                            {
                                throw new BackupExtraException("Incorrect point id!");
                            }

                            IAlgorithm pointAlgorithm = pointList[4] switch
                            {
                                "SingleStorage" => new SingleStorage(),
                                "SplitStorage" => new SplitStorage(),
                                _ => throw new BackupExtraException("Incorrect algorithm type!")
                            };
                            IRepository pointRepository = pointList[5] switch
                            {
                                "Storage" => new Storage(),
                                "Memory" => new Memory(),
                                _ => throw new BackupExtraException("Incorrect repository type!")
                            };
                            pointRepository.SetPath(pointList[8]);
                            if (!int.TryParse(pointList[6], out int pointJobId))
                            {
                                throw new BackupExtraException($"Incorrect point job id!");
                            }

                            if (!int.TryParse(pointList[7], out int pointFilesCount))
                            {
                                throw new BackupExtraException($"Incorrect point files count!");
                            }

                            var point =
                                new RestorePoint(pointId, pointAlgorithm, pointRepository, pointJobId);
                            point.CreationTime = DateTime.Parse(pointList[2] + " " + pointList[3]);
                            for (int i = 0; i < pointFilesCount; i += 1)
                            {
                                string filePath = sr.ReadLine();
                                var file = new Backups.Models.File(filePath);
                                point.Files.Add(file);
                            }

                            backupJobExtra.Points.Add(point);
                        }

                        BackupJobExtras.Add(backupJobExtra);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public void SaveBackupJobs()
        {
            if (File.Exists(ConfigFile))
            {
                File.Delete(ConfigFile);
            }

            foreach (BackupJobExtra backupJobExtra in BackupJobExtras)
            {
                try
                {
                    var sw = new StreamWriter(ConfigFile, true);
                    sw.WriteLine($"Job {backupJobExtra.Id} " +
                                 $"{backupJobExtra.Algorithm.ToString()?.Split('.').Last()} " +
                                 $"{backupJobExtra.Repository.ToString()?.Split('.').Last()} " +
                                 $"{backupJobExtra.Logger?.ToString()?.Split('.').Last()} " +
                                 $"{backupJobExtra.Files.Count} " +
                                 $"{backupJobExtra.Points.Count} " +
                                 $"{backupJobExtra.Repository.Path}");
                    foreach (Backups.Models.File file in backupJobExtra.Files)
                    {
                        sw.WriteLine($"{file.Path}");
                    }

                    foreach (RestorePoint point in backupJobExtra.Points)
                    {
                        sw.WriteLine($"Point {point.Number} " +
                                     $"{point.CreationTime} " +
                                     $"{point.Algorithm.ToString()?.Split('.').Last()} " +
                                     $"{point.Repository.ToString()?.Split('.').Last()} " +
                                     $"{point.JobId} " +
                                     $"{point.Files.Count} " +
                                     $"{point.Repository.Path}");
                        foreach (Backups.Models.File file in point.Files)
                        {
                            sw.WriteLine($"{file.Path}");
                        }
                    }

                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception while saving: " + e.Message);
                }
            }
        }
    }
}