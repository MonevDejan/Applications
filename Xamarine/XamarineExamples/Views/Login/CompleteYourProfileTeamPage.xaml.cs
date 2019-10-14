using SafeSportChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.Login
{




	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompleteYourProfileTeamPage : ContentPage
	{
        CompleteProfileTeamViewModel viewModel;
        public CompleteYourProfileTeamPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new CompleteProfileTeamViewModel();
        }

        private async void CreateTeamButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTeamPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            viewModel.LoadItemsCommand.Execute(null);
        }

        private void CmbState_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

        }

        private void CmbTeam_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

        }
    }
}