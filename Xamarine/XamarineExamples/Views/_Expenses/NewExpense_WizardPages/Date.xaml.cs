using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Date : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public Date(ExpenseEntryViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();
            viewModel = ViewModel;
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
            if (viewModel.SelectedExpenseType == 1)
            {
                if (viewModel.AddingToTask || viewModel.AddingToProject)
                    await Navigation.PushAsync(new ExpenseCode(viewModel));
                else
                    await Navigation.PushAsync(new Company_Project_Task2(viewModel));

            }
            else if (viewModel.SelectedExpenseType == 2)
            {
                if (viewModel.AddingToTask || viewModel.AddingToProject)
                    await Navigation.PushAsync(new Description(viewModel));
                else
                    await Navigation.PushAsync(new Company_Project_Task2(viewModel));
            }
            else if (viewModel.SelectedExpenseType == 3)
                await Navigation.PushAsync(new Description(viewModel));
            

        }
    }
}