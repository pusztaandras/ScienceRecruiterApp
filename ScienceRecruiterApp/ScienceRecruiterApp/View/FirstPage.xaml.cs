using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstPage : ContentPage
    
    {

        public string PrivacyText{
            get
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(FirstPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("ScienceRecruiterApp.Pictures.pp.txt");
                string text = "";
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                return text;
            }
            
            
        }

        public FirstPage()
        {
            
            
            InitializeComponent();
            BindingContext = this;
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if ((bool)CheckBox_accept.IsChecked)
            {
                await Navigation.PushAsync(new SecondPage());
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