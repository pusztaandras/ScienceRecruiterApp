using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.Model.Tasks;
using ScienceRecruiterApp.Model.Tasks.SST;
using ScienceRecruiterApp.Model.Tasks.Stroop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Participant_MainPage : ContentPage
    {
        public string sst_progress;
        public double progress_sst;
        public string aet_progress;
        public double progress_aet;
        public double goalprogress;
        List<ResultsSST> list;
        public Participant_MainPage()
        {

            InitializeComponent();
            goalpic.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.Goal_Penguins.jpg");
            sst_icon.Source = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoL.bmp");
            homeButton.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.homeicon.png");
            questionmark.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.qm.png");
            questionmark2.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.qm.png");
            refreshView.Command= new Command(() =>
            {
                // IsRefreshing is true
                // Refresh data here
                GetList();
                refreshView.IsRefreshing = false;
            });
            GetList();


        }
        protected override void OnAppearing()
        {
            
            base.OnAppearing();
        }

        private async void GetList()
        {
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            list = await apiLogic.GetResults<ResultsSST>(App.user.id, Helpers.Constants.ResultsSSTRetrieveUrl_id);
            //list = list.Where(a => a.id == App.user.id).ToList();
            if (list.Count > 0)
            {
                if (list.Last().datePerf.Month.Equals(DateTime.Now.Month))
                {
                    sst_progress = String.Concat(list.Last().TotalTrials.ToString(), "/", App.settings_sst.TotalTrials.ToString(), " trials");
                    progress_sst = Convert.ToDouble(list.Last().TotalTrials) / Convert.ToDouble(App.settings_sst.TotalTrials);
                }
                else
                {
                    sst_progress = "No trials this month";
                    progress_sst = 0;
                }
            }
            else
            {
                sst_progress = "No trials this month";
                progress_sst = 0;
            }

            List<ResultsStroop> StroopReslist = await apiLogic.GetResults<ResultsStroop>(App.user.id, Helpers.Constants.ResultsSstroopRetrieveUrl_id);
            string stroop_progress;
            double progress_stroop;
            if (StroopReslist.Count > 0)
            {
                if (StroopReslist.Last().datePerf==new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1))
                {
                    stroop_progress = String.Concat(StroopReslist.Last().TotalTrials.ToString(), "/180 trials");
                    progress_stroop = Convert.ToDouble(StroopReslist.Last().TotalTrials / 180);
                }
                else
                {
                    stroop_progress = "No trials this month";
                    progress_stroop = 0;
                }
            }
            else
            {
                stroop_progress = "No trials this month";
                progress_stroop = 0;
            }

            //ProgAet results
            aet_progress = "No trials this month";
            progress_aet = 0;

            sst_curr.Text = sst_progress;
            stroop_curr.Text = stroop_progress;

            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-CA");
            SST_res_Label.Text = progress_sst.ToString("F", cultureInfo);

            // Update progress on goal
            List<ResultsSST> templist = await apiLogic.GetResults<ResultsSST>(Helpers.Constants.ResultsSSTRetrieveUrl_id);
            List<double> trialnums = templist.Select(a => a.TotalTrials).Select(s => Convert.ToDouble(s) / App.settings_sst.TotalTrials).ToList<double>();
            goalprogress = (trialnums.Sum() * 5) / 10000;
            Skiacanvas.PaintSurface += Skiacanvas_PaintSurface;
            if (list.Count > 0)
            {
                ResultsSST currRes = list.Last();
                ProgressLabel.Text = String.Concat("Progress: ", (goalprogress * 10000).ToString(), " $ / 10 000 $");
                List<double> trialnumsown = list.Select(a => a.TotalTrials).Select(s => Convert.ToDouble(s) / App.settings_sst.TotalTrials).ToList();
                double ownprogress = (trialnumsown.Sum() * 5);
                contribLabel.Text = string.Concat("You contribution = ", ownprogress.ToString(), " $");

                double temp = currRes.TotalTrials / App.settings_sst.TotalTrials;
                balance.Text = string.Concat("This month's balance = ", (temp * 5).ToString(), " $");
            }
            else
            {
                contribLabel.Text = string.Concat("No contribution yet");
                balance.Text = string.Concat("No money this month. Do some tests!");
            }


            List<TasksDescription> tasks = new List<TasksDescription>
            {
                //new TasksDescription("Stop Signal Task", App.settings_sst.GoL, sst_progress),
                //new TasksDescription("Acquired equivalence test", ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.AET.Pictures.4_3.jpg"), aet_progress)


            };

            //DataTemplate TemplateRes = new DataTemplate(() =>
            //{
            //    Label nameLabel = new Label();
            //    nameLabel.SetBinding(Label.TextProperty, "Title");

            //    Label ProgressLabel = new Label();
            //    ProgressLabel.SetBinding(Label.TextProperty, "Progress");

            //    Image Icon = new Image();
            //    Icon.SetBinding(Image.SourceProperty, "icon");

            //    Button StartButton = new Button { Text = "Start Task!" };
            //    StartButton.Clicked += StartButton_Clicked;

            //    return new ViewCell
            //    {
            //        View = new FlexLayout
            //        {
            //            Padding = new Thickness(0, 5),
            //            Direction = FlexDirection.Column,
            //            JustifyContent = FlexJustify.SpaceBetween,
            //            AlignItems = FlexAlignItems.Center,
            //            Children =
            //              {
            //                new Frame
            //                {
            //                    CornerRadius=50,
            //                    Content=Icon
            //                },
            //                nameLabel,
            //                new Expander
            //                {
            //                    Header=new Label{ Text="This month progress"},
            //                    Content=ProgressLabel

            //                },

            //                StartButton

            //              }


            //        }
            //    };

            //});

            //TasksCarousel.ItemsSource = tasks;
            //TasksCarousel.ItemTemplate = TemplateRes;
            Skiacanvas.Dispatcher.BeginInvokeOnMainThread(() => Skiacanvas.InvalidateSurface());

        }

        private void Skiacanvas_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White.WithAlpha((byte)(0xFF * goalprogress))


            };

            float x = (float)(info.Width * (1 - goalprogress));
            float width = x;

            canvas.DrawRect(x, 0, width, info.Height, paint);
        }

        public async void StartButton_Clicked(object sender, EventArgs e)
        {
            //Check for current month
            if (list.Count > 0)
            {
                if (list.Last().datePerf.Month.Equals(DateTime.Now.Month))
                {
                    if (await DisplayAlert("Are you sure?", String.Concat("You already tried this task this month. You only get plus points if you do more than ", list.Last().TotalTrials.ToString(), " trials"), "Ok", "Cancel"))
                    {
                        SST_Intro_0 page = new SST_Intro_0();
                        await Navigation.PushAsync(page);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                SST_Intro_0 page = new SST_Intro_0();
                await Navigation.PushAsync(page);
            }

        }

        private async void homeButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage_loggedIn());
        }

        private async void questionmark_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Monthly streak", "A streak is the number of months (multiplied by the progress of the task in each month) in a row you have completed a task", "OK");
        }

        private async void questionmark2_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Calculation method", "Sum of monthly straks in each task x 5$ ", "OK");
        }

        public async void StroopButton_Clicked(object sender, EventArgs e)
        {
            InstrPage_Stroop page = new InstrPage_Stroop();
            await Navigation.PushAsync(page);
        }

    }
}