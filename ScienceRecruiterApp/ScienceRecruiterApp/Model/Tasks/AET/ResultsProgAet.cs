using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceRecruiterApp.Model.Tasks.AET
{
    public class ResultsProgAet:ResultsTasks
    {
        public int CueNum { get; set; }

        public int WMLoad { get; set; }

        public string Field { get; set; }

        public double meanRT { get; set; }

        public double meanACC { get; set; }
    }
}
