using System;
using System.Collections.Generic;
using System.Collections.Specialized;


public class DirectoryOps
{
    private List<System.IO.FileInfo> mFileList;
    private List<string> mExceptionLog;
    private System.IO.FileInfo mFileInfo;

    public enum CameraType { ALL, Z183, Z533, Q178, A144 }
    public enum FileType { ALL, NO_MASTERS, MASTERS };
    public enum FilterType { ALL, LUMA, RED, GREEN, BLUE, HA, O3, S2, SHUTTER }
    public enum FrameType { ALL, LIGHT, DARK, FLAT, BIAS };
    public CameraType Camera { get; set; } = CameraType.ALL;
    public FileType File { get; set; } = FileType.NO_MASTERS;
    public FilterType Filter { get; set; } = FilterType.ALL;
    public FrameType Frame { get; set; } = FrameType.ALL;
    public List<System.IO.FileInfo> Files { get { return mFileList; } }
    public List<string> GetExceptions { get { return mExceptionLog; } }
    public void ClearFileList() { mFileList.Clear(); mExceptionLog.Clear(); }
    public bool Recurse { get; set; } = true;


    public DirectoryOps()
    {
        mFileList = new List<System.IO.FileInfo>();
        mExceptionLog = new List<string>();
    }

    public bool RecuseDirectories(System.IO.DirectoryInfo rootDirectory)
    {

        System.IO.FileInfo[] files = null;

        // Process all the files directly under this folder 
        try
        {
            if (rootDirectory.Name != "Duplicates")
            {
                files = rootDirectory.GetFiles("*.xisf");
            }
            else
            {
                mExceptionLog.Add("Duplicates Directory Found in " + rootDirectory.Parent.FullName);
            }
        }

        catch (UnauthorizedAccessException e)
        {
            mExceptionLog.Add(e.Message);
            return false;
        }

        catch (System.IO.DirectoryNotFoundException e)
        {
            mExceptionLog.Add(e.Message);
            return false;
        }

        if (files != null)
        {
            foreach (System.IO.FileInfo fileInfo in files)
            {
                mFileInfo = fileInfo;

                OnlyFileType(File);
                OnlyCameraType(Camera);
                OnlyFilterType(Filter);
                OnlyFrameType(Frame);

                if (mFileInfo != null)
                    mFileList.Add(mFileInfo);
            }

            if (!Recurse)
            {
                return true;
            }

            // Find all the directories under this directory
            System.IO.DirectoryInfo[] subDirs = rootDirectory.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                RecuseDirectories(dirInfo);
            }
        }

        if (mFileList.Count == 0)
        {
            mExceptionLog.Add("No Files Found.");
            return false;
        }

        return true;
    }

    private void OnlyFileType(FileType fileType)
    {
        if (mFileInfo == null) return;

        switch (fileType)
        {
            case FileType.ALL:
                break;

            case FileType.MASTERS:
                if (!mFileInfo.Name.Contains("Master"))
                    mFileInfo = null;
                break;

            case FileType.NO_MASTERS:
                if (mFileInfo.Name.Contains("Master"))
                    mFileInfo = null;
                break;

            default:
                mExceptionLog.Add("File name does not match FileType: " + mFileInfo.Name);
                mFileInfo = null;
                break;
        }
    }

    private void OnlyCameraType(CameraType CameraType)
    {
        if (mFileInfo == null) return;

        switch (CameraType)
        {
            case CameraType.ALL:
                break;

            case CameraType.Z183:
                if (!mFileInfo.Name.Contains("Z183"))
                    mFileInfo = null;
                break;

            case CameraType.Z533:
                if (!mFileInfo.Name.Contains("Z533"))
                    mFileInfo = null;
                break;

            case CameraType.A144:
                if (!mFileInfo.Name.Contains("A144"))
                    mFileInfo = null;
                break;

            default:
                mExceptionLog.Add("File name does not contain camera type: " + mFileInfo.Name);
                mFileInfo = null;
                break;
        }
    }

    private void OnlyFrameType(FrameType FrameType)
    {
        if (mFileInfo == null) return;

        switch (FrameType)
        {
            case FrameType.ALL:
                break;

            case FrameType.DARK:
                if (!mFileInfo.Name.Contains("Dark"))
                    mFileInfo = null;
                break;

            case FrameType.LIGHT:
                if (!mFileInfo.Name.Contains("L-"))
                    mFileInfo = null;
                break;

            case FrameType.FLAT:
                if (!mFileInfo.Name.Contains("F-"))
                    mFileInfo = null;
                break;

            case FrameType.BIAS:
                if (!mFileInfo.Name.Contains("B-"))
                    mFileInfo = null;
                break;

            default:
                mExceptionLog.Add("File name does not contain frame type: " + mFileInfo.Name);
                mFileInfo = null;
                break;
        }
    }

    private void OnlyFilterType(FilterType filterType)
    {
        if (mFileInfo == null) return;

        switch (filterType)
        {
            case FilterType.ALL:
                break;

            case FilterType.LUMA:
                if (!mFileInfo.Name.Contains("Luma"))
                    mFileInfo = null;
                break;

            case FilterType.RED:
                if (!mFileInfo.Name.Contains("Red"))
                    mFileInfo = null;
                break;

            case FilterType.GREEN:
                if (!mFileInfo.Name.Contains("Green"))
                    mFileInfo = null;
                break;

            case FilterType.BLUE:
                if (!mFileInfo.Name.Contains("Blue"))
                    mFileInfo = null;
                break;

            case FilterType.HA:
                if (mFileInfo.Name.Contains("Ha"))
                    mFileInfo = null;
                break;

            case FilterType.O3:
                if (mFileInfo.Name.Contains("O3"))
                    mFileInfo = null;
                break;

            case FilterType.S2:
                if (mFileInfo.Name.Contains("S2"))
                    mFileInfo = null;
                break;

            case FilterType.SHUTTER:
                if (mFileInfo.Name.Contains("Shutter"))
                    mFileInfo = null;
                break;

            default:
                mExceptionLog.Add("File name does not contain filter: " + mFileInfo.Name);
                mFileInfo = null;
                break;
        }
    }
}

