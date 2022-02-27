using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DirectoryOps
{
    internal class DirectoryOps
    {
        public static List<string> exceptionLog = new List<string>();
        public static List<System.IO.FileInfo> fiFileList = new List<System.IO.FileInfo>();
        public enum FileType { AllFiles, ExcludeMasters, MastersOnly };

        public static void RecuseXisfFiles(System.IO.DirectoryInfo rootDirectory, bool bRecurse, FileType fileType)
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
                    exceptionLog.Add("Duplicates Directory Found in " + rootDirectory.Parent.FullName);
                }
            }

            catch (UnauthorizedAccessException e)
            {
                exceptionLog.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                exceptionLog.Add(e.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    switch (fileType)
                    {
                        case FileType.AllFiles:
                            fiFileList.Add(fi);
                            break;

                        case FileType.ExcludeMasters:
                            if (!fi.Name.Contains("Master"))
                                fiFileList.Add(fi);
                            break;

                        case FileType.MastersOnly:
                            if (fi.Name.Contains("Master"))
                                fiFileList.Add(fi);
                            break;
                    }
                }

                if (!bRecurse) return;

                // Find all the directories under this directory
                System.IO.DirectoryInfo[] subDirs = rootDirectory.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    RecuseXisfFiles(dirInfo, bRecurse, fileType);
                }
            }
        }
    }
}
