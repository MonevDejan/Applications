using HtmlAgilityPack;
using ProjectInsight.Models.Base;
using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensesBulkEntry : ContentPage
    {
        ExpenseBulkEntryViewModel viewModel;

        Guid ProjectId;
        public ExpensesBulkEntry(string title, Guid projectId)
        {
            ProjectId = projectId;
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();


            viewModel = new ExpenseBulkEntryViewModel();
            viewModel.CurrentEntry = new BulkEntry();
            BindingContext = viewModel;
            viewModel.IsBusy = false;
           

            LoadData();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void LoadData()
        {
            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetData();
            viewModel.IsBusy = true;
            viewModel.VisibleLoad = false;
        }
        List<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes;
        private async Task<bool> GetData()
        {
            try
            {

                expenseCodes = await ExpenseCodeService.client.ListActiveTimeEntryForProjectsAsync(new ModelProperties("default,RateBillableDefault"));
                ResetModel(DateTime.Now);
                //viewModel.CurrentEntry = new BulkEntry();
                //viewModel.CurrentEntry.EntryDate = DateTime.Now.Date;
                //viewModel.CurrentEntry.ExpenseCodes = new List<EntryCode>();
                //if (expenseCodes != null)
                //{
                //    foreach (var code in expenseCodes)
                //    {
                //        EntryCode ec = new EntryCode();
                //        ec.Id = code.Id.Value;
                //        ec.Name = code.Name;
                //        ec.Rate = code.RateBillableDefault;
                //        viewModel.CurrentEntry.ExpenseCodes.Add(ec);
                //    }
                   
                //}
             
            }
            catch (Exception ex)
            {
            }

            viewModel.IsBusy = false;
            return true;

        }

     
        private async void Save_Tapped(object sender, EventArgs e)
        {

            foreach (var item in viewModel.CurrentEntry.ExpenseCodes)
            {
                ProjectInsight.Models.TimeAndExpense.ExpenseEntry expenseEntry = new ProjectInsight.Models.TimeAndExpense.ExpenseEntry();

                if (item.Quantity != null && item.Rate != null)
                {
                    expenseEntry.Qty = Convert.ToDecimal(item.Quantity);
                    expenseEntry.ActualCost = expenseEntry.Qty * item.Rate;
                    expenseEntry.Date = viewModel.CurrentEntry.EntryDate;
                    expenseEntry.Description = item.Name;
                    expenseEntry.Project_Id = ProjectId;
                    expenseEntry.ExpenseCode_Id = item.Id;
                    expenseEntry.UserCreated_Id = Common.CurrentWorkspace.UserID;
                    expenseEntry.User_Id = Common.CurrentWorkspace.UserID;

                    ApiSaveResponse result = await ExpenseEntryService.client.SaveAsync(expenseEntry);



                    if (result.HasErrors)
                    {
                        string errorMsg = string.Empty;
                        foreach (var error in result.Errors)
                        {
                            errorMsg += error.ErrorMessage + Environment.NewLine;
                        }
                        Common.Instance.ShowToastMessage(errorMsg, DoubleResources.DangerSnackBar);

                    }
                    else
                    {

                        Common.Instance.ShowToastMessage("Saved", DoubleResources.SuccessSnackBar);
                        ResetModel(DateTime.Now);

                      
                    }

                }
            }

            await Navigation.PopToRootAsync();








        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate != null && e.NewDate.Year != 1900)
            {
                ResetModel(e.NewDate);
            }
        }

        private void ResetModel(DateTime date)
        {
            viewModel.CurrentEntry = new BulkEntry();
            viewModel.CurrentEntry.EntryDate = date.Date;
            viewModel.CurrentEntry.ExpenseCodes = new List<EntryCode>();
            if (expenseCodes != null)
            {
                foreach (var code in expenseCodes)
                {
                    EntryCode ec = new EntryCode();
                    ec.Id = code.Id.Value;
                    ec.Name = code.Name;
                    ec.Rate = code.RateBillableDefault;
                    viewModel.CurrentEntry.ExpenseCodes.Add(ec);
                  
                }
            }
        }

        private async void OnCancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}