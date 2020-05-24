using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace XisfFileManager.XisfFileOperations
{
    public class XisfFileRename
    {
        public bool mBChronological { get; set; } = false;
        private int mDupIndex;
        public bool mBSsWeight { get; set; } = true;
        public bool mKeepIndex { get; set; } = true;
        public bool mWeightFirst { get; set; } = true;
        public bool mIndexFirst { get; set; } = false;
        public List<XisfFile.XisfFile> mFileList;

        public void RenameFiles(XisfFile.XisfFile file)
        {
            try
            {
                string newFileName;
                int index = 1;
                int mDupIndex = 1;
                string sourceFilePath;

                sourceFilePath = Path.GetDirectoryName(file.SourceFileName);

                newFileName = BuildFileName(index, file);
                newFileName += ".xisf";

                // Actually rename the file
                if (file.Unique == true)
                {
                    if (File.Exists(sourceFilePath + "\\" + newFileName) == false)
                    {
                        File.Move(file.SourceFileName, sourceFilePath + "\\" + newFileName);
                    }
                    index++;
                }
                else
                {
                    Directory.CreateDirectory(sourceFilePath + "\\Duplicates");

                    int lastParen = newFileName.LastIndexOf(')');
                    newFileName = newFileName.Insert(lastParen + 1, "-" + mDupIndex.ToString());

                    File.Move(file.SourceFileName, sourceFilePath + "\\Duplicates\\" + newFileName);
                    mDupIndex++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), " RenameFiles(List<XisfFile.XisfFile> fileList)");
                return;
            }
        }

        public void MarkDuplicates(List<XisfFile.XisfFile> fileList)
        {
            // Duplicates are files with identical image capture times
            DateTime entryDateTime = DateTime.Now;

            foreach (var entry in fileList)
            {
                // Only mark a DateTime once
                if (entry.KeywordData.CaptureDateTime() == entryDateTime)
                {
                    continue;
                }

                entry.Unique = true;
                entryDateTime = entry.KeywordData.CaptureDateTime();
            }
        }

        private string BuildFileName(int index, XisfFile.XisfFile mFile)
        {
            string newName;

            if ((mFile.KeywordData.SSWeight() == int.MinValue) || (mBChronological == true))
            {
                newName = index.ToString("D3") + " ";
            }
            else
            {
                string fileName = Path.GetFileName(mFile.SourceFileName);

                bool hasIndex = fileName.Substring(0, 2).All(char.IsDigit);

                if (mKeepIndex && hasIndex)
                {
                    if (mWeightFirst)
                    {
                        newName = mFile.KeywordData.SSWeight() + " " + fileName.Substring(0, 4);
                    }
                    else
                    {
                        newName = fileName.Substring(0, 4) + mFile.KeywordData.SSWeight() + " ";
                    }
                }

                else
                {
                    newName = mFile.KeywordData.SSWeight() + " ";
                }

            }

            newName += mFile.KeywordData.TargetName() + " ";
            newName += mFile.KeywordData.FocalLength();
            newName += mFile.KeywordData.AmbientTemperature() + "C ";

            if (mFile.KeywordData.FrameType() == "Dark")
            {
                newName += mFile.KeywordData.FrameType() + " ";
            }
            else
            {
                newName += mFile.KeywordData.FrameType() + "-" + mFile.KeywordData.FilterName() + " ";
            }

            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + " ";
            newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain() + "O" + mFile.KeywordData.Offset();

            if (mFile.KeywordData.FrameType() == "Dark")
            {
                newName += "@" + mFile.KeywordData.SensorTemperature() + "C ";
            }
            else
            {
                newName += "@" + mFile.KeywordData.SensorTemperature() + "C" + " F" + mFile.KeywordData.FocusPosition() + "@" + mFile.KeywordData.FocusTemperature() + "C";
                if (mFile.KeywordData.ImageAngle() != string.Empty)
                {
                    newName += " R" + mFile.KeywordData.ImageAngle();
                }
                else
                {
                    newName += " ";
                }
            }

            newName += " (" + mFile.KeywordData.ImageDateTime() + " ";

            if (mFile.KeywordData.CaptureSoftware() == "SGP")
            {
                newName += "SGP";// + file.SgpProfile();
            }

            if (mFile.KeywordData.CaptureSoftware() == "TheSkyX")
            {
                newName += "TSX";
            }

            newName += ")";

            return newName;
        }
        private void MoveDuplicates(XisfFile.XisfFile currentFile, string sourceFilePath, string newFileName)
        {
            mDupIndex = 1;

            foreach (var entry in mFileList)
            {
                mDupIndex++;

                Directory.CreateDirectory(sourceFilePath + "\\" + "Duplicates");

                int last = currentFile.SourceFileName.LastIndexOf(@"\");

                // Remove the index or SSWEIGHT from the file name (the first four characters) and postfix duplicate index
                string duplicateFileName = newFileName.Remove(0, 4).Insert(0, mDupIndex.ToString("D3") + " ");

                File.Move(entry.SourceFileName, sourceFilePath + "\\" + "Duplicates" + "\\" + duplicateFileName);
                mFileList.Remove(entry);
            }
        }

        private bool IsFileLocked(string path)
        {
            FileInfo file = new FileInfo(path);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
