using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ScienceRecruiterApp.Model.Tasks.AET
{
    public class TrialClass_ProgAet
    {
        public Antecedent CenterImage { get; set; }
        public Consequent ConsequentA { get; set; }
        public Consequent ConsequentB { get; set; }
        public string CorrectLocation { get; set; }
        public int nTrials { get; set; }
        public int WMload { get; set; }
        public string  LocationA { get; set; }
        public string LocationB { get; set; }

        public TrialClass_ProgAet(Antecedent antecedent, Consequent A, Consequent B, string loc, int trialnum, int wmload)
        {
            CenterImage = antecedent;
            ConsequentA = A;
            ConsequentB = B;
            CorrectLocation = loc;
            nTrials = trialnum;
            WMload = wmload;
            if (antecedent.Name[0].ToString() == "X")
            {
                ConsequentA.Location = loc;
                ConsequentB.Location = changeplc(loc);
            }
            else
            {
                ConsequentB.Location = loc;
                ConsequentA.Location = changeplc(loc);
            }
        }

        private string changeplc(string v)
        {
            if (v == "Left")
            {
                return "Right";
            }
            else
            {
                return "Left";
            }
        }

    }


    public class Antecedent
    {
        public string CueImage { get; set; }
        public string Name { get; set; }
        public int CueLocation { get; set; }
        public int NumberofCues { get; set; }
        
    }

    public class Consequent
    {
        public string TargetChar { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        private string type1;

        public string Type1
        {
            get { return Name[0].ToString(); }
            set { type1 = value; }
        }

        private string type2;

        public string Type2
        {
            get { return Name[1].ToString(); }
            set { type2 = value; }
        }

        public int FontSize { get { return 16; } private set { } }

    }
}
