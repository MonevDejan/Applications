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
    public partial class ActivateCode : ContentPage
    {
        string DeviceActivationId;

        public ActivateCode(string deviceActivationId)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            DeviceActivationId = deviceActivationId;
            Title = "";
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("ActivateCode Initialize");

        }

        private async void OnActivate(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCode.Text) || string.IsNullOrEmpty(txtCode.Text.Trim()))
            {
                Common.Instance.ShowToastMessage("Please provide correct activation code", DoubleResources.DangerSnackBar);
                return;
            }

            var activationCode = txtCode.Text.Trim();

            ProjectInsightWebApiClient client = new ProjectInsightWebApiClient(Common.WorkspaceApi);
            try
            {

                DeviceActivationRequestComplete completeRequest = await client.DeviceActivationRequest.CompleteRequestAsync(DeviceActivationId, activationCode, "Chrome", null);

                if (completeRequest == null)
                {
                    Common.Instance.ShowToastMessage("Please check the activation code or request new one", DoubleResources.DangerSnackBar);
                    return;
                }
                string ApiToken = completeRequest.ApiToken;
                DateTime TokenExpirationDateTime = completeRequest.AuthenticationTokenExpirationDateTimeUTC.Value;
                string DeviceId = completeRequest.DeviceId;
                string UserId = completeRequest.UserId;



                if (AuthenticationService.PI_Client == null)
                {
                    APIsInitialization.InitializeApis(ApiToken);
                    Common.CurrentWorkspace.ApiToken = ApiToken;
                    Common.CurrentWorkspace.UserID = new Guid(completeRequest.UserId);

                    Common.Instance._sqlconnection.Update(Common.CurrentWorkspace);

                    ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();
                    Common.UserGlobalCapability = userMe.UserGlobalCapability;
                    Common.WorkspaceCapability = WorkspaceService.GetWorkspaceCapability();
                    //var logingUser = await UsersService.GetUser(new Guid(UserId));


                    //ProjectInsightMobile.Models.User user = new ProjectInsightMobile.Models.User();
                    //user.Email = logingUser.EmailAddress;
                    //user.FirstName = logingUser.FirstName;
                    //user.LastName = logingUser.LastName;
                    //user.DateTimeCreated = DateTime.Now;
                    //user.UserToken = ApiToken;
                    //user.UserID = new Guid(UserId);
                    //Common.Instance.user = user;
                    //SQLiteConnection connection = Common.Instance.InitializeDatabase();
                    //Common.Instance.CreateTable<User>();
                    //Common.Instance._sqlconnection.Insert(user);
                }
                //Common.Instance.ShowToastMessage("Success", DoubleResources.SuccessSnackBar);
                //await Navigation.PushAsync(new StartupMasterPage());
                App.Current.MainPage = new StartupMasterPage();

            }
            catch (Exception ex)
            {

            }
        }
    }
}