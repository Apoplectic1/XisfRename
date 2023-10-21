using MathNet.Numerics;
using Microsoft.Extensions.FileSystemGlobbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using XisfFileManager;
using XisfFileManager.FileOperations;
using XisfFileManager.Keywords;
using static System.Net.WebRequestMethods;

using XisfFileManager.Enums;

namespace XisfFileManager
{
    // ******************************************************************************************************************
    // ******************************************************************************************************************
    public class Calibration
    {
        private List<(XisfFile TargetFile, XisfFile CalibrationFile)> mMatchedDarkList;
        private List<(XisfFile TargetFile, XisfFile CalibrationFile)> mMatchedFlatList;
        private List<XisfFile> mBiasCalibrationFileList;
        private List<XisfFile> mDarkCalibrationFileList;
        private List<XisfFile> mFlatCalibrationFileList;
        private List<XisfFile> mLibraryCalibrationFileList;
        private List<XisfFile> mLocalCalibrationFileList;
        private List<XisfFile> mUniqueDarkCalibrationFileList;
        private List<XisfFile> mUniqueFlatCalibrationFileList;
        private List<XisfFile> mUnmatchedBiasTargetFileList;
        private List<XisfFile> mUnmatchedDarkTargetFileList;
        private List<XisfFile> mUnmatchedFlatTargetFileList;

        private readonly CalibrationTabPageValues mCalibrationTabValues;
        private readonly DirectoryOps mDirectoryOps;
        public eFile File { get; set; } = eFile.MASTERS;
        public eFilter Filter { get; set; } = eFilter.ALL;
        public eFrame Frame { get; set; } = eFrame.ALL;
        public bool Recurse { get; set; } = true;
        public double ExposureTolerance { get; set; } = 10;
        public double FocuserTolerance { get; set; } = 10000;
        public double GainTolerance { get; set; } = 10;
        public double OffsetTolerance { get; set; } = 5;
        public double RotationTolerance { get; set; } = 180.0;
        public double TemperatureTolerance { get; set; } = 5;

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        // Constructor
        // ******************************************************************************************************************
        public Calibration()
        {
            mBiasCalibrationFileList = new List<XisfFile>();
            mCalibrationTabValues = new CalibrationTabPageValues();
            mDarkCalibrationFileList = new List<XisfFile>();
            mDirectoryOps = new DirectoryOps();
            mFlatCalibrationFileList = new List<XisfFile>();
            mLibraryCalibrationFileList = new List<XisfFile>();
            mLocalCalibrationFileList = new List<XisfFile>();
            mMatchedDarkList = new List<(XisfFile, XisfFile)>();
            mMatchedFlatList = new List<(XisfFile, XisfFile)>();
            mUniqueDarkCalibrationFileList = new List<XisfFile>();
            mUniqueFlatCalibrationFileList = new List<XisfFile>();
            mUnmatchedBiasTargetFileList = new List<XisfFile>();
            mUnmatchedDarkTargetFileList = new List<XisfFile>();
            mUnmatchedFlatTargetFileList = new List<XisfFile>();
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public void ResetAll()
        {
            ExposureTolerance = 10;
            File = eFile.MASTERS;
            Filter = eFilter.ALL;
            Frame = eFrame.ALL;
            GainTolerance = 10;
            OffsetTolerance = 10;
            TemperatureTolerance = 25;
            mBiasCalibrationFileList.Clear();
            mDarkCalibrationFileList.Clear();
            mLocalCalibrationFileList.Clear();
            mFlatCalibrationFileList.Clear();
            mLibraryCalibrationFileList.Clear();
            mLocalCalibrationFileList.Clear();
            mUnmatchedBiasTargetFileList.Clear();
            mUnmatchedDarkTargetFileList.Clear();
            mUnmatchedFlatTargetFileList.Clear();
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public async Task ReadCalibrationFramesAsync(eCalibrationDirectory location, string sCalibrationFrameDirectory)
        {
            // Recursively search sCalibrationFrameDirectory to find calibration frames
            // Add the frames to either mLocalCalibrationFileList or mLibraryCalibrationFileList

            List<XisfFile> calibrationFileList = new List<XisfFile>();
            XisfFileReader fileReader = new XisfFileReader();

            mCalibrationTabValues.MessageMode = eMessageMode.CLEAR;
            mCalibrationTabValues.Progress = 0;
            CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);


            DirectoryInfo diDirectoryTree = new DirectoryInfo(sCalibrationFrameDirectory);
            mDirectoryOps.ClearFileList();
            mDirectoryOps.Filter = Filter;
            mDirectoryOps.File = File;
            mDirectoryOps.Frame = Frame;
            mDirectoryOps.Recurse = Recurse;
            _ = mDirectoryOps.RecuseDirectories(diDirectoryTree);

            if (mDirectoryOps.Files.Count == 0)
            {
                _ = MessageBox.Show("No Calibration Files Found under: " + diDirectoryTree.ToString(), "Select Calibration Folder");
                return;
            }

            mCalibrationTabValues.TotalFiles = mDirectoryOps.Files.Count;

            foreach (FileInfo file in mDirectoryOps.Files)
            {
                Application.DoEvents();

                // Create a new xisf file instance for reads
                XisfFile calibrationFile = new XisfFile
                {
                    FilePath = file.FullName
                };

                mCalibrationTabValues.ProgressMax = mDirectoryOps.Files.Count;
                mCalibrationTabValues.Progress += 1;
                mCalibrationTabValues.FileName = Path.GetDirectoryName(calibrationFile.FilePath) + "\n" + Path.GetFileName(calibrationFile.FilePath);
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

                fileReader.ReadXisfFile(calibrationFile);

                /*
                await Task.Run(async () =>
                {
                    await fileReader.ReadXisfFile(calibrationFile);
                });
                */
                

                calibrationFileList.Add(calibrationFile);
            }

            calibrationFileList.Sort(XisfFile.CaptureTimeComparison);

            if (location == eCalibrationDirectory.LIBRARY)
                mLibraryCalibrationFileList = calibrationFileList.Distinct().ToList();
            else
                mLocalCalibrationFileList = calibrationFileList.Distinct().ToList();

            mCalibrationTabValues.TotalFiles = calibrationFileList.Count;
            CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        private XisfFile FindNearestCalibrationFile(eFrame calibrationFrameMatchType, XisfFile targetFile, List<XisfFile> calibrationLibraryFileList)
        {
            // This routine gets called twice for each target frame
            // The first time the this is used to build non-unique (meaning to find all) lists of target matching calibration files
            // The second time this is used to match unique calibration files with each target file

            // More specifically, the first time this is called, calibrationFileList contains all calibration type files found in the calibration library
            // The second time, calibrationFileList has been reduced to include only calibration files unique to the target List 

            // To cleanly reuse this code, we have to use the margin calculations when differentiating Flats from Darks from Biases. Unfortunate but not really a performance issue...

            // To Do
            // Add code to inform user of how close the match was for each appropriate match parameter (the ones with margins)

            bool bIgnoreFilter = calibrationFrameMatchType == eFrame.DARK;
            bool bIgnoreExposure = calibrationFrameMatchType == eFrame.FLAT || calibrationFrameMatchType == eFrame.BIAS;
            bool bIgnoreRotator = calibrationFrameMatchType == eFrame.DARK || calibrationFrameMatchType == eFrame.BIAS;
            bool bIgnoreFocuser = calibrationFrameMatchType == eFrame.DARK || calibrationFrameMatchType == eFrame.BIAS;

            // Return a list of all calibration files that match the target file camera
            List<XisfFile> CameraList = calibrationLibraryFileList.Where(camera => camera.Camera == targetFile.Camera).ToList();
            if (CameraList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Camera\r\n" 
                                                              + "  Target Camera: " + targetFile.Camera + "\r\n  " 
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine CameraList to match the passed FrameType (DARK, FLAT, etc.)
            List<XisfFile> FrameTypeList = CameraList.Where(frameType => frameType.FrameType == calibrationFrameMatchType).ToList();
            if (FrameTypeList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Frame Type\r\n" 
                                                              + "  Target Frame Type: " + Enum.GetName(typeof(eFrame), calibrationFrameMatchType) + "\r\n  " 
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine FrameTypeList to match  match the target file binning
            List<XisfFile> BinningList = FrameTypeList.Where(binning => binning.Binning == targetFile.Binning).ToList();
            if (BinningList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Binning\r\n"
                                                              + "  Target Binning: " + targetFile.Binning + "x" +targetFile.Binning+ "\r\n  "
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine BinningList to match  match the target file filter
            // We ignore filters for DARKs and BIASs
            List<XisfFile> FilterList = bIgnoreFilter ? BinningList : BinningList.Where(filter => filter.FilterName == targetFile.FilterName).ToList();
            if (FilterList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Filter\r\n"
                                                              + "  Target Filter: " + targetFile.FilterName + "\r\n  "
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine FilterList to match  match the target file gain
            List<XisfFile> GainList = FilterList.Where(gain => Math.Abs(gain.Gain - targetFile.Gain) <= GainTolerance).ToList();
            if (GainList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Gain\r\n"
                                                               + "  Target Gain: " + targetFile.Gain + "Tolerance: "+ GainTolerance + "\r\n  "
                                                               +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine GainList to match  match the target file offset
            List<XisfFile> OffsetList = GainList.Where(offset => Math.Abs(offset.Offset - targetFile.Offset) <= OffsetTolerance).ToList();
            if (OffsetList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "No matching " + calibrationFrameMatchType + " Offset for:\r\n    " + targetFile.FilePath + "\r\n";
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Offset\r\n"
                                                              + "  Target Offset: " + targetFile.Offset + "Tolerance: " + OffsetTolerance + "\r\n  "
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine OffsetList to match  match the target file focuser position
            // We ignore focuser position for DARKs and BIASs
            List<XisfFile> FocuserList = bIgnoreFocuser ? OffsetList : OffsetList.Where(focus => Math.Abs(focus.FocuserPosition - targetFile.FocuserPosition) <= FocuserTolerance).ToList();
            if (FocuserList.Count == 0) FocuserList.AddRange(OffsetList); // Deal with old Masters that don't incude the Focus position
            if (FocuserList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Focus Position\r\n"
                                                              + "  Target Focus Position: " + targetFile.FocuserPosition + "Tolerance: " + FocuserTolerance + "\r\n  "
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine FocuserList to match  match the target file rotator position
            // We ignore rotator position for DARKs and BIASs
            List<XisfFile> RotatorList = bIgnoreRotator ? FocuserList : FocuserList.Where(rotator => Math.Abs(rotator.RotatorAngle - targetFile.RotatorAngle) <= RotationTolerance).ToList();
            if (RotatorList.Count == 0) RotatorList.AddRange(FocuserList); // Deal with old Masters that dont include the Rotator position
            if (RotatorList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Rotator Position\r\n"
                                                               + "  Target Rotator Position: " + targetFile.RotatorAngle + "Tolerance: " + RotationTolerance + "\r\n  "
                                                               +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine RotatorList to match  match the target file CCD temperature
            List<XisfFile> TemperatureList = RotatorList.Where(temperature => Math.Abs(temperature.SensorTemperature - targetFile.SensorTemperature) <= TemperatureTolerance).ToList();
            if (TemperatureList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "No matching " + calibrationFrameMatchType + " Temperature for:\r\n    " + targetFile.FilePath + "\r\n";
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Sensor Temperature\r\n"
                                                              + "  Target Sensor Temperature: " + targetFile.SensorTemperature + "Tolerance: " + TemperatureTolerance + "\r\n  "
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Refine TemperatureList to match  match the target file exposure time
            List<XisfFile> ExposureList = TemperatureList.Where(exposure => Math.Abs(exposure.ExposureSeconds - targetFile.ExposureSeconds) <= ExposureTolerance).ToList();
            List<XisfFile> ExposureToleranceList = ExposureList.Where(value => value.ExposureSeconds >= targetFile.ExposureSeconds).ToList();
            if (!bIgnoreExposure)
            {
                if (ExposureToleranceList.Count == 0)
                {
                    ExposureToleranceList = ExposureList;
                }
            }
            else
            {
                ExposureToleranceList = TemperatureList;
            }
            if (ExposureToleranceList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "Match Failed: Exposure Time\r\n"
                                                              + "  Target Seconds: " + targetFile.ExposureSeconds + "Tolerance: " + ExposureTolerance + "\r\n  "
                                                              +    Path.GetFileName(targetFile.FilePath) + "\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return null;
            }

            // Return the single best and nearest calibration file
            return ExposureToleranceList.OrderBy(nearest => Math.Abs((nearest.CaptureDateTime - targetFile.CaptureDateTime).TotalSeconds)).FirstOrDefault();
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public bool FindLocalTargetCalibrationFrames(List<XisfFile> targetFileList)
        {
            // Find local "Calibration" directory and determine if all Lights match the existing calibation files 

            bool bSilent = true;
            bool bAllExistingDarksMatched = false;
            bool bAllExistingFlatsMatched = false;

            mLocalCalibrationFileList.Clear();

            string localCalibrationDirectory = SetTargetCalibrationFileDirectories(targetFileList[0].FilePath);

            // Does a local "Calibration" directory exist?
            if (Directory.Exists(localCalibrationDirectory))
            {
                // Yes - so see if it's empty
                if (Directory.GetFileSystemEntries(localCalibrationDirectory).Length != 0)
                    // It has calibration files so build a list of existing Calibration Frames
                    _ = ReadCalibrationFramesAsync(eCalibrationDirectory.LOCAL, localCalibrationDirectory);

                if (mLocalCalibrationFileList.Count != 0)
                {
                    // So we found valid calibration files 
                    // Check to see if they still match the LIGHT frames

                    bAllExistingDarksMatched = MatchCalibrationDarkFrames(eCalibrationDirectory.LOCAL, mLocalCalibrationFileList, bSilent);

                    // If all target frames have corresponding Darks and Flats, let us know and return true
                    if (bAllExistingDarksMatched && bAllExistingFlatsMatched)
                    {
                        mCalibrationTabValues.MessageMode = eMessageMode.CLEAR;
                        CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                        mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                        mCalibrationTabValues.TotalMatchedCalibrationFiles = targetFileList.Count;
                        mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            All Target Frames Match Existing Target Capture Directory Calibration Files\r\n";
                        CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                        return true;
                    }
                }
            }

            // So we have unmatched LIGHT target files
            return false;
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        private bool MatchCalibrationDarkFrames(eCalibrationDirectory location, List<XisfFile> targetFrameList, bool bSilent)
        {
            int errorMessageLimit = 4;
            bool bAllDarksMatched = true;

            if (targetFrameList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            MatchCalibrationDarkFrames: No Target Frames Found\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return false;
            }

            mMatchedDarkList.Clear();

            // Now we add to mDarkDictionary for all the Dark calibration files that match/do not match the target
            foreach (var targetFile in mUnmatchedDarkTargetFileList)
            {
                if (location == eCalibrationDirectory.LIBRARY)
                    mMatchedDarkList.Add((targetFile, FindNearestCalibrationFile(eFrame.DARK, targetFile, mLibraryCalibrationFileList)));
                else
                    mMatchedDarkList.Add((targetFile, FindNearestCalibrationFile(eFrame.DARK, targetFile, mLocalCalibrationFileList)));


                if (mMatchedDarkList[mMatchedDarkList.Count - 1].CalibrationFile == null && errorMessageLimit > 0)
                {
                    if (!bSilent)
                    {
                        mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                        mCalibrationTabValues.MatchCalibrationMessage = "No matching Dark for target frame:\r\n" + targetFile.FilePath + "\r\n";
                        CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                    }
                    bAllDarksMatched = false;
                    errorMessageLimit--;
                }
            }

            return bAllDarksMatched;
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        private bool MatchCalibrationFlatFrames(eCalibrationDirectory location, List<XisfFile> targetFrameList, bool bSilent)
        {
            int errorMessageLimit = 4;
            bool bAllFlatsMatched = true;

            if (targetFrameList.Count == 0)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            MatchCalibrationFlatFrames: No Target Frames Found\r\n";
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                return false;
            }

            mMatchedFlatList.Clear();

            // Now we add to mFlatDictionary for all the Flat calibration files that match/do not match the target
            foreach (var targetFile in mUnmatchedFlatTargetFileList)
            {
                if (location == eCalibrationDirectory.LIBRARY)
                    mMatchedFlatList.Add((targetFile, FindNearestCalibrationFile(eFrame.FLAT, targetFile, mLibraryCalibrationFileList)));
                else
                    mMatchedDarkList.Add((targetFile, FindNearestCalibrationFile(eFrame.FLAT, targetFile, mLocalCalibrationFileList)));

                if (mMatchedFlatList[mMatchedFlatList.Count - 1].CalibrationFile == null && errorMessageLimit > 0)
                {
                    mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                    mCalibrationTabValues.MatchCalibrationMessage = "No matching Flat for target frame:\r\n" + targetFile.FilePath + "\r\n";
                    CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);
                    bAllFlatsMatched = false;
                    errorMessageLimit--;
                }
            }

            return bAllFlatsMatched;
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public bool MatchCalibrationLibraryFrames(List<XisfFile> targetFileList)
        {

            mUnmatchedDarkTargetFileList.Clear();
            mUnmatchedFlatTargetFileList.Clear();
            mUnmatchedBiasTargetFileList.Clear();

            mUnmatchedDarkTargetFileList.AddRange(targetFileList);
            mUnmatchedFlatTargetFileList.AddRange(targetFileList);
            mUnmatchedBiasTargetFileList.AddRange(targetFileList);

            // At this point, we have lists of target frames (lights, darks and flats) as well as a list of all Library calibration frames (darks, flats and bias) 

            // Now we need to assign numbered CDARK, CLIGHT and CBIAS values:
            // We match each target frame with a corresponding calibration frame keeping track of what calibration frame matched with what target frame:
            //    1. Find all calibration frames for the target set
            //    2. Uniquify and consecutively number the found calibration frame 
            //    3. Assign the appropriate calibration frame number to each target

            // ----------------------------------------------------------------------------------------------------------------------------------------------------

            // Match each LIGHT frame with a corresponding DARK frame and place the match list in mUniqueDarkCalibrationFileList<Target, Callibration>
            // mUniqueDarkCalibrationFileList.Calibration will be null if no DARK matches any targetfiles
            // This list is unique in that each entry can match multiple target files 
            // This list will eventually be copied out and placed in the local target Calibration directory
            // Targets that don't match any existing DARK calibration file will be flagged later

            bool bAllDarksMatched = MatchCalibrationDarkFrames(eCalibrationDirectory.LIBRARY, mUnmatchedDarkTargetFileList, false);

            mUniqueDarkCalibrationFileList = mMatchedDarkList.Select(item => item.CalibrationFile).Distinct().ToList();

            // Sorting the list by CaptureTime guarantees a consistent File order from run to run.
            mUniqueDarkCalibrationFileList.Sort(XisfFile.CaptureTimeComparison);

            // ----------------------------------------------------------------------------------------------------------------------------------------------------

            // Match each LIGHT frame with a corresponding FLAT frame and place the match list in mUniqueFlatCalibrationFileList<Target, Callibration>
            // mUniqueFlatCalibrationFileList.Calibration will be null if no Flat matches any targetfiles
            // This list is unique in that each entry can match multiple target files 
            // This list will eventually be copied out and placed in the local target Calibration directory
            // Targets that don't match any existing FLAT calibration file will be flagged later

            bool bAllFlatsMatched = MatchCalibrationFlatFrames(eCalibrationDirectory.LIBRARY, mUnmatchedFlatTargetFileList, false);

            mUniqueFlatCalibrationFileList = mMatchedFlatList.Select(item => item.CalibrationFile).Distinct().ToList();

            // Sorting the list by CaptureTime guarantees a consistent File order from run to run.
            mUniqueFlatCalibrationFileList.Sort(XisfFile.CaptureTimeComparison);

            // ----------------------------------------------------------------------------------------------------------------------------------------------------

            if (!bAllFlatsMatched || !bAllDarksMatched)
                return false;

            // ----------------------------------------------------------------------------------------------------------------------------------------------------

            int darkIndex = 1;
            foreach (var uniqueDarkCalibrationFile in mUniqueDarkCalibrationFileList)
            {
                // No Flats to associate with calibration frame darks since these are Master Frames
                uniqueDarkCalibrationFile.CFLAT = string.Empty;
                uniqueDarkCalibrationFile.RemoveKeyword("CFLAT");

                foreach (var darkFile in mMatchedDarkList)
                {
                    if (darkFile.CalibrationFile.FilePath == uniqueDarkCalibrationFile.FilePath)
                    {
                        // Number each Dark based on number of uniqueDarkCalibrationFiles
                        darkFile.TargetFile.CDARK = "D" + darkIndex.ToString();
                        darkFile.TargetFile.AddKeyword("CDARK", darkFile.TargetFile.CDARK);

                        darkFile.CalibrationFile.CDARK = "D" + darkIndex.ToString();
                        darkFile.CalibrationFile.AddKeyword("CDARK", darkFile.CalibrationFile.CDARK);
                    }
                }

                darkIndex++;
            }

            // ----------------------------------------------------------------------------------------------------------------------------------------------------

            int flatIndex = 1;
            foreach (var uniqueFlatCalibrationFile in mUniqueFlatCalibrationFileList)
            {
                // No Darks to associate with calibration frame darks since these are Master Frames
                uniqueFlatCalibrationFile.CDARK = string.Empty;
                uniqueFlatCalibrationFile.RemoveKeyword("CDARK");

                foreach (var flatFile in mMatchedFlatList)
                {
                    if (flatFile.CalibrationFile.FilePath == uniqueFlatCalibrationFile.FilePath)
                    {
                        // Number each Flat based on number of uniqueFlatCalibrationFiles
                        flatFile.TargetFile.CFLAT = "F" + flatIndex.ToString();
                        flatFile.TargetFile.AddKeyword("CFLAT", flatFile.TargetFile.CFLAT);

                        flatFile.CalibrationFile.CFLAT = "F" + flatIndex.ToString();
                        flatFile.CalibrationFile.AddKeyword("CFLAT", flatFile.CalibrationFile.CFLAT);
                    }
                }

                flatIndex++;
            }

            // ----------------------------------------------------------------------------------------------------------------------------------------------------

            // Build a list of all Filters used independent of Camera, Binning, Gain, Offset andTemperature 
            // To Do: The list is only dependent on Binning - maybe

            List<string> filterList = new List<string>();

            foreach (var targetFile in mUnmatchedDarkTargetFileList)
            {
                filterList.Add(targetFile.FilterName);
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
                    if (targetFile.FilterName == filter)
                    {
                        targetFile.AddKeyword("CLIGHT", "L" + filterIndex.ToString());
                        break;
                    }
                    filterIndex++;
                }
            }

            filterList.Clear();

            foreach (var targetFile in mUnmatchedFlatTargetFileList)
            {
                filterList.Add(targetFile.FilterName);
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
                    if (targetFile.FilterName == filter)
                    {
                        targetFile.AddKeyword("CLIGHT", "L" + filterIndex.ToString());
                        break;
                    }
                    filterIndex++;
                }
            }

            mCalibrationTabValues.TotalUniqueFlatCalibrationFiles = mFlatCalibrationFileList.Count;
            mCalibrationTabValues.TotalUniqueDarkCalibrationFiles = mDarkCalibrationFileList.Count;
            mCalibrationTabValues.TotalUniqueBiasCalibrationFiles = mBiasCalibrationFileList.Count;
            mCalibrationTabValues.TotalMatchedCalibrationFiles = mDarkCalibrationFileList.Count + mFlatCalibrationFileList.Count + mBiasCalibrationFileList.Count;

            if (bAllDarksMatched && bAllFlatsMatched)
            {
                mCalibrationTabValues.MessageMode = eMessageMode.APPEND;
                if (mCalibrationTabValues.TotalMatchedCalibrationFiles == 0)
                {
                    mCalibrationTabValues.TotalMatchedCalibrationFiles = targetFileList.Count;
                    mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            All Target Frames Match Existing Target Capture Directory Calibration Files\r\n";
                }
                else
                    mCalibrationTabValues.MatchCalibrationMessage = "\r\n\r\n\r\n\r\n            Matched All Target Frames\r\n";
            }
            CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

            return true;
        }

        // ******************************************************************************************************************
        // ******************************************************************************************************************
        public bool CreateTargetCalibrationDirectory(List<XisfFile> targetFileList, SubFrameLists subFrameLists)
        {
            string targetCalibrationDirectory = SetTargetCalibrationFileDirectories(targetFileList[0].FilePath);

            mCalibrationTabValues.Progress = 0;
            mCalibrationTabValues.ProgressMax = mUniqueDarkCalibrationFileList.Count + mUniqueFlatCalibrationFileList.Count + mBiasCalibrationFileList.Count;

            foreach (var darkFile in mUniqueDarkCalibrationFileList)
            {
                mCalibrationTabValues.Progress += 1;
                mCalibrationTabValues.FileName = targetCalibrationDirectory + "\n" + Path.GetFileName(darkFile.FilePath);
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                FileInfo sourceCalibrationFile = new FileInfo(darkFile.FilePath);
                FileInfo destinationCalibrationFile = new FileInfo(targetCalibrationDirectory + @"\" + Path.GetFileName(darkFile.FilePath));

                _ = sourceCalibrationFile.CopyTo(destinationCalibrationFile.FullName, true);

                string fileNameHolder = darkFile.FilePath;
                darkFile.FilePath = destinationCalibrationFile.FullName;
                _ = XisfFileUpdate.UpdateFile(darkFile, subFrameLists, true);
                darkFile.FilePath = fileNameHolder;
            }

            foreach (var flatFile in mUniqueFlatCalibrationFileList)
            {
                mCalibrationTabValues.Progress += 1;
                if (flatFile != null)
                {
                    mCalibrationTabValues.FileName = targetCalibrationDirectory + "\n" + Path.GetFileName(flatFile.FilePath);
                    CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);


                    FileInfo sourceCalibrationFile = new FileInfo(flatFile.FilePath);
                    FileInfo destinationCalibrationFile = new FileInfo(targetCalibrationDirectory + @"\" + Path.GetFileName(flatFile.FilePath));

                    _ = sourceCalibrationFile.CopyTo(destinationCalibrationFile.FullName, true);

                    string fileNameHolder = flatFile.FilePath;
                    flatFile.FilePath = destinationCalibrationFile.FullName;
                    _ = XisfFileUpdate.UpdateFile(flatFile, subFrameLists, true);
                    flatFile.FilePath = fileNameHolder;
                }
            }

            foreach (var biasFile in mBiasCalibrationFileList)
            {
                mCalibrationTabValues.Progress += 1;
                mCalibrationTabValues.FileName = targetCalibrationDirectory + "\n" + Path.GetFileName(biasFile.FilePath);
                CalibrationTabPageEvent.TransmitData(mCalibrationTabValues);

                FileInfo sourceCalibrationFile = new FileInfo(biasFile.FilePath);
                FileInfo destinationCalibrationFile = new FileInfo(targetCalibrationDirectory + @"\" + Path.GetFileName(biasFile.FilePath));

                _ = sourceCalibrationFile.CopyTo(destinationCalibrationFile.FullName, true);

                string fileNameHolder = biasFile.FilePath;
                biasFile.FilePath = destinationCalibrationFile.FullName;
                _ = XisfFileUpdate.UpdateFile(biasFile, subFrameLists, true);
                biasFile.FilePath = fileNameHolder;
            }

            return true;
        }

        public static string SetTargetCalibrationFileDirectories(string targetFilePath)
        {
            // Create and use a Calibration Directory for the complete project
            // For Calibration files, there is no distiction between an individual Target and a Mosaic
            string targetCalibrationDirectory = Path.GetDirectoryName(targetFilePath);

            // Can we find a "Captures" directory?
            if (targetCalibrationDirectory.Contains(@"Captures\"))
                // Place the Calibration directory under "Captures"
                targetCalibrationDirectory = string.Concat(targetCalibrationDirectory.AsSpan(0, targetCalibrationDirectory.IndexOf("Captures")), @"Captures\Calibration");
            else
                // No - so set just add "Calibration" under the target path
                targetCalibrationDirectory = Path.GetFullPath(Path.Combine(targetCalibrationDirectory, @"..") + @"\Calibration");

            return targetCalibrationDirectory;
        }
        // ******************************************************************************************************************
        // ******************************************************************************************************************
    }
}