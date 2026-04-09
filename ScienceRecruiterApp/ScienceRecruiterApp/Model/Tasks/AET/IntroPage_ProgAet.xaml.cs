using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp.Model.Tasks.AET
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntroPage_ProgAet : ContentPage
    {
        public IntroPage_ProgAet()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new TrialPage_ProgAet());
        }
    }
}