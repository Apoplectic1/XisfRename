using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XisfFileManager;
using XisfFileManager.FileOperations;


namespace XisfFileManager
{
    public class FrameType
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Camera { get; set; }
        public string Filter { get; set; }
        public string Exposure { get; set; }

        public string CDARK { get; set; }
        public string CFLAT { get; set; }
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


        private List<DateTime> mDateList;
        private List<string> mCameraList;
        private List<string> mExposureList;
        private List<string> mFilterList;

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

            mDateList = new List<DateTime>();
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

            mCameraList = new List<string>();
            mFilterList = new List<string>();
            mExposureList = new List<string>();
        }


        public int FindCalibrationFrames(List<XisfFile> targetFileList)
        {
            int progress = 0;

            AddTargetFrameParameters(targetFileList);


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

        private void AddTargetFrameParameters(List<XisfFile> targetFileList)
        {
            // Make a list of unique Parameters
            mCameraList.Clear();
            mFilterList.Clear();
            mDateList.Clear();
            mExposureList.Clear();

            foreach (var frame in targetFileList)
            {
                mCameraList.Add(frame.Camera);
                mFilterList.Add(frame.Filter);
                mExposureList.Add(frame.Exposure);
                mDateList.Add(frame.KeywordData.CaptureDateTime().Date);
            }

            mCameraList = mCameraList.Distinct().ToList();
            mFilterList = mFilterList.Distinct().ToList();
            mExposureList = mExposureList.Distinct().ToList();
            mDateList = mDateList.Distinct().ToList();
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
                                                diff = Math.Abs(calibrationFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Date.Ticks);
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
                                                diff = Math.Abs(calibrationFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Date.Ticks);
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

            XisfFile nearestDarkCalibrationFile;
            XisfFile nearestFlatCalibrationFile;
            XisfFile nearestBiasCalibrationFile;

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

                if (nearestFlatCalibrationFile != null)
                    mFlatFileList.Add(nearestFlatCalibrationFile);
            }

            mDarkFileList = mDarkFileList.Distinct().ToList();
            mFlatFileList = mFlatFileList.Distinct().ToList();
            mBiasFileList = mBiasFileList.Distinct().ToList();

            long min;
            long diff;
            int nearestDarkIndex;
            int darkIndex;
            int flatIndex;
            int nearestFlatIndex;


            // Darks
            foreach (var targetFile in targetFileList)
            {
                min = long.MaxValue;
                nearestDarkIndex = 0;
                darkIndex = 0;

                foreach (var darkFile in mDarkFileList)
                {
                    darkIndex++;
                    if (darkFile.FrameType == "Dark")
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
                                            double calibrationTemperture, targetTemperature;
                                            darkStatus = double.TryParse(darkFile.Temperature, out calibrationTemperture);
                                            targetStatus = double.TryParse(targetFile.Temperature, out targetTemperature);

                                            marginTemperature = Math.Abs(targetTemperature * 0.1);

                                            if (Math.Abs(calibrationTemperture - targetTemperature) < marginTemperature)
                                            {
                                                diff = Math.Abs(darkFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Date.Ticks);
                                                if (diff < min)
                                                {
                                                    min = diff;
                                                    nearestDarkFile = darkFile;
                                                    nearestDarkIndex = darkIndex;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (nearestDarkIndex != 0)
                {
                    nearestDarkFile.CDARK = "D" + nearestDarkIndex.ToString();
                    nearestDarkFile.KeywordData.AddKeyword("CDARK", "D" + nearestDarkIndex.ToString());

                    targetFile.CDARK = "D" + nearestDarkIndex.ToString();
                    targetFile.KeywordData.AddKeyword("CDARK", "D" + nearestDarkIndex.ToString());
                }
            }


            // Flats 
            foreach (var targetFile in targetFileList)
            {
                min = long.MaxValue;
                nearestFlatIndex = 0;
                flatIndex = 0;

                foreach (var flatFile in mFlatFileList)
                {
                    flatIndex++;
                    if (flatFile.FrameType == "Flat")
                    {
                        if (flatFile.Camera == targetFile.Camera)
                        {
                            if (flatFile.Binning == targetFile.Binning)
                            {
                                marginGain = targetFile.Gain * 0.1;

                                if (Math.Abs(flatFile.Gain - targetFile.Gain) < marginGain)
                                {
                                    marginOffset = targetFile.Offset * 0.1;

                                    if (Math.Abs(flatFile.Offset - targetFile.Offset) < marginOffset)
                                    {
                                        double calibrationTemperture, targetTemperature;
                                        flatStatus = double.TryParse(flatFile.Temperature, out calibrationTemperture);
                                        targetStatus = double.TryParse(targetFile.Temperature, out targetTemperature);

                                        marginTemperature = Math.Abs(targetTemperature * 0.1);

                                        if (Math.Abs(calibrationTemperture - targetTemperature) < marginTemperature)
                                        {
                                            diff = Math.Abs(flatFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Date.Ticks);
                                            if (diff < min)
                                            {
                                                min = diff;
                                                nearestFlatFile = flatFile;
                                                nearestFlatIndex = flatIndex;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (nearestFlatIndex != 0)
                {
                    nearestFlatFile.CFLAT = "F" + nearestFlatIndex.ToString();
                    nearestFlatFile.KeywordData.AddKeyword("CFLAT", "F" + nearestFlatIndex.ToString());

                    targetFile.CFLAT = "F" + nearestFlatIndex.ToString();
                    targetFile.KeywordData.AddKeyword("CFLAT", "F" + nearestFlatIndex.ToString());
                }
            }
        }

        public bool CreateTargetCalibrationDirectory(List<XisfFile> targetFileList)
        {
            string directoryName = Path.GetDirectoryName(targetFileList[0].SourceFileName);
            directoryName = Path.GetFullPath(Path.Combine(directoryName, @"..\") + @"\Calibration");

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


            foreach (XisfFile file in mCalibrationFileList)
            {
                //XisfFileUpdate.UpdateFile(file, SubFrameLists);

                //Application.DoEvents();
            }

            return true;

        }
    }
}