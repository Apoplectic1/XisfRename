using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using XisfFileManager.TargetScheduler.Tables;

namespace XisfFileManager.TargetScheduler
{
    internal class SqlLiteReader
    {
        private List<ProfilePreference> mProfilePreferenceList;
        private List<Project> mProjectList = new List<Project>();
        private List<Target> mTargetList = new List<Target>();
        private List<RuleWeight> mRuleWeightList = new List<RuleWeight>();

        public SqlLiteReader()
        {
            mProfilePreferenceList = new List<ProfilePreference>();
            mProjectList = new List<Project>();
            mTargetList = new List<Target>();
            mRuleWeightList = new List<RuleWeight>();
        }

        public bool ReadTargetSchedulerDataBaseFile(string sqlLightFileName)
        {
            mProfilePreferenceList.Clear();
            mProjectList.Clear();
            mRuleWeightList.Clear();
            mTargetList.Clear();

            using (SqliteConnection connection = new SqliteConnection($"Data Source={sqlLightFileName};"))
            {
                connection.Open();

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM profilepreference", connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProfilePreference profilepreferenceRow = new ProfilePreference
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                profileId = reader.GetString(reader.GetOrdinal("profileId")),
                                enableGradeRMS = reader.GetInt32(reader.GetOrdinal("enableGradeRMS")),
                                enableGradeStars = reader.GetInt32(reader.GetOrdinal("enableGradeStars")),
                                enableGradeHFR = reader.GetInt32(reader.GetOrdinal("enableGradeHFR")),
                                maxGradingSampleSize = reader.GetInt32(reader.GetOrdinal("maxGradingSampleSize")),
                                rmsPixelThreshold = reader.GetDouble(reader.GetOrdinal("rmsPixelThreshold")),
                                detectedStarsSigmaFactor = reader.GetDouble(reader.GetOrdinal("detectedStarsSigmaFactor")),
                                hfrSigmaFactor = reader.GetDouble(reader.GetOrdinal("hfrSigmaFactor")),
                                acceptimprovement = reader.GetInt32(reader.GetOrdinal("acceptimprovement")),
                                exposurethrottle = reader.GetDouble(reader.GetOrdinal("exposurethrottle")),
                                parkonwait = reader.GetInt32(reader.GetOrdinal("parkonwait")),
                            };

                            mProfilePreferenceList.Add(profilepreferenceRow);
                        }
                    }
                }

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM project", connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project projectRow = new Project
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                profileId = reader.GetString(reader.GetOrdinal("profileId")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                description = reader.GetString(reader.GetOrdinal("description")),
                                state = reader.GetInt32(reader.GetOrdinal("state")),
                                priority = reader.GetInt32(reader.GetOrdinal("priority")),
                                createdate = reader.GetInt32(reader.GetOrdinal("createdate")),
                                //activedate = reader.GetInt32(reader.GetOrdinal("activedate")),
                                //inactivedate = reader.GetInt32(reader.GetOrdinal("inactivedate")),
                                minimumtime = reader.GetInt32(reader.GetOrdinal("minimumtime")),
                                minimumaltitude = reader.GetDouble(reader.GetOrdinal("minimumaltitude")),
                                usecustomhorizon = reader.GetInt32(reader.GetOrdinal("usecustomhorizon")),
                                horizonoffset = reader.GetDouble(reader.GetOrdinal("horizonoffset")),
                                meridianwindow = reader.GetInt32(reader.GetOrdinal("meridianwindow")),
                                filterswitchfrequency = reader.GetInt32(reader.GetOrdinal("filterswitchfrequency")),
                                ditherevery = reader.GetInt32(reader.GetOrdinal("ditherevery")),
                                enablegrader = reader.GetInt32(reader.GetOrdinal("enablegrader")),
                                isMosaic = reader.GetInt32(reader.GetOrdinal("isMosaic")),
                            };

                            mProjectList.Add(projectRow);
                        }
                    }
                }

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM target", connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Target targetRow = new Target
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                active = reader.GetInt32(reader.GetOrdinal("active")),
                                ra = reader.GetDouble(reader.GetOrdinal("ra")),
                                dec = reader.GetDouble(reader.GetOrdinal("dec")),
                                epochcode = reader.GetInt32(reader.GetOrdinal("epochcode")),
                                rotation = reader.GetDouble(reader.GetOrdinal("rotation")),
                                roi = reader.GetDouble(reader.GetOrdinal("roi")),
                                projectid = reader.GetInt32(reader.GetOrdinal("projectid")),
                                //overrideExposureOrder = reader.GetString(reader.GetOrdinal("overrideExposureOrder")),
                            };

                            mTargetList.Add(targetRow);
                        }
                    }
                }

                using (SqliteCommand command = new SqliteCommand("SELECT * FROM ruleweight", connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RuleWeight ruleWeightRow = new RuleWeight
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                weight = reader.GetDouble(reader.GetOrdinal("weight")),
                                projectid = reader.GetInt32(reader.GetOrdinal("projectid")),
                            };

                            mRuleWeightList.Add(ruleWeightRow);
                        }
                    }
                }
            }

            return true;
        }
    }
}