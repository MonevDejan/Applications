using ProjectInsight.Models.Comments;
using ProjectInsight.Models.Projects;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
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
    public partial class IssueDetailsPage : ContentPage
    {
        IssueDetailsViewModel viewModel;

        public IssueDetailsPage(Guid issueId)
        {
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("IssueDetailsPage Initialize");
            InitializeComponent();
            viewModel = new IssueDetailsViewModel();
            viewModel.VisibleLoad = true;
            BindingContext = viewModel;
            GetObicen(issueId);

        }
        private async void GetObicen(Guid issueId)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetIssues(issueId);

          
            if (isSuccess)
            {
                //viewModel.LoadingMessage = "Success";
            }
            else
            {
                //viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
        }

        private async Task<bool> GetIssues(Guid issueId)
        {

            try
            {
                var issue = await IssuesService.client.GetAsync(issueId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,IssueType,UserAssignedTo_Id,ProjectAffiliation_Id,IssueStatusType,WorkPercentCompleteType,DescriptionText,Description,UserAssignedTo,Severity,UserAssignedBy,FoundBy,FoundDateTimeUTC,IssuePriority,IssueSeverity,ProjectResolution,ProjectAffiliation;Project:ItemNumberFullAndNameDisplayPreference"));



                if (issue != null)
                {

                    var item = new IssueDetailsViewModel
                    {
                        Id = issue.Id.ToString(),
                        Name = issue.Name,
                        ProjectName = issue.ProjectAffiliation != null ? issue.ProjectAffiliation.ItemNumberFullAndNameDisplayPreference : "",
                        FoundBy = issue.FoundBy,
                        StartDate = issue.StartDateTimeUTC.Value,
                        EndDate = issue.EndDateTimeUTC,
                        StatusType = issue.IssueStatusType,
                        UserAssignedTo_Id = issue.UserAssignedTo_Id,
                        Project_Id = issue.ProjectAffiliation_Id

                        //FoundDate = issue.FoundDateTimeUTC != null ? issue.FoundDateTimeUTC.Value :                                                         
                    };

                   // viewModel.WorkStatus = item.WorkPercentCompleteType != null ? item.WorkPercentCompleteType.Name : "";
                   // viewModel.WorkPercentCompleteType = item.WorkPercentCompleteType;

                    if (issue.UserAssignedTo != null)
                    {
                        item.UserAssignedTo = "Assigned To: " + issue.UserAssignedTo.FirstName + " " + issue.UserAssignedTo.LastName;
                    }

                    if (issue.UserAssignedBy != null)
                    {
                        item.UserAssignedBy = "Assigned By: " + issue.UserAssignedBy.FirstName + " " + issue.UserAssignedBy.LastName;
                    }

                    if (issue.IssuePriority != null)
                    {
                        item.Priority = "Priority By: " + issue.IssuePriority.Name;
                    }

                    if (issue.IssueSeverity != null)
                    {
                        item.Severity = "Severity: " + issue.IssueSeverity.Name;
                    }

                    if (issue.DescriptionText != null)
                    {
                        item.Description =  issue.DescriptionText;
                    }



                    viewModel = item;
                    viewModel.VisibleLoad = false;
                    BindingContext = viewModel;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async void OnUpdateIssue(object sender, EventArgs e)
        {
                                                                                                            
        }

        private async void OnAddTimeEntry(object sender, EventArgs e)
        {
            TimeEntryWizardViewModel ViewModel = new TimeEntryWizardViewModel();
            ViewModel.ItemType = ItemType.Issue;
            ViewModel.SelectedTask = new TaskExt(new ProjectInsight.Models.Tasks.Task() { Id = new Guid(viewModel.Id) });
            await Navigation.PushAsync(new TimeEntries.ExpenseCode(ViewModel));

        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {

            MessagingCenter.Subscribe<NewIssuePage, Guid>(this, "IssueUpdated", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    GetObicen(itemId);

                MessagingCenter.Unsubscribe<NewIssuePage, Guid>(this, "IssueUpdated");
            });


            await Navigation.PushAsync(new NewIssuePage(new Guid(viewModel.Id)));
        }



        private async void OnUpdateStatus(object sender, EventArgs e)
        {


            MessagingCenter.Subscribe<IssueStatusPage, Guid>(this, "IssueStatusUpdated", async (obj, taskId) =>
            {
                GetObicen(taskId);

                MessagingCenter.Unsubscribe<IssueStatusPage, Guid>(this, "IssueStatusUpdated");
            });

            await Navigation.PushAsync(new IssueStatusPage(new Guid(viewModel.Id), viewModel.StatusType, viewModel.UserAssignedTo_Id, viewModel.Project_Id));
        }
    }
}