using SafeSportChat.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using SQLite;
using ProjectInsightMobile.Models;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartUp : ContentPage
	{
		public StartUp ()
		{
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent ();
           //HockeyApp.MetricsManager.TrackEvent("StartUp Initialize");

        }
        private async void OnHaveAnExistingAccount(object sender, EventArgs e)
        {
            string workSpaceVal = "dev30-gjokov.sandbox";

            if (workSpaceVal == null || workSpaceVal.Length == 0)
            {
                Common.Instance.ShowToastMessage("Please insert Workspace Name", DoubleResources.DangerSnackBar);
            }
            else
            {
                string url = "https://" + workSpaceVal.Trim() + ".projectinsight.net";

                //THIS IS ONLY FOR DEMO PURPOSSES
                if (workSpaceVal.ToLower().StartsWith("dev"))
                {
                    url = "https://" + workSpaceVal + ".projectinsight.net/ProjectInsight.WebApp";
                }
                //Common.WorkspaceUrl = url;
                await GetLoginSettings(url);
            }

        }

        private async void OnContinue(object sender, EventArgs e)
        {
            

            string workSpaceVal = "dev30-gjokov.sandbox";

            if (workSpaceVal == null || workSpaceVal.Length == 0)
            {
                Common.Instance.ShowToastMessage("Please insert Workspace Name", DoubleResources.DangerSnackBar);
            }
            else
            {
                string url = "https://" + workSpaceVal.Trim() + ".projectinsight.net";

                //THIS IS ONLY FOR DEMO PURPOSSES
                if (workSpaceVal.ToLower().StartsWith("dev"))
                {
                    url = "https://" + workSpaceVal + ".projectinsight.net/ProjectInsight.WebApp";
                }
                //Common.WorkspaceUrl = url;
                await GetLoginSettings(url);
            }
          
        }

        private async Task<bool> GetLoginSettings(string url)
        {
            try
            {
                url = url.ToLower();

                var settings = await AuthenticationService.LoginSettings(url);
                if (settings != null)
                {
                    //Common.Instance.DeleteAll<Workspace>();
                    SQLiteConnection connection = Common.Instance.InitializeDatabase();
                    var workspaces = Common.Instance.GetWorkspaces();
                    foreach (Workspace ws in workspaces.Where(x => x.isActive))
                    {
                        ws.isActive = false;
                        Common.Instance._sqlconnection.Update(ws);
                    }
                    if (!workspaces.Any(x => x.WorkspaceURL.ToLower() == url.ToLower()))
                    {
                        Common.Instance.CreateTable<Workspace>();
                        Workspace workspace = new Workspace()
                        {
                            EmailPasswordEnabled = settings.SSOEnabled,
                            SSOEnabled = settings.SSOEnabled,
                            Name = settings.CustomerName,
                            WorkspaceURL = url,
                            isActive = true,
                            DomainID = settings.DomainId
                        };
                        try
                        {
                            Common.Instance._sqlconnection.Insert(workspace);
                        }
                        catch
                        {
                            Common.Instance._sqlconnection.DropTable<Workspace>();
                            Common.Instance.ShowToastMessage("Table schema has been updated. Please try again!", DoubleResources.DangerSnackBar);
                            Common.CurrentWorkspace = null;
                            return true;
                        }
                        Common.CurrentWorkspace = workspace;
                        await Navigation.PushAsync(new SignInUsingPhoneOrEmailPage());
                    }
                    else
                    {
                        Workspace ws = workspaces.FirstOrDefault(x => x.WorkspaceURL.ToLower() == url.ToLower());
                        ws.isActive = true;
                        Common.Instance._sqlconnection.Update(ws);
                        Common.CurrentWorkspace = ws;
                        await Navigation.PushAsync(new SignInUsingPhoneOrEmailPage());
                    }
                }
                else
                {
                    Common.CurrentWorkspace = null;
                    Common.Instance.ShowToastMessage("Workspace doesn't exist!", DoubleResources.DangerSnackBar);
                }
            }
            catch (Exception ex)
            {
                Common.CurrentWorkspace = null;
                Common.Instance.ShowToastMessage("Workspace doesn't exist!", DoubleResources.DangerSnackBar);
                //await Navigation.PushAsync(new SignInWorkSpace());
            }
            return true;

        }


        private async void OnCreateAccount(object sender, EventArgs e)
        {

            //var answereSelect = await DisplayActionSheet("Thanks for creating an account, we’ll take you to your browser to sign up and once you’re done, you can come back here to log in", "Cancel","OK" );
            //if (answereSelect != null && answereSelect.ToString().Length > 0 && answereSelect.Equals("OK"))
            //{
            //    Device.OpenUri(new Uri("https://team.projectinsight.net/team/signup"));

            //}
            await Navigation.PushAsync(new CreateAccountPage());
        }

        private async void LogInButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInUsingPhoneOrEmailPage());
        }
    }
}