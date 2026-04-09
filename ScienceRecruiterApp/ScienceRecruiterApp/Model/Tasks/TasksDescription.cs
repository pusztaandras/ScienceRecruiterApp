using Newtonsoft.Json;
using ScienceRecruiterApp.Logic;
using ScienceRecruiterApp.Model.Tasks.Stroop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp.Model.Tasks
{
    public class TasksDescription : INotifyPropertyChanged
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
        private string lngDscr;

        public string LongDescription
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

        public ICommand LoadPage { get; set; }

        public ICommand GetSciID { get; set; }


        public TasksDescription(string title, string shrtit, ImageSource image, int progress, string shrt, string lng, ICommand Comand, int mtr)
        {
            icon = image;
            Title = title;
            ShortTitle = shrtit;
            ProgressInt = progress;
            if (progress == 0)
            {
                ProgressStr = "No trials this month";
            }
            else
            {
                ProgressStr = String.Concat(progress.ToString(), "/", mtr.ToString(), " trials completed");
            }
            MaximumTrials = mtr;
            ShortDescription = shrt;
            LongDescription = lng;
            LoadPage = Comand;
            GetSciID = new Command(
                    execute: async () =>
                    {
                        string result = await Application.Current.MainPage.DisplayPromptAsync("Invitation code", "Type code:");
                        InvitedBy = result;
                    });



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
