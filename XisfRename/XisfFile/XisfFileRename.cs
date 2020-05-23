using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfRename.XisfFile
{
    public class XisfFileRename
    {
        public bool mBChronological { get; set; } = false;
        private int mDupIndex;
        public bool mBSsWeight { get; set; } = true;
        public bool mKeepIndex { get; set; } = true;
        public bool mWeightFirst { get; set; } = true;
        public bool mIndexFirst { get; set; } = false;
        List<XisfFileRead> mFileList;

        public void RenameFiles(List<XisfFileRead> fileList)
        {
            
            string newFileName;
            int index = 1;
            int mDupIndex = 1;

            mFileList = fileList;


            MarkDuplicates();

            string sourceFilePath;

            foreach (XisfFile.XisfFileRead file in mFileList)
            {
                sourceFilePath = Path.GetDirectoryName(file.SourceFileName);

                newFileName = BuildFileName(index, file);

                newFileName += ".xisf";

                // Actually rename the file
                if (file.Unique == true)
                {
                    if (File.Exists(sourceFilePath + "\\" + newFileName))
                    {
                        //File.Move(file.SourceFileName, sourceFilePath + "\\Renun " + newFileName);
                    }
                    else
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
        }

        private void MarkDuplicates()
        {

            // Duplicates are files with identical image capture times

            DateTime entryDateTime = DateTime.Now;

            //var results = mFileList.GroupBy(x => x.CaptureDateTime).Select(g => g.First()).ToList();

            foreach (var entry in mFileList)
            {
                // Only mark a DateTime once
                if (entry.CaptureDateTime == entryDateTime)
                {
                    continue;
                }

                entry.Unique = true;
                entryDateTime = entry.CaptureDateTime;
            }
        }

        private string BuildFileName(int index, XisfFile.XisfFileRead imageFile)
        {
            string newName;

            if ((imageFile.SubFrameSelectorWeight() == string.Empty) || (mBChronological == true))
            {
                newName = index.ToString("D3") + " ";
            }
            else
            {
                string fileName = Path.GetFileName(imageFile.SourceFileName);

                bool hasIndex = fileName.Substring(0, 2).All(char.IsDigit);

                if (mKeepIndex && hasIndex)
                {
                    if (mWeightFirst)
                    {
                        newName = imageFile.SubFrameSelectorWeight() + " " + fileName.Substring(0, 4);
                    }
                    else
                    {
                        newName = fileName.Substring(0, 4) + imageFile.SubFrameSelectorWeight() + " ";
                    }
                }

                else
                {
                    newName = imageFile.SubFrameSelectorWeight() + " ";
                }

            }

            newName += imageFile.TargetName() + " ";
            newName += imageFile.FocalLength() + " ";
            if (imageFile.FrameType() == "Dark")
            {
                newName += imageFile.FrameType() + " ";
            }
            else
            {
                newName += imageFile.FrameType() + "-" + imageFile.FilterName() + " ";
            }
            newName += imageFile.ExposureSeconds() + "Sx" + imageFile.Binning() + " ";
            newName += imageFile.CameraModel() + "G" + imageFile.CameraGain() + "O" + imageFile.CameraOffset();
            if (imageFile.FrameType() == "Dark")
            {
                newName += "@" + imageFile.SensorTemperature() + "C ";
            }
            else
            {
                newName += "@" + imageFile.SensorTemperature() + "C" + " F" + imageFile.FocusPosition() + "@" + imageFile.FocusTemperature() + "C";
                if (imageFile.ImageAngle() != string.Empty)
                {
                    newName += " R" + imageFile.ImageAngle();
                }
                else
                {
                    newName += " ";
                }
            }


            newName += " (" + imageFile.ImageDateTime() + " ";

            if (imageFile.CaptureSoftware() == "SGP")
            {
                newName += "SGP";// + file.SgpProfile();
            }

            if (imageFile.CaptureSoftware() == "TheSkyX")
            {
                newName += "TSX";
            }

            newName += ")";

            return newName;
        }
        private void MoveDuplicates(XisfFile.XisfFileRead currentFile, string sourceFilePath, string newFileName)
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
