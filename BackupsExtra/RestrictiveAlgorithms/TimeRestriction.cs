using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Models;
using BackupsExtra.Models;
using BackupsExtra.Tools;

namespace BackupsExtra.RestrictiveAlgorithms
{
    public class TimeRestriction : IRestrictiveAlgorithm
    {
        public TimeRestriction(DateTime dateTime)
        {
            TimeLimit = dateTime;
        }

        public DateTime TimeLimit { get; private set; }

        public void SetTimeLimit(DateTime dateTime)
        {
            TimeLimit = dateTime;
        }

        public List<RestorePoint> Check(BackupJobExtra backupJobExtra)
        {
            var restorePoints = backupJobExtra
                .Points.Where(p => p.CreationTime < TimeLimit).ToList();
            if (restorePoints.Count == backupJobExtra.Points.Count)
            {
                throw new BackupExtraException("Error! This algorithm will delete all points!");
            }

            return restorePoints;
        }
    }
}