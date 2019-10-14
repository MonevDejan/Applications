using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Total : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public Total(ExpenseEntryViewModel ViewModel)
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
            {
                Title = "Add Other";
                viewModel.Total = 1;
            }
            else
                Title = "Add";

        }
        private async void Continue_Tapped(object sender, EventArgs e)
        {
            if (!viewModel.ShowContinueOnTotalPage)
            {
                sfQuantity.HasError = true;
            }
            else
            {
                sfQuantity.HasError = false;
                await Navigation.PushAsync(new Amount(viewModel));
            }

        }

        private void NumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            sfQuantity.HasError = !viewModel.ShowContinueOnTotalPage;

        }
    }
}