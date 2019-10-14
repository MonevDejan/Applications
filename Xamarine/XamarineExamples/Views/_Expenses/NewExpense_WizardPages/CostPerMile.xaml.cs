using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CostPerMile : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public CostPerMile(ExpenseEntryViewModel ViewModel)
        {
            NavigationPage.SetBackButtonTitle(this, "");

            viewModel = ViewModel;
            InitializeComponent();
            BindingContext = viewModel;

            StartLoading();


            if (viewModel.SelectedExpenseType == 1)
                Title = "Add Receipt";
            else if (viewModel.SelectedExpenseType == 2)
                Title = "Add Mileage";
            else if (viewModel.SelectedExpenseType == 3)
                Title = "Add Other";
            else
                Title = "Add";

        }
        private async void Continue_Tapped(object sender, EventArgs e)
        {
            if (!viewModel.ShowContinueOnCostPerMilePage)
            {
                sfCostPerMile.HasError = true;
            }
            else
            {
                sfCostPerMile.HasError = false;
                await Navigation.PushAsync(new Date(viewModel));
            }

        }

        private async void StartLoading()
        {


            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetSettings();

            //isSuccess = await GetDammyData();

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
                //viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
        }
        private async Task<bool> GetSettings()
        {
            try
            {
                viewModel.SelectedExpenseCode = null;
                List <ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes =await ExpenseCodeService.client.MileageCodesAsync(new ProjectInsight.Models.Base.ModelProperties("default,ExpenseCodeType,RateBillableDefault"));

                if (expenseCodes != null && expenseCodes.Count > 0)
                {
                    viewModel.ExpenseCodes = new System.Collections.ObjectModel.ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(expenseCodes);
                    if (expenseCodes.Count == 1)
                    {
                        slMileageExpenseCode.IsVisible = false;
                        viewModel.SelectedExpenseCode = viewModel.ExpenseCodes[0];

                        viewModel.CostPerMile = viewModel.SelectedExpenseCode.RateBillableDefault ?? 0;

                        //TEST ONLY
                        //if (viewModel.SelectedExpenseCode.RateBillableDefault == null)
                        //    viewModel.CostPerMile = 5;
                    }
                    else
                    {
                        slMileageExpenseCode.IsVisible = true;
                    }
                }
                else
                {

                    List<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodesActive = await ExpenseCodeService.client.ListActiveAsync(new ProjectInsight.Models.Base.ModelProperties("default,ExpenseCodeType,RateBillableDefault"));

                    if (expenseCodesActive != null && expenseCodesActive.Count > 0)
                    {
                        viewModel.ExpenseCodes = new System.Collections.ObjectModel.ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(expenseCodesActive);
                        slMileageExpenseCode.IsVisible = true;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void CmbExpenseCodes_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
           // viewModel.CostPerMile = viewModel.SelectedExpenseCode.RateBillableDefault ?? 0;

            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null || cmb.SelectedValue.ToString() == "")
            {
                viewModel.CostPerMile = null;

                sfCostPerMile.HasError = true;
            }
            else
            {
                if (viewModel.SelectedExpenseCode != null && viewModel.SelectedExpenseCode.RateBillableDefault != null)
                {
                    viewModel.CostPerMile = viewModel.SelectedExpenseCode.RateBillableDefault;
                    sfCostPerMile.HasError = false;
                }
                else
                {
                    sfCostPerMile.HasError = true;
                    viewModel.CostPerMile = null;
                }
            }

        }

        private void NumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            sfCostPerMile.HasError = !viewModel.ShowContinueOnCostPerMilePage;
        }
    }
}