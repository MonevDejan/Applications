using Plugin.DeviceInfo;
using ProjectInsight.Models.Authentication;
using ProjectInsight.Models.Users;
using ProjectInsight.WebApi.Client;
using ProjectInsight.WebApi.Client.Authentication;
using ProjectInsight.WebApi.Client.Issues;
using ProjectInsight.WebApi.Client.Tasks;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views;
using SafeSportChat.Views.Login;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//I will create a login to my dev site:
//https://dev28.sandbox.projectinsight.net/projectinsight.webapp/ 
//Use the new library with the dev version.I will also make a project and add a few tasks for both of you so that the main screen with all of the tasks/issues and work items can be shown.
//For the login look in the C# Client in the authentication client... look for LoginRequestForCustomApp    
//Make sure you name the Custom App Name property also.  
//When you've logged on, you can use the token returned for that user.  It will be good for 2 years by default.   
//The next call after that will be User.MyWork() which returns the values for the first screen.
//The object returned gives the order of items in a GUID list, but then the items are separated by type since models can't be polymorphic via REST.    
//RIght now only tasks are returned, but an update will be made to show all major items that the mobile app will return
//The token you get from the new login won't quite work right now, but will shortly.   
//With the new library you can work on the login process though.    There are two login processes.   One user name and password... 
//that's what we are doing first...then a second device activation method, which launches to the web app to login and get a code 
//to input into the mobile app.   THis one we can work on later.THis is what is used for clients with custom SSO etc.
//PI Mobile App - ok some notes about the main screen after login.I think we should store the data so that when someone logs in, 
//the existing data is there, and an async call in the back ground is made to update the screen.If items are to be removed, 
//then remove them with a fade, and add new items into the list with a fade.If that's possible.  The screen should be updated when 
//the app is launched, or if the data is 30 minutes old.. but the idea is to make it look like the data isn't being updated a tall, super 
//responsive to the user.So show the old data that  was there until the data is updated...anyway I trust that you guys understand what 
//i'm talking about.   Basically we don't want the mobile app to lock, or always query when returning from the main list on a display page


//1. Check the workspace if exists
//2. Move Project item to Project List
//3. Switching between workspaces
//4. Store all LoginResponse data in DB


namespace ProjectInsightMobile.Services
{
    public class AuthenticationService
    {
        public static ProjectInsightWebApiClient PI_Client;
        public static ProjectInsightWebApiClient PI_Client_New;
        public static LoginRequestResponse user = null;
        public static AuthenticationClient authenticationClient;
        //public static TaskClient taskClient;
        public static IssueClient issueClient;
        public bool CheckWebApiClient()
        {
            if (PI_Client == null)
                return true;
            else
                return false;
        }
        public async Task<LoginRequestResponse> Login(string email, string password)
        {
            //for (int i = 0; i < 5; i++)
            //{
            //    await Task.Factory.StartNew(() => {

            //    });
            //    await Task.Delay(1000);
            //}

            LoginRequestForCustomApp loginRequest = new LoginRequestForCustomApp();
            loginRequest.CustomAppName = "ProjectInsight.WebApp";
            loginRequest.EmailAddress = email;
            loginRequest.Password = password;

            PI_Client = new ProjectInsightWebApiClient(Common.WorkspaceApi);

            try
            {

                await Task.Factory.StartNew(() =>
                {
                    user = PI_Client.Authentication.LoginRequestForCustomApp(loginRequest);
                });
                //user = await PI_Client.Authentication.LoginRequestForCustomAppAsync(loginRequest);

                //ProjectInsight.Models.Users.User userL = new ProjectInsight.Models.Users.User();
                //userL.EmailAddress = email;
                //userL.UserName = email;
                //userL.Password = password;

                //user = await PI_Client.Authentication.LoginRequestForCustomAppAsync(userL);

                if (user.IsValid)
                {
                    //PI_Client.Authentication.AddQueryParam("api-token", user.Token);
                    authenticationClient = PI_Client.Authentication;
                    authenticationClient.ProjectInsightWebApiClient.ApiToken = user.Token;
                    //var sso = authenticationClient.GetLoginSettings();

                    //APIsInitialization.InitializeApis(Common.WorkspaceApi, user.Token);

                    return user;
                }
            }
            catch (Exception ex)
            {

            }
            return null;

        }

        public static async Task<ProjectInsight.Models.Authentication.LoginSettings> LoginSettings(string url)
        {
            ProjectInsightWebApiClient client = new ProjectInsightWebApiClient(url);
            try
            {
                ProjectInsight.Models.Authentication.LoginSettings response = await client.Authentication.GetLoginSettingsAsync();
                return response;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public static void Logout()
        {


            SQLiteConnection connection = Common.Instance.InitializeDatabase();
            Common.Instance.DeleteAll<MyWorkItem>();
            //Common.Instance.DeleteAll<Workspace>();
            //Common.Instance.DeleteAll<Models.User>();
            //Common.Instance.DeleteAll<NotificationLocal>();


            //clear PN device token
            //PushNotificationService client = new PushNotificationService();

            //client.SetPushNotificationTokenLegacy(string.Empty);

           

            string deviceId = CrossDeviceInfo.Current.Id;
            if (!string.IsNullOrEmpty(deviceId))
            {
                PushNotificationService client = new PushNotificationService();
                client.RemovePushNotificationTokenForCurrentUser(deviceId);
            }

            Common.CurrentWorkspace.ApiToken = string.Empty;
            Common.CurrentWorkspace.UserID = Guid.Empty;
            Common.CurrentWorkspace.PushNotifDateSent = null;
            Common.UserGlobalCapability = null;
            Common.WorkspaceCapability = null;

            var workspaces = Common.Instance.GetWorkspaces();
            foreach (Workspace ws in workspaces)
            {
                if (ws.Id == Common.CurrentWorkspace.Id)
                {
                    ws.ApiToken = string.Empty;
                    ws.UserID = Guid.Empty;
                    //ws.PushNotifDateSent = null;
                    Common.Instance._sqlconnection.Update(ws);
                    break;
                }
            }
          


            AuthenticationService.PI_Client = null;

            Common.Instance.ShowToastMessage("You successfully signed out", DoubleResources.SuccessSnackBar);

            App.Current.MainPage = new NavigationPage(new SignInUsingPhoneOrEmailPage());
        }

        public bool ForgotPassword(string workspace, string emailOrUsername)
        {
            try
            {
                ProjectInsightWebApiClient client = new ProjectInsightWebApiClient(Common.WorkspaceApi);
                var result = client.Authentication.ForgotPassword(emailOrUsername);
                return true;
            }
            catch(Exception ex)
            {
                return false;

            }
        }

        public bool ForgotUsername(string workspace, string email)
        {
            try
            {
                ProjectInsightWebApiClient client = new ProjectInsightWebApiClient(Common.WorkspaceApi);
                var result = client.Authentication.ForgotUsername(email);
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        
    }
}
