using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            try
            {
                App.settings_sst = new SettingsClass_SST();
                InitializeComponent();
                logoImage.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.SciRecLogo_trpbckgr.png");
                tabbedpage_main.IconImageSource = ImageSource.FromResource("ScienceRecruiterApp.Pictures.SciRecLogo_trpbckgr.png");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

       
    }
}