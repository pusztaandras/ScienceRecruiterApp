using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Foundation;
using ObjCRuntime;
using ScienceRecruiterApp.Model.Tasks.SST;
using Syncfusion.SfRangeSlider.XForms.iOS;
using UIKit;
using Xamarin.Forms;

namespace ScienceRecruiterApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public bool allowRotation;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //


        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, [Transient] UIWindow forWindow)
        {


            if(this.allowRotation==true)
            {
                return UIInterfaceOrientationMask.All;
            }
            else
            {
                return UIInterfaceOrientationMask.Landscape;
            }
            
        }


        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.SetFlags("CarouselView_Experimental");
            allowRotation = true;
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();

            MessagingCenter.Subscribe<TrialPage_SST>(this, "AllowLandscape", sender =>
            {
                this.allowRotation = false;

            });
            MessagingCenter.Subscribe<TrialPage_SST>(this, "PreventLandscape", sender =>
            {
                this.allowRotation = true;
            });

            MessagingCenter.Subscribe<SST_Intro_0>(this, "AllowLandscape", sender =>
            {
                this.allowRotation = false;
            });
            MessagingCenter.Subscribe<SST_Intro_0>(this, "PreventLandscape", sender =>
            {
                this.allowRotation = true;
            });

            MessagingCenter.Subscribe<SST_Intro>(this, "AllowLandscape", sender =>
            {
                this.allowRotation = false;
            });
            MessagingCenter.Subscribe<SST_Intro>(this, "PreventLandscape", sender =>
            {
                this.allowRotation = true;
            });

            string db = "ScienceRecruiterApp_db.sqlite";
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"..");
            string fullpath = Path.Combine(path, db);

            LoadApplication(new App(fullpath));
            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            new SfRangeSliderRenderer();
            // Add the below line if you are using SfLinearProgressBar.
            Syncfusion.XForms.iOS.ProgressBar.SfLinearProgressBarRenderer.Init();

            // Add the below line if you are using SfCircularProgressBar.  
            Syncfusion.XForms.iOS.ProgressBar.SfCircularProgressBarRenderer.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}
