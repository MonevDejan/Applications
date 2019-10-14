using Plugin.Connectivity;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInWorkSpace : ContentPage
	{
        CustomBaseViewModel vm;

        public SignInWorkSpace ()
		{
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent ();
            //HockeyApp.MetricsManager.TrackEvent("SignInWorkSpace Initialize");
            slNoConnection.Children.Clear();
            slNoConnection.Children.Add(new NoConnectionBand());

            vm = new CustomBaseViewModel();

            BindingContext = vm;


        }
        private async void OnContinue(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            vm.LoadingMessage = "";
            vm.VisibleLoad = true;
            string workSpaceVal = txtWorkspace.Text;

            if (workSpaceVal == null  || workSpaceVal.Length == 0 )
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
            vm.VisibleLoad = false;
        }

        private async Task<bool> GetLoginSettings(string url)
        {
            try
            {
                url = url.ToLower();
                //await Navigation.PushAsync(new SignIn(true, true));

                var settings = await AuthenticationService.LoginSettings(url);
                if (settings != null)
                {
                    //Common.Instance.DeleteAll<Workspace>();
                    SQLiteConnection connection = Common.Instance.InitializeDatabase();
                    var workspaces = Common.Instance.GetWorkspaces();
                    foreach (Workspace ws in workspaces.Where(x=>x.isActive))
                    {
                        ws.isActive = false;
                        Common.Instance._sqlconnection.Update(ws);
                    }
                    if (!workspaces.Any(x=>x.WorkspaceURL.ToLower() == url.ToLower()))
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
                        await Navigation.PushAsync(new SignIn(workspace));
                    }
                    else
                    {
                        Workspace ws = workspaces.FirstOrDefault(x => x.WorkspaceURL.ToLower() == url.ToLower());
                        ws.isActive = true;
                        Common.Instance._sqlconnection.Update(ws);
                        Common.CurrentWorkspace = ws;
                        await Navigation.PushAsync(new SignIn(ws));
                    }
                }
                else
                {
                    Common.CurrentWorkspace =null;
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

        private async void loginWithUrl(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;
            await Navigation.PushAsync(new SignInWorkSpaceURL());
        }
    }
}