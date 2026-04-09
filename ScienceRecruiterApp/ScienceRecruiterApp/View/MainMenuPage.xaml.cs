using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
            logo.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.SciRecLogo.jpg");
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {

            LoginPage page = new LoginPage();
            await Navigation.PushAsync(page);


        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            //await UserSpec.RetriveID();

            RegisterPage page = new RegisterPage();
            await Navigation.PushAsync(page);

        }
    }
    
}