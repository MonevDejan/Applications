using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpenseCode : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public ExpenseCode(ExpenseEntryViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();
            
            viewModel = ViewModel;

            if (viewModel.AddingToProject || viewModel.AddingToTask)
            {
                var inputSettings = ExpenseEntryService.client.GetExpenseEntryInputSettingsAndLists(
                 includeCompanySelectionList: false,
                 includeProjectSelectionList: false,
                 includeTaskSelectionList: false,
                 includeExpenseCodeSelectionList: true,
                 lookupSelectedFromLastEntry: true,
                  selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null,
                  selectedTaskId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null
                 );
                if (inputSettings != null)
                {
                   // if (viewModel.ExpenseCodes != null) viewModel.ExpenseCodes.Clear();
                    ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(inputSettings.ExpenseCodeList);
                    viewModel.ExpenseCodes = expenseCodes;

                    if (inputSettings.SelectedExpenseCodeId.HasValue)
                        viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedExpenseCodeId).FirstOrDefault();
                }
            }

            BindingContext = viewModel;
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
            if (viewModel.SelectedExpenseCode == null)
            {
                frmExpenseCode.BorderColor = (Color)Application.Current.Resources["RedTextColor"];
            }
            else
            {
                frmExpenseCode.BorderColor = (Color)Application.Current.Resources["DarkGrayTextColor"];
                if (viewModel.SelectedExpenseType == 3)
                    await Navigation.PushAsync(new Total(viewModel));
                else
                    await Navigation.PushAsync(new Description(viewModel));
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