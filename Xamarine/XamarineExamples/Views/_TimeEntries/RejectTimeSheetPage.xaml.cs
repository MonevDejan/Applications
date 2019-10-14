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
    public partial class RejectTimeSheetPage : ContentPage
    {
        BaseViewModel viewModel;
        TimeSheet Timesheet;
        public RejectTimeSheetPage(TimeSheet timesheet)
        {
            Timesheet = timesheet;
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("RejectTimesheetPage Initialize");
            InitializeComponent();
            Title = "Reject Timesheet";
            viewModel = new BaseViewModel();
            BindingContext = viewModel;
        }



        private async void OnUpdateStatus(object sender,  EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
            try
            {
                ApiSaveResponse result = await TimeSheetService.RejectAsync(Timesheet, txtCommentBody.Text ?? string.Empty);
                if (result.HasErrors)
                {
                    viewModel.VisibleLoad = false;
                    foreach (var error in result.Errors)
                        Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);

                    
                }
                else
                {
                    Common.Instance.ShowToastMessage("Timesheet rejected!", DoubleResources.SuccessSnackBar);
                    viewModel.VisibleLoad = false;
                    MessagingCenter.Send(this, "TimeSheetRejected", true);
                    await Navigation.PopToRootAsync();
                }

            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
                viewModel.VisibleLoad = false;
            }
        }

        private async void OnCancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}