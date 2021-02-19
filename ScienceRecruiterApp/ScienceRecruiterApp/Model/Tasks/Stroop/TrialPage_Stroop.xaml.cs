using ScienceRecruiterApp.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.Model.Tasks.Stroop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrialPage_Stroop : ContentPage
    {
        SettingsClass_Stroop settings = new SettingsClass_Stroop();
        Random rnd = new Random();
        public Condition_Stroop CurrCondition = new Condition_Stroop();
        public bool isPressed;
        public bool isCorrect;
        public bool isTerminated;
        Stopwatch stopwatcha = new Stopwatch();
        SavedTrials_Stroop SavedTrials = new SavedTrials_Stroop();
        public TrialPage_Stroop()
        {
            InitializeComponent();
            isTerminated = false;
            isPressed = false;
            DoSession();
        }
        
        protected override void OnDisappearing()
        {
            SaveResults();

            base.OnDisappearing();
            isTerminated = true;
            Navigation.PopToRootAsync();
        }

        private async void SaveResults()
        {
            List<ResultsStroop> listofRes;
            ResultsStroop res = new ResultsStroop();

            if (SavedTrials.TrialNum > 0)
            {
                if (SavedTrials.RT_Congr.Count > 0)
                {
                    res.ACCCongr = SavedTrials.ACC_Congr.Average();
                    res.mRTCongr = SavedTrials.RT_Congr.Average();
                }
                
                if (SavedTrials.RT_Incongr.Count > 0)
                {
                    res.ACCIncongr = SavedTrials.ACC_Incongr.Average();
                    res.mRTIncongr = SavedTrials.RT_Incongr.Average();
                }

                if (SavedTrials.RT_Control.Count > 0)
                {
                    res.ACCControl = SavedTrials.ACC_Control.Average();
                    res.mRTControl = SavedTrials.RT_Control.Average();
                }



                res.UserSpecKey = App.user.id;
                res.TotalTrials = SavedTrials.TrialNum;
                res.gender = App.user.gender;
                res.age = DateTime.Now.Year - App.user.age;
                res.hand = App.user.hand;

                DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                res.datePerf = currDate;

                if (res.ACCControl > 0.5) //check for ommission ratio
                {
                    Logic.ApiLogic apiLogic = new Logic.ApiLogic();
                    listofRes = await apiLogic.GetResults<ResultsStroop>(App.user.id, Constants.ResultsSstroopRetrieveUrl_id);

                    if (listofRes.Count > 0)
                    {
                        if (listofRes.Last().datePerf < currDate)
                        {
                            //save
                            apiLogic.PostResults<ResultsStroop>(res, Helpers.Constants.StroopPostUrl);
                        }
                        if (listofRes.Last().datePerf == currDate)
                        {
                            if (listofRes.Last().TotalTrials < SavedTrials.TrialNum)
                            {
                                //delete old, save new
                                apiLogic.PostResults<ResultsStroop>(res, Helpers.Constants.StroopPutUrl);
                            }
                        }
                    }
                    else
                    {
                        apiLogic.PostResults<ResultsStroop>(res, Helpers.Constants.StroopPostUrl);
                    }
                }
            }

        }

        private async void DoSession()
        {
            for (int i = 0; i < settings.TotalTrials; i++)
            {
                if (isTerminated)
                {
                    break;
                }
                await DoTrial(settings.TrialList[i]);
            }

        }

        private async Task DoTrial(int cond)
        {
            isPressed = false;
            CurrCondition = settings.Conditions[cond];
            WholeLayout.IsVisible = false;
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                ButtonLayout.IsVisible = true;
            });
            await Task.Run(() => System.Threading.Thread.Sleep(rnd.Next(settings.minITITime, settings.maxITITime)));
            WholeLayout.IsVisible = true;
            stopwatcha.Restart();
            Textlabel.Text = CurrCondition.TextName;
            Textlabel.TextColor = TranslateStringToColor(CurrCondition.ColorName);
            await Task.Delay(settings.maxWaitingTime, new CancellationToken(isPressed)).ContinueWith(_ =>
            {
                Evaluate();

            });
            await Task.Run(() => System.Threading.Thread.Sleep(500));
        }

        private void Evaluate()
        {
            if (!isPressed)
            {
                Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    Textlabel.Text = "Too Late!";
                    Textlabel.TextColor = Color.Black;
                });
                Savetrial(0, Convert.ToInt32(stopwatcha.ElapsedMilliseconds));
            }
            else
            {
                if (isCorrect)
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        Textlabel.Text = "Correct!";
                        Textlabel.TextColor = Color.Black;
                    });
                    Savetrial(1, Convert.ToInt32(stopwatcha.ElapsedMilliseconds));
                }
                else
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        Textlabel.Text = "Incorrect!";
                        Textlabel.TextColor = Color.Black;
                    });
                    Savetrial(0, Convert.ToInt32(stopwatcha.ElapsedMilliseconds));
                }
            }
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                ButtonLayout.IsVisible = false;
            });
        }

        private void Savetrial(int ACC, int RT)
        {
            switch (CurrCondition.ConditionName)
            {
                case "Congruent":
                    SavedTrials.ACC_Congr.Add(ACC);
                    SavedTrials.RT_Congr.Add(RT);
                    break;
                case "Incongruent":
                    SavedTrials.ACC_Incongr.Add(ACC);
                    SavedTrials.RT_Incongr.Add(RT);
                    break;
                case "Control":
                    SavedTrials.ACC_Control.Add(ACC);
                    SavedTrials.RT_Control.Add(RT);
                    break;

            }
            SavedTrials.TrialNum += 1;

        }

        private Color TranslateStringToColor(string colorname)
        {
            switch (colorname)
            {
                case "Red":
                    return Color.Red;
                case "Green":
                    return Color.Green;
                case "Blue":
                    return Color.Blue;
                default:
                    return Color.White;
            }
        }

        private void RedButton_Clicked(object sender, EventArgs e)
        {
            if (CurrCondition.ColorName == "Red")
            {
                isCorrect = true;
            }
            else
            {
                isCorrect = false;
            }
            isPressed = true;
        }

        private void GreenButton_Clicked(object sender, EventArgs e)
        {
            if (CurrCondition.ColorName == "Green")
            {
                isCorrect = true;
            }
            else
            {
                isCorrect = false;
            }
            isPressed = true;
        }

        private void BlueButton_Clicked(object sender, EventArgs e)
        {
            if (CurrCondition.ColorName == "Blue")
            {
                isCorrect = true;
            }
            else
            {
                isCorrect = false;
            }
            isPressed = true;
        }
    }
}