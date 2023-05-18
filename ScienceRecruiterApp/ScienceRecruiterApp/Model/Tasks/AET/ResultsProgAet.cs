using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;

namespace ScienceRecruiterApp.Model.Tasks.AET
{
    public class ResultsProgAet:ResultsTasks
    {
        [PrimaryKey]
        [JsonIgnore]
        public int id { get; set; }

        public int CueNum { get; set; }

        public int WMLoad { get; set; }

        public string Field { get; set; }

        public double meanRT { get; set; }

        public double meanACC { get; set; }
    }
}
