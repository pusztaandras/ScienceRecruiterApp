using System;
using Microsoft.Maui.Controls.Xaml;
using SQLite;
using System.Collections.Generic;
using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.View;
using Microsoft.WindowsAzure.MobileServices;
using ScienceRecruiterApp.Model.Tasks.Stroop;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp
{
    public partial class App : Application
    {
        public static string DataBaseLocation;

        public static FilterClass Filter;

        public static UserSpec user;

        //public static MobileServiceClient client = new MobileServiceClient("https://scirecretrieve.azurewebsites.net");

        public static SettingsClass_SST settings_sst;

        public static SettingsClass_Stroop settings_stroop;


        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        public App(string dbLocation)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ1NzUxNEAzMjM2MmUzMDJlMzBpSGFrT1RiczhpMVRTaDVCZXM2ZmtyL2o0TkxCeHlUaUhPWjhVNFR3Mzk4PQ==");
            InitializeComponent();
            DataBaseLocation = dbLocation;
            
            settings_stroop = new SettingsClass_Stroop();

            MainPage = new NavigationPage(new AnimationPage());
           
            App.Filter = new FilterClass();
            


        }

        //private async void TrytoConnect()
        //{
        //    if (await UserSpec.RetriveID())
        //    {
        //        MainPage = new NavigationPage(new MainPage_loggedIn());
        //    }
        //    else
        //    {
        //        MainPage = new NavigationPage(new LoginPage());
        //    }
        //}

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
