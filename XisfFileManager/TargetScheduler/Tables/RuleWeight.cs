using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfFileManager.TargetScheduler.Tables
{
    /*
     *CREATE TABLE `ruleweight` (
	`Id`			INTEGER NOT NULL,
	`name`			TEXT NOT NULL,
    `weight`		REAL NOT NULL,
	`projectid`		INTEGER,
	FOREIGN KEY(`projectId`) 
    REFERENCES `project`(`Id`),
	PRIMARY KEY(`Id`)
    )
    */
    internal class RuleWeight
    {
        public int Id { get; set; }
        public string name { get; set; }
        public double weight { get; set; }
        public int projectid { get; set; }
    }
}
