using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ScienceRecruiterApp.Model;
using Xamarin.Forms;

namespace ScienceRecruiterApp
{
    public class SettingsClass_SST
    {

        public string gender { get; set; }

        public int age { get; set; }
        public bool isDisorder { get; set; }

        public bool isDrug { get; set; }

        public string Disorder { get; set; }

        public string Drug { get; set; }
        public int TrialNumGoHighR { get; set; }
        public int TrialNumGoLowR { get; set; }
        public int TrialNumStopHighR { get; set; }
        public int TrialNumStopLowR { get; set; }

        public int TrialNumGoHighL { get; set; }
        public int TrialNumGoLowL { get; set; }
        public int TrialNumStopHighL { get; set; }
        public int TrialNumStopLowL { get; set; }

        public int FeedbackTime { get; set; }

        public int minCueTime { get; set; }
        public int maxCueTime { get; set; }

        public int StartSSD { get; set; }

        public int SSDWrong { get; set; }

        public int SSDCorrect { get; set; }

        public FileStream File { get; set; }

        public int BlockNum { get; set; }

        public int minITITime { get; set; }
        public int maxITITime { get; set; }
        public int maxWaitingTime { get; set; }

        public int TotalTrials { get; set; }

        public int TrialsPerf { get; set; }


        public SettingsClass_SST()
        {
           
            Go = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.Go.bmp");
            GoR = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoR.bmp");
            GoL = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoL.bmp");
            Stop = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.Stop.bmp");
            High = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.High.bmp");
            Low = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.Low.bmp");


            ////////////////////////////////////////////////////////
            /////////Settings (change here if necessarry)//////////
            ///////////////////////////////////////////////////////

            //////Trial numbers of one block:////////
            TrialNumGoHighR = 12;
            TrialNumGoHighL = 12;
            TrialNumGoLowR = 15;
            TrialNumGoLowL = 15;
            TrialNumStopHighR = 8;
            TrialNumStopHighL = 8;
            TrialNumStopLowR = 5;
            TrialNumStopLowL = 5;


            BlockNum = 5; //number of blocks

            TotalTrials = BlockNum * (TrialNumGoHighR + TrialNumGoHighL + TrialNumGoLowR + TrialNumGoLowL + TrialNumStopHighR + TrialNumStopHighL + TrialNumStopLowR + TrialNumStopLowL);


            ////Debug-mode:uncomment these:
            //TrialNumGoHighR = 1;
            //TrialNumGoLowR = 1;
            //TrialNumStopHighR = 1;
            //TrialNumStopLowR = 1;
            //TrialNumGoHighL = 1;
            //TrialNumGoLowL = 1;
            //TrialNumStopHighL = 1;
            //TrialNumStopLowL = 1;

            //////Timing/////
            minCueTime = 700;
            maxCueTime = 1000;
            
            maxWaitingTime = 1000;
            StartSSD = 250;
            SSDCorrect = 50;
            SSDWrong = 50;

            FeedbackTime = 10000;
            
            minITITime = 500;
            maxITITime = 800;
            GetTrialsPerf();
            
        }

        private async void GetTrialsPerf()
        {
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            List<ResultsSST> listofRes = new List<ResultsSST>();
            listofRes = await apiLogic.GetResults<ResultsSST>(App.user.id, Helpers.Constants.ResultsSSTRetrieveUrl_id);
            listofRes = listofRes.Where(a => a.UserSpecKey == App.user.id).ToList();
            if (listofRes.Count > 0)
            {
                if (listofRes.Last().datePerf < currDate)
                {

                    TrialsPerf = 0;
                }
                else
                {
                    TrialsPerf = listofRes.Last().TotalTrials;
                }
            }
            else
            {
                TrialsPerf = 0;
            }
        }

        public ImageSource Low;
        public ImageSource High;
        public ImageSource Go;
        public ImageSource GoL;
        public ImageSource GoR;

        public ImageSource Stop;
    }

    
    
}
