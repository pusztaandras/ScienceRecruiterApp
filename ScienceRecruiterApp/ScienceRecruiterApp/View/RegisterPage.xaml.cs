using ScienceRecruiterApp.Model;
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
    public partial class RegisterPage : ContentPage
    {
        public List<string> genderlist;
        public List<string> handlist;
        public List<string> druglist;
        public RegisterPage()
        {
            InitializeComponent();
            disorderlabel.TextColor = Color.FromRgba(0,0,0,0.1);
            druglabel.TextColor = Color.FromRgba(0, 0, 0, 0.1);
            genderlist = new List<string>();
            genderlist.Add("female");
            genderlist.Add("male");
            genderBox.ComboBoxSource = genderlist;
            handlist = new List<string>();
            handlist.Add("left");
            handlist.Add("right");
            handBox.ComboBoxSource = handlist;
            druglist = new List<string>();
            druglist.Add("Yes");
            druglist.Add("No");
            druglist.Add("I wish not to answer");
            drugBox.ComboBoxSource = druglist;
            disorderBox.ComboBoxSource = druglist;
            disorderBox.SelectionChanged += DisorderBox_SelectionChanged;
            drugBox.SelectionChanged += DrugBox_SelectionChanged;
        }

        private void DrugBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (drugBox.SelectedIndex == 0)
            {
                druglabel.TextColor = Color.Black;
            }
            else
            {
                druglabel.TextColor = Color.FromRgba(0, 0, 0, 0.1);
            }
        }

        private void DisorderBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (disorderBox.SelectedIndex == 0)
            {
                disorderlabel.TextColor = Color.Black;
            }
            else
            {
                disorderlabel.TextColor = Color.FromRgba(0, 0, 0, 0.1);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //check name entry
            if (String.IsNullOrWhiteSpace(NameEntry.Text))
            {
                await DisplayAlert("Alert", "Please fill out your name!", "OK");
                return;
            }
            
            //check age entry
            if (String.IsNullOrWhiteSpace(Age.Text))
            {
                await DisplayAlert("Alert", "Missing age", "OK");
                return;
            }
            //check gender
            if (genderBox.SelectedIndex==-1)
            {
                await DisplayAlert("Alert", "Select a gender", "OK");
                return;
            }
            //check hand
            if (handBox.SelectedIndex == -1)
            {
                await DisplayAlert("Alert", "Select a dominant hand", "OK");
                return;
            }
            //check drug
            if (drugBox.SelectedIndex == -1)
            {
                await DisplayAlert("Alert", "Question remain unanswered", "OK");
                return;
            }
            //check drug 2
            if (drugBox.SelectedIndex == 0)
            {
                if (String.IsNullOrWhiteSpace(DrugEntry.Text))
                {
                    await DisplayAlert("Alert", "Please fill out the entry!", "OK");
                    return;
                }
                else
                {
                    
                }
            }
            //check disorder
            if (disorderBox.SelectedIndex == -1)
            {
                await DisplayAlert("Alert", "Question remain unanswered", "OK");
                return;
            }
            //check disorder 2
            if (disorderBox.SelectedIndex == 0)
            {
                if (String.IsNullOrWhiteSpace(DisorderEntry.Text))
                {
                    await DisplayAlert("Alert", "Please fill out the entry!", "OK");
                    return;
                }
                else
                {
                    
                }
            }

            //Check if email already exist in db
            Logic.ApiLogic resultsLogic = new Logic.ApiLogic();
            //List<string> emails = await resultsLogic.GetMails();
            //if (emails.Contains(emailEntry.Text))
            //{
            //    await DisplayAlert("Alert", "Already registered with this email address", "OK");
            //    return;
            //}
            //Put in variables
            var tempuser = new Model.UserSpec();
            tempuser.name = NameEntry.Text;
            
            tempuser.age =  DateTime.Now.Year-Int32.Parse(Age.Text);
            tempuser.gender = genderlist[genderBox.SelectedIndex];
            tempuser.hand = handlist[handBox.SelectedIndex];
            tempuser.isDisorder=druglist[disorderBox.SelectedIndex];
            tempuser.isDrug = druglist[drugBox.SelectedIndex];
            tempuser.Drug = DrugEntry.Text;
            tempuser.Disorder = DisorderEntry.Text;
            //POST to local db
            SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DataBaseLocation);
            conn.CreateTable<UserSpec>();
            conn.Insert(tempuser);

            App.user = conn.Table<UserSpec>().FirstOrDefault();

            //POST to db
            await resultsLogic.PostUser(tempuser);
            Application.Current.MainPage = new NavigationPage(new MainPage());
            
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