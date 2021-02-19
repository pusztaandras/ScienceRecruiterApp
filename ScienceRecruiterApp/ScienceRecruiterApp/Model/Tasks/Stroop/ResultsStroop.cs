using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceRecruiterApp.Model.Tasks.Stroop
{
    public class ResultsStroop : ResultsTasks
    {
        [PrimaryKey]
        public int id { get; set; }

        public double mRTCongr { get; set; }

        public double mRTIncongr { get; set; }

        public double mRTControl { get; set; }

        public double ACCCongr { get; set; }

        public double ACCIncongr { get; set; }

        public double ACCControl { get; set; }

        
    }
}
