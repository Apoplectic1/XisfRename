using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XisfFileManager.FileOperations;

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
            // Added fileExposureTime Time to further refine duplicates. This is due manually setting capture time to make old files process properly

            foreach (var item in fileList)
            {
                item.Unique = true;
            }

            // Group the list by mExposure and mDateTime
            var duplicates = fileList.GroupBy(item => new { item.Exposure, item.CaptureDateTime, item.FrameType, item.Binning, item.Filter, item.Camera })
                                  .Where(group => group.Count() > 1)
                                  .SelectMany(group => group);

            // Mark duplicate items
            foreach (var item in duplicates)
            {
                item.Unique = false;
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
                targetName = mFile.Target;
                frameType = mFile.FrameType;

                if (targetName.Contains("Master"))
                {
                    mFile.KeywordData.TotalFrames(true);

                    if (frameType.Contains("Light"))
                    {
                        newName = targetName + "  Integration  L-" + mFile.Filter + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";

                        newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                        newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C";

                        if (mFile.KeywordData.Rejection() != string.Empty)
                            newName += "  (" + mFile.KeywordData.Rejection().Replace("'", "") + "  " + mFile.CaptureDateTime.ToString("yyyy-MM-dd");
                        else
                            newName += "  (" + mFile.CaptureDateTime.ToString("yyyy-MM-dd");

                        if (mFile.CaptureSoftware != string.Empty)
                        {
                            newName += "  " + mFile.CaptureSoftware;
                        }

                        newName += ")";
                    }

                    if (frameType.Contains("Dark"))
                    {
                        newName += "Dark  " + mFile.CaptureDateTime.ToString("yyyy-MM-dd") + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";

                        newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                        newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C";
                    }

                    if (frameType.Contains("Bias"))
                    {
                        newName += "Bias  " + mFile.CaptureDateTime.ToString("yyyy-MM-dd") + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";

                        newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                        newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C";
                    }

                    if (frameType.Contains("Flat"))
                    {
                        newName += "Flat " + mFile.Filter + "  " + mFile.CaptureDateTime.ToString("yyyy-MM-dd") + "  ";

                        if (mFile.KeywordData.TotalFrames().ToString("D3") != string.Empty)
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "x" + mFile.KeywordData.TotalFrames() + "  ";
                        else
                            newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";

                        newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                        newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C  ";

                        newName += mFile.Telescope + "@";
                        newName += mFile.FocalLength.ToString("F0");

                        if (mFile.FocuserPosition != -1.0)
                        {
                            newName += "  F" + mFile.FocuserPosition.ToString("D5");
                        }

                        if (mFile.RotatorAngle != double.MinValue)
                        {
                            newName += "  R" + mFile.RotatorAngle.FormatRotationAngle();
                        }
                    }

                    newName += "  (";

                    if (mFile.KeywordData.Rejection() != string.Empty)
                    {
                        newName += mFile.KeywordData.Rejection().Replace("'", "");

                        if (mFile.CaptureSoftware != string.Empty)
                            newName += "  " + mFile.CaptureSoftware;
                    }
                    else
                    {
                        if (mFile.CaptureSoftware != string.Empty)
                            newName += mFile.CaptureSoftware;
                    }

                    newName += ")  ";
                }

                return newName;
            }

            // *************************************************************************************************************

            if (mFile.FrameType == "Dark")
            {
                newName = index.ToString("D3") + " ";
                newName += " Dark  ";
                newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";
                newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C";

                newName += "  (" + mFile.CaptureDateTime.ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.CaptureSoftware;
                newName += ")";

                return newName;
            }

            if (mFile.FrameType == "Bias")
            {
                newName = index.ToString("D4") + " ";
                newName += " Bias  ";
                newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";
                newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C";

                newName += "  (" + mFile.CaptureDateTime.ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.CaptureSoftware;
                newName += ")";

                return newName;
            }

            if (mFile.FrameType == "Flat")
            {
                newName = index.ToString("D3") + " ";
                newName += " F-" + mFile.Filter + "  ";

                newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";
                newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C  ";

                newName += mFile.Telescope + "@";
                newName += mFile.FocalLength.ToString("F0");

                if (mFile.AmbientTemperature != -273)
                    newName += mFile.AmbientTemperature.FormatTemperature() + "C  ";
                else
                    newName += "  ";

                if ((mFile.FocuserPosition != int.MinValue) && mFile.FocuserTemperature != -273.0)
                {
                    newName += "F" + mFile.FocuserPosition.ToString("D5") + "@" + mFile.FocuserTemperature.FormatTemperature() + "C";
                }

                if (mFile.RotatorAngle != int.MinValue)
                    newName += "  R" + mFile.RotatorAngle.FormatRotationAngle() + "  ";
                else
                    newName += "  ";


                newName += "(" + mFile.CaptureDateTime.ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.CaptureSoftware;
                newName += ")";

                return newName;
            }

            if (mFile.FrameType == "Light")
            {
                switch (RenameOrder)
                {
                    case OrderType.INDEX:
                        newName = index.ToString("D3") + " ";
                        break;

                    case OrderType.INDEXWEIGHT:
                        if (mFile.SSWeight < 0)
                        {
                            newName = index.ToString("D3") + " ";
                        }
                        else
                        {
                            newName = index.ToString("D3") + " " + Convert.ToInt32(mFile.SSWeight * 1000.0).ToString("D4") + " ";
                        }
                        break;
                    case OrderType.WEIGHT:
                        if (Double.IsNaN(mFile.SSWeight))
                        {
                            newName = index.ToString("D3") + " ";
                        }
                        else
                        {
                            newName = Convert.ToInt32(mFile.SSWeight * 1000.0).ToString("D4") + " ";
                        }
                        break;
                    case OrderType.WEIGHTINDEX:
                        if (Double.IsNaN(mFile.SSWeight))
                        {
                            newName = index.ToString("D3") + " ";
                        }
                        else
                        {
                            newName = Convert.ToInt32(mFile.SSWeight * 1000.0).ToString("D4") + " " + index.ToString("D3") + " ";
                        }
                        break;
                }

                newName += " " + mFile.Target + "  L-" + mFile.Filter + "  ";

                newName += mFile.Exposure.FormatExposureTime() + "x" + mFile.Binning + "  ";
                newName += mFile.Camera + "G" + mFile.Gain.ToString("D3") + "O" + mFile.Offset;
                newName += "@" + mFile.SensorTemperature.FormatTemperature() + "C  ";

                newName += mFile.Telescope + "@";
                newName += mFile.FocalLength.ToString("F0");
                newName += mFile.AmbientTemperature.FormatTemperature() + "C  ";

                newName += "F" + mFile.FocuserPosition.ToString("D5") + "@" + mFile.FocuserTemperature.FormatTemperature() + "C";
                if (mFile.RotatorAngle != double.MinValue)
                    newName += "  R" + mFile.RotatorAngle.FormatRotationAngle() + "  ";
                else
                    newName += "  ";

                newName += "(" + mFile.CaptureDateTime.ToString("yyyy-MM-dd  hh-mm-ss tt") + "  ";
                newName += mFile.CaptureSoftware;
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
