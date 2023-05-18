using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.Model.Tasks.AET
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrialPage_ProgAet : ContentPage
    {
        //SettingsClass_ProgAet settings = new SettingsClass_ProgAet();
        List<TrialRes> BlockRes;
        List<ResultsProgAet> TaskResults;
        public string pressedLoc;
        Random rnd = new Random();
        public bool isPressed;
        public bool isCorrect;
        public bool isTerminated;
        Stopwatch stopwatcha = new Stopwatch();
        List<string> ToExclude;
        public EventWaitHandle WaitToPress;
        public int currblocknum;

        public TrialPage_ProgAet()
        {
            InitializeComponent();
            WaitToPress= new EventWaitHandle(false, EventResetMode.AutoReset);
            isTerminated = false;
            isPressed = false;
            TaskResults = new List<ResultsProgAet>();
            ToExclude = new List<string>();
            currblocknum = 0;
            List<BlockClass_ProgAet> BlockList = new List<BlockClass_ProgAet>();
            BlockClass_ProgAet tempBlock = new BlockClass_ProgAet(4, "Left", ToExclude);
            ToExclude.AddRange(tempBlock.ToExclude);
            BlockList.Add(tempBlock);
            tempBlock = new BlockClass_ProgAet(4, "Right", ToExclude);
            ToExclude.AddRange(tempBlock.ToExclude);
            BlockList.Add(tempBlock);
            tempBlock = new BlockClass_ProgAet(6, "Left", ToExclude);
            ToExclude.AddRange(tempBlock.ToExclude);
            BlockList.Add(tempBlock);
            tempBlock = new BlockClass_ProgAet(6, "Right", ToExclude);
            ToExclude.AddRange(tempBlock.ToExclude);
            BlockList.Add(tempBlock);

            var assembly = typeof(TrialPage_ProgAet).GetTypeInfo().Assembly;
            var ooo = assembly.GetManifestResourceNames();
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }

            DoSession(BlockList);
        }
        protected override void OnDisappearing()
        {
            if (isTerminated)
            {
                Navigation.PopToRootAsync();
            }
            MessagingCenter.Send(this, "PreventLandscape");
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            isTerminated = true;
            SaveAndExit();
            return base.OnBackButtonPressed();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }

        private async void DoSession(List<BlockClass_ProgAet> blocklist)
        {
            for (int i = 0; i < blocklist.Count; i++)
            {
                if (isTerminated)
                {
                    break;
                }
                int x = rnd.Next(blocklist.Count);
                await DoBlock(blocklist[x]);
                currblocknum += 1;
                Save();
                blocklist.RemoveAt(x);
            }
            await SaveAndExit();
        }



        private async Task DoBlock(BlockClass_ProgAet currblock)
        {
            BlockRes = new List<TrialRes>();
            InstrBlock_ProgAet instrBlock = new InstrBlock_ProgAet(currblock, currblocknum);
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            await Navigation.PushModalAsync(instrBlock);
            instrBlock.Disappearing += (sender2, e2) =>
            {
                waitHandle.Set();
            };
            await Task.Run(() => waitHandle.WaitOne());
            
            for (int i = 0; i < currblock.TrialList.Count; i++)
            {
                await DoTrial(currblock.TrialList[i]);
            }
        }

        private async Task DoTrial(TrialClass_ProgAet currtrial)
        {
            FeedbackView.IsVisible = false;
            stopwatcha.Restart();
            
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                CenterImage.Source=ImageSource.FromResource(currtrial.CenterImage.CueImage);
               if (currtrial.CorrectLocation == "Left")
                {
                    if (currtrial.CenterImage.Name[0].ToString() == "X")
                    {
                        LeftImage.Text = currtrial.ConsequentA.TargetChar;
                        RightImage.Text = currtrial.ConsequentB.TargetChar;
                    }
                    else
                    {
                        LeftImage.Text = currtrial.ConsequentB.TargetChar;
                        RightImage.Text = currtrial.ConsequentA.TargetChar;
                    }
                }
                else
                {
                    if (currtrial.CenterImage.Name[0].ToString() == "X")
                    {
                        LeftImage.Text = currtrial.ConsequentB.TargetChar;
                        RightImage.Text = currtrial.ConsequentA.TargetChar;
                    }
                    else
                    {
                        LeftImage.Text = currtrial.ConsequentA.TargetChar;
                        RightImage.Text = currtrial.ConsequentB.TargetChar;
                    }
                }
            });

            await Task.Run(() => WaitToPress.WaitOne(10000));
            Evaluate(currtrial);

            
            await Task.Run(() => System.Threading.Thread.Sleep(1500));
            WaitToPress.Reset();
        }

        private void Evaluate(TrialClass_ProgAet currtrial)
        {
            int currACC;
            if (isPressed)
            {
                if (pressedLoc == currtrial.CorrectLocation)
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        FeedbackImage.Text = "Correct!";
                        FeedbackImage.TextColor = Color.Green;
                        FeedbackView.IsVisible = true;
                    });
                    currACC = 1;
                }
                else
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        FeedbackImage.Text = "Wrong!";
                        FeedbackImage.TextColor = Color.Red;
                        FeedbackView.IsVisible = true;
                    });
                    currACC = 0;
                }
            }
            else
            {
                Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    FeedbackImage.Text = "Late!";
                    FeedbackImage.TextColor = Color.Red;
                    FeedbackView.IsVisible = true;
                });
                currACC = 0;
            }
            
            
            int currRt = Convert.ToInt32(stopwatcha.ElapsedMilliseconds);
            BlockRes.Add(new TrialRes
            {
                RT = currRt,
                ACC = currACC,
                NumberofCues = currtrial.CenterImage.NumberofCues,
                CueName = currtrial.CenterImage.Name,
                CueLocation = currtrial.CenterImage.CueLocation,
                CorrectLoc = currtrial.CorrectLocation,
                Atext = currtrial.ConsequentA.TargetChar,
                Btext = currtrial.ConsequentB.TargetChar,
                WMLoad = currtrial.WMload
            });

            isPressed = false;
        }

        private void Save()
        {
            if (BlockRes.Count > 2)
            {
                string fieldname;
                int id = BlockRes[0].CueLocation;
                id -= 1;
                if (id == 0) { id = BlockRes[0].NumberofCues; }
                if(id <= BlockRes[0].NumberofCues / 2) //cue on the left
                {
                    if (App.user.hand == "Left")
                    {
                        fieldname = "Dominant";
                    }
                    else
                    {
                        fieldname = "Subdominant";
                    }
                }
                else
                {
                    if (App.user.hand == "Left")
                    {
                        fieldname = "Subdominant";
                    }
                    else
                    {
                        fieldname = "Dominant";
                    }
                }
                TaskResults.Add(new ResultsProgAet
                {
                    age = DateTime.Now.Year - App.user.age,
                    hand = App.user.hand,
                    UserSpecKey = App.user.id,
                    datePerf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                    TotalTrials = BlockRes.Where(x => x.WMLoad == 2).ToList().Count,
                    gender = App.user.gender,

                    meanACC = BlockRes.Where(x => x.WMLoad == 2).Average(a => a.ACC),
                    meanRT = BlockRes.Where(x => x.WMLoad == 2).Average(a => a.RT),
                    CueNum = BlockRes.Where(x => x.WMLoad == 2).LastOrDefault().NumberofCues,
                    WMLoad = 2,
                    Field = fieldname

                }) ;

                if (BlockRes.Where(a => a.WMLoad == 4).ToList().Count > 2)
                {
                    TaskResults.Add(new ResultsProgAet
                    {
                        age = DateTime.Now.Year - App.user.age,
                        hand = App.user.hand,
                        UserSpecKey = App.user.id,
                        datePerf = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                        TotalTrials = BlockRes.Where(x => x.WMLoad == 4).ToList().Count,
                        gender = App.user.gender,

                        meanACC = BlockRes.Where(x => x.WMLoad == 4).Average(a => a.ACC),
                        meanRT = BlockRes.Where(x => x.WMLoad == 4).Average(a => a.RT),
                        CueNum = BlockRes.Where(x => x.WMLoad == 4).LastOrDefault().NumberofCues,
                        WMLoad = 4,
                        Field=fieldname
                    });
                }



            }
        }

        private async Task SaveAll()
        {
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            List<ResultsProgAet> listofRes = await apiLogic.GetResults<ResultsProgAet>(App.user.id, Helpers.Constants.ProgAetGetUrl);
            listofRes = listofRes.Where(a => a.UserSpecKey == App.user.id).ToList();
            //listofRes = await App.client.GetTable<ResultsSST>().Where(a => a.UserSpecKey == App.user.id).ToListAsync();
            if (listofRes.Count > 0)
            {
                if (listofRes.Last().datePerf < TaskResults.LastOrDefault().datePerf)
                {
                    //save
                    foreach (ResultsProgAet a in TaskResults)
                    {
                        await apiLogic.PostResults<ResultsProgAet>(a, Helpers.Constants.ProgAetPostUrl);
                    }

                }
                if (listofRes.Last().datePerf == TaskResults.LastOrDefault().datePerf)
                {
                    if (listofRes.Where(a => a.datePerf == TaskResults.LastOrDefault().datePerf).Count() < TaskResults.Count())
                    {
                        foreach (ResultsProgAet a in listofRes.Where(a => a.datePerf == TaskResults.LastOrDefault().datePerf))
                        {
                            //Results to Put
                            if (TaskResults.Where(x => x.CueNum == a.CueNum && x.Field == a.Field && x.WMLoad == a.WMLoad).Any())
                            {
                                List<ResultsProgAet> ToPut = TaskResults.Where(x => x.CueNum == a.CueNum && x.Field == a.Field && x.WMLoad == a.WMLoad).ToList();
                                foreach (ResultsProgAet put in ToPut)
                                {
                                    await apiLogic.PutResults<ResultsProgAet>(put, Helpers.Constants.ProgAetPutUrl, a.id);
                                }


                            }
                            //Results to Post
                            if (TaskResults.Where(x => x.CueNum != a.CueNum && x.Field != a.Field && x.WMLoad != a.WMLoad).Any())
                            {
                                List<ResultsProgAet> ToPost = TaskResults.Where(x => x.CueNum != a.CueNum && x.Field != a.Field && x.WMLoad != a.WMLoad).ToList();
                                foreach (ResultsProgAet post in ToPost)
                                {
                                    await apiLogic.PostResults<ResultsProgAet>(post, Helpers.Constants.ProgAetPostUrl);
                                }
                                
                            }


                        }
                    }
                }
            }
            else
            {
                foreach (ResultsProgAet post in TaskResults)
                {
                    await apiLogic.PostResults<ResultsProgAet>(post, Helpers.Constants.ProgAetPostUrl);
                }
            }
        }

        private async Task SaveAndExit()
        {
            if (isTerminated)
            {
                Save();
            }
            await SaveAll();
        }


        private void LeftImageView_Tapped(object sender, EventArgs e)
        {
            pressedLoc = "Left";
            isPressed = true;
            WaitToPress.Set();
        }

        private void RightImageView_Tapped(object sender, EventArgs e)
        {
            pressedLoc = "Right";
            isPressed = true;
            WaitToPress.Set();
        }

        public class TrialRes
        {
            public int RT { get; set; }
            public int ACC { get; set; }
            public int NumberofCues { get; set; }
            public string CueName { get; set; }
            public int CueLocation { get; set; }
            public string CorrectLoc { get; set; }
            public string Atext { get; set; }
            public string Btext { get; set; }
            public int WMLoad { get; set; }
        }
    }
}