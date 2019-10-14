using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Views;
using SQLite;
using Xamarin.Forms;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
//using HockeyApp;
using System.Collections.Generic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Plugin.Connectivity;
using SafeSportChat.Views.Login;

namespace ProjectInsightMobile
{
    public partial class App : Application
    {
        public App()
        {
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTMwMzUyQDMxMzcyZTMyMmUzMGJNN243dHFuWkNMRlVpaTY2U3BjVjVSSDh1Z2pMM0haVVdUSkFmOHh1ZnM9");

            InitializeComponent();


            //Check internet connection
            bool isConnected = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;


            if(isConnected)
            {
                Common.RefreshWorkList = true;
                Common.RefreshProjectsList = true;
            }
             //HockeyApp.MetricsManager.TrackEvent("Application initialization");

             SQLiteConnection connection = Common.Instance.InitializeDatabase();

            Common.Instance.bottomNavigationViewModel = new BottomNavigationViewModel();

            Common.Filter = new Filter();
            


            NavigationPage.SetBackButtonTitle(this, "");

            var workspaces = Common.Instance.GetWorkspaces();


            /// todo fixme
            if (workspaces == null || workspaces.Count() == 0)
            {
                if (Device.RuntimePlatform.ToLower() == "android")
                    MainPage = new NavigationPage(new StartUp());
                else
                    MainPage = new NavigationPage(new SignInWorkSpace());
                return;
            }

            Workspace ws;
            if (Common.PushNotificationAction != null)
            {
                ws = workspaces.FirstOrDefault(x => x.DomainID == Common.PushNotificationAction.DomainId);
                foreach (Workspace wsp in workspaces.Where(x => x.isActive))
                {
                    wsp.isActive = false;
                    Common.Instance._sqlconnection.Update(wsp);
                }
                if (ws != null)
                {
                    ws.isActive = true;
                    Common.Instance._sqlconnection.Update(ws);
                }

            }
            else
            {
                ws = workspaces.FirstOrDefault(x => x.isActive);

            }
            if (ws != null)
            {
                ws.isActive = true;
                Common.Instance._sqlconnection.Update(ws);

                Common.CurrentWorkspace = ws;

                //ws = workspaces.FirstOrDefault(x => x.isActive);

                if (!string.IsNullOrEmpty(ws.ApiToken) && ws.UserID != null)
                {
                    //check if ApiToken is not expired
                    var handler = new JwtSecurityTokenHandler();
                    var tokenS = handler.ReadToken(ws.ApiToken) as JwtSecurityToken;
                    var exp = tokenS.Claims.First(claim => claim.Type == "exp").Value;

                    var expDate = tokenS.ValidTo;
                    if (expDate < DateTime.UtcNow.AddMinutes(1))
                    {
                      
                        ws.ApiToken = null;
                        Common.Instance._sqlconnection.Update(ws);
                        Common.CurrentWorkspace = ws;
                        MainPage = new NavigationPage(new SignInUsingPhoneOrEmailPage());

                        return;
                    }

                    //if (AuthenticationService.PI_Client == null)
                    APIsInitialization.InitializeApis();


                    if (isConnected)
                    {
                        ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();

                        if (userMe != null)
                        {
                            Common.UserGlobalCapability = userMe.UserGlobalCapability;


                            //userMe.NotificationNewCount = 131;
                            //var respo = UsersService.SaveUser(userMe);

                            string notifCount = string.Empty;
                            //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = false;
                            if (userMe.NotificationNewCount != null && userMe.NotificationNewCount.Value > 0)
                            {
                                //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = true;
                                if (userMe.NotificationNewCount.Value > 99)
                                    notifCount = "99+";
                                else
                                    notifCount = userMe.NotificationNewCount.Value.ToString();
                            }
                            Common.Instance.bottomNavigationViewModel.NumberNottificationItems = notifCount;
                            Common.Instance.bottomNavigationViewModel.NumberWorkListItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType != ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;
                            Common.Instance.bottomNavigationViewModel.NumberProjectItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType == ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;


                        }
                        Common.WorkspaceCapability = WorkspaceService.GetWorkspaceCapability();
                    }

                    MainPage = new StartupMasterPage();

                    return;
                }
                else
                {
                    MainPage = new NavigationPage(new StartUp());
                    return;
                }
            }
            else
            {
                MainPage = new NavigationPage(new ChooseWorkspace());
                return;
            }

        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (e.IsConnected)
            {
                Common.RefreshWorkList = true;
                Common.RefreshProjectsList = true;

                ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();

                if (userMe != null)
                {
                    Common.UserGlobalCapability = userMe.UserGlobalCapability;


                    //userMe.NotificationNewCount = 131;
                    //var respo = UsersService.SaveUser(userMe);

                    string notifCount = string.Empty;
                    //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = false;
                    if (userMe.NotificationNewCount != null && userMe.NotificationNewCount.Value > 0)
                    {
                        //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = true;
                        if (userMe.NotificationNewCount.Value > 99)
                            notifCount = "99+";
                        else
                            notifCount = userMe.NotificationNewCount.Value.ToString();
                    }
                    Common.Instance.bottomNavigationViewModel.NumberNottificationItems = notifCount;
                    Common.Instance.bottomNavigationViewModel.NumberWorkListItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType != ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;
                    Common.Instance.bottomNavigationViewModel.NumberProjectItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType == ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;


                }
                Common.WorkspaceCapability = WorkspaceService.GetWorkspaceCapability();

            }
            else
            {
                Common.RefreshWorkList = false;
                Common.RefreshProjectsList = false;
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        
    }
}
