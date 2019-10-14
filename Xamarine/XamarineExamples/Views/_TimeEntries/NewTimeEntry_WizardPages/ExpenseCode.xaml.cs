using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views.TimeEntries
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpenseCode : ContentPage
    {

        TimeEntryWizardViewModel viewModel;
        public ExpenseCode(TimeEntryWizardViewModel ViewModel)
        {
            NavigationPage.SetBackButtonTitle(this, "");


            InitializeComponent();

            viewModel = ViewModel;



            //viewModel.ExpenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>();
            //foreach (var item in expenseCodes)
            //    viewModel.ExpenseCodes.Add(item);




            List<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = ExpenseCodeService.client.ListActive(modelProperties: new ProjectInsight.Models.Base.ModelProperties("default")); ;
            viewModel.ExpenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(expenseCodes);

            var inputSettings = TimeEntryService.client.GetTimeEntryInputSettingsAndLists(
                      selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null,
                      selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null
                     );
            if (inputSettings != null)
            {
                inputSettings.UseHoursMinutesInput = false;
                if (inputSettings.SelectedTimeEntryExpenseCodeId.HasValue)
                    viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedTimeEntryExpenseCodeId).FirstOrDefault();

                viewModel.UseHoursMinutesInput = inputSettings.UseHoursMinutesInput;
                viewModel.ShowTimeInHours = inputSettings.UseHoursMinutesInput;
                viewModel.ShowTimeInDecimals = !inputSettings.UseHoursMinutesInput;


                ProjectInsight.Models.TimeAndExpense.TimeEntryInputSettings settings = null;
                if (viewModel.SelectedTask != null)
                    settings = inputSettings.SelectedTaskTimeEntryInputSettings;
                else if (viewModel.SelectedProject != null)
                    settings = inputSettings.SelectedProjectTimeEntryInputSettings;
                else if (viewModel.SelectedCompany != null)
                    settings = inputSettings.SelectedCompanyTimeEntryInputSettings;


                if (settings  != null && settings.ShowBillableCheckboxInput.HasValue)
                {
                    viewModel.ShowBillableCheckboxInput = settings.ShowBillableCheckboxInput.Value;
                    if (settings.IsBillableDefaultOn != null)
                        viewModel.IsBillable = settings.IsBillableDefaultOn.Value;
                }
            }

            BindingContext = viewModel;


            Title = "Add Time Entry";


        }
        private async void Continue_Tapped(object sender, EventArgs e)
        {
            if (viewModel.SelectedExpenseCode == null)
            {
                frmExpenseCode.BorderColor = (Color)Application.Current.Resources["RedTextColor"];
            }
            else
            {
                frmExpenseCode.BorderColor = (Color)Application.Current.Resources["DarkGrayTextColor"];
                await Navigation.PushAsync(new TimeEntries.ActualHours(viewModel));
            }

        }
        void Company_OnSelectionChanged(object sender, EventArgs e)
        {
        }

        private void CmbExpenseCodes_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
            {
                viewModel.SelectedExpenseCode = null;
                frmExpenseCode.BorderColor = (Color)Application.Current.Resources["RedTextColor"];
            }
            else
            {
                frmExpenseCode.BorderColor = (Color)Application.Current.Resources["DarkGrayTextColor"];
            }
        }
    }
}