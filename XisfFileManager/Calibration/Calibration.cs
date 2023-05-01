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
        private List<XisfFile> mBiasFileList;
        private List<XisfFile> mDarkFileList;
        private List<XisfFile> mFlatFileList;
        private List<XisfFile> mLibraryCalibrationFileList;
        private List<XisfFile> mTargetCalibrationFileList;
        private List<XisfFile> mUnmatchedBiasTargetFileList;
        private List<XisfFile> mUnmatchedDarkTargetFileList;
        private List<XisfFile> mUnmatchedFlatTargetFileList;
        private TreeNode mDatesTree;
        private readonly CalibrationTabPageValues mCalibrationTabValues;
        private readonly DirectoryOps mDirectoryOps;
        private readonly XisfFileRead mFileReader;

        public bool UpdateAll { get; set; }
        public bool UpdateNew { get; set; }
        public bool ProtectAll { get; set; }
        public double ExposureTolerance { get; set; } = 10;
        public double GainTolerance { get; set; } = 10;
        public double OffsetTolerance { get; set; } = 5;
        public double TemperatureTolerance { get; set; } = 5;
        public double RotationTolerance { get; set; } = 0.1;

        public DirectoryOps.FileType File { get; set; } = DirectoryOps.FileType.MASTERS;
        public DirectoryOps.FilterType Filter { get; set; } = DirectoryOps.FilterType.ALL;
        public DirectoryOps.FrameType Frame { get; set; } = DirectoryOps.FrameType.ALL;
        public List<XisfFile> LibraryCalibrationFileList { get { return mLibraryCalibrationFileList; } set { mLibraryCalibrationFileList = value; } }
        public List<XisfFile> TargetCalibrationFileList { get { return mTargetCalibrationFileList; } set { mTargetCalibrationFileList = value; } }
        public bool Recurse { get; set; } = true;

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        // Constructor
        // ******************************************************************************************************************
        public Calibration()
        {
            mBiasFileList = new List<XisfFile>();
            mLibraryCalibrationFileList = new List<XisfFile>();
            mCalibrationTabValues = new CalibrationTabPageValues();
            mDarkFileList = new List<XisfFile>();
            mDirectoryOps = new DirectoryOps();
            mFileReader = new XisfFileRead();
            mFlatFileList = new List<XisfFile>();
            mUnmatchedDarkTargetFileList = new List<XisfFile>();
            mUnmatchedFlatTargetFileList = new List<XisfFile>();
            mUnmatchedBiasTargetFileList = new List<XisfFile>();
            mTargetCalibrationFileList = new List<XisfFile>();
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public void ResetAll()
        {
            mLibraryCalibrationFileList.Clear();
            mTargetCalibrationFileList.Clear();
            mDarkFileList.Clear();
            mFlatFileList.Clear();
            mBiasFileList.Clear();
            mUnmatchedDarkTargetFileList.Clear();
            mUnmatchedFlatTargetFileList.Clear();
            mUnmatchedBiasTargetFileList.Clear();
            File = DirectoryOps.FileType.MASTERS;
            Filter = DirectoryOps.FilterType.ALL;
            Frame = DirectoryOps.FrameType.ALL;
            ExposureTolerance = 10;
            GainTolerance = 10;
            OffsetTolerance = 10;
            TemperatureTolerance = 25;
        }


        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public void PopulateDatesTree()
        {
            TreeNode node = new TreeNode("No Dates");
            //TreeView_CalibrationTab_Dates.Nodes.Add(node);

        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public List<XisfFile> ReadCalibrationFrames(string sCalibrationFrameDirectory)
        {
            List<XisfFile> calibrationFileList = new List<XisfFile>();

            mCalibrationTabValues.MessageMode = CalibrationTabPageValues.eMessageMode.CLEAR;
            mCalibrationTabValues.Progress = 0;
            CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

            try
            {
                DirectoryInfo diDirectoryTree = new DirectoryInfo(sCalibrationFrameDirectory);
                mDirectoryOps.ClearFileList();
                mDirectoryOps.Filter = Filter;
                mDirectoryOps.File = File;
                mDirectoryOps.Frame = Frame;
                mDirectoryOps.Recurse = Recurse;
                mDirectoryOps.RecuseDirectories(diDirectoryTree);

                if (mDirectoryOps.Files.Count == 0)
                {
                    MessageBox.Show("No Calibration Files Found under: " + diDirectoryTree.ToString(), "Select Calibration Folder");
                    return null;
                }

                mCalibrationTabValues.TotalFiles = mDirectoryOps.Files.Count;

                foreach (FileInfo file in mDirectoryOps.Files)
                {
                    Application.DoEvents();

                    bool bStatus = false;

                    // Create a new xisf file instance for reads
                    XisfFile readFile = new XisfFile
                    {
                        SourceFileName = file.FullName
                    };

                    mCalibrationTabValues.ProgressMax = mDirectoryOps.Files.Count;
                    mCalibrationTabValues.Progress += 1;
                    mCalibrationTabValues.FileName = Path.GetDirectoryName(readFile.SourceFileName) + "\n" + Path.GetFileName(readFile.SourceFileName);
                    CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);


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


                    bStatus = mFileReader.ReadXisfFile(readFile);

                    // If data was able to be properly read from our current .xisf file, add the current mFile instance to our master list mFileList.
                    if (bStatus)
                    {
                        calibrationFileList.Add(readFile);
                    }
                }

                calibrationFileList.Sort(XisfFile.CaptureTimeComparison);
                calibrationFileList = calibrationFileList.Distinct().ToList();

                mCalibrationTabValues.TotalFiles = calibrationFileList.Count;
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                return calibrationFileList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                         "An exception occured during file Browse/Read.\n\n" + ex.ToString(),
                         "\nCalibration.cs Button_Browse_Click()",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);
                return null;
            }
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        private XisfFile MatchNearestCalibrationFile(string frameType, XisfFile targetFile, List<XisfFile> calibrationFileList)
        {
            // This routine gets called twice for each target frame
            // The first time the this is used to build non-unique (meaning to find all) lists of target matching calibration files
            // The second time this is used to match unique calibration files with each target file

            // More specifically, the first time this is called, each mXXXFileList (calibration list) contains all calibration type files found in our calibration library
            // The second time, the calibaration list has been reduced to include only calibration files unique to the target List 

            // To cleanly reuse this code, we have to use the margin calculations when differentiating Flats from Darks from Biases. Unfortunate but not really a performance issue...

            XisfFile nearestCalibrationFile = (XisfFile)null;
            long min = DateTime.MaxValue.Ticks;

            // To Do
            // Add code to test for Flat rotation angle
            // Add code to inform user of how close the match was for each appropriate match parameter (the ones with margins)

            foreach (var calibrationFile in calibrationFileList)
            {
                bool bIgnoreCalibrationFilter = false;
                bool bIgnoreCalibrationExposure = false;

                if (calibrationFile.FrameType == frameType)
                {
                    if (frameType == "Dark")
                    {
                        // This is a DARK frame so we don't need to match target frame filters
                        bIgnoreCalibrationFilter = true;
                    }
                    else if (frameType == "Flat")
                    {
                        // This is a FLAT frame so we don't need to match target exposure times
                        bIgnoreCalibrationExposure = true;
                    }
                    else if (frameType == "Bias")
                    {
                        // This is a BIAS frame so we don't need to match target frame filters
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

                                if ((Math.Abs(calibrationExposure - targetExposure) <= ExposureTolerance) || (bIgnoreCalibrationExposure))
                                {
                                    if (Math.Abs(calibrationFile.Gain - targetFile.Gain) <= GainTolerance)
                                    {
                                        if (Math.Abs(calibrationFile.Offset - targetFile.Offset) <= OffsetTolerance)
                                        {
                                            double calibrationFrameTemperture = double.Parse(calibrationFile.Temperature);
                                            double targetFrameTemperature = double.Parse(targetFile.Temperature);

                                            if (Math.Abs(calibrationFrameTemperture - targetFrameTemperature) <= TemperatureTolerance)
                                            {
                                                long diff = Math.Abs(calibrationFile.KeywordData.CaptureDateTime().Ticks - targetFile.KeywordData.CaptureDateTime().Ticks);
                                                if (diff <= min)
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
        public bool FindTargetCalibrationFrames(List<XisfFile> targetFileList)
        {
            string targetCalibrationDirectory = GetTargetCalibrationFileDirectory(targetFileList[0].SourceFileName);

            mTargetCalibrationFileList.Clear();

            if (Directory.Exists(targetCalibrationDirectory))
            {
                if (Directory.GetFileSystemEntries(targetCalibrationDirectory).Length != 0)
                    // Build a list of existing Calibration Frames
                    mTargetCalibrationFileList = ReadCalibrationFrames(targetCalibrationDirectory);
            }

            // Match each target file with any files found in the target "Calibration" directory to populate the mTargetxxTargetFiles Lists
            // If the Calibration Directpry does not exist or is empty, each mTargetxxxTargetFileList will contain the full target list
            return MatchTargetCalibrationFrames(targetFileList);
        }
        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public bool FindLibraryCalibrationFrames(List<XisfFile> targetFileList)
        {
            mLibraryCalibrationFileList.Clear();

            mLibraryCalibrationFileList = ReadCalibrationFrames(@"E:\Photography\Astro Photography\Calibration");
            //mLibraryCalibrationFileList = ReadCalibrationFrames(@"E:\Temp");

            return MatchLibraryCalibrationFrames(targetFileList);
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public bool MatchTargetCalibrationFrames(List<XisfFile> targetFileList)
        {
            // Find any umatched target frames
            mUnmatchedDarkTargetFileList = targetFileList.Where(p => !mTargetCalibrationFileList.Any(p2 => p2.CDARK == p.CDARK)).ToList();
            mUnmatchedFlatTargetFileList = targetFileList.Where(p => !mTargetCalibrationFileList.Any(p2 => p2.CFLAT == p.CFLAT)).ToList();
            mUnmatchedBiasTargetFileList = targetFileList.Where(p => !mTargetCalibrationFileList.Any(p2 => p2.CBIAS == p.CBIAS)).ToList();

            // If all target frames have corresponding Darks, Flats and Biases, let us know and retun true
            if ((mUnmatchedDarkTargetFileList.Count == 0) && (mUnmatchedFlatTargetFileList.Count == 0))
            {
                mCalibrationTabValues.MessageMode = CalibrationTabPageValues.eMessageMode.CLEAR;
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                mCalibrationTabValues.MessageMode = CalibrationTabPageValues.eMessageMode.APPEND;
                mCalibrationTabValues.TotalMatchedCalibrationFiles = targetFileList.Count();
                mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            All Target Frames Match Existing Target Capture Directory Calibration Files\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return true;
            }

            // We have unmatched target files
            // Each mUnmatchedxxxxFileList conatains the unmatched files by type
            return false;
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public bool MatchLibraryCalibrationFrames(List<XisfFile> targetFileList)
        {
            //int iUnmatchedFileCount = FindUnmatchedTargetFiles(targetFileList);

            // At this point we have lists of target frames (lights) and calibration frames (darks, flats and bias). 
            // All to be found/matched calibration frames are assumed to be masters - This means we can't use these routines for PixInsight WBPP calibration of Flats yet
            // All target frames are assumed to be uncalibrated lights

            // To match each light frame with a corresponding calibration frame:
            //    1. Find all calibration frames for the target set
            //    2. Clean up and consecutuvely number the found calibration frame lists
            //    3. Match and assign the appropriate calibraion frame number to each target

            mDarkFileList.Clear();
            mFlatFileList.Clear();
            bool bAllDarksMatched = true;
            bool bAllFlatsMatched = true;
            
            // Build lists of all the nearest (by DateTime) Dark and Flat calibration files
            // These lists will initially be the same size as the target list (contains many duplicates) 
            foreach (var targetFile in mUnmatchedDarkTargetFileList.ToList())
            {
                XisfFile nearestDarkCalibrationFile = MatchNearestCalibrationFile("Dark", targetFile, mLibraryCalibrationFileList);

                if (nearestDarkCalibrationFile != null)
                    mDarkFileList.Add(nearestDarkCalibrationFile);
                else
                {
                    mUnmatchedDarkTargetFileList.Remove(targetFile);
                    mCalibrationTabValues.MessageMode = CalibrationTabPageValues.eMessageMode.APPEND;
                    mCalibrationTabValues.MatchCalibrationMessage = "No matching Dark for target frame: " + targetFile.SourceFileName + "\r\n";
                    CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                    bAllDarksMatched = false;
                }
            }

            foreach (var targetFile in mUnmatchedFlatTargetFileList.ToList())
            {
                XisfFile nearestFlatCalibrationFile = MatchNearestCalibrationFile("Flat", targetFile, mLibraryCalibrationFileList);

                if (nearestFlatCalibrationFile != null)
                    mFlatFileList.Add(nearestFlatCalibrationFile);
                else
                {
                    mUnmatchedFlatTargetFileList.Remove(targetFile);
                    mCalibrationTabValues.MessageMode = CalibrationTabPageValues.eMessageMode.APPEND;
                    mCalibrationTabValues.MatchCalibrationMessage = "No matching Flat for target frame: " + targetFile.SourceFileName + "\r\n";
                    CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                    bAllFlatsMatched = false;
                }
            }

            // Sort and Remove all duplicate list elements
            mDarkFileList.Sort(XisfFile.CaptureTimeComparison);
            mFlatFileList.Sort(XisfFile.CaptureTimeComparison);
            mDarkFileList = mDarkFileList.Distinct().ToList();
            mFlatFileList = mFlatFileList.Distinct().ToList();


            // Bias frames can be matched with short Darks and short Flats
            // This logic is NOT COMPLETE
            mBiasFileList.Clear();
            bool bAllBiasMatched = true;

            // Now build a list of nearest Bias files that match the actual flat files
            // This is a stub; to use biases to calibrate flats we need to look at exposure times to decide on using a bias or a dark
            // Again for now, we are assuming that only masters are being used so the bias stuff is not used and should be optimized out
            foreach (var targetFile in targetFileList)
            {
                XisfFile nearestBiasCalibrationFile = MatchNearestCalibrationFile("Bias", targetFile, mLibraryCalibrationFileList);

                if (nearestBiasCalibrationFile != null)
                    mBiasFileList.Add(nearestBiasCalibrationFile);
                else
                {
                    bAllBiasMatched = true;
                }
            }

            // Sort and Remove all duplicate list elements
            mBiasFileList.Sort(XisfFile.CaptureTimeComparison);
            mBiasFileList = mBiasFileList.Distinct().ToList();


            // Now we have lists of unique nearest calibration frames for the target list
            // We now need to assign each calibration frame to each frame in the target list by type:
            //    1. Number each unique calibration frame type consecutively
            //    2. Do a second matching of the calibration frames with target frames.
            //       Since the calibration frames are unique, we can now assign the matches to the target frames 

            // Do any of our unmatched target Darks match any existing Dark Calibration files?
            List<XisfFile> matchedDarkTargetFileList = new List<XisfFile>();
            foreach (var darkFile in mUnmatchedDarkTargetFileList)
            {
                XisfFile match = MatchNearestCalibrationFile("Dark", darkFile, mTargetCalibrationFileList);

                if (match != null)
                    matchedDarkTargetFileList.Add(match);
            }

            List<XisfFile> matchedDarkExistingList = new List<XisfFile>();
            foreach (var darkFile in matchedDarkTargetFileList)
            {
                List<XisfFile> singleMatchedDarkList = new List<XisfFile>();
                singleMatchedDarkList.Add(darkFile);

                // Find the matching existing Dark file for each unmatched target file
                foreach (var file in mUnmatchedDarkTargetFileList)
                {
                    XisfFile match = MatchNearestCalibrationFile("Dark", file, singleMatchedDarkList);
                    if (match != null)
                    {
                        // Now that we found it (there should only be one), update it
                        file.CDARK = match.CDARK;
                        matchedDarkExistingList.Add(file);
                    }
                }
            }

            mUnmatchedDarkTargetFileList = mUnmatchedDarkTargetFileList.Except(matchedDarkExistingList).ToList();

            // Do any of our unmatched target Flats match any existing Flats Calibration files?
            List<XisfFile> matchedFlatTargetFileList = new List<XisfFile>();
            foreach (var flatFile in mUnmatchedFlatTargetFileList)
            {
                XisfFile match = MatchNearestCalibrationFile("Flat", flatFile, mTargetCalibrationFileList);

                if (match != null)
                    matchedFlatTargetFileList.Add(match);
            }

            List<XisfFile> matchedFlatExistingList = new List<XisfFile>();
            foreach (var flatFile in matchedFlatTargetFileList)
            {
                List<XisfFile> singleMatchedFlatList = new List<XisfFile>();
                singleMatchedFlatList.Add(flatFile);

                // Find the matching existing Flat file for each unmatched target file
                foreach (var file in mUnmatchedFlatTargetFileList)
                {
                    XisfFile match = MatchNearestCalibrationFile("Flat", file, singleMatchedFlatList);
                    if (match != null)
                    {
                        // Now that we found it (there should only be one), update it
                        file.CFLAT = match.CFLAT;
                        matchedFlatExistingList.Add(file);
                    }
                }
            }

            mUnmatchedFlatTargetFileList = mUnmatchedFlatTargetFileList.Except(matchedFlatExistingList).ToList();

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

            // Add Bias stuff here

            // Match each unique, numbered, calibration file with it's corresponding targetFile
            // Note that ALL matching criteria used here MUST be identical to the criteria used to initially find and build the calibration frame lists
            foreach (var darktFile in mUnmatchedDarkTargetFileList)
            {
                XisfFile nearestDarkCalibrationFile = MatchNearestCalibrationFile("Dark", darktFile, mLibraryCalibrationFileList);

                darktFile.CDARK = nearestDarkCalibrationFile.CDARK;
                darktFile.KeywordData.AddKeyword("CDARK", nearestDarkCalibrationFile.CDARK);
            }

            // Match each unique, numbered, calibration file with it's corresponding targetFile
            // Note that ALL matching criteria used here MUST be identical to the criteria used to initially find and build the calibration frame lists
            foreach (var flatFile in mUnmatchedFlatTargetFileList)
            {
                XisfFile nearestFlatCalibrationFile = MatchNearestCalibrationFile("Flat", flatFile, mLibraryCalibrationFileList);

                flatFile.CFLAT = nearestFlatCalibrationFile.CFLAT;
                flatFile.KeywordData.AddKeyword("CFLAT", nearestFlatCalibrationFile.CFLAT);
            }



            // Build a list of all Filters used independent of Camera, Binning, Gain, Offset andTemperature 
            // To Do: The list is only dependent on Binning - maybe

            List<string> filterList = new List<string>();

            foreach (var targetFile in mUnmatchedDarkTargetFileList)
            {
                filterList.Add(targetFile.Filter);
            }

            filterList.Sort();
            filterList = filterList.Distinct().ToList();

            // Match each target with Filter and index
            // To Do: Handle Protected and adding new frames and new filters
            // Now set CLIGHT to group all filters together independent of exposure time
            foreach (var targetFile in mUnmatchedDarkTargetFileList)
            {
                int filterIndex = 1;
                foreach (var filter in filterList)
                {
                    if (targetFile.Filter == filter)
                    {
                        targetFile.KeywordData.AddKeyword("CLIGHT", "L" + filterIndex.ToString());
                        break;
                    }
                    filterIndex++;
                }
            }

            filterList.Clear();

            foreach (var targetFile in mUnmatchedFlatTargetFileList)
            {
                filterList.Add(targetFile.Filter);
            }

            filterList.Sort();
            filterList = filterList.Distinct().ToList();

            // Match each target with Filter and index
            // To Do: Handle Protected and adding new frames and new filters
            // Now set CLIGHT to group all filters together independent of exposure time
            foreach (var targetFile in mUnmatchedFlatTargetFileList)
            {
                int filterIndex = 1;
                foreach (var filter in filterList)
                {
                    if (targetFile.Filter == filter)
                    {
                        targetFile.KeywordData.AddKeyword("CLIGHT", "L" + filterIndex.ToString());
                        break;
                    }
                    filterIndex++;
                }
            }


            mCalibrationTabValues.TotalUniqueFlatCalibrationFiles = mFlatFileList.Count;
            mCalibrationTabValues.TotalUniqueDarkCalibrationFiles = mDarkFileList.Count;
            mCalibrationTabValues.TotalUniqueBiasCalibrationFiles = mBiasFileList.Count;
            mCalibrationTabValues.TotalMatchedCalibrationFiles = mDarkFileList.Count + mFlatFileList.Count + mBiasFileList.Count;

            if (bAllDarksMatched && bAllFlatsMatched & bAllBiasMatched)
            {
                mCalibrationTabValues.MessageMode = CalibrationTabPageValues.eMessageMode.APPEND;
                if (mCalibrationTabValues.TotalMatchedCalibrationFiles == 0)
                {
                    mCalibrationTabValues.TotalMatchedCalibrationFiles = targetFileList.Count();
                    mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            All Target Frames Match Existing Target Capture Directory Calibration Files\r\n";
                }
                else
                    mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            Matched All Target Frames\r\n";
            }
            CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

            return true;
            // To Do
            // What do we do with Bias frames?
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public bool CreateTargetCalibrationDirectory(List<XisfFile> targetFileList, SubFrameLists subFrameLists)
        {
            string targetCalibrationDirectory = GetTargetCalibrationFileDirectory(targetFileList[0].SourceFileName);

            mCalibrationTabValues.Progress = 0;
            mCalibrationTabValues.ProgressMax = mDarkFileList.Count + mFlatFileList.Count + mBiasFileList.Count;

            foreach (var darkFile in mDarkFileList)
            {
                mCalibrationTabValues.Progress += 1;
                mCalibrationTabValues.FileName = targetCalibrationDirectory + "\n" + Path.GetFileName(darkFile.SourceFileName);
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                FileInfo sourceCalibrationFile = new FileInfo(darkFile.SourceFileName);
                FileInfo destinationCalibrationFile = new FileInfo(targetCalibrationDirectory + @"\" + Path.GetFileName(darkFile.SourceFileName));

                sourceCalibrationFile.CopyTo(destinationCalibrationFile.FullName, true);

                string fileNameHolder = darkFile.SourceFileName;
                darkFile.SourceFileName = destinationCalibrationFile.FullName;
                XisfFileUpdate.UpdateFile(darkFile, subFrameLists, true);
                darkFile.SourceFileName = fileNameHolder;
            }

            foreach (var flatFile in mFlatFileList)
            {
                mCalibrationTabValues.Progress += 1;
                mCalibrationTabValues.FileName = targetCalibrationDirectory + "\n" + Path.GetFileName(flatFile.SourceFileName);
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                FileInfo sourceCalibrationFile = new FileInfo(flatFile.SourceFileName);
                FileInfo destinationCalibrationFile = new FileInfo(targetCalibrationDirectory + @"\" + Path.GetFileName(flatFile.SourceFileName));

                sourceCalibrationFile.CopyTo(destinationCalibrationFile.FullName, true);

                string fileNameHolder = flatFile.SourceFileName;
                flatFile.SourceFileName = destinationCalibrationFile.FullName;
                XisfFileUpdate.UpdateFile(flatFile, subFrameLists, true);
                flatFile.SourceFileName = fileNameHolder;
            }

            foreach (var biasFile in mBiasFileList)
            {
                mCalibrationTabValues.Progress += 1;
                mCalibrationTabValues.FileName = targetCalibrationDirectory + "\n" + Path.GetFileName(biasFile.SourceFileName);
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                FileInfo sourceCalibrationFile = new FileInfo(biasFile.SourceFileName);
                FileInfo destinationCalibrationFile = new FileInfo(targetCalibrationDirectory + @"\" + Path.GetFileName(biasFile.SourceFileName));

                sourceCalibrationFile.CopyTo(destinationCalibrationFile.FullName, true);

                string fileNameHolder = biasFile.SourceFileName;
                biasFile.SourceFileName = destinationCalibrationFile.FullName;
                XisfFileUpdate.UpdateFile(biasFile, subFrameLists, true);
                biasFile.SourceFileName = fileNameHolder;
            }

            return true;
        }

        public string GetTargetCalibrationFileDirectory(string targetFilePath)
        {
            string targetCalibrationDirectory = Path.GetDirectoryName(targetFilePath);

            if (targetCalibrationDirectory.Contains(@"Captures\"))
                targetCalibrationDirectory = targetCalibrationDirectory.Substring(0, targetCalibrationDirectory.IndexOf("Captures")) + @"Captures\Calibration";
            else
                targetCalibrationDirectory = Path.GetFullPath(Path.Combine(targetCalibrationDirectory, @"..") + @"\Calibration");

            return targetCalibrationDirectory;
        }
        // ******************************************************************************************************************
        // ******************************************************************************************************************
    }
}