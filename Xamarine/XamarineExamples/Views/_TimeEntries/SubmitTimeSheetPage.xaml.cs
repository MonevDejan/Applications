using ProjectInsight.Models.Base;
using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubmitTimeSheetPage : ContentPage
    {
        SubmitTimeSheetViewModel viewModel;
        public SubmitTimeSheetPage(TimeSheetPeriod timeSheet)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            
            //HockeyApp.MetricsManager.TrackEvent("SubmitTimeSheetPage Initialize");

            viewModel = new SubmitTimeSheetViewModel();
            LoadData(timeSheet);
           
            BindingContext = viewModel;
            viewModel.VisibleLoad = false;
        }

        private async void LoadData(TimeSheetPeriod timeSheet)
        {
            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            viewModel.VisibleLoad = true;
            viewModel.TimeSheet = timeSheet;

            viewModel.CanSelectTimeSheetApprover = Common.UserGlobalCapability.CanSelectTimeSheetApprover;

            if (Common.UserGlobalCapability.CanSelectTimeSheetApprover)
            {
                viewModel.TimeSheetApprovers = await UsersService.GetMyTimeSheetApprovers();
                if (Common.UserGlobalCapability.UserId_TimeSheetApproverDefault != null)
                    viewModel.SelectedUser = viewModel.TimeSheetApprovers.FirstOrDefault(x => x.Id == Common.UserGlobalCapability.UserId_TimeSheetApproverDefault);
            }
            viewModel.IsBusy = true;
            viewModel.VisibleLoad = false;
        }
        //private async Task<bool> GetData()
        //{
        //}
            private async void OnSubmit(object sender, EventArgs e)
        {
            viewModel.VisibleLoad = true;
            try
            {
                ApiSaveResponse result = await TimeSheetService.CreateTimeSheetAndSubmitForPeriod(viewModel.TimeSheet.StartDate, viewModel.TimeSheet.EndDate, viewModel.SelectedUser != null ? viewModel.SelectedUser.Id : null);

                if (result.HasErrors)
                {
                    foreach (var error in result.Errors)
                    {
                        Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                }
                else
                {
                    Common.Instance.ShowToastMessage("Timesheet submited!", DoubleResources.SuccessSnackBar);

                    MessagingCenter.Send(this, "TimeSheetSubmited", true);
                    await Navigation.PopToRootAsync();
                }
            }
            catch (Exception ex)
            {
            }

            viewModel.VisibleLoad = false;

        }

        private async void OnCancel(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "TimeSheetSubmited", false);
            await Navigation.PopAsync();
        }

        private void CmbUsers_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
            {
                viewModel.SelectedUser = null;
            }
        }
    }
}