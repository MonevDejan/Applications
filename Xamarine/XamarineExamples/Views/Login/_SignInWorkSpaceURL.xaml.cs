using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
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
	public partial class SignInWorkSpaceURL : ContentPage
	{
		public SignInWorkSpaceURL()
		{
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent ();
           //HockeyApp.MetricsManager.TrackEvent("SignInWorkSpace Initialize");


        }
        private async void OnContinue(object sender, EventArgs e)
        {

            string workspaceUrl = txtWorkspace.Text;

            if (workspaceUrl == null || workspaceUrl.Length == 0)
            {
                Common.Instance.ShowToastMessage("Please insert workspace full URL", DoubleResources.DangerSnackBar);
            }
            else
            {
                //check URL 

                //if (workspaceUrl.ToLower().StartsWith("http://"))
                //{

                //    workspaceUrl = workspaceUrl.ToLower().Replace("http://", "https://");

                //}
                //else 
                if (!workspaceUrl.ToLower().StartsWith("http://") && !workspaceUrl.ToLower().StartsWith("https://"))
                {
                    workspaceUrl = "https://" + workspaceUrl;
                }

                Uri uriResult;
                bool isValid = Uri.TryCreate(workspaceUrl, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                bool isValid2 = Uri.IsWellFormedUriString(uriResult.ToString(), UriKind.Absolute);

                if (!isValid || !isValid2)
                {
                    Common.Instance.ShowToastMessage("Please insert valid URL", DoubleResources.DangerSnackBar);
                }
                else
                {
                    //Common.WorkspaceUrl = workspaceUrl;
                    GetLoginSettings(workspaceUrl);
                }

                //SQLiteConnection connection = Common.Instance.InitializeDatabase();
                //WorkSpace workSpace = new WorkSpace();
                //workSpace.Name = workSpaceVal;
                //Common.Instance.CreateTable<WorkSpace>();
                //Common.Instance._sqlconnection.Insert(workSpace);
            }
        }

        private async void GetLoginSettings(string workspaceUrl)
        {
            try
            {
                
                //await Navigation.PushAsync(new SignIn(true, true));

                var settings = await AuthenticationService.LoginSettings(workspaceUrl);
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

                    if (!workspaces.Any(x => x.WorkspaceURL.ToLower() == workspaceUrl.ToLower()))
                    {
                        Common.Instance.CreateTable<Workspace>();
                        Workspace workspace = new Workspace()
                        {
                            EmailPasswordEnabled = settings.SSOEnabled,
                            SSOEnabled = settings.SSOEnabled,
                            Name = settings.CustomerName,
                            WorkspaceURL = workspaceUrl,
                            isActive = true,
                            DomainID = settings.DomainId
                        };
                        Common.Instance._sqlconnection.Insert(workspace);
                        Common.CurrentWorkspace = workspace;
                        await Navigation.PushAsync(new SignIn(workspace));
                    }
                    else
                    {
                        Workspace ws = workspaces.FirstOrDefault(x => x.WorkspaceURL.ToLower() == workspaceUrl.ToLower());
                        ws.isActive = true;
                        Common.CurrentWorkspace = ws;
                        Common.Instance._sqlconnection.Update(ws);
                        await Navigation.PushAsync(new SignIn(ws));
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
        }
        
    }
}