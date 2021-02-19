using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using System.IO;
using ScienceRecruiterApp.Model.Tasks.SST;
using ScienceRecruiterApp.Model.Tasks.AET;

namespace ScienceRecruiterApp.Droid
{
    [Activity(Label = "ScienceRecruiterApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            MessagingCenter.Subscribe<TrialPage_SST>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
                
            });
            MessagingCenter.Subscribe<TrialPage_SST>(this, "PreventLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });

            MessagingCenter.Subscribe<SST_Intro_0>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });
            MessagingCenter.Subscribe<SST_Intro_0>(this, "PreventLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });

            MessagingCenter.Subscribe<SST_Intro>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });
            MessagingCenter.Subscribe<SST_Intro>(this, "PreventLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });
            MessagingCenter.Subscribe<InstrBlock_ProgAet>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;

            });
            MessagingCenter.Subscribe<IntroPage_ProgAet>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;

            });
            MessagingCenter.Subscribe<TrialPage_ProgAet>(this, "AllowLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Landscape;
            });
            MessagingCenter.Subscribe<TrialPage_ProgAet>(this, "PreventLandscape", sender =>
            {
                RequestedOrientation = ScreenOrientation.Unspecified;
            });

            string db = "ScienceRecruiterApp_db.sqlite";
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fullpath = Path.Combine(path, db);

            Xamarin.Forms.Forms.SetFlags("CarouselView_Experimental");
            Xamarin.Forms.Forms.SetFlags("Expander_Experimental");
            Rg.Plugins.Popup.Popup.Init(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(fullpath));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}