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
    public partial class SubmitExpenseReportPage : ContentPage
    {
        SubmitExpenseReportViewModel viewModel;
        public SubmitExpenseReportPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            
            //HockeyApp.MetricsManager.TrackEvent("SubmitTimeSheetPage Initialize");

            viewModel = new SubmitExpenseReportViewModel();
            BindingContext = viewModel;
            LoadData();
        }

        private async void LoadData()
        {
            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            //Common.UserGlobalCapability.CanSelectExpenseReportApprover = true;

            var report = await ExpenseReportService.client.GetExpenseReportInfoAsync(Common.CurrentWorkspace.UserID);

            if (report != null)
            {
                viewModel.ActualCost = report.UnsubmittedExpenseTotal != null ? report.UnsubmittedExpenseTotal.Value : 0;

                if (report.EarliestExpenseEntryDate != null)
                {
                    viewModel.PeriodFormated = string.Format("{0} - {1}", report.EarliestExpenseEntryDate.Value.ToString("M/d/yy"), "Present");
                    viewModel.EarliestExpenseEntryDate = report.EarliestExpenseEntryDate.Value;
                }
            }
            viewModel.CanSelectExpenseReportApprover = Common.UserGlobalCapability.CanSelectExpenseReportApprover;
            
            if (Common.UserGlobalCapability.CanSelectExpenseReportApprover)
            {
                var approvers = await ExpenseReportService.client.GetMyExpenseReportApproversAsync();
                viewModel.ExpenseReportApprovers = approvers;
                if (Common.UserGlobalCapability.UserId_ExpenseReportApproverDefault != null)
                    viewModel.SelectedUser = viewModel.ExpenseReportApprovers.FirstOrDefault(x => x.Id == Common.UserGlobalCapability.UserId_ExpenseReportApproverDefault);
            }
            viewModel.IsBusy = true;
            viewModel.VisibleLoad = false;
        }
        //private async Task<bool> GetData()
        //{
        //}
            private async void OnSubmit(object sender, EventArgs e)
        {
            try
            {
                viewModel.VisibleLoad = true;
                ApiSaveResponse result = await ExpenseReportService.client.SubmitExpenseReportAsync(viewModel.EarliestExpenseEntryDate, DateTime.Now, viewModel.SelectedUser != null ? viewModel.SelectedUser.Id : null);
             
                if (result.HasErrors)
                {
                    foreach (var error in result.Errors)
                    {
                        Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                }
                else
                {
                    MessagingCenter.Send(this, "CurrentExpenseReportSubmited", true);
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
            MessagingCenter.Send(this, "CurrentExpenseReportSubmited", false);
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