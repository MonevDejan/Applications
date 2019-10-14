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
using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.TimeAndExpense;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RejectApprovalRequestPage : ContentPage
    {
        BaseViewModel viewModel;
        Guid ApprovalRequestId;
        public RejectApprovalRequestPage(Guid approvalRequestId)
        {
            ApprovalRequestId = approvalRequestId;
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("RejectTimesheetPage Initialize");
            InitializeComponent();
            Title = "Reject Approval Request";
            viewModel = new BaseViewModel();
            BindingContext = viewModel;
        }



        private async void OnUpdateStatus_Tapped(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtCommentBody.Text))
            {
                sfComment.HasError = true;
                //Common.Instance.ShowToastMessage("Please Add Rejection Comment", DoubleResources.DangerSnackBar);
                return;
            }
          
            try
            {

                var resp = await DisplayActionSheet("Are you sure you want to reject?", "No", "Yes");
                if (resp != null && resp.ToString().Length > 0 && resp.Equals("Yes"))
                {
                    viewModel.VisibleLoad = true;
                    viewModel.LoadingMessage = "";
                   
                    ApiSaveResponse result = await ApprovalRequestApprovalService.client.DeniedApprovalRequestAsync(ApprovalRequestId, txtCommentBody.Text ?? txtCommentBody.Text);
                    if (result.HasErrors)
                    {
                        viewModel.VisibleLoad = false;
                        foreach (var error in result.Errors)
                            Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                    else
                    {
                        Common.Instance.ShowToastMessage("Approval Request Rejected!", DoubleResources.SuccessSnackBar);
                        viewModel.VisibleLoad = false;
                        MessagingCenter.Send(this, "ApprovalRequestRejected", true);
                        Common.RefreshWorkList = true;
                        await Navigation.PopToRootAsync();

                    }
                   
                }
            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                
            }
            viewModel.VisibleLoad = false;
        }

        private async void OnCancel_Tapped(object sender, EventArgs e)
        {
           //MessagingCenter.Send(this, "CurrentExpenseReportSubmited", false);
            await Navigation.PopAsync();
        }
    }
}