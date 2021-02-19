using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.Model.Tasks;
using ScienceRecruiterApp.Model.Tasks.AET;
using ScienceRecruiterApp.Model.Tasks.SST;
using ScienceRecruiterApp.Model.Tasks.Stroop;
using ScienceRecruiterApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksTabPage : ContentPage
    {

        public List<TasksDescription> Tasks;
        public TasksTabPage()
        {
            Tasks = new List<TasksDescription>();
            Tasks.Add(new TasksDescription(
                "Stop Signal Task",
                "SST",
                ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoR.bmp"),
                0,
                "A test to measure your inhibitory control",
                "The stop-signal task is an essential tool for studying response inhibition in neuroscience, psychiatry, and psychology. Research using the task has revealed links between inhibitory-control capacities and a wide range of behavioral and impulse-control problems in everyday life, including attention-deficit/hyperactivity disorder, substance abuse, eating disorders, and obsessive-compulsive behaviors.",
                new Command(
                    execute: async () =>
                    {
                        await Navigation.PushAsync(new SST_Intro_0());
                    }),
                App.settings_sst.TotalTrials
                
            ));
            Tasks.Add(new TasksDescription(
                "Stroop Task",
                "Stroop",
                ImageSource.FromResource("ScienceRecruiterApp.Pictures.Stroop_icon.jpg"),
                0,
                "A test to measure the so-called 'Stroop effect' ",
                "Stroop effect is the delay in reaction time between congruent and incongruent stimuli. Stroop effect permit to measure a person's selective attention capacity and skills, as well as their processing speed ability.",
                new Command(
                    execute: async () =>
                    {
                        await Navigation.PushAsync(new InstrPage_Stroop());
                    }),
                App.settings_stroop.TotalTrials
                

            ));
            Tasks.Add(new TasksDescription(
                "Acquired equivalence task",
                "ProgAet",
                ImageSource.FromResource("ScienceRecruiterApp.Pictures.Stroop_icon.jpg"),
                0,
                "Complex task to measure spatial attention and worwing memory ",
                "Some reeeeeeeeallly very long description",
                new Command(
                    execute: async () =>
                    {
                        await Navigation.PushAsync(new IntroPage_ProgAet());
                    }),
                4


            ));

            InitializeComponent();
            carouselView.ItemsSource = Tasks;
            ReloadPageAsync();
            ICommand refreshCommand = new Command(async () =>
            {
                // IsRefreshing is true
                await ReloadPageAsync();
                // Refresh data here
                refreshView.IsRefreshing = false;
            });
            refreshView.Command = refreshCommand;
            
            //TaskIcon.Source = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoL.bmp");
        }
        private async Task ReloadPageAsync()
        {
            RetrivedResults mySST = await RetriveResults<ResultsSST>(Helpers.Constants.ResultsSSTRetrieveUrl_id);
            RetrivedResults myStroop = await RetriveResults<ResultsStroop>(Helpers.Constants.ResultsSstroopRetrieveUrl_id);
            RetrivedResults myProgAet = await RetriveResults<ResultsProgAet>(Helpers.Constants.ProgAetGetUrl);

            Tasks.Where(a => a.Title == "Stop Signal Task").LastOrDefault().ProgressInt = mySST.CurrMonthTrial;
            Tasks.Where(a => a.Title == "Stop Signal Task").LastOrDefault().ProgressStr = String.Concat(mySST.CurrMonthTrial.ToString(), "/",App.settings_sst.TotalTrials.ToString(), " trials this month") ;

            Tasks.Where(a => a.Title == "Acquired equivalence task").LastOrDefault().ProgressInt = myProgAet.CurrMonthTrial;
            Tasks.Where(a => a.Title == "Acquired equivalence task").LastOrDefault().ProgressStr = String.Concat(myProgAet.CurrMonthTrial.ToString(), "/ 4 blocks this month");

            Tasks.Where(a => a.Title == "Stroop Task").LastOrDefault().ProgressInt = myStroop.CurrMonthTrial;
            Tasks.Where(a => a.Title == "Stroop Task").LastOrDefault().ProgressStr = String.Concat(myStroop.CurrMonthTrial.ToString(), "/", App.settings_sst.TotalTrials.ToString(), " trials this month");

        }

        private async Task<RetrivedResults> RetriveResults<T>(string ApiKey) where T : ResultsTasks
        {
            RetrivedResults res = new RetrivedResults();
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            
            List<T> list = await apiLogic.GetResults<T>(App.user.id, ApiKey);
            list = list.Where(a => a.UserSpecKey == App.user.id).ToList();
            if (list.Count > 0)
            {
                if (list.Last().datePerf.Equals(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)))
                {
                    if (typeof(T) == typeof(ResultsProgAet))
                    {
                        res.CurrMonthTrial = list.Where(a => a.datePerf == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToList().Count;
                        res.MyAllTrial = list.Count();
                        res.InvitedBy = list.Last().InvitedBy;
                        res.Streaks = GetStreaks(ref list);
                    }
                    else
                    {
                        res.CurrMonthTrial = list.Last().TotalTrials;
                        res.MyAllTrial = list.Select(x => x.TotalTrials).Sum();
                        res.InvitedBy = list.Last().InvitedBy;
                        res.Streaks = GetStreaks(ref list);
                    }
                }
                else
                {
                    res.CurrMonthTrial = 0;
                    res.MyAllTrial = list.Select(x => x.TotalTrials).Sum();
                    res.InvitedBy = list.Last().InvitedBy;
                    res.Streaks = 0;
                    
                }
            }
            else
            {
                res.CurrMonthTrial = 0;
                res.MyAllTrial = 0;
                res.Streaks = 0;
                
            }
            
            return res;
        }

        private int GetStreaks<T>(ref List<T> list) where T : ResultsTasks
        {
            int streaks=0;
            while(list.Count>0)
            {
                if(list.Last().datePerf.Equals(new DateTime(DateTime.Now.Year, DateTime.Now.Month - streaks, 1)))
                {
                    streaks += 1;
                }
                
                list.Remove(list.Last());
            }
            return streaks;
        }

        private void ProgressBar_Tapped(object sender, EventArgs e)
        {
            InfoExpander.BindingContext = carouselView.CurrentItem;
            InfoExpander.IsExpanded = !InfoExpander.IsExpanded;
            
        }

        private void ProgressBar_Changed(object sender, EventArgs e)
        {
            InfoExpander.BindingContext = carouselView.CurrentItem;
        }
    }
}