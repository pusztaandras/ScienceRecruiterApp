using ScienceRecruiterApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScienceRecruiterApp.Logic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScienceRecruiterApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimationPage : ContentPage
    {
        public AnimationPage()
        {
            InitializeComponent();
            ScirecLogo.Source= ImageSource.FromResource("ScienceRecruiterApp.Pictures.SciRecLogo_trpbckgr.png");
            Animate();
            
        }

        private async void Animate()
        {
            
            await ScirecLogo.FadeTo(1, 800);
            await ScirecLogo.FadeTo(0, 800);
            await ScirecLogo.FadeTo(1, 800);
            await ScirecLogo.FadeTo(0, 800);
            SQLiteConnection conn = new SQLiteConnection(App.DataBaseLocation);
            conn.CreateTable<UserSpec>();
            //Debug mode:create firstpage anyways

            //if (conn.Table<UserSpec>().ToList().Count > 0)
            //{
            //    App.user = conn.Table<UserSpec>().FirstOrDefault();
                
            //    //check if user is on server
            //    ApiLogic apiLogic = new ApiLogic();
            //    UserSpec tempuser = await apiLogic.GetUserId(App.user.id);
            //    if(tempuser!=null)
            //    {
            //        Application.Current.MainPage = new NavigationPage(new MainPage());
            //        App.user = tempuser;
            //    }
            //    else
            //    {
            //        conn.DeleteAll<UserSpec>();
            //        Application.Current.MainPage = new NavigationPage(new FirstPage());
            //    }
            //}
            //else
            //{
                Application.Current.MainPage = new NavigationPage(new FirstPage());
            //}
        }
    }
}