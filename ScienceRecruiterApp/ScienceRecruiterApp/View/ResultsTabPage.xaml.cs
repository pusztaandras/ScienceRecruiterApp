using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScienceRecruiterApp.Model.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Syncfusion.SfChart.XForms;
using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.Model.Tasks.AET;
using ScienceRecruiterApp.Model.Tasks.Stroop;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsTabPage : ContentPage
    {
        public List<IndividualTaskResults> Tasks;
        public string CurrRank { get; set; }
        public ResultsTabPage()
        {
            CurrRank = "Youngling";
            Tasks = new List<IndividualTaskResults>();
            Tasks.Add(new IndividualTaskResults(
                "Stop Signal Task",
                "SST",
                ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoR.bmp"),
                0,
                new ObservableCollection<ChartDataPoint>()
                {
                    new ChartDataPoint("Cue==Low", 1),
                    new ChartDataPoint("Cue==High",  1)
                },
                App.settings_sst.TotalTrials

            ));
            Tasks.Add(new IndividualTaskResults(
                "Stroop Task",
                "Stroop",
                ImageSource.FromResource("ScienceRecruiterApp.Pictures.Stroop_icon.jpg"),
                0,
                new ObservableCollection<ChartDataPoint>()
                {
                    new ChartDataPoint("Congruent", 1),
                    new ChartDataPoint("Incongruent", 1),
                    new ChartDataPoint("Control", 1)
                },
                App.settings_stroop.TotalTrials
            )) ;
            Tasks.Add(new IndividualTaskResults(
                "Progressive acquired equivalence task",
                "ProgAet",
                ImageSource.FromResource("ScienceRecruiterApp.Pictures.Stroop_icon.jpg"),
                0,
                new ObservableCollection<ChartDataPoint>()
                {
                    new ChartDataPoint("Low WM-load", 1),
                    new ChartDataPoint("High WM-load", 1)
                },
                4
            ));
            InitializeComponent();
            carousel_Results.ItemsSource = Tasks;
            Details_Layout.BindingContext = carousel_Results.CurrentItem;
            Explanation_layout.BindingContext = carousel_Results.CurrentItem;
            ReloadPageAsync();
        }

        private async void ReloadPageAsync()
        {
           List<ResultsSST> mySST = await RetriveResults<ResultsSST>(Helpers.Constants.ResultsSSTRetrieveUrl_id);
           List<ResultsStroop> myStroop = await RetriveResults<ResultsStroop>(Helpers.Constants.ResultsSstroopRetrieveUrl_id);
           List<ResultsProgAet> myProgAet = await RetriveResults<ResultsProgAet>(Helpers.Constants.ProgAetGetUrl);

            //SST-results
           if(mySST.Count()>0)
            {
                Tasks.Where(a => a.Title == "Stop Signal Task").LastOrDefault().ProgressInt = mySST.LastOrDefault().TotalTrials;
                Tasks.Where(a => a.Title == "Stop Signal Task").LastOrDefault().ProgressStr = String.Concat(mySST.LastOrDefault().TotalTrials.ToString(), "/", App.settings_sst.TotalTrials.ToString(), " trials this month");
                Tasks.Where(a => a.Title == "Stop Signal Task").LastOrDefault().LongDescription = new ObservableCollection<ChartDataPoint>()
                {
                    new ChartDataPoint("Cue==Low", mySST.LastOrDefault().meanRTLow),
                    new ChartDataPoint("Cue==High", mySST.LastOrDefault().meanRTHigh)
                };
                if (mySST.LastOrDefault().TotalTrials < App.settings_sst.TotalTrials)
                {
                    Tasks.Where(a => a.Title == "Stop Signal Task").LastOrDefault().ShortDescription = "Try to do more trials to get meaningful results!";
                }
                else
                {
                    Tasks.Where(a => a.Title == "Stop Signal Task").LastOrDefault().ShortDescription = "Under development";
                }

            }
            //ProgAet-results
            if (myProgAet.Count() > 0)
            {
            Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().ProgressInt = myProgAet.Count();
            Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().ProgressStr = String.Concat(myProgAet.Count().ToString(), "/ 4 blocks this month");
            Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().LongDescription = new ObservableCollection<ChartDataPoint>();
                if (myProgAet.Where(a => a.WMLoad == 2).Count()>0)
                {
                        Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().LongDescription.Add(new ChartDataPoint("Low WM-load", myProgAet.Where(a => a.WMLoad == 2).Select(u => u.meanRT).Average()));
                }
                else
                {
                    Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().LongDescription.Add(new ChartDataPoint("Low WM-load", 0));
                }
                if (myProgAet.Where(a => a.WMLoad == 4).Count() > 0)
                {
                    Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().LongDescription.Add(new ChartDataPoint("High WM-load", myProgAet.Where(a => a.WMLoad == 4).Select(u => u.meanRT).Average()));
                }
                else
                {
                    Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().LongDescription.Add(new ChartDataPoint("High WM-load", 0));
                }
            }
            if (myProgAet.Count() < 4)

            {
                Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().ShortDescription = "Try to do more trials to get meaningful results!";
            }
            else
            {
                Tasks.Where(a => a.Title == "Progressive acquired equivalence task").LastOrDefault().ShortDescription = "Under developmnet";
            }


            //Stroop-results
            if (myStroop.Count() > 0)
            {
                Tasks.Where(a => a.Title == "Stroop Task").LastOrDefault().ProgressInt = myStroop.LastOrDefault().TotalTrials;
            Tasks.Where(a => a.Title == "Stroop Task").LastOrDefault().ProgressStr = String.Concat(myStroop.LastOrDefault().TotalTrials.ToString(), "/", App.settings_sst.TotalTrials.ToString(), " trials this month");
            Tasks.Where(a => a.Title == "Stroop Task").LastOrDefault().LongDescription = new ObservableCollection<ChartDataPoint>()
            {
                    new ChartDataPoint("Congruent", myStroop.LastOrDefault().mRTCongr),
                    new ChartDataPoint("Incongruent", myStroop.LastOrDefault().mRTIncongr),
                    new ChartDataPoint("Control", myStroop.LastOrDefault().mRTControl)
            };
                if(myStroop.LastOrDefault().TotalTrials<App.settings_stroop.TotalTrials)
                {
                    Tasks.Where(a => a.Title == "Stroop Task").LastOrDefault().ShortDescription= "Try to do more trials to get meaningful results!";
                }
                else
                {
                    Tasks.Where(a => a.Title == "Stroop Task").LastOrDefault().ShortDescription = "Under development";
                }
            }
        }
        private async Task<List<T>> RetriveResults<T>(string ApiKey) where T : ResultsTasks
        {
            
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            List<T> res = new List<T>();
            List<T> list = await apiLogic.GetResults<T>(App.user.id, ApiKey);
            list = list.Where(a => a.UserSpecKey == App.user.id).ToList();
            if (list.Count > 0)
            {
                if (list.Last().datePerf.Equals(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)))
                {
                   res.Add(list.Last());
                }
                if (typeof(T) == typeof(ResultsProgAet))
                {
                    res = list.Where(a => a.datePerf == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToList();
                }

            }
            
            return res;
        }

        private void ProgressBar_Tapped(object sender, EventArgs e)
        {
            Details_Layout.BindingContext = carousel_Results.CurrentItem;
            Explanation_layout.BindingContext = carousel_Results.CurrentItem;

        }

        private void ProgressBar_Changed(object sender, EventArgs e)
        {
            Details_Layout.BindingContext = carousel_Results.CurrentItem;
            Explanation_layout.BindingContext = carousel_Results.CurrentItem;
        }
    }
}