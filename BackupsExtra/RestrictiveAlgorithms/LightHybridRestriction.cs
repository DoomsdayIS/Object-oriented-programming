using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Models;
using BackupsExtra.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.RestrictiveAlgorithms
{
    public class LightHybridRestriction : IRestrictiveAlgorithm
    {
        public LightHybridRestriction(DateTime dateTime, int count)
        {
            TimeLimit = dateTime;
            MaxPoints = count;
        }

        public DateTime TimeLimit { get; private set; }
        public int MaxPoints { get; private set; }

        public void SetPointsCount(int newCount)
        {
            if (newCount < 1) return;
            MaxPoints = newCount;
        }

        public void SetTimeLimit(DateTime dateTime)
        {
            TimeLimit = dateTime;
        }

        public List<RestorePoint> Check(BackupJobExtra backupJobExtra)
        {
            var restorePoints1 = backupJobExtra
                .Points.Where(p => p.CreationTime < TimeLimit).ToList();
            var restorePoints2 = new List<RestorePoint>();
            for (int i = 0; i < backupJobExtra.Points.Count - MaxPoints; i++)
            {
                restorePoints2.Add(backupJobExtra.Points[i]);
            }

            var restorePoints = restorePoints1.Intersect(restorePoints2).ToList();

            if (restorePoints.Count == backupJobExtra.Points.Count)
            {
                throw new BackupExtraException("Error! This algorithm will delete all points!");
            }

            return restorePoints;
        }
    }
}