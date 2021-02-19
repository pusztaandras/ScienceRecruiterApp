using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScienceRecruiterApp.Model
{
    public class ResultsSST:ResultsTasks
    {

        [PrimaryKey]
        public int id { get; set; }

        public double meanRTLow { get; set; }

        public double meanRTHigh { get; set; }

        public double meanSSDLow { get; set; }

        public double meanSSDHigh { get; set; }

        public double mOmmRatioH { get; set; }

        public double mOmmRatioL { get; set; }

        public double mCommRatioH { get; set; }
        public double mCommRatioL { get; set; }

        public double SSRTiLow { get; set; }

        public double SSRTiHigh { get; set; }

        




    }
}
