using ScienceRecruiterApp.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Syncfusion;
using System.Data;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;
using MySql.Data.MySqlClient;
using Rg.Plugins.Popup.Extensions;
using ScienceRecruiterApp.ViewModel;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scientist_MainPage : ContentPage
    {
        public List<mResultsTasks_SST> tasks;

        public Scientist_MainPage()
        {
            InitializeComponent();
            homeButton.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.homeicon.png");
            GetList();
        }

        private async void GetList()
        {

            Logic.ApiLogic logic = new Logic.ApiLogic();
            List<ResultsSST> list = await logic.GetResults<ResultsSST>(Helpers.Constants.ResultsSSTRetrieveUrl);
            list = list.Where(x => App.Filter.idx.Contains(x.id)).ToList();
            //List<ResultsSST> list = await App.client.GetTable<ResultsSST>().ToListAsync();
            //List<ResultsSST> list=new List<ResultsSST>();
            tasks = new List<mResultsTasks_SST>
            {
                new mResultsTasks_SST("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoL.bmp", list)
            };


            List<string> Means = new List<string>();
            List<string> SDs = new List<string>();
            List<string> Mins = new List<string>();
            List<string> Maxs = new List<string>();

            List<string> ParameterNames = new List<string>();
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-CA");

            List<PropertyInfo> props = typeof(ResultsSST).GetProperties().Where(a => a.PropertyType == typeof(double)).ToList();
            foreach (PropertyInfo prop in props)
            {
                List<double> parameter = list.Select(l => ((double)prop.GetValue(l, null))).ToList();

                Means.Add(parameter.Average().ToString("F", cultureInfo));

                SDs.Add(Math.Sqrt(parameter.Sum(y => Math.Pow(y - parameter.Average(), 2)) / (parameter.Count() - 1)).ToString("F", cultureInfo));

                Mins.Add(parameter.Min().ToString("F", cultureInfo));
                Maxs.Add(parameter.Max().ToString("F", cultureInfo));

                ParameterNames.Add(prop.Name);

            }


            DataTemplate templateofMeans = new DataTemplate(() =>
            {
                Label label = new Label();
                label.SetBinding(Label.TextProperty, new Binding("."));
                label.Padding = 0;

                return new ViewCell
                {

                    View = new StackLayout
                    {
                        Padding = 0,
                        Spacing = 0,
                        Children =
                        {
                            new Frame
                            {
                                BorderColor=Color.Black,
                                Padding = 0,
                                Content=label
                            }
                        }
                    }
                };
            });



            DataTemplate dataTemplate = new DataTemplate(() =>
            {
                Image icon = new Image();
                icon.SetBinding(Image.SourceProperty, "icon");
                icon.HorizontalOptions = LayoutOptions.End;
                icon.HeightRequest = 50;

                Button filterButton = new Button
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Text = "Filter",

                };
                filterButton.Clicked += FilterButton_Clicked;

                Label NameLabel = new Label();
                NameLabel.SetBinding(Label.TextProperty, "TaskName");

                Label NumberofPar = new Label();
                NumberofPar.SetBinding(Label.TextProperty, "nSamples");

                ListView parametermeans = new ListView();
                //parametermeans.Header = "Mean";
                parametermeans.ItemsSource = Means;
                parametermeans.ItemTemplate = templateofMeans;
                parametermeans.HasUnevenRows = true;


                ListView parameterSDs = new ListView();
                //parameterSDs.Header = "SD";
                parameterSDs.ItemTemplate = templateofMeans;
                parameterSDs.ItemsSource = SDs;
                parameterSDs.HasUnevenRows = true;

                ListView parameterMins = new ListView();
                //parameterMins.Header = "Min";
                parameterMins.ItemTemplate = templateofMeans;
                parameterMins.ItemsSource = Mins;
                parameterMins.HasUnevenRows = true;

                ListView parameterMaxs = new ListView();
                //parameterMaxs.Header = "Max";
                parameterMaxs.ItemTemplate = templateofMeans;
                parameterMaxs.ItemsSource = Maxs;
                parameterMaxs.HasUnevenRows = true;

                ListView parameterNs = new ListView();
                //parameterNs.Header = "Parameter name";
                parameterNs.ItemTemplate = templateofMeans;
                parameterNs.ItemsSource = ParameterNames;
                parameterNs.HasUnevenRows = true;


                return new StackLayout
                {
                    BindingContext = tasks,
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = 0,
                    Spacing = 0,
                    Children =
                        {
                            new StackLayout
                            {
                                Orientation=StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    icon,
                                    new StackLayout
                                    {
                                        Orientation=StackOrientation.Vertical,
                                        HorizontalOptions = LayoutOptions.EndAndExpand,
                                        Children =
                                        {
                                            NameLabel,
                                            NumberofPar,
                                        }
                                    }
                                }
                            },
                            new Frame
                            {
                                BorderColor=Color.Black,
                                BackgroundColor=Color.DarkOliveGreen,
                                Padding =5,
                                Content=new Grid
                                {

                                    ColumnDefinitions=new ColumnDefinitionCollection
                                    {
                                        new ColumnDefinition{Width = new GridLength(2, GridUnitType.Star)},
                                        new ColumnDefinition(),
                                        new ColumnDefinition(),
                                        new ColumnDefinition(),
                                        new ColumnDefinition(),
                                    },
                                    Padding = 0,
                                    RowSpacing=0,
                                    Children =
                                    {
                                        {new Label{Text="Parameter name", HorizontalTextAlignment=TextAlignment.Center, TextColor=Color.White}, 0,0 },
                                        {new Label{Text="Mean", HorizontalTextAlignment=TextAlignment.Center,TextColor=Color.White}, 1,0 },
                                        {new Label{Text="SD", HorizontalTextAlignment=TextAlignment.Center,TextColor=Color.White}, 2,0 },
                                        {new Label{Text="Min", HorizontalTextAlignment=TextAlignment.Center,TextColor=Color.White}, 3,0 },
                                        {new Label{Text="Max", HorizontalTextAlignment=TextAlignment.Center,TextColor=Color.White}, 4,0 }
                                    }
                                }
                            },



                            new ScrollView
                            {
                                Orientation = ScrollOrientation.Vertical,
                                HeightRequest = 100,
                                Content=new Grid
                                {

                                    ColumnDefinitions=new ColumnDefinitionCollection
                                    {
                                        new ColumnDefinition{Width = new GridLength(2, GridUnitType.Star)},
                                        new ColumnDefinition(),
                                        new ColumnDefinition(),
                                        new ColumnDefinition(),
                                        new ColumnDefinition(),
                                    },
                                    Padding = 0,
                                    RowSpacing=0,


                                    Children =
                                    {
                                        {parameterNs,0,0 },
                                        {parametermeans, 1, 0 },
                                        {parameterSDs, 2,0 },
                                        {parameterMins, 3,0 },
                                        {parameterMaxs, 4, 0 }
                                    },


                                }
                            },


                            filterButton,

                            //new Button
                            //{
                            //    Text="Invite participants",
                            //    HorizontalOptions=LayoutOptions.Center

                            //}

                        }

                };
            });

            currMonthCarousel.ItemsSource = tasks;
            currMonthCarousel.ItemTemplate = dataTemplate;

            currMonthCarousel2.ItemsSource = tasks;
            currMonthCarousel2.ItemTemplate = dataTemplate;

        }

        private async void FilterButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FilterPopUpPage());
        }

        private async void homeButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage_loggedIn());
        }

        protected override void OnAppearing()
        {
            GetList();
        }

    }

}
