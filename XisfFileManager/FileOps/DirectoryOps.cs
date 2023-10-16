using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using XisfFileManager.Enums;

namespace XisfFileManager
{
    public class DirectoryOps
    {
        private readonly List<System.IO.FileInfo> mFileList;
        private readonly List<string> mExceptionLog;
        private System.IO.FileInfo mFileInfo;

       
        public eCamera Camera { get; set; } = eCamera.ALL;
        public eFile File { get; set; } = eFile.NO_MASTERS;
        public eFilter Filter { get; set; } = eFilter.ALL;
        public eFrame Frame { get; set; } = eFrame.ALL;
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
            FileInfo[] files = null;

            // Find and process all the files directly under this folder 
            try
            {
                if (
                    (rootDirectory.Name != "Duplicates") && 
                    (rootDirectory.Name != "PreProcessing") &&
                    (rootDirectory.Name != "Project")
                    )
                {
                    FileInfo[] allfiles = rootDirectory.GetFiles("*.xisf");

                    // Filter out files that contain a '#' character (This is to exclude emacs temporary save files during debug)
                    files = allfiles.Where(file => !file.Name.Contains('#')).ToArray();
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
                    _ = RecuseDirectories(dirInfo);
                }
            }

            if (mFileList.Count == 0)
            {
                mExceptionLog.Add("No Files Found.");
                return false;
            }

            return true;
        }

        private void OnlyFileType(eFile fileType)
        {
            if (mFileInfo == null) return;

            switch (fileType)
            {
                case eFile.ALL:
                    break;

                case eFile.MASTERS:
                    if (!mFileInfo.Name.ToLower().Contains("master"))
                        mFileInfo = null;
                    break;

                case eFile.NO_MASTERS:
                    if (mFileInfo.Name.ToLower().Contains("master"))
                        mFileInfo = null;
                    break;

                default:
                    mExceptionLog.Add("File name does not match FileType: " + mFileInfo.Name);
                    mFileInfo = null;
                    break;
            }
        }

        private void OnlyCameraType(eCamera CameraType)
        {
            if (mFileInfo == null) return;

            switch (CameraType)
            {
                case eCamera.ALL:
                    break;

                case eCamera.Z183:
                    if (!mFileInfo.Name.Contains("Z183"))
                        mFileInfo = null;
                    break;

                case eCamera.Z533:
                    if (!mFileInfo.Name.Contains("Z533"))
                        mFileInfo = null;
                    break;

                case eCamera.A144:
                    if (!mFileInfo.Name.Contains("A144"))
                        mFileInfo = null;
                    break;

                default:
                    mExceptionLog.Add("File name does not contain camera type: " + mFileInfo.Name);
                    mFileInfo = null;
                    break;
            }
        }

        private void OnlyFrameType(eFrame frameType)
        {
            if (mFileInfo == null) return;

            switch (frameType)
            {
                case eFrame.ALL:
                    break;

                case eFrame.DARK:
                    if (!mFileInfo.Name.Contains("Dark"))
                        mFileInfo = null;
                    break;

                case eFrame.LIGHT:
                    if (!mFileInfo.Name.Contains("L-"))
                        mFileInfo = null;
                    break;

                case eFrame.FLAT:
                    if (!mFileInfo.Name.Contains("F-"))
                        mFileInfo = null;
                    break;

                case eFrame.BIAS:
                    if (!mFileInfo.Name.Contains("B-"))
                        mFileInfo = null;
                    break;

                default:
                    mExceptionLog.Add("File name does not contain frame type: " + mFileInfo.Name);
                    mFileInfo = null;
                    break;
            }
        }

        private void OnlyFilterType(eFilter filterType)
        {
            if (mFileInfo == null) return;

            switch (filterType)
            {
                case eFilter.ALL:
                    break;

                case eFilter.LUMA:
                    if (!mFileInfo.Name.Contains("Luma"))
                        mFileInfo = null;
                    break;

                case eFilter.RED:
                    if (!mFileInfo.Name.Contains("Red"))
                        mFileInfo = null;
                    break;

                case eFilter.GREEN:
                    if (!mFileInfo.Name.Contains("Green"))
                        mFileInfo = null;
                    break;

                case eFilter.BLUE:
                    if (!mFileInfo.Name.Contains("Blue"))
                        mFileInfo = null;
                    break;

                case eFilter.HA:
                    if (mFileInfo.Name.Contains("Ha"))
                        mFileInfo = null;
                    break;

                case eFilter.O3:
                    if (mFileInfo.Name.Contains("O3"))
                        mFileInfo = null;
                    break;

                case eFilter.S2:
                    if (mFileInfo.Name.Contains("S2"))
                        mFileInfo = null;
                    break;

                case eFilter.SHUTTER:
                    if (mFileInfo.Name.Contains("Shutter"))
                        mFileInfo = null;
                    break;

                default:
                    mExceptionLog.Add("File " + mFileInfo.Name + " is not referencing a specific filter.");
                    mFileInfo = null;
                    break;
            }
        }
    }
}
