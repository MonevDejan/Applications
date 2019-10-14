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

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectTasksPage : ContentPage
    {
        TasksViewModel viewModel;
        public ObservableCollection<TaskListItem> listWork { get; set; }
        public ProjectTasksPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            lstCurrentItems.ItemsSource = viewModel.CurrentItems;

           //HockeyApp.MetricsManager.TrackEvent("ProjectTasksPage Initialize");

        }
        Guid? pId = null;
        public ProjectTasksPage(Guid parentId, String parentName)
        {
            pId = parentId;
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("ProjectTasksPage Initialize");

            Title = parentName;

            BindingContext = viewModel = new TasksViewModel(parentId);
            lstCurrentItems.ItemsSource = viewModel.CurrentItems;
        }

        private async void GridTemplate_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid || sender is Grid)
                {
                    var templateGrid = (Grid)sender;
                    if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                    {
                        var item = ((TaskListItem)templateGrid.Parent.BindingContext);
                        await Navigation.PushAsync(new TaskDetailsPage(item.Id));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage(parentId:pId));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();


            viewModel.LoadItemsCommand.Execute(null);

        }

        public async void LoadData(object sender, EventArgs e)
        {
            IsBusy = true;

            viewModel.LoadItemsCommand.Execute(null);

            IsBusy = false;
        }


        public async void OnTappedTasks(object sender, EventArgs e)
        {
            try
            {

                if (Device.RuntimePlatform.ToLower() != "android") return;
                
                    if (sender is StackLayout)
                {
                  //  lstCurrentItems.BeginRefresh();

                    var menuItem = ((StackLayout)sender);
                    if (menuItem != null && menuItem.BindingContext != null)
                    {
                        TaskListItem item = ((TaskListItem)menuItem.BindingContext);

                        bool wasExpanded = item.IsExpanded;
                        foreach (var task in viewModel.CurrentItems)
                        {
                            if (task.TaskScheduleCurrentState == item.TaskScheduleCurrentState)
                            {
                                if (task.IsHeader)
                                {
                                    task.IsExpanded= !wasExpanded;
                                    if (task.IsExpanded)
                                        task.Icon = "Arrow_down.png";
                                    else
                                        task.Icon = "Arrow_right.png";
                                }
                                else
                                {
                                    task.IsItemVisible = !wasExpanded;
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}