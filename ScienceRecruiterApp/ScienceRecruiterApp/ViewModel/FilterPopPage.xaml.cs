using ScienceRecruiterApp.Model;
using Syncfusion.SfRangeSlider.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        List<string> genderlist;
        List<string> handlist;
        bool isstartagechanged=false;
        bool isstopagechanged = false;
        int ageStart;
        int ageStop;
        bool isgenderchanged=false;
        string gender;
        bool ishandchanged = false;
        string hand;

        public FilterPopPage()
        {
            InitializeComponent();
            

            genderlist = new List<string>();
            genderlist.Add(" ");
            genderlist.Add("female");
            genderlist.Add("male");
            genderBox.ComboBoxSource = genderlist;
            handlist = new List<string>();
            handlist.Add(" ");
            handlist.Add("left");
            handlist.Add("right");
            handBox.ComboBoxSource = handlist;

            setrange_age();
            UpdateFilter();


            ageslider.RangeChanging += (object sender, RangeEventArgs e) =>
            {
                if (e.Start > ageslider.RangeStart)
                {
                    ageStart = (int)e.Start;
                    isstartagechanged = true;
                    
                }
                if (e.End < ageslider.RangeEnd)
                {
                    ageStop = (int)e.End;
                    isstopagechanged = true;

                }
                
            };

            handBox.SelectionChanged += HandBox_SelectionChanged;
            genderBox.SelectionChanged += GenderBox_SelectionChanged;
        }

        private void GenderBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (genderBox.SelectedIndex > 0)
            {
                isgenderchanged = true;
                gender = genderlist[genderBox.SelectedIndex];
            }
        }

        private void HandBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (handBox.SelectedIndex > 0)
            {
                ishandchanged = true;
                hand = handlist[handBox.SelectedIndex];
            }
        }

        private void UpdateFilter()
        {
            
        }

        private async void setrange_age()
        {
            int[] res = new int[2];
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            List<ResultsSST> tmp=await apiLogic.GetResults<ResultsSST>(Helpers.Constants.ResultsSSTRetrieveUrl);
            ageslider.RangeStart = tmp.Select(a => a.age).ToList().Min();
            ageslider.RangeEnd = tmp.Select(a => a.age).ToList().Max();
            
        }

        private void setfilter_Clicked(object sender, EventArgs e)
        {
            List<FilterProperties> props = new List<FilterProperties>();
            if (isgenderchanged)
            {
                props.Add(new FilterProperties{ propertyname = "gender", propertyvalue = gender });
            }
            if (ishandchanged)
            {
                props.Add(new FilterProperties{ propertyname = "hand", propertyvalue = hand });
            }
            if (isstartagechanged)
            {
                props.Add(new FilterProperties { propertyname = "age", propertyvalue = ageStart, condition="bigger" });
            }
            if (isstopagechanged)
            {
                props.Add(new FilterProperties { propertyname = "age", propertyvalue = ageStop, condition = "smaller" });
            }

            App.Filter = new FilterClass(props);

        }

        private void clearButton_Clicked(object sender, EventArgs e)
        {
            App.Filter = new FilterClass();
        }
    }
}