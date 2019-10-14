using ProjectInsight.Models.Devices;
using ProjectInsight.Models.Users;
using ProjectInsight.WebApi.Client;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.General
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotNameOrPassword : ContentPage
    {

        public ForgotNameOrPassword()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("ForgotNameOrPassword Initialize");

        }

        private async void ForgotPassword(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword(true));

        }
        private async void ForgotUsername(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ForgotPassword(false));
        }
    }
}