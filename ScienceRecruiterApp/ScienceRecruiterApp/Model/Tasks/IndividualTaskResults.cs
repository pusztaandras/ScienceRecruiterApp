using Newtonsoft.Json;
using ScienceRecruiterApp.Logic;
using ScienceRecruiterApp.Model.Tasks.Stroop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Syncfusion.SfChart.XForms;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;




namespace ScienceRecruiterApp.Model.Tasks
{
    public class IndividualTaskResults : INotifyPropertyChanged
    {
        private ImageSource Icon;

        public ImageSource icon
        {
            get { return Icon; }
            set
            {
                if (Icon != value)
                {
                    Icon = value;
                    OnProperyChanged("icon");
                }

            }
        }


        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnProperyChanged("Title");
            }
        }


        private string progressstr;

        public string ProgressStr
        {
            get { return progressstr; }
            set
            {
                progressstr = value;
                OnProperyChanged("ProgressStr");
            }
        }


        private int pInt;

        public int ProgressInt
        {
            get { return pInt; }
            set
            {
                pInt = value;
                OnProperyChanged("ProgressInt");
            }
        }


        private int mTrials;

        public int MaximumTrials
        {
            get { return mTrials; }
            set
            {
                mTrials = value;
                OnProperyChanged("MaximumTrials");
            }
        }


        private string shrtDscr;

        public string ShortDescription
        {
            get { return shrtDscr; }
            set
            {

                shrtDscr = value;
                OnProperyChanged("ShortDescription");
            }
        }
        private ObservableCollection<ChartDataPoint> lngDscr;

        public ObservableCollection<ChartDataPoint> LongDescription
        {
            get { return lngDscr; }
            set
            {
                lngDscr = value;
                OnProperyChanged("LongDescription");
            }
        }
        private string invitedby;

        public string InvitedBy
        {
            get
            {
                if (String.IsNullOrEmpty(invitedby))
                {
                    return invitedby;
                }
                else
                {
                    return string.Format("Invited by {0}", invitedby);
                }

            }
            set
            {
                invitedby = value;
                OnProperyChanged("InvitedBy");
                //OnProperyChanged_PutSql();
            }
        }

        private string shrtTit;

        public string ShortTitle
        {
            get { return shrtTit; }
            set
            {
                shrtTit = value;
                OnProperyChanged("ShortTitle");
            }
        }



        public List<string> Tags { get; set; }

        

        


        public IndividualTaskResults(string title, string shrtit, ImageSource image, int progress, ObservableCollection<ChartDataPoint> lng, int mtr)
        {
            string shrt;
            icon = image;
            Title = title;
            ShortTitle = shrtit;
            ProgressInt = progress;
            if (progress == 0)
            {
                ProgressStr = "0 trials";
                shrt = "Try to do more trials to get meaningful results!";
            }
            else
            {
                if(progress<mtr)
                {
                    ProgressStr = String.Concat(progress.ToString(), "/", mtr.ToString(), " trials completed");
                    shrt = "Try to do more trials to get meaningful results!";
                }
                else
                {
                    ProgressStr = String.Concat(progress.ToString(), "/", mtr.ToString(), " trials completed");
                    shrt = "Try to do more trials to get meaningful results!";
                }
            }
            MaximumTrials = mtr;
            ShortDescription = shrt;
            LongDescription = lng;
            
            


        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void OnProperyChanged(string propertyname)
        {
            if (PropertyChanged != null)
            {

                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));


            }

        }

        private async void OnProperyChanged_PutSql()
        {
            if (PropertyChanged != null)
            {
                ApiLogic logic = new ApiLogic();
                if (ShortTitle == "SST")
                {
                    List<ResultsSST> list = await logic.GetResults<ResultsSST>(App.user.id, Helpers.Constants.ResultsSSTRetrieveUrl_id);
                    logic.PostResults<ResultsSST>(list.OrderBy(a => a.datePerf).LastOrDefault(), Helpers.Constants.SSTPutUrl);
                }
                else
                {
                    List<ResultsStroop> list = await logic.GetResults<ResultsStroop>(App.user.id, Helpers.Constants.ResultsSstroopRetrieveUrl_id);
                    logic.PostResults<ResultsStroop>(list.OrderBy(a => a.datePerf).LastOrDefault(), Helpers.Constants.StroopPutUrl);

                }
                
            }

        }
    }
}
