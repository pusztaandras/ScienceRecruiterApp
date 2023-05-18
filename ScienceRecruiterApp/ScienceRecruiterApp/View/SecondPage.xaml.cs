using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecondPage : ContentPage
    {
        public string TermsText
        {
            get
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(FirstPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("ScienceRecruiterApp.Pictures.terms.txt");
                string text = "";
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }


        }
        public SecondPage()
        {
            InitializeComponent();
            BindingContext = this;
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