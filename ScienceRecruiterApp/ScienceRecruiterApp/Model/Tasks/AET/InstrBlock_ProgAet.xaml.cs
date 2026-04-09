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
    public partial class InstrBlock_ProgAet : ContentPage
    {
        public InstrBlock_ProgAet(BlockClass_ProgAet block, int n)
        {
            InitializeComponent();
            Cue.Source = ImageSource.FromResource(block.IntroImage);
            progress.Text = String.Concat("You have ", (4 - n).ToString(), " blocks left");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}