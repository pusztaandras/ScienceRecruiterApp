using ScienceRecruiterApp.Model;
using ScienceRecruiterApp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp
{
    public partial class MainPage_loggedIn : ContentPage
    {
        public MainPage_loggedIn()
        {
            InitializeComponent();
            ////trialimage.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.Goal_Penguins.jpg") ;
            //logo.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.SciRecLogo.jpg");
            welcometext.Text = String.Concat("Welcome, ", App.user.name, "!");
            App.Filter = new FilterClass();
        }

        private async void ParticipantsButton_Clicked(object sender, EventArgs e)
        {
            
            Participant_MainPage page = new Participant_MainPage();
            await Navigation.PushAsync(page);


        }

        private async void ScientistButton_Clicked(object sender, EventArgs e)
        {
            //await UserSpec.RetriveID();
            
            Scientist_MainPage page = new Scientist_MainPage();
            await Navigation.PushAsync(page);

        }
    }
}
