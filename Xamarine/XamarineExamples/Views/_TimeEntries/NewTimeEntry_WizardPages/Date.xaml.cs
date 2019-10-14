using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views.TimeEntries
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Date : ContentPage
    {

        TimeEntryWizardViewModel viewModel;
        public Date(TimeEntryWizardViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();
            viewModel = ViewModel;
            BindingContext = viewModel;
        
                Title = "Add Time Entry";

        }
        private async void Continue_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Description(viewModel));
        }
    }
}