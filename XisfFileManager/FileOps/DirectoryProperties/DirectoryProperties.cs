using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XisfFileManager.FileOperations;
using XisfFileManager.Enums;
using System.Collections;

namespace XisfFileManager.FileOps.DirectoryProperties;

internal class DirectoryProperties
{
    // In each GetxxxDirectoryPath, we return Path.GetDirectoryName(directoryPath) if the key does not exist.
    // This allows us to fall through passing the complete directory structure to that point on to the next Grouping

    private List<string> mFilePathList = new List<string>();
    private List<GroupedKey> mGroupedPaths = new List<GroupedKey>();

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
        // Assumes one letter followed any 3 digits
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], @"^[ZQA]\d{3}$"))
            {
                string cameraDirectory = string.Join("\\", directories.Take(i + 1));
                return cameraDirectory;
            }
        }
        return string.Empty;
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
        return string.Empty;
    }

    private string GetLightsDirectoryPath(string directoryPath)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], @"^(?:(?!Stars).)*(Luma|Red|Green|Blue|Ha|O3|S2|Shutter)"))
            {
                string lightsDirectory = string.Join("\\", directories.Take(i + 1));
                return lightsDirectory;
            }
        }
        return string.Empty;
    }

    private string GetStarsDirectoryPath(string directoryPath)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            // Starts with "Stars"
            if (Regex.IsMatch(directories[i], @"^(?=.*Stars)(?=.*(?:Luma|Red|Green|Blue|Ha|O3|S2|Shutter)).*$"))
            {
                string starsDirectory = string.Join("\\", directories.Take(i + 1));
                return starsDirectory;
            }
        }
        return string.Empty;
    }

    private string GetFilterDirectoryPath(string directoryPath, string sFilter)
    {
        string[] directories = directoryPath.Split('\\');
        for (int i = directories.Length - 1; i >= 0; i--)
        {
            if (Regex.IsMatch(directories[i], $".*{Regex.Escape(sFilter)}.*"))
            {
                string filterDirectory = string.Join("\\", directories.Take(i + 1));
                return filterDirectory;
            }
        }
        return string.Empty;
    }

    internal class GroupedKey
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

        mGroupedPaths = ((IEnumerable<GroupedKey>)
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
                 Shutter = GetFilterDirectoryPath(filePath, "Shutter"),
                 Files = filePath
             } into pathGroup
             select new GroupedKey
             {
                 Target = pathGroup.Key.Target,
                 Camera = pathGroup.Key.Camera,
                 Panel = pathGroup.Key.Panel,
                 Lights = pathGroup.Key.Lights,
                 Stars = pathGroup.Key.Stars,
                 Files = pathGroup.ToList()
             })).ToList();
    }

    public IEnumerable<IGrouping<string, GroupedKey>> TargetGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Target);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> CameraGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Camera);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> PanelGroup()
    {
        if (mGroupedPaths != null)
        { 
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Panel);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> LightsGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Lights);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> StarsGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Stars);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> LumaGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Luma);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> RedGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Red);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> GreenGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Green);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> BlueGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Luma);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> HaGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Ha);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> O3Group()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.O3);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> S2Group()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.S2);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> ShutterGroup()
    {
        if (mGroupedPaths != null)
        {
            return mGroupedPaths.GroupBy(groupedPath => groupedPath.Shutter);
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }

    public IEnumerable<IGrouping<string, GroupedKey>> FilterInGroup(string sFilter, IEnumerable<IGrouping<string, GroupedKey>> group)
    {
        if (mGroupedPaths != null)
        {
            switch (sFilter)
            {
                case "Luma":
                    return group.SelectMany(g => g).GroupBy(gk => gk.Luma);
                case "Red":
                    return group.SelectMany(g => g).GroupBy(gk => gk.Red);
                case "Green":
                    return group.SelectMany(g => g).GroupBy(gk => gk.Green);
                case "Blue":
                    return group.SelectMany(g => g).GroupBy(gk => gk.Blue);
                case "Ha":
                    return group.SelectMany(g => g).GroupBy(gk => gk.Ha);
                case "O3":
                    return group.SelectMany(g => g).GroupBy(gk => gk.O3);
                case "S2":
                    return group.SelectMany(g => g).GroupBy(gk => gk.S2);
                default:
                    return group.SelectMany(g => g).GroupBy(gk => gk.Shutter);
            }
        }
        else
        {
            return Enumerable.Empty<IGrouping<string, GroupedKey>>();
        }
    }
}