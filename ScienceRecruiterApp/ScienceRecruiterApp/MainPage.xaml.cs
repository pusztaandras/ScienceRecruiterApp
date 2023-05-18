using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            App.settings_sst = new SettingsClass_SST();
            InitializeComponent();
            logoImage.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.SciRecLogo_trpbckgr.png");
            tabbedpage_main.IconImageSource= ImageSource.FromResource("ScienceRecruiterApp.Pictures.SciRecLogo_trpbckgr.png");
        }

       
    }
}