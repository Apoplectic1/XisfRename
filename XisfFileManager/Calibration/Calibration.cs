using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XisfFileManager.FileOperations;

namespace XisfFileManager.Calibration
{
    internal class Calibration
    {
        private XisfFile mFile;
        public List<XisfFile> mFileList;
        private XisfFileRead mReader;
        private readonly XisfFileRead mFileReader = new XisfFileRead();

        void MasterFileList(bool bRecurse)
        {
            try
            {
                DirectoryInfo diDirectoryTree = new DirectoryInfo(@"E:\Photography\Astro Photography\Calibration\");
                DirectoryOps.DirectoryOps.RecuseXisfFiles(diDirectoryTree, bRecurse, DirectoryOps.DirectoryOps.FileType.MastersOnly);

                //Label_Calibration_ReadFileName.Text = "Reading " + DirectoryOps.DirectoryOps.fiFileList.Count.ToString() + " Master Files";

                //ProgressBar_FileSelection_OverAll.Value = 0;

                if (DirectoryOps.DirectoryOps.fiFileList.Count == 0)
                {
                    MessageBox.Show("No Master .xisf Files Found", "Select Calibration Folder");
                    return;
                }

                foreach (FileInfo file in DirectoryOps.DirectoryOps.fiFileList)
                {
                    bool bStatus = false;
                    //ProgressBar_FileSelection_OverAll.Value += 1;
                    Application.DoEvents();

                    // Create a new xisf file instance
                    mFile = new XisfFile
                    {
                        SourceFileName = file.FullName
                    };

                    //Label_FileSelection_BrowseFileName.Text = Path.GetDirectoryName(file.FullName) + "\n" + Path.GetFileName(file.FullName);

                    // Get the keyword data contained within the current file (mFile)
                    // The keyword data is copied to and fills out the Keyword Class. The Keyword Class is an instance in mFile and specific to that file.
                    //
                    // FileSubFrameKeywordLists
                    // This set of lists will conatin the data initially supplied by PixInsight's SubFrame Selector in the form of an exported .csv file.
                    // This set of lists is not in mFile; rather it is global since it has data for each of the .xisf files that are read.
                    // Once an exported subframe Selector .csv file is read, the file specific data will be added to the mFile keywords and saved by clicking the "Update" button.
                    // If we are reading an .xisf file that already has these .csv keyords, add this files's csv specific data to each of FileSubFrameKeywordLists lists.
                    // This list addition happens in read order. Assignement to the correct mFile is based in the FileName list element in FileSubFrameKeywordLists.
                    // If this .csv data doesn't already exist in the current mFile, we will manually add it later by reading a selected .csv file from the UI.
                    //
                    // Note that each list in FileSubFrameKeywordLists contains a Keyword Class element that can be directly used to write keyword data back into an xisf file.
                    // What I mean by this is that FileSubFrameKeywordLists is basically string data and is not in a form easily used for calculations (a major point of this program).


                    bStatus = mFileReader.ReadXisfFile(mFile);

                    // If data was able to be properly read from our current .xisf file, add the current mFile instance to our master list mFileList.
                    if (bStatus)
                    {
                        mFileList.Add(mFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                         "An exception occured during file Browse/Read.\n\n" + ex.ToString(),
                         "\nMainForm.cs Button_Browse_Click()",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);

                //Label_FileSelection_Statistics_Task.Text = "Browse Aborted";
                return;
            }
        }
        void FindUniqueNights(List<XisfFile> mFileList)
        {
            foreach (XisfFile file in mFileList)
            {

                string Text = Path.GetDirectoryName(file.SourceFileName) + "\n" + Path.GetFileName(file.SourceFileName);


            }
        }
    }
}
