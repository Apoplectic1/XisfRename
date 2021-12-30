using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DirectoryOps
{
    internal class DirectoryOps
    {
        public static List<string> exceptionLog = new List<string>();
        public static List<System.IO.FileInfo> fiFileList = new List<System.IO.FileInfo>();

        public static void RecuseXisfFiles(System.IO.DirectoryInfo rootDirectory, bool recurse)
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

            if (!recurse) return;

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    fiFileList.Add(fi);
                }

                // Find all the directories under this directory
                System.IO.DirectoryInfo[] subDirs = rootDirectory.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                        // Resursive call for each subdirectory.
                        RecuseXisfFiles(dirInfo, recurse);
                }
            }
        }
    }
}
