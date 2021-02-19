using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.Model.Tasks;
using ScienceRecruiterApp.Model.Tasks.SST;
using ScienceRecruiterApp.View;
using ScienceRecruiterApp.ViewModel;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrialPage_SST : ContentPage
    {
        public int CueTime;
        public SettingsClass_SST settings;
        public int SSDL;
        public int SSDH;
        public int RTime;
        public Random rnd = new Random();
        Stopwatch stopwatcha = new Stopwatch();
        OutputVar outvar = new OutputVar();
        public EventWaitHandle WaitToPress;
        public string State;
        string TrialType;
        string TrialType2;
        public string TrialType3;
        bool canPress = false;
        public bool isPressed;
        public bool isCorrect;
        public int currTrialNum;
        public int sameTrials = 0;
        public BlockVarClass BlockVar;
        public int currBlockNum;
        public string pressedKey;
        public Xamarin.Forms.Color fill;
        public float radius;
        public bool isTerminated;


        public TrialPage_SST(SettingsClass_SST sett)
        {
            InitializeComponent();
            WaitToPress = new EventWaitHandle(false, EventResetMode.AutoReset);
            settings = sett;
            SSDL = settings.StartSSD;
            SSDH = settings.StartSSD;
            outvar.SSDH = SSDH;
            outvar.SSDL = SSDL;
            using (SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation))
            {
                conn.CreateTable<SavedTrials>();
                conn.DeleteAll<SavedTrials>();
            }
            radius = Convert.ToSingle(((Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height) / 2) * 0.5);
            currBlockNum = 1;
            DoSession();

            Screen.PaintSurface += Screen_PaintSurface;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }

        protected override bool OnBackButtonPressed()
        {
            isTerminated = true;
            return base.OnBackButtonPressed();

        }

        protected override async void OnDisappearing()
        {
            MessagingCenter.Send(this, "PreventLandscape");
            base.OnDisappearing();
            await Navigation.PopToRootAsync();
            if (isTerminated)
            {
                SaveAsync();
            }





        }

        private async Task SaveAsync()
        {
            if (currTrialNum > 16)
            {

                List<SavedTrials> listofTrials;
                List<ResultsSST> listofRes;
                ///////Calculate the results of the current session /////
                ResultsSST res = new ResultsSST();
                using (SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation))
                {
                    conn.CreateTable<SavedTrials>();
                    listofTrials = conn.Table<SavedTrials>().ToList();

                }
                listofTrials.RemoveRange(0, 16);
                DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                res.datePerf = currDate;
                if (listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "Low").Count() > 0)
                {
                    res.meanRTLow = listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "Low").Average(s => s.RT);
                    res.mOmmRatioL = listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "Low").Average(s => s.Ommission);
                }
                else
                {
                    res.meanRTLow = 0;
                    res.mOmmRatioL = 1;
                }
                if (listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "High").Count() > 0)
                {
                    res.meanRTHigh = listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "High").Average(s => s.RT);
                    res.mOmmRatioH = listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "High").Average(s => s.Ommission);
                }
                else
                {
                    res.meanRTHigh = 0;
                    res.meanSSDHigh = 0;
                    res.mOmmRatioH = 1;
                    res.mCommRatioH = 1;
                }


                if (listofTrials.Where(a => a.TrialType == "Stop" && a.TrialType2 == "Low").Count() > 0)
                {
                    res.meanSSDLow = listofTrials.Where(a => a.TrialType == "Stop" && a.TrialType2 == "Low").Average(s => s.SSDL);
                    res.mCommRatioL = listofTrials.Where(a => a.TrialType == "Stop" && a.TrialType2 == "Low").Average(s => s.Commission);
                }
                else
                {
                    res.meanSSDLow = 0;
                    res.mCommRatioL = 1;
                }
                if (listofTrials.Where(a => a.TrialType == "Stop" && a.TrialType2 == "High").Count() > 0)
                {
                    res.meanSSDHigh = listofTrials.Where(a => a.TrialType == "Stop" && a.TrialType2 == "High").Average(s => s.SSDH);
                    res.mCommRatioH = listofTrials.Where(a => a.TrialType == "Stop" && a.TrialType2 == "High").Average(s => s.Commission);
                }
                else
                {
                    res.meanSSDHigh = 0;
                    res.mCommRatioH = 1;
                }

                //SSRT calculation
                if (listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "High").Count() > 0)
                {
                    List<int> rtH = listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "High").Select(d => d.RT).ToList();
                    for (int i = 0; i <= listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "High").Sum(u => u.Ommission); i++)
                    {
                        rtH.Add(App.settings_sst.maxWaitingTime);
                    }
                    rtH.Sort();
                    int idx_H = Convert.ToInt32(Math.Round(rtH.Count() * res.mCommRatioH));
                    if (idx_H != 0) idx_H -= 1;
                    res.SSRTiHigh = rtH.ElementAt(idx_H) - res.meanSSDHigh;
                }
                else
                {
                    res.SSRTiHigh = 0;
                }

                if (listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "Low").Count() > 0)
                {
                    List<int> rtL = listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "Low").Select(d => d.RT).ToList();
                    for (int i = 0; i <= listofTrials.Where(a => a.TrialType == "Go" && a.TrialType2 == "Low").Sum(u => u.Ommission); i++)
                    {
                        rtL.Add(App.settings_sst.maxWaitingTime);
                    }
                    rtL.Sort();
                    int idx_L = Convert.ToInt32(Math.Round(rtL.Count() * res.mCommRatioL));
                    if (idx_L != 0) idx_L -= 1;
                    res.SSRTiLow = rtL.ElementAt(idx_L) - res.meanSSDLow;
                }
                else
                {
                    res.SSRTiLow = 0;
                }
                res.UserSpecKey = App.user.id;
                res.TotalTrials = listofTrials.Count();
                res.gender = App.user.gender;
                res.age = DateTime.Now.Year - App.user.age;
                res.hand = App.user.hand;
                //Check if performed trial num>previously perfomed trial in the current month
                Logic.ApiLogic apiLogic = new Logic.ApiLogic();
                bool isNew = true;
                bool isUpload = true;
                if (res.mOmmRatioH < 0.5 || res.mOmmRatioL < 0.5 || res.mCommRatioH < 0.8 || res.mCommRatioL < 0.8) //check for ommission ratio
                {

                    listofRes = await apiLogic.GetResults<ResultsSST>(App.user.id, Helpers.Constants.ResultsSSTRetrieveUrl_id);
                    listofRes = listofRes.Where(a => a.UserSpecKey == App.user.id).ToList();
                    //listofRes = await App.client.GetTable<ResultsSST>().Where(a => a.UserSpecKey == App.user.id).ToListAsync();

                    if (listofRes.Count > 0)
                    {
                        if (listofRes.Last().datePerf < currDate)
                        {
                            //save
                            apiLogic.PostResults<ResultsSST>(res, Helpers.Constants.SSTPostUrl);
                            isNew = true;
                            isUpload = true;
                        }
                        if (listofRes.Last().datePerf == currDate)
                        {
                            if (listofRes.Last().TotalTrials < listofTrials.Count())
                            {
                                //delete old, save new
                                //add here
                                apiLogic.PostResults<ResultsSST>(res, Helpers.Constants.SSTPutUrl);
                                isNew = false;
                                isUpload = true;
                            }
                            else
                            {
                                isUpload = false;

                            }
                        }
                    }
                    else
                    {
                        apiLogic.PostResults<ResultsSST>(res, Helpers.Constants.SSTPostUrl);
                        isNew = true;
                        isUpload = true;
                    }
                }

                //Get the id of the saved ResultsSST and Save the detailed results
                listofRes = await apiLogic.GetResults<ResultsSST>(App.user.id, Helpers.Constants.ResultsSSTRetrieveUrl_id);
                int id = listofRes.Where(a => a.UserSpecKey == App.user.id).ToList().LastOrDefault().id;
                if (!isNew)
                {
                    apiLogic.DeleteResults(id.ToString(), Helpers.Constants.DetailedSSTDeleteUrl);
                }
                if (isUpload)
                {
                    foreach (SavedTrials savedTrials in listofTrials)
                    {
                        DetailedResultsSST detailedResultsSST = new DetailedResultsSST();
                        detailedResultsSST.Commission = savedTrials.Commission;
                        detailedResultsSST.Ommission = savedTrials.Ommission;
                        detailedResultsSST.CueTime = savedTrials.CueTime;
                        detailedResultsSST.metaid = id;
                        detailedResultsSST.PressedKey = savedTrials.PressedKey;
                        detailedResultsSST.RT = savedTrials.RT;
                        detailedResultsSST.SSDH = savedTrials.SSDH;
                        detailedResultsSST.SSDL = savedTrials.SSDL;
                        detailedResultsSST.TrialType = savedTrials.TrialType;
                        detailedResultsSST.TrialType2 = savedTrials.TrialType2;
                        detailedResultsSST.TrialType3 = savedTrials.TrialType3;

                        apiLogic.PostResults<DetailedResultsSST>(detailedResultsSST, Helpers.Constants.DetailedSSTPostUrl);
                    }
                }


            }
        }



        private async Task DoSession()
        {
            for (int i = 0; i < settings.BlockNum; i++)
            {
                List<TrialList_SST> trialList = CreateTrialList();
                if (isTerminated)
                {
                    break;
                }

                await DoBlock(trialList);
                currBlockNum++;
            }
            if (!isTerminated)
            {
                await SaveAndExit();
            }
        }

        private async Task SaveAndExit()
        {

            GoodByePage page = new GoodByePage("Stop Signal Task");
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            await Navigation.PushModalAsync(page);
            page.Disappearing += (sender2, e2) =>
            {
                waitHandle.Set();
            };
            await Task.Run(() => waitHandle.WaitOne());
            await Navigation.PopToRootAsync();
            SaveAsync();
        }

        private async Task DoBlock(List<TrialList_SST> trialList)
        {

            for (int i = 0; i < trialList.Count; i++)
            {

                if (isTerminated)
                {
                    break;
                }

                await DoTrial(trialList[i]);

            }
            if (!isTerminated)
            {
                Feedback();
            }

        }



        private async Task DoTrial(TrialList_SST trialList_SST)
        {
            State = "blank";
            WaitToPress.Reset();

            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
            isPressed = false;
            int itiTime = rnd.Next(settings.minITITime, settings.maxITITime);
            await Task.Run(() => System.Threading.Thread.Sleep(itiTime));

            if (trialList_SST.TrialType == "Stop")
            {
                await DoStopTrial(trialList_SST);
            }
            else
            {
                await DoGoTrial(trialList_SST);
            }
            if (currTrialNum > 16)
            {
                Device.BeginInvokeOnMainThread(() => Progress.Progress = Convert.ToDouble(Convert.ToDouble(currTrialNum - 16) / Convert.ToDouble(settings.TotalTrials)));
            }

            currTrialNum += 1;
        }

        private async Task DoStopTrial(TrialList_SST trialList_SST)
        {
            canPress = false;
            State = trialList_SST.TrialType2;
            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
            CueTime = rnd.Next(settings.minCueTime, settings.maxCueTime);
            await Task.Run(() => System.Threading.Thread.Sleep(CueTime));
            stopwatcha.Restart();
            State = trialList_SST.TrialType3;
            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
            canPress = true;

            if (String.Equals(trialList_SST.TrialType2, "High"))
            {
                await Task.Run(() => System.Threading.Thread.Sleep(SSDH));
            }
            else
            {
                await Task.Run(() => System.Threading.Thread.Sleep(SSDL));
            }
            State = "Stop";
            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());

            await Task.Run(() => WaitToPress.WaitOne(settings.maxWaitingTime));

            EvaluateTrial(trialList_SST);
        }

        private void EvaluateTrial(TrialList_SST trialList_SST)
        {

            SavedTrials currSave = new SavedTrials();
            currSave.TrialType = trialList_SST.TrialType;
            currSave.TrialType2 = trialList_SST.TrialType2;
            currSave.TrialType3 = trialList_SST.TrialType3;
            currSave.SSDL = SSDL;
            currSave.SSDH = SSDH;
            currSave.CueTime = CueTime;
            currSave.PressedKey = pressedKey;

            if (trialList_SST.TrialType == "Stop")
            {

                if (isPressed) //commission error
                {
                    currSave.Ommission = 0;
                    currSave.Commission = 1;
                    currSave.RT = RTime;
                    if (String.Equals(trialList_SST.TrialType2, "High"))
                    {
                        if (SSDH > settings.SSDWrong + 10)
                        {
                            SSDH -= settings.SSDWrong;
                        }

                    }
                    else
                    {
                        if (SSDL > settings.SSDWrong + 10)
                        {
                            SSDL -= settings.SSDWrong;
                        }
                    }
                }
                else
                {
                    currSave.Ommission = 0;
                    currSave.Commission = 0;
                    currSave.RT = 0;
                    if (String.Equals(trialList_SST.TrialType2, "High"))
                    {
                        SSDH += settings.SSDCorrect;
                    }
                    else
                    {
                        SSDL += settings.SSDCorrect;
                    }
                }
            }
            else // Go trial
            {
                if (isPressed)
                {
                    currSave.Ommission = 0;
                    currSave.Commission = 0;
                    currSave.RT = RTime;
                }
                else
                {
                    currSave.Ommission = 1;
                    currSave.Commission = 0;
                    currSave.RT = settings.maxWaitingTime;
                }
            }

            using (SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation))
            {
                conn.CreateTable<SavedTrials>();
                conn.Insert(currSave);

            }

        }

        private async Task DoGoTrial(TrialList_SST trialList_SST)
        {
            canPress = false;
            State = trialList_SST.TrialType2;
            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
            int cueTime = rnd.Next(settings.minCueTime, settings.maxCueTime);
            await Task.Run(() => System.Threading.Thread.Sleep(cueTime));
            stopwatcha.Restart();
            State = trialList_SST.TrialType3;
            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
            canPress = true;
            await Task.Run(() => WaitToPress.WaitOne(settings.maxWaitingTime));

            EvaluateTrial(trialList_SST);
        }

        private List<TrialList_SST> CreateTrialList()
        {
            List<TrialList_SST> TrialTypes = new List<TrialList_SST>();
            //High probability of Stop trials
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoHighL, TrialType = "Go", TrialType2 = "High", TrialType3 = "Left" }); //GoLeft
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoHighR, TrialType = "Go", TrialType2 = "High", TrialType3 = "Right" }); //GoRight
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopHighL, TrialType = "Stop", TrialType2 = "High", TrialType3 = "Left" }); //Stop Left
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopHighR, TrialType = "Stop", TrialType2 = "High", TrialType3 = "Right" }); //Stop Right
                                                                                                                                                          //Low probability of Stop trials
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoLowL, TrialType = "Go", TrialType2 = "Low", TrialType3 = "Left" }); //GoLeft
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoLowR, TrialType = "Go", TrialType2 = "Low", TrialType3 = "Right" });//GoRight
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopLowL, TrialType = "Stop", TrialType2 = "Low", TrialType3 = "Left" }); //StopLeft
            TrialTypes.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopLowR, TrialType = "Stop", TrialType2 = "Low", TrialType3 = "Right" }); //StopRight

            List<TrialList_SST> TrialList = new List<TrialList_SST>();
            int consecutivestop = 0;
            int consecutivego = 0;

            //generate triallist
            // 1. Add Practice trials
            if (currBlockNum == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    TrialList.Add(new TrialList_SST() { TrialType = "Go", TrialType2 = "High", TrialType3 = "Left" });
                    TrialList.Add(new TrialList_SST() { TrialType = "Go", TrialType2 = "High", TrialType3 = "Right" });
                    TrialList.Add(new TrialList_SST() { TrialType = "Go", TrialType2 = "Low", TrialType3 = "Left" });
                    TrialList.Add(new TrialList_SST() { TrialType = "Go", TrialType2 = "Low", TrialType3 = "Right" });
                }
                TrialList.OrderBy(x => rnd.Next());
            }

            for (int i = 1; i <= settings.TotalTrials / settings.BlockNum; i++)
            {
                int currtype = rnd.Next(TrialTypes.Count);
                if (consecutivestop >= 3 && TrialTypes.Where(a => a.TrialType == "Stop").ToList().Count < TrialTypes.Count)
                {
                    while (TrialTypes[currtype].TrialType == "Stop")
                    {
                        currtype = rnd.Next(TrialTypes.Count);
                    }
                }
                if (consecutivego >= 8 && TrialTypes.Where(a => a.TrialType == "Go").ToList().Count < TrialTypes.Count)
                {
                    while (TrialTypes[currtype].TrialType == "Go")
                    {
                        currtype = rnd.Next(TrialTypes.Count);
                    }
                }

                TrialList.Add(TrialTypes[currtype]);
                if (TrialTypes[currtype].TrialType == "Stop")
                {
                    consecutivestop += 1;
                    consecutivego = 0;
                }
                else
                {
                    consecutivego += 1;
                    consecutivestop = 0;
                }
                TrialTypes[currtype].TrialNum -= 1;
                if (TrialTypes[currtype].TrialNum == 0)
                {
                    TrialTypes.RemoveAt(currtype);
                }

            }
            return TrialList;
        }

        private async void Feedback()
        {
            List<SavedTrials> savedTrials;
            using (SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation))
            {
                conn.CreateTable<SavedTrials>();
                savedTrials = conn.Table<SavedTrials>().ToList();
            }
            Screen.IsVisible = false;
            FeedbackLayout.IsVisible = true;
            Accuracy.Text = String.Concat("Accuracy=", (100 * (1 - savedTrials.Where(a => a.TrialType == "Stop").Average(x => x.Commission))).ToString(), "%");
            RT.Text = String.Concat("Average reaction time=", Math.Round(savedTrials.Where(a => a.TrialType == "Go").Average(x => x.RT)).ToString(), "ms");

            if ((1 - savedTrials.Where(a => a.TrialType == "Stop").Average(x => x.Commission)) < 0.4)
            {
                Accuracy.TextColor = Xamarin.Forms.Color.Red;
                if (savedTrials.Where(a => a.TrialType == "Go").Average(x => x.RT) > 600)
                {
                    RT.TextColor = Xamarin.Forms.Color.Red;
                    Additional.Text = "You are too slow and inaccurate! Pay attention!";
                    Additional.TextColor = Xamarin.Forms.Color.Red;
                }
                else
                {
                    RT.TextColor = Xamarin.Forms.Color.Green;
                    Additional.Text = "Fast but inaccurate! Pay attention!";
                    Additional.TextColor = Xamarin.Forms.Color.Yellow;
                }

            }
            else
            {
                Accuracy.TextColor = Xamarin.Forms.Color.Green;
                if (savedTrials.Where(a => a.TrialType == "Go").Average(x => x.RT) > 600)
                {
                    RT.TextColor = Xamarin.Forms.Color.Red;
                    Additional.Text = "You are too slow! Try faster!";
                    Additional.TextColor = Xamarin.Forms.Color.Yellow;
                }
                else
                {
                    RT.TextColor = Xamarin.Forms.Color.Green;
                    Additional.Text = "Outstanding! You are a Jack Bauer! Keep goin'!";
                    Additional.TextColor = Xamarin.Forms.Color.Green;
                }
            }


            await Task.Run(() => WaitToPress.WaitOne(settings.FeedbackTime));
            FeedbackLayout.IsVisible = false;
            Screen.IsVisible = true;
        }

        private void Screen_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            float x = info.Width / 2;
            float y = info.Height / 2;
            SKPoint center = new SKPoint(x, y);

            SKRect rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);

            SKPath path = new SKPath();

            switch (State)
            {
                case "Left":
                    //GoLeft
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 90, 180, false);
                    path.Close();

                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.Green
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });

                    //White half
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 90 + 180, 180, false);
                    path.Close();
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.White
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });
                    break;
                case "Right":
                    //GoRight
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 270, 180, false);
                    path.Close();

                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.Green
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });

                    //White half
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 90, 180, false);
                    path.Close();
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.White
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });
                    break;
                case "High":
                    //High
                    canvas.DrawCircle(new SKPoint(center.X, center.Y), radius, new SKPaint
                    {
                        Color = SKColors.Black,
                        Style = SKPaintStyle.Fill
                    });
                    break;
                case "Low":
                    //High
                    canvas.DrawCircle(new SKPoint(center.X, center.Y), radius, new SKPaint
                    {
                        Color = SKColors.Gray,
                        Style = SKPaintStyle.Fill
                    });
                    break;
                case "Stop":
                    //High
                    canvas.DrawCircle(center, radius, new SKPaint
                    {
                        Color = SKColors.Red,
                        Style = SKPaintStyle.Fill
                    });
                    break;
                case "blank":
                    canvas.Clear();
                    break;
            }
        }

        public void HalfCircle(SKPoint center, SKCanvas canvas, int v)
        {
            SKRect rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);

            //Green half
            SKPath path = new SKPath();
            path.MoveTo(center);
            path.ArcTo(rect, v, 180, false);
            path.Close();

            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Green
            });
            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black
            });

            //White half
            path = new SKPath();
            path.MoveTo(center);
            path.ArcTo(rect, v + 180, 180, false);
            path.Close();
            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            });
            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black
            });
        }

        private void LeftTapped(object sender, EventArgs e)
        {
            if (canPress)
            {
                RTime = Convert.ToInt32(stopwatcha.ElapsedMilliseconds);
                isPressed = true;
                pressedKey = "Left";
                WaitToPress.Set();
            }

        }

        private void RightTapped(object sender, EventArgs e)
        {
            if (canPress)
            {
                RTime = Convert.ToInt32(stopwatcha.ElapsedMilliseconds);
                isPressed = true;
                pressedKey = "Right";
                WaitToPress.Set();

            }

        }

        private void Exit_Tapped(object sender, EventArgs e)
        {
            isTerminated = true;

        }
    }
}