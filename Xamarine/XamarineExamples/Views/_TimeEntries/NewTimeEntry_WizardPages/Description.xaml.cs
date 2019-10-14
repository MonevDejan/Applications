using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views.TimeEntries
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Description : ContentPage
    {

        TimeEntryWizardViewModel viewModel;
        public Description(TimeEntryWizardViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();
            BindingContext = viewModel = ViewModel;
            Title = "Add Time Entry";
        }
        private void Continue_Tapped(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new Congratulations(viewModel));

            var app = Application.Current as App;
            var mainPage = (StartupMasterPage)app.MainPage;
            NavigationPage page = null;
            page = new NavigationPage(new Congratulations(viewModel));
            mainPage.Detail = page;
            mainPage.Title = page.Title;
        }
    }
}