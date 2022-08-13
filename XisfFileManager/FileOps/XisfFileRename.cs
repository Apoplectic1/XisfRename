using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace XisfFileManager.FileOperations
{
    public class XisfFileRename
    {
        private int mDupIndex;

        public List<XisfFile> mFileList;
        public enum OrderType { WEIGHTINDEX, INDEXWEIGHT, WEIGHT, INDEX }
        public OrderType RenameOrder;

        private string RecurseDupFileName(string dupFileName)
        {
            if (File.Exists(dupFileName) == true)
            {
                int lastParen = dupFileName.LastIndexOf(')');
                dupFileName = dupFileName.Insert(lastParen + 1, "-Dup");

                dupFileName = RecurseDupFileName(dupFileName);
            }
            return dupFileName;
        }

        public Tuple<int, string> RenameFile(int index, XisfFile file)
        {
            try
            {
                string newFileName;
                string dupFileName;
                string sourceFilePath;

                sourceFilePath = Path.GetDirectoryName(file.SourceFileName);

                // Actually rename the file
                if (file.Unique == true)
                {
                    newFileName = BuildFileName(index, file);
                    int lastParen = newFileName.LastIndexOf(')');
                    newFileName = newFileName.Remove(lastParen);
                    newFileName += ").xisf";

                    // Rename the file if its name actually changed and the new file name doen't already exist
                    if (file.SourceFileName != sourceFilePath + "\\" + newFileName)
                    {
                        if (File.Exists(sourceFilePath + "\\" + newFileName) == false)
                        {
                            File.Move(file.SourceFileName, sourceFilePath + "\\" + newFileName);
                        }
                    }
                    return new Tuple<int, string>(1, newFileName);
                }
                else
                {
                    Directory.CreateDirectory(sourceFilePath + "\\Duplicates");

                    dupFileName = BuildFileName(index - 1, file);
                    int lastParen = dupFileName.LastIndexOf(')');
                    dupFileName = dupFileName.Remove(lastParen);
                    dupFileName += ").xisf";

                    dupFileName = RecurseDupFileName(sourceFilePath + "\\Duplicates\\" + dupFileName);

                    File.Move(file.SourceFileName, dupFileName);

                    return new Tuple<int, string>(0, dupFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), " RenameFiles(List<XisfFile.XisfFile> fileList)");
                return new Tuple<int, string>(-1, "");
            }
        }

        public void MarkDuplicates(List<XisfFile> fileList)
        {
            // Duplicates are files with identical image capture times
            DateTime entryDateTime = DateTime.Now;

            foreach (var entry in fileList)
            {
                // Only mark a DateTime once
                if (entry.KeywordData.CaptureDateTime() == entryDateTime)
                {
                    entry.Unique = false;
                    continue;
                }

                entry.Unique = true;
                entryDateTime = entry.KeywordData.CaptureDateTime();
            }
        }

        private string BuildFileName(int index, XisfFile mFile)
        {
            string newName = string.Empty;
            string targetName;
            string frameType;

            if (mFile.Master)
            {
                newName = "Master ";
                targetName = mFile.KeywordData.TargetName();
                frameType = mFile.KeywordData.FrameType();

                if (targetName.Contains("Master"))
                {
                    mFile.KeywordData.TotalFrames(true);

                    if (frameType.Contains("Light"))
                    {
                        newName = targetName + "  Integration  L-" + mFile.KeywordData.FilterName() + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";

                        newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                        newName += "@" + mFile.KeywordData.SensorTemperature() + "C";

                        if (mFile.KeywordData.Rejection() != string.Empty)
                            newName += "  (" + mFile.KeywordData.Rejection().Replace("'", "") + "  " + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd");
                        else
                            newName += "  (" + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd");

                        if (mFile.KeywordData.CaptureSoftware() != string.Empty)
                        {
                            newName += "  " + mFile.KeywordData.CaptureSoftware();
                        }

                        newName += ")";
                    }

                    if (frameType.Contains("Dark"))
                    {
                        newName += "Dark  " + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd") + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";

                        newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                        newName += "@" + mFile.KeywordData.SensorTemperature() + "C";
                    }

                    if (frameType.Contains("Bias"))
                    {
                        newName += "Bias  " + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd") + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";

                        newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                        newName += "@" + mFile.KeywordData.SensorTemperature() + "C";
                    }

                    if (frameType.Contains("Flat"))
                    {
                        newName += "Flat " + mFile.KeywordData.FilterName() + "  " + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd") + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";

                        newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                        newName += "@" + mFile.KeywordData.SensorTemperature() + "C  ";

                        newName += mFile.KeywordData.Telescope() + "@";
                        newName += mFile.KeywordData.FocalLength();

                        string focusPosition = (string)mFile.KeywordData.FocuserPosition(Keywords.Keyword.eType.STRING);
                        if (!string.IsNullOrEmpty(focusPosition))
                        {
                            newName += "  F" + (string)mFile.KeywordData.FocuserPosition(Keywords.Keyword.eType.STRING);
                        }

                        string imageAngle = mFile.KeywordData.ImageAngle();
                        if (!string.IsNullOrEmpty(imageAngle))
                        {
                            newName += "  R" + mFile.KeywordData.ImageAngle();
                        }
                    }

                    newName += "  (";

                    if (mFile.KeywordData.Rejection() != string.Empty)
                    {
                        newName += mFile.KeywordData.Rejection().Replace("'", "");

                        if (mFile.KeywordData.CaptureSoftware() != string.Empty)
                            newName += "  " + mFile.KeywordData.CaptureSoftware();
                    }
                    else
                    {
                        if (mFile.KeywordData.CaptureSoftware() != string.Empty)
                            newName += mFile.KeywordData.CaptureSoftware();
                    }
                    
                    newName += ")  ";
                }

                return newName;
            }

            // *************************************************************************************************************

            if (mFile.KeywordData.FrameType() == "Dark")
            {
                newName = index.ToString("D3") + " ";
                newName += " Dark  ";
                newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";
                newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                newName += "@" + mFile.KeywordData.SensorTemperature() + "C";

                newName += "  (" + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.KeywordData.CaptureSoftware();
                newName += ")";

                return newName;
            }

            if (mFile.KeywordData.FrameType() == "Bias")
            {
                newName = index.ToString("D3") + " ";
                newName += " Bias  ";
                newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";
                newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                newName += "@" + mFile.KeywordData.SensorTemperature() + "C";

                newName += "  (" + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.KeywordData.CaptureSoftware();
                newName += ")";

                return newName;
            }

            if (mFile.KeywordData.FrameType() == "Flat")
            {
                newName = index.ToString("D3") + " ";
                newName += " F-" + mFile.KeywordData.FilterName() + "  ";

                newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";
                newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                newName += "@" + mFile.KeywordData.SensorTemperature() + "C  ";

                newName += mFile.KeywordData.Telescope() + "@";
                newName += mFile.KeywordData.FocalLength();

                if (mFile.KeywordData.AmbientTemperature() != null && mFile.KeywordData.AmbientTemperature() != "")
                    newName += mFile.KeywordData.AmbientTemperature() + "C  ";
                else
                    newName += "  ";

                if (mFile.KeywordData.FocuserPosition(Keywords.Keyword.eType.STRING) != null && (string)mFile.KeywordData.FocuserPosition(Keywords.Keyword.eType.STRING) != string.Empty)
                {
                    newName += "F" + (string)mFile.KeywordData.FocuserPosition(Keywords.Keyword.eType.STRING) + "@" + mFile.KeywordData.FocuserTemperature() + "C";
                    if (mFile.KeywordData.ImageAngle() != string.Empty)
                        newName += "  R" + mFile.KeywordData.ImageAngle() + "  ";
                    else
                        newName += "  ";
                }

                newName += "(" + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.KeywordData.CaptureSoftware();
                newName += ")";

                return newName;
            }

            if (mFile.KeywordData.FrameType() == "Light")
            {
                switch (RenameOrder)
                {
                    case OrderType.INDEX:
                        newName = index.ToString("D3") + " ";
                        break;

                    case OrderType.INDEXWEIGHT:
                        if (Double.IsNaN(mFile.KeywordData.SSWeight()))
                        {
                            newName = index.ToString("D3") + " ";
                        }
                        else
                        {
                            newName = index.ToString("D3") + " " + Convert.ToInt32(mFile.KeywordData.SSWeight() * 1000.0).ToString("D4") + " ";
                        }
                        break;
                    case OrderType.WEIGHT:
                        if (Double.IsNaN(mFile.KeywordData.SSWeight()))
                        {
                            newName = index.ToString("D3") + " ";
                        }
                        else
                        {
                            newName = Convert.ToInt32(mFile.KeywordData.SSWeight() * 1000.0).ToString("D4") + " ";
                        }
                        break;
                    case OrderType.WEIGHTINDEX:
                        if (Double.IsNaN(mFile.KeywordData.SSWeight()))
                        {
                            newName = index.ToString("D3") + " ";
                        }
                        else
                        {
                            newName = Convert.ToInt32(mFile.KeywordData.SSWeight() * 1000.0).ToString("D4") + " " + index.ToString("D3") + " ";
                        }
                        break;
                }

                newName += " " + mFile.KeywordData.TargetName() + "  L-" + mFile.KeywordData.FilterName() + "  ";

                newName += mFile.KeywordData.ExposureSeconds() + "x" + mFile.KeywordData.Binning() + "  ";
                newName += mFile.KeywordData.Camera() + "G" + mFile.KeywordData.Gain().ToString("D3") + "O" + mFile.KeywordData.Offset();
                newName += "@" + mFile.KeywordData.SensorTemperature() + "C  ";

                newName += mFile.KeywordData.Telescope() + "@";
                newName += mFile.KeywordData.FocalLength();
                newName += mFile.KeywordData.AmbientTemperature() + "C  ";

                newName += "F" + (string)mFile.KeywordData.FocuserPosition(Keywords.Keyword.eType.STRING) + "@" + mFile.KeywordData.FocuserTemperature() + "C";
                if (mFile.KeywordData.ImageAngle() != string.Empty)
                    newName += "  R" + mFile.KeywordData.ImageAngle() + "  ";
                else
                    newName += "  ";

                newName += "(" + mFile.KeywordData.CaptureDateTime().ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.KeywordData.CaptureSoftware();
                newName += ")";

                return newName;
            }

            return index.ToString("D3") + " " + "Rename Failed";
        }

        private void MoveDuplicates(XisfFile currentFile, string sourceFilePath, string newFileName)
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
