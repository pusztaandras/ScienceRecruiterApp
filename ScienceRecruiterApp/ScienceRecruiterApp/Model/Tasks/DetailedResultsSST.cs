using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceRecruiterApp.Model.Tasks
{
    public class DetailedResultsSST:DetailedResultsTask
    {
        public string TrialType { get; set; }

        public string TrialType2 { get; set; }

        public string TrialType3 { get; set; }

        public int Ommission { get; set; }

        public int Commission { get; set; }

        public int RT { get; set; }

        public int SSDH { get; set; }

        public int SSDL { get; set; }

        public int CueTime { get; set; }

        public string PressedKey { get; set; }
    }
}
