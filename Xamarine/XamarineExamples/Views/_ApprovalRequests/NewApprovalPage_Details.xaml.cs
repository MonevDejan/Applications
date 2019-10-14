using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Services;
using System.Collections.ObjectModel;
using static ProjectInsightMobile.ViewModels.NewProjectViewModel;
using System.Linq;
using ProjectInsight.Models.Base.Responses;
using System.Collections.Generic;
using static ProjectInsightMobile.ViewModels.NewApprovalViewModel;
using ProjectInsight.Models.Users;
using HtmlAgilityPack;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_Details : ContentPage
    {
      
        NewApprovalViewModel viewModel;
        public NewApprovalPage_Details(Guid? approvalId = null, Guid? container_Id =null)
		{
            NavigationPage.SetBackButtonTitle(this,"");
            //HockeyApp.MetricsManager.TrackEvent("NewTaskPage Initialize");
            //approvalId = new Guid("33471854b2334b53a5519a1d62bcb24e");

            InitializeComponent();

            viewModel = new NewApprovalViewModel();
            if (container_Id.HasValue)
                viewModel.Container_Id = container_Id;
            StartLoading(selectedTaskId:approvalId);

            BindingContext = viewModel;

            if (approvalId != null && approvalId != Guid.Empty)
            {
                Title = "Edit Approval";
            }
        }
        private async void StartLoading(Guid? selectedTaskId = null)
        {

            viewModel.VisibleLoad = true;

            bool isSuccess = false;
         
            if (selectedTaskId != null && selectedTaskId != Guid.Empty)
                isSuccess = await GetApproval(selectedTaskId.Value);

            BindingContext = viewModel;

            viewModel.VisibleLoad = false;
          
        }


        private async Task<bool> GetApproval(Guid approvalId)
        {
            try
            {
                ProjectInsight.Models.ApprovalRequests.ApprovalRequest approval = await ApprovalRequestService.GetApprovalRequest(approvalId);

                viewModel.Name = approval.Name;
                viewModel.Description = approval.Description;
                viewModel.DeadlineDate= approval.DeadlineDateTimeUTC.Value;

                viewModel.SelectedParentItem = viewModel.ParentItems.Where(x => x.Id == approval.ItemContainer_Id).FirstOrDefault();
                //viewModel.SelectedApprovers= viewModel.Approvers.Where(x => x.Id == task.TaskOwner_Id).FirstOrDefault();
                viewModel.Id = approvalId;

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }
        private async void OnContinue_Tapped(object sender, EventArgs e)
        {

            //selectiraniot Task e 
            //var selTask = viewModel.SelectedTask;
            bool validationError = false;
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                viewModel.NameError = true;
                validationError = true;
            }
         
            if (validationError) return;

            await Navigation.PushAsync(new NewApprovalPage_Approvers(viewModel));
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.NewTextValue))
                viewModel.NameError = false;

        }
    }
}