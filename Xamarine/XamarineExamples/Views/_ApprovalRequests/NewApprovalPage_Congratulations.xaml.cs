using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_Congratulations : ContentPage
    {
      
        NewApprovalViewModel viewModel;


        public NewApprovalPage_Congratulations()
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();


            BindingContext = viewModel = new NewApprovalViewModel();
           
        }

        private async void OnCancel_Tapped(object sender,DateChangedEventArgs e)
        {

            await Navigation.PopAsync();
        }

        private async void AddAnotherApproval_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewApprovalPage_WhatToApprove());
        }

        private async void BackToMyWork_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}