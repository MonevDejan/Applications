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
using ProjectInsight.WebApi.Client.ReferenceData;
using ProjectInsight.Models.ReferenceData;
using ProjectInsight.Models.Issues;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IssueStatusPage : ContentPage
    {
        IssueStatusViewModel viewModel;
        Guid? SelectedUserId = null;
        Guid? SelectedProjectId = null;
        public IssueStatusPage(Guid? issueId, IssueStatusType selectedStatusType, Guid? selectedUserId, Guid? selectedProjectId)
        {
            SelectedUserId = selectedUserId;
            SelectedProjectId = selectedProjectId;
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("TaskStatusPage Initialize");
            InitializeComponent();
            Title = "Update Status";

            viewModel = new IssueStatusViewModel();
            viewModel.SelectedIssueId = issueId;
           


            BindingContext = viewModel;
          
            GetData(selectedStatusType);

            if (Device.RuntimePlatform.ToLower() == "android")
            {
                cmbStatuses.Margin = new Thickness(10, 0, 10, 0);
            }


        }
        private async void GetData(IssueStatusType selectedStatusType)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";


            viewModel.IssueStatusTypes = await IssueStatusTypeService.client.ListActiveAsync(new ProjectInsight.Models.Base.ModelProperties("default"));
            if (selectedStatusType != null)
            {
                var selStatus = viewModel.IssueStatusTypes.Where(x => x.Id == selectedStatusType.Id.Value).FirstOrDefault();
                if (selStatus != null)
                    viewModel.SelectedStatusType = selStatus;
            }

            await GetAllUsers();
            viewModel.VisibleLoad = false;
          
        }


        private async Task<bool> GetAllUsers()
        {
            try
            {
                if (SelectedUserId != null)
                {
                    viewModel.Users = null;
                    viewModel.SelectedUser = null;
                }

                List<ProjectInsight.Models.Users.User> listofUsers = await UsersService.ListByProjectForTaskAssignment(SelectedProjectId ?? Guid.Empty);
                ObservableCollection<ProjectInsight.Models.Users.User> users = new ObservableCollection<ProjectInsight.Models.Users.User>(listofUsers);
                // viewModel.Users = createInitElem(users, new ProjectInsight.Models.Users.User());
                viewModel.Users = users;

                if (SelectedUserId != null)
                    viewModel.SelectedUser = viewModel.Users.Where(x => x.Id == SelectedUserId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }
        private void CmbUser_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
            {
                viewModel.SelectedUser = null;
            }

        }
        private async void OnUpdateStatus(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
         
            try
            {
                Issue issue = new Issue();
                issue.Id = viewModel.SelectedIssueId;
                issue.IssueStatusType_Id = viewModel.SelectedStatusType?.Id;

                var response = await IssuesService.client.UpdateIssueStatusAsync(issue);
                if (response != null && response.HasErrors)
                    Common.Instance.ShowToastMessage(response.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }

            try
            {
                if (SelectedUserId != viewModel.SelectedUser.Id)
                {
                    Issue issue = new Issue();
                    issue.Id = viewModel.SelectedIssueId;
                    issue.UserAssignedTo_Id = viewModel.SelectedUser.Id;
                    var response = await IssuesService.client.UpdateUserAssignedToAsync(viewModel.SelectedIssueId.Value, viewModel.SelectedUser.Id);
                    if (response != null && response.HasErrors)
                        Common.Instance.ShowToastMessage(response.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                }
            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }


            try
            {

                if (txtCommentBody.Text != null && txtCommentBody.Text.Trim() != string.Empty)
                {
                    Comment comment = new Comment();
                    comment.CommentBody = txtCommentBody.Text;// "Comment Added from Mobile Application";
                    comment.CreatedDateTimeUTC = DateTime.UtcNow;
                    comment.UserCreated_Id = Common.CurrentWorkspace.UserID;
                    comment.ObjectId = viewModel.SelectedIssueId;

                    try
                    {
                        var respComment = await CommentsService.AddComment(comment);
                        if (respComment.HasErrors)
                            Common.Instance.ShowToastMessage(respComment.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                    catch (Exception ex)
                    {
                        Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            Common.RefreshWorkList = true;
            Common.Instance.ShowToastMessage("Issue Status updated!", DoubleResources.SuccessSnackBar);
            MessagingCenter.Send(this, "IssueStatusUpdated", viewModel.SelectedIssueId.Value);
            await Navigation.PopAsync();

            viewModel.LoadingMessage = "";
            viewModel.VisibleLoad = false;

            //await Navigation.PushAsync(new TaskDetailsPage(viewModel.SelectedTaskId.Value));
        }

        private async void OnCancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}