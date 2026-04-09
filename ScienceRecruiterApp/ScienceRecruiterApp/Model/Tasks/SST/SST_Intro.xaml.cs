using System;
using System.Collections.Generic;
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
    public partial class SST_Intro : ContentPage
    {
        public SST_Intro()
        {
            InitializeComponent();
            goL.Source = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoL.bmp");
            goR.Source = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.GoR.bmp");
            high.Source = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.High.bmp");
            low.Source = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.Low.bmp");
            stop.Source = ImageSource.FromResource("ScienceRecruiterApp.Model.Tasks.SST.Pictures.Stop.bmp");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //MessagingCenter.Send(this, "AllowLandscape");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //MessagingCenter.Send(this, "PreventLandscape");
        }
        protected override bool OnBackButtonPressed()
        {
            
            return base.OnBackButtonPressed();
            Navigation.PopToRootAsync();

        }

        private async void Grid_Tapped(object sender, EventArgs e)
        {
            //SST_TrialPage Page = new SST_TrialPage(new SettingsClass_SST());
            TrialPage_SST Page = new TrialPage_SST(App.settings_sst);
            await Navigation.PushAsync(Page);
        }
    }
}