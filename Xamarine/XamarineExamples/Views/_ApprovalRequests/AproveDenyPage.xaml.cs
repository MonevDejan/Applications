using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.ApprovalRequests
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AproveDenyPage : ContentPage
    {
        ApprovalRequestViewModel viewModel;                   
        public ProjectInsight.Models.ApprovalRequests.ApprovalRequest approval;
        public AproveDenyPage (ApprovalRequestViewModel model, ProjectInsight.Models.ApprovalRequests.ApprovalRequest approval)
		{
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("AproveDenyPage Initialize");

            InitializeComponent();

            this.approval = approval;
            BindingContext = viewModel = model;
        }

        
        private async void OnApprovedApprovalRequest(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {                                      
                var response = await ApprovalRequestService.Update(approval);

                //if (!response.HasErrors)
                //{

                //}
                //else
                //    Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);


            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            viewModel.LoadingMessage = "Success";
            viewModel.VisibleLoad = false;
        }


        private async void OnApprovedChangesApprovalRequest(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {
                //var response = await ApprovalRequestService.Update(approval);

                //if (!response.HasErrors)
                //{

                //}
                //else
                //    Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);


            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            viewModel.LoadingMessage = "Success";
            viewModel.VisibleLoad = false;
        }

        private async void OnSkipApprovalRequest(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {
                //var response = await ApprovalRequestService.Update(approval);

                //if (!response.HasErrors)
                //{

                //}
                //else
                //    Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);


            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            viewModel.LoadingMessage = "Success";
            viewModel.VisibleLoad = false;
        }

        private async void OnDeniedApprovalRequest(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {
                //var response = await ApprovalRequestService.Update(approval);

                //if (!response.HasErrors)
                //{

                //}
                //else
                //    Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);


            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            viewModel.LoadingMessage = "Success";
            viewModel.VisibleLoad = false;
        }

    }
}