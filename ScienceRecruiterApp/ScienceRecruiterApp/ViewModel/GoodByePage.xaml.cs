using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.ViewModel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoodByePage : ContentPage
    {
        public GoodByePage(string text)
        {
            InitializeComponent();
            label.Text = String.Concat("Thank you for your participation in ", text, "!. Tap anywhere to go to Main Page!");
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}