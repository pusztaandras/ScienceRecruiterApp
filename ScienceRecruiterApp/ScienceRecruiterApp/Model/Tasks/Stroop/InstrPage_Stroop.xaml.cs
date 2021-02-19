using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.Model.Tasks.Stroop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InstrPage_Stroop : ContentPage
    {
        public InstrPage_Stroop()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            TrialPage_Stroop page = new TrialPage_Stroop();
            await Navigation.PushAsync(page);
        }
        protected override bool OnBackButtonPressed()
        {
            
            return base.OnBackButtonPressed();
            Navigation.PopToRootAsync();

        }
    }
}