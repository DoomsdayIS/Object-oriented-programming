using System.Collections.Generic;
using Backups.Models;
using BackupsExtra.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.RestrictiveAlgorithms
{
    public class CountRestriction : IRestrictiveAlgorithm
    {
        public CountRestriction(int count)
        {
            if (count < 1)
            {
                throw new BackupExtraException("Incorrect value for Count algorithm!");
            }

            MaxPoints = count;
        }

        public int MaxPoints { get; private set; }

        public void SetPointsCount(int newCount)
        {
            if (newCount < 1) return;
            MaxPoints = newCount;
        }

        public List<RestorePoint> Check(BackupJobExtra backupJobExtra)
        {
            var restorePoints = new List<RestorePoint>();
            for (int i = 0; i < backupJobExtra.Points.Count - MaxPoints; i++)
            {
                restorePoints.Add(backupJobExtra.Points[i]);
            }

            if (restorePoints.Count == backupJobExtra.Points.Count)
            {
                throw new BackupExtraException("Error! This algorithm will delete all points!");
            }

            return restorePoints;
        }
    }
}