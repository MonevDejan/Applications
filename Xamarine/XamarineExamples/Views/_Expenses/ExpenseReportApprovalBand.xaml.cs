using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsight.Models.Tasks;
using ProjectInsightMobile.Services;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using Syncfusion.SfChart.XForms;
using ProjectInsight.Models.TimeAndExpense;
using Plugin.Connectivity;
//using Syncfusion.SfChart.XForms;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseReportApprovalBand : ContentView
    {




        public ExpenseReportApprovalBand()
        {
            InitializeComponent();

            if (CrossConnectivity.Current.IsConnected && Common.UserGlobalCapability != null && Common.UserGlobalCapability.IsExpenseReportApprover)
            {
                List<ExpenseReport> resForApproval = ExpenseReportService.client.Search(userApproverGuidList: Common.CurrentWorkspace.UserID.ToString(),
                    excludeApproved: true, excludePendingApproval: false, excludeRejected: true,
                    modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,User")); ;


                if (resForApproval != null && resForApproval.Count > 0)
                {
                    var count = resForApproval.Count();
                    el1.Padding = new Thickness(0, 5);
                    lblTimesheetToApprove.Text = String.Format("You have {0} expense report{1} to approve", count.ToString(), (count > 1 ? "s" : ""));
                    el1.IsVisible = true;
                    el2.IsVisible = true;
                    el3.IsVisible = true;
                    lblTimesheetToApprove.IsVisible = true;
                }
            }
        }




            private async void GoToApproval_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExpensesPage());
        }
    }
}