using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScienceRecruiterApp.Model
{
    public class ResultsSST:ResultsTasks
    {

        [PrimaryKey]
        [JsonIgnore]
        public int? id { get; set; }

        public int meanRTLow { get; set; }

        public int meanRTHigh { get; set; }

        public int meanSSDLow { get; set; }

        public int meanSSDHigh { get; set; }

        public double mOmmRatioH { get; set; }

        public double mOmmRatioL { get; set; }

        public double mCommRatioH { get; set; }
        public double mCommRatioL { get; set; }

        public int SSRTiLow { get; set; }

        public int SSRTiHigh { get; set; }

        




    }
}
