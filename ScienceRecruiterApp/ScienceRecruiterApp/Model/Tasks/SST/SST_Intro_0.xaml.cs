using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp.Model.Tasks.SST
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SST_Intro_0 : ContentPage
    {
        public SST_Intro_0()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    MessagingCenter.Send(this, "PreventLandscape");
        //}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            SST_Intro Page = new SST_Intro();
            await Navigation.PushAsync(Page);
        }
    }
}