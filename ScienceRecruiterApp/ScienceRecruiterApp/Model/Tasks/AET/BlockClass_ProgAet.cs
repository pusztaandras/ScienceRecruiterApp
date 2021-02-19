using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Linq;

namespace ScienceRecruiterApp.Model.Tasks.AET
{
    public class BlockClass_ProgAet
    {

        public int nCue { get; set; }
        public List<TrialClass_ProgAet> TrialList { get; set; }
        public int CueLocation { get; set; }
        public string IntroImage { get; set; }
        public Dictionary<string, Antecedent> antecedentDict { get; set; }
        public Dictionary<string, Consequent> consequentDict { get; set; }
        public List<string> ToExclude { get; set; }
        Random rnd = new Random();

        public BlockClass_ProgAet(int CueNumber, string Location, List<string> Exclude)
        {
            
            int idnum;
            nCue = CueNumber;
            ToExclude = Exclude;
            if (ToExclude == null)
            {
                ToExclude = new List<string>();
            }

            string X1Source;
            string X2Source;
            string X3Source;
            string Y1Source;
            string Y2Source;
            string Y3Source;

            antecedentDict = new Dictionary<string, Antecedent>();
            consequentDict = new Dictionary<string, Consequent>();
            //create Antecedents
            if (Location == "Right")
            {
                idnum = rnd.Next(1, CueNumber / 2);
            }
            else
            {
                idnum = rnd.Next((CueNumber / 2) + 1, CueNumber);
            }
            switch (idnum)
            {
                case 1:
                    X1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".1.png");
                    X2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".2.png");
                    X3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".3.png");
                    Y1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".4.png");
                    Y2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".5.png");
                    Y3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".6.png");
                    break;
                case 2:
                    X1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".1.png");
                    X2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".3.png");
                    X3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".4.png");
                    Y1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".2.png");
                    Y2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".5.png");
                    Y3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".6.png");
                    break;
                case 3:
                    X1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".1.png");
                    X2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".2.png");
                    X3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".4.png");
                    Y1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".3.png");
                    Y2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".5.png");
                    Y3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".6.png");
                    break;
                case 4:
                    X1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".1.png");
                    X2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".3.png");
                    X3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".5.png");
                    Y1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".2.png");
                    Y2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".4.png");
                    Y3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".6.png");
                    break;
                case 5:
                    X1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".1.png");
                    X2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".2.png");
                    X3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".5.png");
                    Y1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".3.png");
                    Y2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".4.png");
                    Y3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".6.png");
                    break;
                case 6:
                    X1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".1.png");
                    X2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".4.png");
                    X3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".5.png");
                    Y1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".2.png");
                    Y2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".3.png");
                    Y3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".6.png");
                    break;
                default:
                    X1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".1.png");
                    X2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".2.png");
                    X3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".3.png");
                    Y1Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".4.png");
                    Y2Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".5.png");
                    Y3Source = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures._", CueNumber.ToString(), ".6.png");
                    break;
            }

            if (idnum == CueNumber)
            {
                idnum = 1;
            }
            else
            {
                idnum += 1;
            }
            CueLocation = idnum;
            IntroImage = String.Concat("ScienceRecruiterApp.Model.Tasks.AET.Pictures.vege_", CueNumber.ToString(), idnum.ToString(), ".png");

            antecedentDict.Add("X1", new Antecedent { CueImage = X1Source, CueLocation = idnum, Name = "X1", NumberofCues = CueNumber });
            antecedentDict.Add("X2", new Antecedent { CueImage = X2Source, CueLocation = idnum, Name = "X2", NumberofCues = CueNumber });
            antecedentDict.Add("X3", new Antecedent { CueImage = X3Source, CueLocation = idnum, Name = "X3", NumberofCues = CueNumber });
            antecedentDict.Add("Y1", new Antecedent { CueImage = Y1Source, CueLocation = idnum, Name = "Y1", NumberofCues = CueNumber });
            antecedentDict.Add("Y2", new Antecedent { CueImage = Y2Source, CueLocation = idnum, Name = "Y2", NumberofCues = CueNumber });
            antecedentDict.Add("Y3", new Antecedent { CueImage = Y3Source, CueLocation = idnum, Name = "Y3", NumberofCues = CueNumber });

            //Creating Consequents A
            ConsequentAdd("A1");
            ConsequentAdd("A2");
            ConsequentAdd("B1");
            ConsequentAdd("B2");



            TrialList = new List<TrialClass_ProgAet>();
            //First block:trialanderror
            TrialList_ProgAet temp = new TrialList_ProgAet(12, 2, antecedentDict, consequentDict);
            TrialList.AddRange(temp.TrialList);
            //First block consequents=2
            temp = new TrialList_ProgAet(48, 2, antecedentDict, consequentDict);
            TrialList.AddRange(temp.TrialList);
            //Second block:trialanderror
            temp = new TrialList_ProgAet(24, 4, antecedentDict, consequentDict);
            TrialList.AddRange(temp.TrialList);
            //First block consequents=2
            temp = new TrialList_ProgAet(48, 2, antecedentDict, consequentDict);
            TrialList.AddRange(temp.TrialList);
        }

        private void ConsequentAdd(string v)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string selected = chars[rnd.Next(0, chars.Length - 1)].ToString();
            if (ToExclude != null && ToExclude.Any())
            {
                while (ToExclude.Contains(selected))
                {
                    selected = chars[rnd.Next(0, chars.Length - 1)].ToString();
                }
            }
            
            consequentDict.Add(v, new Consequent { Name = v, TargetChar = selected });
            ToExclude.Add(selected);
        }
    }
}
