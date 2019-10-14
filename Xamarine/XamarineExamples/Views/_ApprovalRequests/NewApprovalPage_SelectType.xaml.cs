using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_SelectType : ContentPage
    {
      
        NewApprovalViewModel viewModel;


        public NewApprovalPage_SelectType()
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();


            BindingContext = viewModel = new NewApprovalViewModel();
           
        }

        private async void OnCancel_Tapped(object sender,DateChangedEventArgs e)
        {

            await Navigation.PopAsync();
        }

        private async void NewItem_Tapped(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new NewApprovalPage_step1());

        }

        private async void ApprovalOnFile_Tapped(object sender, EventArgs e)
        {
                await Navigation.PushAsync(new NewApprovalPage_SearchItem(3));
        }

        private async void ApprovalOnProject_Tapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewApprovalPage_SelectProject());
            await Navigation.PushAsync(new NewApprovalPage_SearchItem(24));

        }

        private void ApprovalOnToDo_Tapped(object sender, EventArgs e)
        {

        }

        private async void ApprovalOnProposal_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewApprovalPage_SearchItem(30));

        }
    }
}