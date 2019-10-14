using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Distance : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public Distance(ExpenseEntryViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");

            viewModel = ViewModel;
            InitializeComponent();
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
            if (!viewModel.ShowContinueOnAmountPage)
            {
                sfDistance.HasError = true;
            }
            else

            {
                if (viewModel.SelectedExpenseType == 1)
                    await Navigation.PushAsync(new Date(viewModel));
                else if (viewModel.SelectedExpenseType == 2)
                    await Navigation.PushAsync(new CostPerMile(viewModel));
            }
        }

        private void NumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
                sfDistance.HasError = !viewModel.ShowContinueOnAmountPage;

        }
    }
}