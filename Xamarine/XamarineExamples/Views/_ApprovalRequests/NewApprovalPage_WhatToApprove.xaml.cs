using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_WhatToApprove : ContentPage
    {
      
        NewApprovalViewModel viewModel;


        public NewApprovalPage_WhatToApprove()
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();


            BindingContext = viewModel = new NewApprovalViewModel();
           
        }

        private async void NewItem_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewApprovalPage_UploadFile());

        }

        private async void OnCancel_Tapped(object sender,DateChangedEventArgs e)
        {

            await Navigation.PopAsync();
        }


        private async void ExistingItem_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewApprovalPage_SelectType());
        }

        private async void StandaloneApproval_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewApprovalPage_Details());
        }
    }
}