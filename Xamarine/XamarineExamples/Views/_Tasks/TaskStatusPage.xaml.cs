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

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskStatusPage : ContentPage
    {
        TaskStatusViewModel viewModel;
     
        public TaskStatusPage(Guid? taskId, WorkPercentCompleteType workPercentCompleteType)
        {
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("TaskStatusPage Initialize");
            InitializeComponent();
            Title = "Update Status";

            viewModel = new TaskStatusViewModel();
            viewModel.SelectedTaskId = taskId;
           


            BindingContext = viewModel;
          
            GetData(workPercentCompleteType.Id.Value);

            if (Device.RuntimePlatform.ToLower() == "android")
            {
                cmbStatuses.Margin = new Thickness(10, 0, 10, 0);
            }


        }
        private async void GetData(Guid statusId)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetStatusTypes();
            var selStatus = viewModel.WorkPercentCompleteTypes.Where(x => x.Id == statusId).FirstOrDefault();
            if (selStatus != null)
                viewModel.SelectedWorkPercentCompleteType = selStatus;

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
                viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Your session has expired!", DoubleResources.DangerSnackBar);
            }
        }

        private async Task<bool> GetStatusTypes()
        {
            try
            {
              
                viewModel.WorkPercentCompleteTypes = await WorkPercentCompleteTypeService.GetAllTypes();
                if (viewModel != null)
                {
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


        private async void OnUpdateStatus(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {
                var response = TasksService.client.UpdateWorkPercentComplete(viewModel.SelectedTaskId.Value, viewModel.SelectedWorkPercentCompleteType.WorkPercentComplete.Value);

                if (!response.HasErrors)
                {
                    if (txtCommentBody.Text != null && txtCommentBody.Text.Trim() != string.Empty)
                    {
                        Comment comment = new Comment();
                        comment.CommentBody = txtCommentBody.Text;// "Comment Added from Mobile Application";
                        comment.CreatedDateTimeUTC = DateTime.UtcNow;
                        comment.UserCreated_Id = Common.CurrentWorkspace.UserID;
                        comment.ObjectId = viewModel.SelectedTaskId;

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
                    Common.RefreshWorkList = true;
                    Common.Instance.ShowToastMessage("Task Status updated!", DoubleResources.SuccessSnackBar);
                    MessagingCenter.Send(this, "TaskStatusUpdated", viewModel.SelectedTaskId.Value);
                    await Navigation.PopAsync();
                }
                else
                    Common.Instance.ShowToastMessage(response.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
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