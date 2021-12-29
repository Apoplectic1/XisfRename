using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DirectoryOperations
{
    internal class DirectoryOps
    {
        private static readonly StringCollection log = new StringCollection();
        public static List<System.IO.FileInfo> fiFileList = new List<System.IO.FileInfo>();


        public static void CreateFileInfoDirectoryTree(System.IO.DirectoryInfo root, bool recurse)
        {
            System.IO.FileInfo[] files = null;

            // First, process all the files directly under this folder 
            try
            {
                files = root.GetFiles("*.xisf");
            }
            // This is thrown if even one of the files requires permissions greater 
            // than the application provides. 
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse. 
                // You may decide to do something different here. For example, you 
                // can try to elevate your privileges and access the file again.
                log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            foreach (System.IO.FileInfo fi in files)
            {
                // In this example, we only access the existing FileInfo object. If we 
                // want to open, delete or modify the file, then 
                // a try-catch block is required here to handle the case 
                // where the file has been deleted since the call to TraverseTree().

                fiFileList.Add(fi);
            }

            if (!recurse) return;

            // Now find all the subdirectories under this directory.
            System.IO.DirectoryInfo[] subDirs = root.GetDirectories();

            foreach (System.IO.DirectoryInfo dirInfo in subDirs)
            {
                // Resursive call for each subdirectory.
                CreateFileInfoDirectoryTree(dirInfo, recurse);
            }
        }
    }
}
