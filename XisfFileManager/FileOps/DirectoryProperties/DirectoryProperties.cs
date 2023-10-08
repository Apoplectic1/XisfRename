using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XisfFileManager.FileOperations;
using XisfFileManager.Enums;
using System.Collections;
using System;

namespace XisfFileManager.FileOps.DirectoryProperties;

internal class DirectoryProperties
{
    public Dictionary<string, string> GroupStatistics = new Dictionary<string, string>();

    public void SetDirectoryStatistics(List<XisfFile> xFileList, bool bNoTotals)
    {
        var directoryGroups = xFileList.GroupBy(path => Path.GetDirectoryName(path.FilePath));

        foreach (var group in directoryGroups)
        {
            string groupName = group.Key;

            int groupCount = group.Count();
            double totalExposureTime = group.Sum(fileItem => fileItem.ExposureSeconds) / 3600.0;

            string string2 = bNoTotals ? "" : $"{groupCount}, {totalExposureTime:F1}";
            int indexOfDashSpace = groupName.LastIndexOf(" -");

            string newStats = indexOfDashSpace >= 0 ? $"{groupName.Substring(0, indexOfDashSpace)} - {string2}" : $"{groupName} - {string2}".Trim();

            GroupStatistics[groupName] = newStats;
        }
    }
}