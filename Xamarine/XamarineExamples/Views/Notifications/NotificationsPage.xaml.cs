using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using SQLite;                            
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsight.Models;
using System.Text.RegularExpressions;
using ProjectInsightMobile.Views.RelatedItems;
using ProjectInsightMobile.Views.Profile;
using FFImageLoading.Forms;
using HtmlAgilityPack;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NotificationsPage : ContentPage
    {
        List<BodyItemNorification> bodyItemsNorification;
        //ObservableCollection<ProjectInsight.Models.Notifications.Notification> myCollection;
        ObservableCollection<ProjectInsightMobile.ViewModels.Notification> myCollection;
        NotificationsViewModel viewModel;
        public NotificationsPage()
		{
            NavigationPage.SetBackButtonTitle(this, "");

            InitializeComponent ();
            viewModel = new NotificationsViewModel();
            //viewModel.Notifications = new ObservableCollection<ProjectInsight.Models.Notifications.Notification>();
            bodyItemsNorification = new List<BodyItemNorification>();
            StartLoading();
            BindingContext = viewModel;

           //HockeyApp.MetricsManager.TrackEvent("NotificationsPage");


            //if (Device.RuntimePlatform.ToLower() == "android")
            //{
            //    ItemsListViewDroid.IsVisible = true;
            //    ItemsListViewiOS.IsVisible = false;
            //}
            //else
            //{
            //    ItemsListViewDroid.IsVisible = false;
            //    ItemsListViewiOS.IsVisible = true;
            //}

        }

        class BodyItemNorification
        {                                 
            public Guid Id { get; set; }
            public String Value { get; set; }
        }

        private async void StartLoading()
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetNotifications();

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
                viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            ItemsListViewiOS.EndRefresh();
            IsBusy = false;
        }

        private async Task<bool> GetNotifications()
        {

            try
            {
                //Reset the badge counter
                SQLiteConnection connection = Common.Instance.InitializeDatabase();
                ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();
                userMe.NotificationNewCount = 0;
                //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = false;
                Common.Instance.bottomNavigationViewModel.NumberNottificationItems = string.Empty;
                var saveresp = UsersService.SaveUser(userMe);


                var notifications = await NotificationsService.Get();
                if (notifications != null)
                {
                    myCollection = new ObservableCollection<Notification>();
                    foreach (var notif in notifications)
                    {

                        while (notif.Body.Contains("display:none"))
                        {
                            int from = notif.Body.Substring(0, notif.Body.IndexOf("display:none")).LastIndexOf("<");
                            int to = notif.Body.IndexOf(">", notif.Body.IndexOf("display:none;\">") + 15) + 1;
                            notif.Body = notif.Body.Substring(0, from) + notif.Body.Substring(to);
                        }

                        var newNotif = new Notification()
                        {
                            Body = notif.Body,
                            CreatedDateTimeUTC = notif.CreatedDateTimeUTC,
                            IconImageUrl = notif.IconImageUrl,
                            IconUrl = notif.IconUrl,
                            ObjectEvent_Id = notif.ObjectEvent_Id,
                            ObjectTypeString = notif.ObjectTypeString,
                            IsBusy = false,
                            Object_Id = notif.Object_Id,
                            UserAvatarHTML = notif.UserAvatarHTML,
                            UserCreated_Id = notif.UserCreated_Id,
                            UserPhotoImageUrl = notif.UserPhotoImageUrl
                        };

                        newNotif.AvatarColor = "#1474BA";
                        newNotif.AvatarInitials = "PI";
                        if (string.IsNullOrEmpty(notif.UserPhotoImageUrl))
                        {
                            newNotif.ShowUserAvatar = true;
                            newNotif.ShowUserPhoto = false;
                            if (!string.IsNullOrEmpty(notif.UserAvatarHTML))
                            {
                                string userHTML = notif.UserAvatarHTML;
                                //"<div class=\"user-avatar\" title=\"Gjoko Veljanoski\" style=\"background-color:#00bfff\">GV</div>"

                                string myDiv = notif.UserAvatarHTML;
                                HtmlDocument doc = new HtmlDocument();
                                doc.LoadHtml(myDiv);
                                HtmlNode node = doc.DocumentNode.SelectSingleNode("div");

                                newNotif.AvatarInitials = (node.ChildNodes[0]).OuterHtml;

                                foreach (HtmlAttribute attr in node.Attributes)
                                {
                                    if (attr.Name.ToLower() == "style")
                                    {
                                        string[] parts = attr.Value.Split('#');
                                        if (parts != null && parts.Length > 1)
                                        {
                                            newNotif.AvatarColor = "#" + parts[1];
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            newNotif.IconImageUrl = Common.CurrentWorkspace.WorkspaceURL + notif.UserPhotoImageUrl;
                            newNotif.ShowUserAvatar = false;
                            newNotif.ShowUserPhoto = true;
                        }
                        myCollection.Add(newNotif);

                    }
                }

                var template = new DataTemplate(typeof(TextCell));

                // We can set data bindings to our supplied objects.
                template.SetBinding(TextCell.TextProperty, "CreatedDateTimeUTC");
                template.SetBinding(TextCell.DetailProperty, "Body");

                //ItemsListView.ItemTemplate = template;

                if (myCollection == null || myCollection.Count == 0)
                    lblNoNotif.IsVisible =  true;
                else
                    lblNoNotif.IsVisible = false;

                viewModel.Notifications = myCollection;
                BindingContext = viewModel;
                


                
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async void ExecuteLoadItemsNotifications(object sender, EventArgs e)
        {
            IsBusy = true;
            StartLoading();
            BindingContext = viewModel;
            ItemsListViewiOS.EndRefresh();
            IsBusy = false;
        }
        //private async void StartLoading()
        //{

        //    viewModel.VisibleLoad = true;
        //    viewModel.LoadingMessage = "Loading...";

        //    bool isSuccess =false;//await GetNotifications();

        //    viewModel.VisibleLoad = false;
        //    if (isSuccess)
        //    {
        //        viewModel.LoadingMessage = "Success";
        //    }
        //    else
        //    {
        //        viewModel.VisibleLoad = false;
        //        Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
        //    }
        //}

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    viewModel.LoadItemsCommand.Execute(null);

        //}


        public async void OnUserNotificationDetailsClick(object sender, EventArgs e)
        {
            try
            {
                if (sender is CachedImage)
                {
                    var templateGrid = (CachedImage)sender;
                    if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                    {
                        var item = ((Notification)templateGrid.Parent.BindingContext);
                        await Navigation.PushAsync(new UserProfile(item.UserCreated_Id));
                    }
                }
                else if (sender is StackLayout)
                {
                    var templateGrid = (StackLayout)sender;
                    if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                    {
                        var item = ((Notification)templateGrid.Parent.BindingContext);
                        await Navigation.PushAsync(new UserProfile(item.UserCreated_Id));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public async void OnTypeOfNotificationClick(object sender, EventArgs e)
        {
            try
            {
                if (sender is StackLayout)
                {
                    var templateGrid = (StackLayout)sender;
                    if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                    {

                        var item = ((Notification)templateGrid.Parent.BindingContext);




                        if (item.ObjectTypeString.Equals("ProjectInsight.Items.Domains.DomainIntegrations.DomainIntegrationExternalObject")) //URL link or Projects link
                        {
                            Device.OpenUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + item.IconImageUrl));
                        }
                        else if (item.ObjectTypeString.Equals("ProjectInsight.Items.Projects.Project")) //URL link or Projects link
                        {
                            await Navigation.PushAsync(new ProjectDetailsPage(item.Object_Id));
                        }
                        //else if (item.ObjectTypeString.Equals("ProjectInsight.Items.Files.File")) //file link
                        //{

                        //    await Navigation.PushAsync(new RelatedItemDetailsPage(item, ItemType, viewModel));
                        //}
                        else if (item.ObjectTypeString.Equals("ProjectInsight.Items.Projects.Tasks.Task")) //Task link
                        {
                            await Navigation.PushAsync(new TaskDetailsPage(item.Object_Id));
                        }
                        else if (item.ObjectTypeString.Equals("ProjectInsight.Items.Projects.Issues.Issue") || item.ObjectTypeString.Equals("ProjectInsight.Items.Issues.Issue")) //Issue link
                        {
                            await Navigation.PushAsync(new IssueDetailsPage(item.Object_Id));
                        }
                        else if (item.ObjectTypeString.Equals("ProjectInsight.Items.Projects.ToDos.ToDo")) //ToDo link
                        {
                            await Navigation.PushAsync(new ToDoDetailsPage(item.Object_Id));
                        }
                        else if (item.ObjectTypeString.Equals("ProjectInsight.Items.ApprovalRequests.ApprovalRequest")) //ApprovalRequests link
                        {
                            await Navigation.PushAsync(new ApprovalRequestPage(item.Object_Id));
                        }
                        else if (item.ObjectTypeString.Equals("ProjectInsight.Reports.ReportOutput"))
                        {
                            Device.OpenUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + "/reports/scheduled/" + item.Object_Id));
                        }
                        else
                        {
                            Device.OpenUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + "/" + item.Object_Id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewItemPage());
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
                        layoutShowHidden.IsVisible = true;
                        templateGrid.ColumnDefinitions[1].Width = new GridLength(50, GridUnitType.Absolute);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private static void FindHrefs(string input)
        {
            Regex regex = new Regex("href\\s*=\\s*(?:\"(?<1>[^\"]*)\"|(?<1>\\S+))", RegexOptions.IgnoreCase);
            Match match;
            for (match = regex.Match(input); match.Success; match = match.NextMatch())
            {
                Console.WriteLine("Found a href. Groups: ");
                foreach (Group group in match.Groups)
                {
                    //bodyItemsNorification.Add(new BodyItemNorification { Value});
                    Console.WriteLine("Group value: {0}", group);
                }
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
                        layoutShowHidden.IsVisible = false;
                        templateGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        private async void UserImageClicked(object sender, WebNavigatingEventArgs e)
        {
            e.Cancel = true;
            try
            {
                if (sender is WebView)
                {
                    var templateGrid = (WebView)sender;
                    if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                    {

                        var item = ((Notification)templateGrid.Parent.BindingContext);
                        await Navigation.PushAsync(new UserProfile(item.UserCreated_Id));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

      
    }
}