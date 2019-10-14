using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using ProjectInsight.Models.Comments;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.Views.General;
using System.Net;
using System.IO;
using Plugin.Connectivity;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectDetailsPage : ContentPage
    {
        ProjectDetailsViewModel viewModel;
        public ProjectInsight.Models.Projects.Project project;
        Guid pId;
        public ProjectDetailsPage(Guid projectId)
        {
            pId = projectId;
            NavigationPage.SetBackButtonTitle(this, "");
            //HockeyApp.MetricsManager.TrackEvent("ProjectDetailsPage Initialize");

            viewModel = new ProjectDetailsViewModel();

            viewModel.VisibleLoad = true;

            InitializeComponent();
            BindingContext = viewModel;

        }
        private async void GetObicen(Guid projectId)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetProject(projectId);

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
               // viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Your session has expired!", DoubleResources.DangerSnackBar);
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            GetObicen(pId);
        }

        private async Task<bool> GetProject(Guid projectId)
        {

            try
            {
                project = await ProjectsService.GetProject(projectId);

                if (project != null)
                {
                    var userWork = Common.Instance.GetUserWork().Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.ProjectId == project.Id);

                    if (userWork != null && userWork.Count() > 0)
                    {
                        lblProjectItems.Text = string.Format("Work ({0})", userWork.Count());
                        slProjectItems.IsVisible = true;

                    }

                    //check permissions to view all tasks
                    //bool userHasEditTaskPermissions = project.UserHasEditTaskPermissions(Common.CurrentWorkspace.UserID);

                    bool userHasEditTaskPermissions = false;
                    if (project.ProjectResources != null)
                    {
                        foreach (ProjectInsight.Models.Projects.ProjectResource r in project.ProjectResources)
                        {
                            if (
                                (
                                    (r.IsProjectScheduler.HasValue && r.IsProjectScheduler.Value == true)
                                    ||
                                    (r.IsProjectManager.HasValue && r.IsProjectManager.Value == true)
                                )
                                    && (r.User_Id.HasValue && r.User_Id.Value == Common.CurrentWorkspace.UserID)

                            )
                            {
                                userHasEditTaskPermissions = true;
                            }
                        }

                    }


                    var item = new ProjectDetailsViewModel
                    {


                        Id = project.Id.ToString(),
                        Title = project.ItemNumberFullAndNameDisplayPreference,
                        WorkPercentComplete = project.WorkPercentComplete.Value,
                        Status = project.ProjectStatus != null ? project.ProjectStatus.Name : "",
                        TasksCount = project.TaskCount ?? 0,
                        
                        UserHasEditTaskPermissions = userHasEditTaskPermissions,
                        RefreshControls = true
                    };


                    //TODO IssuesCount property needed on Project model

                    try
                    {
                        var itemCounts = await ProjectsService.client.GetProjectItemCountsAsync(project.Id.Value);
                        if (itemCounts != null)
                        {
                            item.IssuesCount = itemCounts.IssueCount.Value;
                            item.TasksCount = itemCounts.TaskCount.Value;
                        }

                        //List<ProjectInsight.Models.Issues.Issue> issues = await IssuesService.client.GetByProjectAsync(project.Id.Value);
                        //item.IssuesCount = issues == null ? 0 : issues.Count();
                    }
                    catch (Exception ex)
                    {

                    }
                   

                    item.Description = System.Text.RegularExpressions.Regex.Replace(project.Description, "<.*?>", String.Empty);
                    if (project.ProjectStatus != null)
                    {
                        if (string.IsNullOrEmpty(project.ProjectStatus.ChartColor))
                        {
                            item.StatusColor = ProjectInsightMobile.Helpers.ExtensionMethods.GetHexString((Color)Application.Current.Resources["LightBackgroundColor"]);
                            item.StatusFontColor = ProjectInsightMobile.Helpers.ExtensionMethods.GetHexString((Color)Application.Current.Resources["DarkGrayTextColor"]);
                        }
                        else
                        {
                            item.StatusColor = project.ProjectStatus.ChartColor;
                            item.StatusFontColor = ProjectInsightMobile.Helpers.ExtensionMethods.GetHexString((Color)Application.Current.Resources["WhiteTextColor"]);
                        }
                    }

                    string percentComplete = "0";
                    if (project.WorkPercentComplete != null)
                    {
                        percentComplete = project.WorkPercentComplete.Value.ToString();
                    }

                    item.Duration = project.DurationString;
                    item.Duration = String.Format("{0} ({1}% Complete)", project.DurationString, percentComplete);

                    if (project.StartDateTimeUserLocal != null)
                    {
                        item.StartDate = project.StartDateTimeUserLocal.Value;

                    }
                    if (project.EndDateTimeUserLocal != null)
                    {
                        item.EndDate = project.EndDateTimeUserLocal.Value;
                    }

                    item.Description = item.Description.Replace("<img ", "<img style='max-width: 100%;' ");

                    viewModel = item;
                    BindingContext = viewModel;
                }
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }


        public async void OnTappedDocuments(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DocumentsPage(pId, true));
        }

        private async void OnProjectItems_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyWorkPage(new Guid(viewModel.Id), "All", viewModel.Title));
        }

        private async void OnExpenses_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseExpense(projectId: new Guid(viewModel.Id)));
         
        }

        private async void OnTimeEntry_Tapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new CreateTimeEntryPage());
            TimeEntryWizardViewModel ViewModel = new TimeEntryWizardViewModel();
            ViewModel.ItemType = ItemType.Projects;
            ViewModel.SelectedProject = new ProjectInsight.Models.Projects.Project() { Id = new Guid(viewModel.Id) };
            await Navigation.PushAsync(new TimeEntries.ExpenseCode(ViewModel));
        }



        async void AddItem_Clicked(object sender, EventArgs e)
        {

            //if (!CrossConnectivity.Current.IsConnected) return;

        
            //MessagingCenter.Subscribe<NewToDoPage, Guid>(this, "TodoAdded", async (obj, itemId) =>
            //{
            //    //if (itemId != Guid.Empty)
            //    //    LoadData(null, null);

            //    MessagingCenter.Unsubscribe<NewToDoPage, Guid>(this, "TodoAdded");
            //});
            //MessagingCenter.Subscribe<NewTaskPage, Guid>(this, "TaskAdded", async (obj, itemId) =>
            //{
            //    //if (itemId != Guid.Empty)
            //    //    LoadData(null, null);

            //    MessagingCenter.Unsubscribe<NewTaskPage, Guid>(this, "TaskAdded");
            //});

            //MessagingCenter.Subscribe<NewIssuePage, Guid>(this, "IssueAdded", async (obj, itemId) =>
            //{
            //    //if (itemId != Guid.Empty)
            //    //    LoadData(null, null);

            //    MessagingCenter.Unsubscribe<NewIssuePage, Guid>(this, "IssueAdded");
            //});

            await Navigation.PushAsync(new AddItemPage(parentId: pId));
        }

    }
}