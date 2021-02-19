using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecondPage : ContentPage
    {
        public SecondPage()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if ((bool)CheckBox_accept.IsChecked)
            {
                await Navigation.PushAsync(new RegisterPage());
            }
            else
            {
                await DisplayAlert("Alert", "You need to accept before continue", "OK");
                CheckBox_accept.TextColor = Color.Red;
                return;
            }
        }
    }
}