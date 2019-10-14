using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Amount : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public Amount(ExpenseEntryViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");

            viewModel = ViewModel;
            InitializeComponent();
            BindingContext = viewModel;
            txtAmount.HasError = false;
            if (viewModel.SelectedExpenseType == 1)
                Title = "Add Receipt";
            else if (viewModel.SelectedExpenseType == 2)
                Title = "Add Mileage";
            else if (viewModel.SelectedExpenseType == 3)
            {
                Title = "Add Other";
                //lblTitle.Text = 
                    txtAmount.Hint ="Unit Price";
            }
            else
                Title = "Add";

        }
        private async void Continue_Tapped(object sender, EventArgs e)
        {
            if (viewModel.AmountError)
            {
                txtAmount.HasError = true;
            }
            else
            {
                if (viewModel.SelectedExpenseType == 2)
                    await Navigation.PushAsync(new CostPerMile(viewModel));
                else
                    await Navigation.PushAsync(new Date(viewModel));
            }

        }

        private void NumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            if (viewModel.AmountError)
                txtAmount.HasError = true;
            else
                txtAmount.HasError = false;
        }
    }
}