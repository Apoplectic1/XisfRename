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
    // ******************************************************************************************************************
    // ******************************************************************************************************************
    public class Calibration
    {
        private XisfFile mFile;
        private List<XisfFile> mCalibrationLibraryFileList;
        private readonly XisfFileRead mFileReader;

        private DirectoryOps mDirectoryOps;

        public DirectoryOps.CameraType Camera { get; set; } = DirectoryOps.CameraType.ALL;
        public DirectoryOps.FileType File { get; set; } = DirectoryOps.FileType.MASTERS;
        public DirectoryOps.FilterType Filter { get; set; } = DirectoryOps.FilterType.ALL;
        public DirectoryOps.FrameType Frame { get; set; } = DirectoryOps.FrameType.ALL;
        public List<XisfFile> CalibrationFiles { get { return mCalibrationLibraryFileList; } }
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

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        // Constructor
        // ******************************************************************************************************************
        public Calibration()
        {
            mCalibrationLibraryFileList = new List<XisfFile>();
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

        // ******************************************************************************************************************
        // ******************************************************************************************************************
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
                        mCalibrationLibraryFileList.Add(mFile);
                    }
                }

                mCalibrationLibraryFileList.Sort(XisfFile.CaptureTimeComparison);
                mCalibrationLibraryFileList = mCalibrationLibraryFileList.Distinct().ToList();

                mValues.TotalFiles = mCalibrationLibraryFileList.Count;
                CalibrationTabPageEvent.TransmitData(mValues);

                return mCalibrationLibraryFileList.Count;
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

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        private XisfFile MatchCalibrationFile(string frameType, XisfFile targetFile)
        {
            // This routine gets called twice for each target frame
            // The first time the this is used to build non-unique (meaning to find all) lists of target matching calibration files
            // The second time this is used to match unique calibration files with each target file

            // More specifically, the first time this is called, each mXXXFileList (calibration list) contains all calibration type files found in our calibration library
            // The second time, the calibaration list has been reduced to include only calibration files unique to the target List 

            // To cleanly reuse this code, we have to use the margin calculations when differentiating Flats from Darks from Biases. Unfortunate but not really a performance issue...

            XisfFile nearestCalibrationFile;
            double exposureTolerance = 1.0;
            double temperatureTolerance = 1.0;

            long min = long.MaxValue;
            nearestCalibrationFile = (XisfFile)null;

            // To Do
            // Add code to test for Flat rotation angle
            // Add code to inform user of how close the match was for each appropriate match parameter (the ones with margins)

            foreach (var calibrationFile in mCalibrationLibraryFileList)
            {
                bool bIgnoreCalibrationFilter = false;
                bool bIgnoreCalibrationExposure = false;

                if (calibrationFile.FrameType == frameType)
                {
                    if (frameType == "Dark")
                    {
                        exposureTolerance = 0.1;
                        temperatureTolerance = 0.25;
                        bIgnoreCalibrationFilter = true;
                    }
                    else if (frameType == "Flat")
                    {
                        exposureTolerance = 1.0; // Flats are exposure independent; set a big value so margin test never fails
                        temperatureTolerance = 0.25;
                        bIgnoreCalibrationExposure = true;
                    }
                    else if (frameType == "Bias")
                    {
                        exposureTolerance = 1.0; // Biases are exposure independent; set a big value so margin test never fails
                        temperatureTolerance = 0.25;
                        bIgnoreCalibrationExposure = true;
                    }

                    if (calibrationFile.Camera == targetFile.Camera)
                    {
                        if (calibrationFile.Binning == targetFile.Binning)
                        {
                            if ((calibrationFile.Filter == targetFile.Filter) || (bIgnoreCalibrationFilter))
                            {
                                double calibrationExposure = double.Parse(calibrationFile.Exposure);
                                double targetExposure = double.Parse(targetFile.Exposure);
                                double marginExposure = calibrationExposure * exposureTolerance;

                                if ((Math.Abs(calibrationExposure - targetExposure) < marginExposure) || (bIgnoreCalibrationExposure))
                                {
                                    double marginGain = calibrationFile.Gain * 0.1;

                                    if (Math.Abs(calibrationFile.Gain - targetFile.Gain) < marginGain)
                                    {
                                        double marginOffset = calibrationFile.Offset * 0.1;

                                        if (Math.Abs(calibrationFile.Offset - targetFile.Offset) < marginOffset)
                                        {
                                            double calibrationTemperture = double.Parse(calibrationFile.Temperature);
                                            double targetTemperature = double.Parse(targetFile.Temperature);
                                            double marginTemperature = Math.Abs(calibrationTemperture) * temperatureTolerance;

                                            if (Math.Abs(calibrationTemperture - targetTemperature) < marginTemperature)
                                            {
                                                long diff = Math.Abs(calibrationFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Ticks);
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

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public void MatchCalibrationFrames(List<XisfFile> targetFileList)
        {
            // At this point we have lists of target frames (lights) and calibration frames (darks, flats and bias). 
            // All to be found/matched calibration frames are assumed to be masters - This means we can't use these routines for PixInsight WBPP calibration of Flats yet
            // All target frames are assumed to be uncalibrated lights

            // To match each light frame with a corresponding calibration frame:
            //    1. Find all calibration frames for the target set
            //    2. Clean up and consecutuvely number the found calibration frame lists
            //    3. Match and assign the appropriate calibraion frame number to each target

            mDarkFileList.Clear();
            mFlatFileList.Clear();

            // Build lists of all the nearest (by DateTime) Dark and Flat calibration files
            // These lists will initially be the same size as the target list (contains many duplicates) 
            foreach (var targetFile in targetFileList)
            {
                XisfFile nearestDarkCalibrationFile = MatchCalibrationFile("Dark", targetFile);
                XisfFile nearestFlatCalibrationFile = MatchCalibrationFile("Flat", targetFile);

                if (nearestDarkCalibrationFile != null)
                    mDarkFileList.Add(nearestDarkCalibrationFile);

                if (nearestFlatCalibrationFile != null)
                    mFlatFileList.Add(nearestFlatCalibrationFile);
            }

            // Remove all duplicate list elements
            mDarkFileList = mDarkFileList.Distinct().ToList();
            mFlatFileList = mFlatFileList.Distinct().ToList();

            // Bias frames can be matched with short Darks and short Flats
            // This logic is NOT COMPLETE
            mBiasFileList.Clear();

            // Now build a list of nearest Bias files that match the actual flat files
            // This is a stub; to use biases to calibrate flats we need to look at exposure times to decide on using a bias or a dark
            // Again for now, we are assuming that only masters are being used so the bias stuff is not used and should be optimized out
            foreach (var flatFile in mFlatFileList)
            {
                XisfFile nearestBiasCalibrationFile = MatchCalibrationFile("Bias", flatFile);

                if (nearestBiasCalibrationFile != null)
                    mBiasFileList.Add(nearestBiasCalibrationFile);
            }

            // Remove all duplicate list elements
            mBiasFileList = mBiasFileList.Distinct().ToList();


            // Now we have lists of unique nearest calibration frames for the target list
            // We now need to assign each calibration frame to each frame in the target list by type:
            //    1. Number each unique calibration frame type consecutively
            //    2. Do a second matching of the calibration frames with target frames.
            //       Since the calibration frames are unique, we can now assign the matches to the target frames 

            int darkIndex = 1;
            foreach (var darkFile in mDarkFileList)
            {
                // No Flats to associate with calibration frame darks since these are Master Frames
                // How do Bias frames deal with this?
                darkFile.CFLAT = string.Empty;
                darkFile.KeywordData.RemoveKeyword("CFLAT");

                // Consecutively number each dark 
                darkFile.CDARK = "D" + darkIndex.ToString();
                darkFile.KeywordData.AddKeyword("CDARK", darkFile.CDARK);
                darkIndex++;
            }

            int flatIndex = 1;
            foreach (var flatFile in mFlatFileList)
            {
                // No Darks to associate with calibration frame darks since these are Master Frames
                // How do Bias frames deal with this?
                flatFile.CDARK = string.Empty;
                flatFile.KeywordData.RemoveKeyword("CDARK");

                flatFile.CFLAT = "F" + flatIndex.ToString();
                flatFile.KeywordData.AddKeyword("CFLAT", flatFile.CFLAT);
                flatIndex++;
            }


            // Match each unique, numbered, calibration file with it's corresponding targetFile
            // Note that ALL matching criteria used here MUST be identical to the criteria used to initially find and build the calibration frame lists
            foreach (var targetFile in targetFileList)
            {
                XisfFile nearestDarkCalibrationFile = MatchCalibrationFile("Dark", targetFile);
                XisfFile nearestFlatCalibrationFile = MatchCalibrationFile("Flat", targetFile);

                targetFile.KeywordData.AddKeyword("Protected", true);

                targetFile.CDARK = nearestDarkCalibrationFile.CDARK;
                targetFile.KeywordData.AddKeyword("CDARK", nearestDarkCalibrationFile.CDARK);

                targetFile.CFLAT = nearestFlatCalibrationFile.CFLAT;
                targetFile.KeywordData.AddKeyword("CFLAT", nearestFlatCalibrationFile.CFLAT);
            }

            // To Do
            // What do we do with Bias frames?
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
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

        // ******************************************************************************************************************
        // ******************************************************************************************************************
    }
}