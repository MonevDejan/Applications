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
    public partial class ForgotPassword : ContentPage
    {
      
        private bool isForgotPassword = true;

        public ForgotPassword()
        {
        }

        public ForgotPassword(bool forgotPassword)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("ForgotPassword Initialize");

            try
            {
                isForgotPassword = forgotPassword;
                if (isForgotPassword)
                {
                    //lblTitle.Text = "Username or Email Address";
                    txtUserName.Placeholder = "Username or Email Address";
                    Title = "";
                    txtHelp.Text = "Send Password Reset";
                    btnSend.Text = "Send Password Reset";
                }
                else
                {
                    //lblTitle.Text = "Email Address";
                    txtUserName.Placeholder = "Email Address";
                    Title = "";
                    txtHelp.Text = "Send Username";
                    btnSend.Text = "Send Username";

                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void Send(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                Common.Instance.ShowToastMessage("Please insert email or username", DoubleResources.DangerSnackBar);
            }
            else
            {
                AuthenticationService cs = new AuthenticationService();
                if (isForgotPassword)
                    cs.ForgotPassword(Common.CurrentWorkspace.WorkspaceURL, txtUserName.Text.Trim());
                else
                    cs.ForgotUsername(Common.CurrentWorkspace.WorkspaceURL, txtUserName.Text.Trim());

                Common.Instance.ShowToastMessage("You will receive Email with details soon", DoubleResources.SuccessSnackBar);

                await Navigation.PushAsync(new SignInWorkSpace());
            }
        }
    }
}