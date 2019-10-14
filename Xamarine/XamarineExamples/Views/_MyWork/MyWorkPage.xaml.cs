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
using Plugin.Connectivity;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyWorkPage : ContentPage
    {
        MyWorkViewModel viewModel;
        public MyWorkPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("MyWorkPage Initialize");
            //Common.Instance.DeleteAll<MyWorkItem>();
            BindingContext = viewModel = new MyWorkViewModel() { Title = "Work" };

            InitListViews();
        }

     

        public MyWorkPage(Guid parentId, String objectType, String parentName)
        {

            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("MyWorkPage Initialize");
            Title = parentName;

            BindingContext = viewModel = new MyWorkViewModel(parentId, objectType);

            InitListViews();
        }

        private void InitListViews()
        {
            //ItemsListViewDroid.SeparatorColor = Color.Gray;
            ItemsListViewiOS.SeparatorColor = Color.Gray;

            //if (Device.RuntimePlatform.ToLower() == "android")
            //{
            //    ItemsListViewDroid.IsVisible = true;
            //    ItemsListViewiOS.IsVisible = false;
            //    ItemsListViewDroid.ItemsSource = viewModel.Items;
            //}
            //else
            {
               // ItemsListViewDroid.IsVisible = false;
                ItemsListViewiOS.IsVisible = true;
                ItemsListViewiOS.ItemsSource = viewModel.Items;
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //bool isrechable1 = await CrossConnectivity.Current.IsRemoteReachable(Common.CurrentWorkspace.WorkspaceURL + "/api");
            //bool isrechable = await  CrossConnectivity.Current.IsReachable(Common.CurrentWorkspace.WorkspaceURL + "/api"); 

            slTimesheetForApproval.Children.Clear();
            slTimesheetForApproval.Children.Add(new TimesheetApprovalBand());

            slExpenseReportForApproval.Children.Clear();
            slExpenseReportForApproval.Children.Add(new ExpenseReportApprovalBand());

            slNoConnection.Children.Clear();
            slNoConnection.Children.Add(new NoConnectionBand());

            viewModel.LoadLocalContent = true;
            viewModel.LoadItemsCommand.Execute(null);
        }

        public async void LoadData(object sender, EventArgs e)
        {
            IsBusy = true;
            viewModel.LoadLocalContent = false;
            viewModel.LoadItemsCommand.Execute(null);
            IsBusy = false;
        }
        private async void GridTemplate_Tapped(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            if (sender is SwipeGestureGrid || sender is Grid)
            {
                var templateGrid = (Grid)sender;
                if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                {

                    var item = ((MyWorkItem)templateGrid.Parent.BindingContext);

                    var itemID = item.Id;

                    if (item.ItemType == ItemType.Task)
                    {
                        await Navigation.PushAsync(new TaskDetailsPage(itemID));
                    }
                    else if (item.ItemType == ItemType.Issue)
                    {
                        await Navigation.PushAsync(new IssueDetailsPage(itemID));
                    }
                    else if (item.ItemType == ItemType.Todo)
                    {
                        await Navigation.PushAsync(new ToDoDetailsPage(itemID));
                    }
                    else if (item.ItemType == ItemType.ApprovalRequests)
                    {
                        await Navigation.PushAsync(new ApprovalRequestPage(itemID));
                    }
                    else if (item.ItemType == ItemType.TimeSheet)
                    {
                        await Navigation.PushAsync(new ApproveTimesheetPage(itemID));
                    }
                    else
                    {

                    }
                }
            }
        }
        async void AddItem_Clicked(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            MessagingCenter.Subscribe<NewProjectPage, Guid>(this, "ProjectAdded", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    LoadData(null, null);

                MessagingCenter.Unsubscribe<NewProjectPage, Guid>(this, "ProjectAdded");
            });
            MessagingCenter.Subscribe<NewToDoPage, Guid>(this, "TodoAdded", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    LoadData(null, null);

                MessagingCenter.Unsubscribe<NewToDoPage, Guid>(this, "TodoAdded");
            });
            MessagingCenter.Subscribe<NewTaskPage, Guid>(this, "TaskAdded", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    LoadData(null, null);

                MessagingCenter.Unsubscribe<NewTaskPage, Guid>(this, "TaskAdded");
            });

            MessagingCenter.Subscribe<NewIssuePage, Guid>(this, "IssueAdded", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    LoadData(null, null);

                MessagingCenter.Unsubscribe<NewIssuePage, Guid>(this, "IssueAdded");
            });

            await Navigation.PushAsync(new AddItemPage());
        }
        #region NotUsed

        public void OnDelete(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
        private void GridTemplate_SwipeLeft(object sender, EventArgs e)
        {
            //TODO Swipe left is disabled temporary because not implemented
            return;
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

        private void GridTemplate_SwipeRight(object sender, EventArgs e)
        {
            //TODO Swipe left is disabled temporary because not implemented
            return;
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


        private void OnHide(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected) return;
                if (sender is MenuItem)
                {
                    var templateGrid = (MenuItem)sender;
                    if (templateGrid != null) //&& templateGrid is Grid)
                    {
                        layoutShowHidden.IsVisible = false;
                        //templateGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion NotUsed
    }
}