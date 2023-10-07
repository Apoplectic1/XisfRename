using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XisfFileManager.FileOperations;
using XisfFileManager.Enums;

namespace XisfFileManager.FileOps.DirectoryProperties;

internal class DirectoryProperties
{
    // In each GetxxxDirectoryPath, we return Path.GetDirectoryName(directoryPath) if the key does not exist.
    // This allows us to fall through passing the complete directory structure to that point on to the next Grouping

    private List<string> mFilePathList = new List<string>();
    private List<GroupedPath> mGroupedPaths = new List<GroupedPath>();

    private string GetTargetDirectoryPath(string directoryPath)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], @"^Captures"))
            {
                string targetDirectory = string.Join("\\", directories.Take(i));
                return targetDirectory;
            }
        }
        return string.Empty;
    }

    private string GetCameraDirectoryPath(string directoryPath)
    {
        // Assumes one letter followed ny 3 digits
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], @"^[ZQA]\d{3}$"))
            {
                string cameraDirectory = string.Join("\\", directories.Take(i + 1));
                return cameraDirectory;
            }
        }
        return GetTargetDirectoryPath(directoryPath);
    }

    private string GetPanelDirectoryPath(string directoryPath)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], @"^Panel\s+(\S+)$"))
            {
                string panelDirectory = string.Join("\\", directories.Take(i + 1));
                return panelDirectory;
            }
        }
        return GetCameraDirectoryPath(directoryPath);
    }

    private string GetLightsDirectoryPath(string directoryPath)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], @"^Panel\s+(\S+)$"))
            {
                string panelDirectory = string.Join("\\", directories.Take(i + 1));
                return panelDirectory;
            }
        }
        return Path.GetDirectoryName(directoryPath);
    }

    private string GetStarsDirectoryPath(string directoryPath)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], @"^Panel\s+(\S+)$"))
            {
                string panelDirectory = string.Join("\\", directories.Take(i + 1));
                return panelDirectory;
            }
        }
        return Path.GetDirectoryName(directoryPath);
    }

    private string GetFilterDirectoryPath(string directoryPath, string sFilter)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (directories[i].Contains(sFilter))
            {
                string panelDirectory = string.Join("\\", directories.Take(i + 1));
                return panelDirectory;
            }
        }
        return Path.GetDirectoryName(directoryPath);
    }

    internal class GroupedPath
    {
        public string Target { get; set; }
        public string Camera { get; set; }
        public string Panel { get; set; }
        public string Lights { get; set; }
        public string Stars { get; set; }
        public string Luma { get; set; }
        public string Red { get; set; }
        public string Green { get; set; }
        public string Blue { get; set; }
        public string Ha { get; set; }
        public string O3 { get; set; }
        public string S2 { get; set; }
        public string Shutter { get; set; }
        public List<string> Files { get; set; }
    }

    public void SetDirectoryStatistics(List<XisfFile> xFileList)
    {
        mFilePathList.Clear();

        foreach (var xFile in xFileList)
        {
            mFilePathList.Add(xFile.FilePath);
        }

        mGroupedPaths = ((IEnumerable<GroupedPath>)
            (from filePath in mFilePathList
             group filePath by new
             {
                 Target = GetTargetDirectoryPath(filePath),
                 Camera = GetCameraDirectoryPath(filePath),
                 Panel = GetPanelDirectoryPath(filePath),
                 Lights = GetLightsDirectoryPath(filePath),
                 Stars = GetStarsDirectoryPath(filePath),
                 Luma = GetFilterDirectoryPath(filePath, "Luma"),
                 Red = GetFilterDirectoryPath(filePath, "Red"),
                 Green = GetFilterDirectoryPath(filePath, "Green"),
                 Blue = GetFilterDirectoryPath(filePath, "Blue"),
                 Ha = GetFilterDirectoryPath(filePath, "Ha"),
                 O3 = GetFilterDirectoryPath(filePath, "O3"),
                 S2 = GetFilterDirectoryPath(filePath, "S2"),
                 Shutter = GetFilterDirectoryPath(filePath, "Shutter")
             } into pathGroup
             select new GroupedPath
             {
                 Target = pathGroup.Key.Target,
                 Camera = pathGroup.Key.Camera,
                 Panel = pathGroup.Key.Panel,
                 Lights = pathGroup.Key.Lights,
                 Stars = pathGroup.Key.Stars,
                 Files = pathGroup.ToList()
             })).ToList();
    }

    public IEnumerable<IGrouping<string, GroupedPath>> TargetGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Target);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedPath>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedPath>> CameraGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Camera);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedPath>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedPath>> PanelGroup()
    {
        if (mGroupedPaths != null)
        { 
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Panel);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedPath>>();
        }
    }
}