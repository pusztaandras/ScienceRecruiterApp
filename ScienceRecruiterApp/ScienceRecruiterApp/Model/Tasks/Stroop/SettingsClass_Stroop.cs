using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceRecruiterApp.Model.Tasks.Stroop
{
    public class Condition_Stroop
    {
        public string ColorName { get; set; }
        public string TextName { get; set; }
        public string ConditionName { get; set; }
    }
    public class SettingsClass_Stroop
    {
        public int TotalTrials { get; set; }

        public int nTrialCong { get; set; }

        public int nTrialIncong { get; set; }

        public int nTrialControl { get; set; }

        public List<int> TrialList { get; set; }

        public Dictionary<int, Condition_Stroop> Conditions { get; set; }

        public int minITITime { get; set; }

        public int maxITITime { get; set; }

        public int maxWaitingTime { get; set; }

        public int SameinRow;

        public int SameTypeinRow;

        public int PrevCondition;

        public string PrevType;
        public Random rnd = new Random();

        public SettingsClass_Stroop()
        {
            Conditions = new Dictionary<int, Condition_Stroop>();
            //Control Conditions:
            Conditions.Add(1, new Condition_Stroop { ColorName = "Red", TextName = "######", ConditionName="Control" });
            Conditions.Add(2, new Condition_Stroop { ColorName = "Green", TextName = "######", ConditionName = "Control" });
            Conditions.Add(3, new Condition_Stroop { ColorName = "Blue", TextName = "######", ConditionName = "Control" });
            //Congruent Conditions:
            Conditions.Add(4, new Condition_Stroop { ColorName = "Red", TextName = "Red", ConditionName = "Congruent" });
            Conditions.Add(5, new Condition_Stroop { ColorName = "Green", TextName = "Green", ConditionName = "Congruent" });
            Conditions.Add(6, new Condition_Stroop { ColorName = "Blue", TextName = "Blue", ConditionName = "Congruent" });
            //Incongruent Conditions:
            Conditions.Add(7, new Condition_Stroop { ColorName = "Red", TextName = "Green", ConditionName = "Incongruent" });
            Conditions.Add(8, new Condition_Stroop { ColorName = "Red", TextName = "Blue", ConditionName = "Incongruent" });
            Conditions.Add(9, new Condition_Stroop { ColorName = "Green", TextName = "Red", ConditionName = "Incongruent" });
            Conditions.Add(10, new Condition_Stroop { ColorName = "Green", TextName = "Blue", ConditionName = "Incongruent" });
            Conditions.Add(11, new Condition_Stroop { ColorName = "Green", TextName = "Red", ConditionName = "Incongruent" });
            Conditions.Add(12, new Condition_Stroop { ColorName = "Green", TextName = "Blue", ConditionName = "Incongruent" });

            minITITime = 300;
            maxITITime = 500;
            maxWaitingTime = 1500;
            TotalTrials = 180;

            TrialList = new List<int>();
            string CurrType;
            for(int i=1; i<=180; i++)
            {
                int x = rnd.Next(1, 12);
                if (x > 6)
                {
                    CurrType = "Incongruent";
                }
                else if (x > 3)
                {
                    CurrType = "Congruent";
                }
                else
                {
                    CurrType = "Control";
                }
                if (i > 1) //Check with previous types
                {
                    if (PrevCondition == x)
                    {
                        SameinRow += 1;

                    }
                    if (PrevType == CurrType)
                    {
                        SameTypeinRow += 1;
                    }
                    if(SameinRow>2 || SameTypeinRow > 3)
                    {
                        SameinRow = 0;
                        SameTypeinRow = 0;
                        i -= 1;
                    }
                    else
                    {
                        TrialList.Add(x);
                        PrevCondition = x;
                        PrevType = CurrType;
                    }
                }
                else
                {
                    TrialList.Add(x);
                    PrevCondition = x;
                    PrevType = CurrType;
                }

                
                
            }


        }
    }
}
