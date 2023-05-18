using System.IO;
using Foundation;
using ObjCRuntime;
using ScienceRecruiterApp.Model.Tasks.SST;
using Syncfusion.SfRangeSlider.XForms.iOS;
using UIKit;
using Xamarin.Forms;

using CarouselView.FormsPlugin.iOS;

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
            Xamarin.Forms.Forms.SetFlags("Expander_Experimental");
            Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            allowRotation = true;
            Rg.Plugins.Popup.Popup.Init();
            

            MessagingCenter.Subscribe<Page>(this, "AllowLandscape", sender =>
            {
                this.allowRotation = false;
                UIDevice.CurrentDevice.SetValueForKey(NSNumber.FromNInt((int)(UIInterfaceOrientation.LandscapeLeft)), new NSString("orientation"));

            });
            MessagingCenter.Subscribe<Page>(this, "PreventLandscape", sender =>
            {
                this.allowRotation = true;
            });

            

            string db = "ScienceRecruiterApp_db.sqlite";
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"..", "Library");
            string fullpath = Path.Combine(path, db);

            Syncfusion.XForms.iOS.Border.SfBorderRenderer.Init();
            Syncfusion.XForms.iOS.Buttons.SfCheckBoxRenderer.Init();

            Syncfusion.SfChart.XForms.iOS.Renderers.SfChartRenderer.Init();

            Syncfusion.XForms.iOS.TabView.SfTabViewRenderer.Init();
            new SfRangeSliderRenderer();
            // Add the below line if you are using SfLinearProgressBar.
            Syncfusion.XForms.iOS.ProgressBar.SfLinearProgressBarRenderer.Init();

            // Add the below line if you are using SfCircularProgressBar.  
            Syncfusion.XForms.iOS.ProgressBar.SfCircularProgressBarRenderer.Init();


            Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer.Init();

            Syncfusion.XForms.iOS.Expander.SfExpanderRenderer.Init();

            CarouselViewRenderer.Init();

            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App(fullpath));
            

            return base.FinishedLaunching(app, options);
        }
    }
}
