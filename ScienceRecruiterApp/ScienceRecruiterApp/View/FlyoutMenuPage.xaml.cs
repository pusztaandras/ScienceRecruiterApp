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
    public partial class FlyoutMenuPage : ContentPage
    {
        public FlyoutMenuPage()
        {
            InitializeComponent();
            avatar.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.avatar_placeholder.png");

            NameLabel.Text = App.user.name;
            RankLabel.Text = "Youngling";

            PointsImage.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.points.png");
            SettingsImage.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.settings_icon.png");
            AboutImage.Source = ImageSource.FromResource("ScienceRecruiterApp.Pictures.qm.png");
        }
    }
}