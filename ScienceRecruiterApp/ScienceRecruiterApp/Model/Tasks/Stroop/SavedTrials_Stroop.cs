using System;
using System.Collections.Generic;
using System.Text;

namespace ScienceRecruiterApp.Model.Tasks.Stroop
{
    public class SavedTrials_Stroop
    {

        public List<int> RT_Congr { get; set; }

        public List<int> RT_Incongr { get; set; }

        public List<int> RT_Control { get; set; }

        public List<int> ACC_Congr { get; set; }

        public List<int> ACC_Incongr { get; set; }

        public List<int> ACC_Control { get; set; }
        public int TrialNum { get; set; }

        public SavedTrials_Stroop()
        {
            RT_Congr = new List<int>();
            RT_Control = new List<int>();
            RT_Incongr = new List<int>();
            ACC_Congr = new List<int>();
            ACC_Incongr = new List<int>();
            ACC_Control = new List<int>();
            TrialNum = 0;
        }
    }
}
