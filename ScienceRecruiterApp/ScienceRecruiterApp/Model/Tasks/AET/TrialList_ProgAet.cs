using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceRecruiterApp.Model.Tasks.AET
{
    public class TrialList_ProgAet
    {
        public List<TrialClass_ProgAet> TrialList { get; private set; }
        public int previoustye;
        public int currtype;
        public Random rnd;
        public TrialList_ProgAet(int NTrials, int NConsequents, Dictionary<string, Antecedent> AntecedentDict, Dictionary<string, Consequent> ConsequentDict)
        {
            rnd = new Random();
            List<TrialClass_ProgAet> TrialTypes = new List<TrialClass_ProgAet>();
            int ncond = Convert.ToInt32(Math.Floor(Convert.ToDouble(NTrials / (6 * NConsequents))));
            for (int a = 1; a <= NConsequents/2; a++)
            {
               
                Consequent A = ConsequentDict[String.Concat("A", a.ToString())];
                Consequent B = ConsequentDict[String.Concat("B", a.ToString())];
                for (int i = 1; i <= 3; i++)
                {
                    Antecedent antecedent = AntecedentDict[String.Concat("X", i.ToString())];
                    TrialTypes.Add(new TrialClass_ProgAet(antecedent, A, B, "Left", ncond, a*2));
                    TrialTypes.Add(new TrialClass_ProgAet(antecedent, A, B, "Right", ncond, a*2));
                }
                for (int i = 1; i <= 3; i++)
                {
                    Antecedent antecedent = AntecedentDict[String.Concat("Y", i.ToString())];
                    TrialTypes.Add(new TrialClass_ProgAet(antecedent, A, B, "Left", ncond, a*2));
                    TrialTypes.Add(new TrialClass_ProgAet(antecedent, A, B, "Right", ncond, a*2));
                }
            }

            TrialList = new List<TrialClass_ProgAet>();


            for (int i = 1; i <= NTrials; i++)
            {
                if (i > 1 && TrialTypes.Count>1)
                {
                    currtype = rnd.Next(TrialTypes.Count);
                    while (currtype == previoustye)
                    {
                       currtype = rnd.Next(0, TrialTypes.Count);
                    }
                    TrialList.Add(TrialTypes[currtype]);
                    TrialTypes[currtype].nTrials -= 1;
                    if (TrialTypes[currtype].nTrials == 0)
                    {
                        TrialTypes.RemoveAt(currtype);
                    }
                    previoustye = currtype;

                }
                else
                {
                    currtype = rnd.Next(TrialTypes.Count);
                    TrialList.Add(TrialTypes[currtype]);
                    TrialTypes[currtype].nTrials -= 1;
                    previoustye = currtype;
                }
            }
        }
    }
}
