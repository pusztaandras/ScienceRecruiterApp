using ScienceRecruiterApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public UserSpec spec;

        public LoginPage()
        {
            InitializeComponent();
        }

       
       

        private async void Login(object sender, EventArgs e)
        {
            //check email entry
            if (String.IsNullOrWhiteSpace(emailEntry.Text))
            {
                await DisplayAlert("Alert", "E-mail missing", "OK");
                return;
            }
            else
            {
                if (!validatemail(emailEntry.Text))
                {
                    await DisplayAlert("Alert", "Incorrect email", "OK");
                    return;
                }
            }
            //check pass entry
            if (String.IsNullOrWhiteSpace(passEntry.Text))
            {
                await DisplayAlert("Alert", "Missing password", "OK");
                return;
            }

            //Get userId from db
            Logic.ApiLogic apiLogic = new Logic.ApiLogic();
            UserSpec datafromdb = await apiLogic.GetUserId(emailEntry.Text);
            //
            if (datafromdb.pass == passEntry.Text)
            {
                App.user = datafromdb;
                MainPage_loggedIn page = new MainPage_loggedIn();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert("Alert", "Wrong password", "OK");
                return;
            }
        }

        private bool validatemail(string text)
        {
            try
            {
                MailAddress m = new MailAddress(text);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }



    }
}