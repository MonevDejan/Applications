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
using ProjectInsightMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.ViewModels;
using System.Collections.ObjectModel;
using ProjectInsightMobile.CustomControls;
using Plugin.DeviceInfo;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseWorkspace : ContentPage
    {
        WorkspacesViewModel vm;
        public ChooseWorkspace()
        {

            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("ChooseWorkspace Initialize");

            var workspaces = Common.Instance.GetWorkspaces();
            //TODO Temp solution to extract the names
            foreach (var ws in workspaces)
            {
                //string temp = ws.WorkspaceURL.Replace("https://", "").Replace("http://", "");
                //temp = temp.Substring(0, temp.IndexOf(".projectinsight.net"));
                ws.Name = ws.Name;
                ws.Icon = "ws_logo.png";
            }

            vm = new WorkspacesViewModel();
            vm.Items = new ObservableCollection<Workspace>(workspaces); ;

            //Add Existing Workspace Button
            Workspace addExisting = new Workspace();
            addExisting.WorkspaceURL = "";
            addExisting.Name = "Add Existing Workspace";
            addExisting.Id = -1;
            addExisting.Icon = "ws_plus.png";
            vm.Items.Add(addExisting);

            if (Xamarin.Forms.Device.RuntimePlatform.ToLower() == "android")
            {
                //Add Existing Workspace Button
                Workspace addNew = new Workspace();
                addNew.WorkspaceURL = "";
                addNew.Name = "Create New PI#team";
                addNew.Id = -2;
                addNew.Icon = "ws_plus.png";
                vm.Items.Add(addNew);
            }

            BindingContext = vm;

            if (Xamarin.Forms.Device.RuntimePlatform.ToLower() == "android")
            {
                ItemsListViewDroid.IsVisible = true;
                ItemsListViewiOS.IsVisible = false;
            }
            else
            {
                ItemsListViewDroid.IsVisible = false;
                ItemsListViewiOS.IsVisible = true;
            }

        }

        private async void GridTemplate_Tapped(object sender, EventArgs e)
        {
            //try
            //{
            if (sender is SwipeGestureGrid || sender is Grid)
            {
                var templateGrid = (Grid)sender;
                if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                {
                    Workspace workspace = ((Workspace)templateGrid.Parent.BindingContext);
                    if (workspace.Id >= 0)
                    {
                        var workspaces = Common.Instance.GetWorkspaces();
                        foreach (Workspace ws in workspaces.Where(x => x.isActive))
                        {
                            ws.isActive = false;
                            Common.Instance._sqlconnection.Update(ws);
                        }


                        workspace.isActive = true;

                        Common.Instance._sqlconnection.Update(workspace);
                        Common.CurrentWorkspace = workspace;

                        if (string.IsNullOrEmpty(workspace.ApiToken) || workspace.UserID == null)
                        {
                            await Navigation.PushAsync(new SignIn(workspace));
                        }
                        else
                        {
                            APIsInitialization.InitializeApis();

                            ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();

                            Common.UserGlobalCapability = userMe.UserGlobalCapability;
                            Common.WorkspaceCapability = WorkspaceService.GetWorkspaceCapability();
                            Common.Instance.bottomNavigationViewModel.ActiveIcon = 1;


                            string notifCount = string.Empty;
                            //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = false;
                            if (userMe.NotificationNewCount != null && userMe.NotificationNewCount.Value > 0)
                            {
                              //  Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = true;
                                if (userMe.NotificationNewCount.Value > 99)
                                    notifCount = "99+";
                                else
                                    notifCount = userMe.NotificationNewCount.Value.ToString();
                            }

                            Common.Instance.bottomNavigationViewModel.NumberNottificationItems = notifCount;
                            Common.Instance.bottomNavigationViewModel.NumberWorkListItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType != ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;
                            Common.Instance.bottomNavigationViewModel.NumberProjectItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType == ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;


                            App.Current.MainPage = new StartupMasterPage();
                        }
                        
                        //var itemID = item.Id;
                        
                    }
                    else if (workspace.Id == -1)
                    {
                        await Navigation.PushAsync(new SignInWorkSpace());
                    }
                    else if (workspace.Id == -2)
                    {
                        if (Xamarin.Forms.Device.RuntimePlatform.ToLower() == "android")
                            await Navigation.PushAsync(new StartUp());
                        else
                            await Navigation.PushAsync(new SignInWorkSpace());
                        
                    }
                }
            }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
        private void GridTemplate_SwipeLeft(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = (Grid)sender;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                        Workspace workspace = ((Workspace)templateGrid.Parent.BindingContext);

                        if (workspace.Id >= 0)
                        {
                            templateGrid.ColumnDefinitions[1].Width = new GridLength(50, GridUnitType.Absolute);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void GridTemplate_SwipeRight(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = (Grid)sender;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                       
                        templateGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void deleteWorkspace(object sender, EventArgs e)
        {
            try
            {
                if (sender is StackLayout)
                {
                    var templateGrid = (StackLayout)sender;

                    //var grid = templateGrid.Parent.Parent.Children.IEnumerator.[0];
                    var par = templateGrid.Parent.Parent;
                    var grid = (SwipeGestureGrid)par;
                    
                    if (grid != null && grid.BindingContext != null)
                    {

                        Workspace item = ((Workspace)grid.BindingContext);
                        grid.IsVisible = false;
                        grid.HeightRequest = 0;

                        Common.Instance._sqlconnection.Delete(item);
                        string deviceId = CrossDeviceInfo.Current.Id;
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            PushNotificationService client = new PushNotificationService(item);
                            client.RemovePushNotificationTokenForWorkspace(deviceId);
                        }


                    }
                   
                }
                if (sender is MenuItem)
                {
                    var menuItem = ((MenuItem)sender);
                    if (menuItem != null && menuItem.BindingContext != null)
                    {
                        Workspace ws = ((Workspace)menuItem.BindingContext);
                        if (ws.Id >= 0)
                        {
                            Common.Instance._sqlconnection.Delete(ws);
                            vm.Items.Remove(ws);
                            string deviceId = CrossDeviceInfo.Current.Id;
                            if (!string.IsNullOrEmpty(deviceId))
                            {
                                PushNotificationService client = new PushNotificationService(ws);
                                client.RemovePushNotificationTokenForWorkspace(deviceId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnHide(object sender, EventArgs e)
        {
            try
            {
                if (sender is StackLayout)
                {
                    var templateGrid = (StackLayout)sender;

                    //var grid = templateGrid.Parent.Parent.Children.IEnumerator.[0];
                    var par = templateGrid.Parent.Parent;
                    var grid = (SwipeGestureGrid)par;

                    if (grid != null && grid.BindingContext != null)
                    {

                        Workspace item = ((Workspace)grid.BindingContext);
                        grid.IsVisible = false;
                        grid.HeightRequest = 0;

                        Common.Instance._sqlconnection.Delete(item);
                        //vm.Items.Remove(item);
                        //var itemID = item.Id;


                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell theViewCell = ((ViewCell)sender);
            var item = theViewCell.BindingContext as Workspace;
            theViewCell.ContextActions.Clear();

            if (item != null)
            {
                if (item.Id >= 0)
                {
                    var deleteItem = new MenuItem()
                    {
                        Text = "Delete",
                        IsDestructive = true,

                    };
                    deleteItem.Clicked += deleteWorkspace;
                    theViewCell.ContextActions.Add(deleteItem);
                }
            }
        }
    }
}