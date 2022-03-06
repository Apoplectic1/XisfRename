using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XisfFileManager;
using XisfFileManager.FileOperations;
using XisfFileManager.Keywords;

namespace XisfFileManager
{
    public class FrameType
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Camera { get; set; }
        public string Filter { get; set; }
        public string Exposure { get; set; }

        public string CDARK { get; set; } = string.Empty;
        public string CFLAT { get; set; } = string.Empty;
    }

    public class Calibration
    {
        private XisfFile mFile;
        private List<XisfFile> mCalibrationFileList;
        private readonly XisfFileRead mFileReader;

        private DirectoryOps mDirectoryOps;

        public DirectoryOps.CameraType Camera { get; set; } = DirectoryOps.CameraType.ALL;
        public DirectoryOps.FileType File { get; set; } = DirectoryOps.FileType.MASTERS;
        public DirectoryOps.FilterType Filter { get; set; } = DirectoryOps.FilterType.ALL;
        public DirectoryOps.FrameType Frame { get; set; } = DirectoryOps.FrameType.ALL;
        public List<XisfFile> CalibrationFiles { get { return mCalibrationFileList; } }
        public bool Recurse { get; set; } = true;

        private CalibrationTabPageValues mValues;

        private List<XisfFile> mLumaFileList;
        private List<XisfFile> mRedFileList;
        private List<XisfFile> mGreenFileList;
        private List<XisfFile> mBlueFileList;
        private List<XisfFile> mHaFileList;
        private List<XisfFile> mO3FileList;
        private List<XisfFile> mS2FileList;

        private List<XisfFile> mDarkFileList;
        private List<XisfFile> mFlatFileList;
        private List<XisfFile> mBiasFileList;

        public Calibration()
        {
            mCalibrationFileList = new List<XisfFile>();
            mFileReader = new XisfFileRead();
            mDirectoryOps = new DirectoryOps();
            mValues = new CalibrationTabPageValues();

            mLumaFileList = new List<XisfFile>();
            mRedFileList = new List<XisfFile>();
            mGreenFileList = new List<XisfFile>();
            mBlueFileList = new List<XisfFile>();
            mHaFileList = new List<XisfFile>();
            mO3FileList = new List<XisfFile>();
            mS2FileList = new List<XisfFile>();

            mDarkFileList = new List<XisfFile>();
            mFlatFileList = new List<XisfFile>();
            mBiasFileList = new List<XisfFile>();

        }


        public int FindCalibrationFrames(List<XisfFile> targetFileList)
        {
            int progress = 0;

            //AddTargetFrameParameters(targetFileList);


            try
            {
                DirectoryInfo diDirectoryTree = new DirectoryInfo(@"E:\Photography\Astro Photography\Calibration\");
                mDirectoryOps.ClearFileList();
                mDirectoryOps.Filter = Filter;
                mDirectoryOps.File = File;
                mDirectoryOps.Camera = Camera;
                mDirectoryOps.Frame = Frame;
                mDirectoryOps.Recurse = Recurse;
                mDirectoryOps.RecuseDirectories(diDirectoryTree);

                if (mDirectoryOps.Files.Count == 0)
                {
                    MessageBox.Show("No Master .xisf Files Found", "Select Calibration Folder");
                    return 0;
                }

                int index = 1;
                mValues.TotalFiles = mDirectoryOps.Files.Count;

                foreach (FileInfo file in mDirectoryOps.Files)
                {
                    bool bStatus = false;

                    // Create a new xisf file instance
                    mFile = new XisfFile
                    {
                        SourceFileName = file.FullName
                    };

                    progress = (int)(((double)index++ / (double)mDirectoryOps.Files.Count) * 100.0);

                    mValues.Progress = progress;
                    mValues.FileName = Path.GetDirectoryName(mFile.SourceFileName) + "\n" + Path.GetFileName(mFile.SourceFileName);
                    CalibrationTabPageEvent.TransmitData(mValues);


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
                        mCalibrationFileList.Add(mFile);
                    }
                }


                mCalibrationFileList.Sort(XisfFile.CaptureTimeComparison);
                mCalibrationFileList = mCalibrationFileList.Distinct().ToList();

                mValues.TotalFiles = mCalibrationFileList.Count;
                CalibrationTabPageEvent.TransmitData(mValues);

                return mCalibrationFileList.Count;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                         "An exception occured during file Browse/Read.\n\n" + ex.ToString(),
                         "\nMainForm.cs Button_Browse_Click()",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);

                //Label_FileSelection_Statistics_Task.Text = "Browse Aborted";
                return -1;
            }
        }

        public XisfFile FindNearestCalibrationFile(string type, XisfFile targetFile)
        {
            bool calibrationStatus;
            bool targetStatus;
            double marginExposure = 0;
            double marginGain = 0;
            double marginOffset = 0;
            double marginTemperture = 0;

            XisfFile nearestCalibrationFile = null;
            long min = long.MaxValue;
            long diff;

            if (type == "Dark")
            {
                foreach (var calibrationFile in mCalibrationFileList)
                {
                    if (calibrationFile.Camera == targetFile.Camera)
                    {
                        if (calibrationFile.Binning == targetFile.Binning)
                        {
                            if (calibrationFile.FrameType == "Dark")
                            {
                                double calibrationExposure, targetExposure;
                                calibrationStatus = double.TryParse(calibrationFile.Exposure, out calibrationExposure);
                                targetStatus = double.TryParse(targetFile.Exposure, out targetExposure);

                                marginExposure = targetExposure * 0.1;

                                if (Math.Abs(calibrationExposure - targetExposure) < marginExposure)
                                {
                                    marginGain = targetFile.Gain * 0.1;

                                    if (Math.Abs(calibrationFile.Gain - targetFile.Gain) < marginGain)
                                    {
                                        marginOffset = targetFile.Offset * 0.1;

                                        if (Math.Abs(calibrationFile.Offset - targetFile.Offset) < marginOffset)
                                        {
                                            double calibrationTemperture, targetTemperature;
                                            calibrationStatus = double.TryParse(calibrationFile.Temperature, out calibrationTemperture);
                                            targetStatus = double.TryParse(targetFile.Temperature, out targetTemperature);

                                            marginTemperture = Math.Abs(targetTemperature * 0.1);

                                            if (Math.Abs(calibrationTemperture - targetTemperature) < marginTemperture)
                                            {
                                                diff = Math.Abs(calibrationFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Ticks);
                                                if (diff < min)
                                                {
                                                    min = diff;
                                                    nearestCalibrationFile = calibrationFile;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (type == "Flat")
            {
                foreach (var calibrationFile in mCalibrationFileList)
                {
                    if (calibrationFile.Camera == targetFile.Camera)
                    {
                        if (calibrationFile.Binning == targetFile.Binning)
                        {
                            if (calibrationFile.FrameType == "Flat")
                            {
                                if (calibrationFile.Filter == targetFile.Filter)
                                {
                                    marginGain = targetFile.Gain * 0.1;

                                    if (Math.Abs(calibrationFile.Gain - targetFile.Gain) < marginGain)
                                    {
                                        marginOffset = targetFile.Offset * 0.1;

                                        if (Math.Abs(calibrationFile.Offset - targetFile.Offset) < marginOffset)
                                        {
                                            double calibrationTemperture, targetTemperature;
                                            calibrationStatus = double.TryParse(calibrationFile.Temperature, out calibrationTemperture);
                                            targetStatus = double.TryParse(targetFile.Temperature, out targetTemperature);

                                            marginTemperture = Math.Abs(targetTemperature * 0.1);

                                            if (Math.Abs(calibrationTemperture - targetTemperature) < marginTemperture)
                                            {
                                                diff = Math.Abs(calibrationFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Ticks);
                                                if (diff < min)
                                                {
                                                    min = diff;
                                                    nearestCalibrationFile = calibrationFile;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return nearestCalibrationFile;
        }

        public void MatchCalibrationFrames(List<XisfFile> targetFileList)
        {
            bool targetStatus, darkStatus, flatStatus;
            double marginExposure, marginGain, marginOffset, marginTemperature;
            XisfFile nearestDarkFile = null;
            XisfFile nearestFlatFile = null;
            XisfFile nearestBiasFile = null;

            XisfFile nearestDarkCalibrationFile = null;
            XisfFile nearestFlatCalibrationFile = null;
            XisfFile nearestBiasCalibrationFile = null;

            mDarkFileList.Clear();
            mFlatFileList.Clear();
            mBiasFileList.Clear();

            foreach (var targetFile in targetFileList)
            {
                nearestDarkCalibrationFile = FindNearestCalibrationFile("Dark", targetFile);
                nearestFlatCalibrationFile = FindNearestCalibrationFile("Flat", targetFile);
                nearestBiasCalibrationFile = FindNearestCalibrationFile("Bias", targetFile);

                if (nearestDarkCalibrationFile != null)
                    mDarkFileList.Add(nearestDarkCalibrationFile);

                if (nearestFlatCalibrationFile != null)
                    mFlatFileList.Add(nearestFlatCalibrationFile);

                if (nearestBiasCalibrationFile != null)
                    mBiasFileList.Add(nearestBiasCalibrationFile);
            }

            mDarkFileList = mDarkFileList.Distinct().ToList();
            mFlatFileList = mFlatFileList.Distinct().ToList();
            mBiasFileList = mBiasFileList.Distinct().ToList();

            long min;
            long diff;
            int darkIndex;
            int flatIndex;

            darkIndex = 1;
            foreach (var darkFile in mDarkFileList)
            {
                darkFile.CFLAT = string.Empty;
                darkFile.KeywordData.RemoveKeyword("CFLAT");

                darkFile.CDARK = "D" + darkIndex.ToString();
                darkFile.KeywordData.AddKeyword("CDARK", darkFile.CDARK);
                darkIndex++;
            }

            // Match Each found Dark calibration file with its corresponding targetFile
            // Darks
            foreach (var targetFile in targetFileList)
            {
                nearestDarkFile = null;
                min = long.MaxValue;

                foreach (var darkFile in mDarkFileList)
                {
                    if (darkFile.Camera == targetFile.Camera)
                    {
                        if (darkFile.Binning == targetFile.Binning)
                        {
                            double darkExposure, targetExposure;
                            darkStatus = double.TryParse(darkFile.Exposure, out darkExposure);
                            targetStatus = double.TryParse(targetFile.Exposure, out targetExposure);

                            marginExposure = targetExposure * 0.1;

                            if (Math.Abs(darkExposure - targetExposure) < marginExposure)
                            {
                                marginGain = targetFile.Gain * 0.1;

                                if (Math.Abs(darkFile.Gain - targetFile.Gain) < marginGain)
                                {
                                    marginOffset = targetFile.Offset * 0.1;

                                    if (Math.Abs(darkFile.Offset - targetFile.Offset) < marginOffset)
                                    {
                                        double darkTemperture, targetTemperature;
                                        darkStatus = double.TryParse(darkFile.Temperature, out darkTemperture);
                                        targetStatus = double.TryParse(targetFile.Temperature, out targetTemperature);

                                        marginTemperature = Math.Abs(targetTemperature * 0.1);

                                        if (Math.Abs(darkTemperture - targetTemperature) < marginTemperature)
                                        {
                                            diff = Math.Abs(darkFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Ticks);
                                            if (diff < min)
                                            {
                                                min = diff;
                                                nearestDarkFile = darkFile;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                targetFile.CDARK = nearestDarkFile.CDARK;
                targetFile.KeywordData.AddKeyword("CDARK", targetFile.CDARK);
            }


            flatIndex = 1;
            foreach (var flatFile in mFlatFileList)
            {
                flatFile.CDARK = string.Empty;
                flatFile.KeywordData.RemoveKeyword("CDARK");

                flatFile.CFLAT = "F" + flatIndex.ToString();
                flatFile.KeywordData.AddKeyword("CFLAT", flatFile.CFLAT);
                flatIndex++;
            }

            // Match Each found Flat calibration file with its corresponding targetFile
            // Flats 
            foreach (var targetFile in targetFileList)
            {
                nearestFlatFile = null;
                min = long.MaxValue;

                foreach (var flatFile in mFlatFileList)
                {
                    if (flatFile.Camera == targetFile.Camera)
                    {
                        if (flatFile.Binning == targetFile.Binning)
                        {
                            if (flatFile.Filter == targetFile.Filter)
                            {
                                marginGain = targetFile.Gain * 0.1;

                                if (Math.Abs(flatFile.Gain - targetFile.Gain) < marginGain)
                                {
                                    marginOffset = targetFile.Offset * 0.1;

                                    if (Math.Abs(flatFile.Offset - targetFile.Offset) < marginOffset)
                                    {
                                        double flatTemperture, targetTemperature;
                                        flatStatus = double.TryParse(flatFile.Temperature, out flatTemperture);
                                        targetStatus = double.TryParse(targetFile.Temperature, out targetTemperature);

                                        marginTemperature = Math.Abs(targetTemperature * 0.1);

                                        if (Math.Abs(flatTemperture - targetTemperature) < marginTemperature)
                                        {
                                            diff = Math.Abs(flatFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Ticks);
                                            if (diff < min)
                                            {
                                                min = diff;
                                                nearestFlatFile = flatFile;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                targetFile.CFLAT = nearestFlatFile.CFLAT;
                targetFile.KeywordData.AddKeyword("CFLAT", targetFile.CFLAT);
            }
        }

        public bool CreateTargetCalibrationDirectory(List<XisfFile> targetFileList, SubFrameLists subFrameLists)
        {
            string directoryName = Path.GetDirectoryName(targetFileList[0].SourceFileName);
            directoryName = Path.GetFullPath(Path.Combine(directoryName, @"..\..") + @"\Calibration");

            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName, true);
            }

            Directory.CreateDirectory(directoryName);

            foreach (var file in mDarkFileList)
            {
                FileInfo fileInfo = new FileInfo(file.SourceFileName);
                fileInfo.CopyTo(directoryName + @"\" + Path.GetFileName(file.SourceFileName), true);
            }

            foreach (var file in mFlatFileList)
            {
                FileInfo fileInfo = new FileInfo(file.SourceFileName);
                fileInfo.CopyTo(directoryName + @"\" + Path.GetFileName(file.SourceFileName), true);
            }

            foreach (var file in mBiasFileList)
            {
                FileInfo fileInfo = new FileInfo(file.SourceFileName);
                fileInfo.CopyTo(directoryName + @"\" + Path.GetFileName(file.SourceFileName), true);
            }

            foreach (var file in mDarkFileList)
            {
                file.SourceFileName = directoryName + @"\" + Path.GetFileName(file.SourceFileName);
                XisfFileUpdate.UpdateTargetCalibrationFile(directoryName + @"\" + Path.GetFileName(file.SourceFileName), file, subFrameLists);
            }

            foreach (var file in mFlatFileList)
            {
                file.SourceFileName = directoryName + @"\" + Path.GetFileName(file.SourceFileName);
                XisfFileUpdate.UpdateTargetCalibrationFile(directoryName + @"\" + Path.GetFileName(file.SourceFileName), file, subFrameLists);
            }

            foreach (var file in mBiasFileList)
            {
                file.SourceFileName = directoryName + @"\" + Path.GetFileName(file.SourceFileName);
                XisfFileUpdate.UpdateTargetCalibrationFile(directoryName + @"\" + Path.GetFileName(file.SourceFileName), file, subFrameLists);
            }
            return true;

        }
    }
}