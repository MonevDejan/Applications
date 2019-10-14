using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Description : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public Description(ExpenseEntryViewModel ViewModel)
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
        private void Continue_Tapped(object sender, EventArgs e)
        {
          //  await Navigation.PushAsync(new Congratulations(viewModel));

            var app = Application.Current as App;
            var mainPage = (StartupMasterPage)app.MainPage;
            NavigationPage page = null;
            page = new NavigationPage(new Congratulations(viewModel));
            mainPage.Detail = page;
            mainPage.Title = page.Title;
        }
    }
}