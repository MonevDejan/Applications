using SafeSportChat.Views.ContactsScreens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SafeSportChat.ViewModels;

namespace SafeSportChat.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddSomeoneFromATeamPage : ContentPage
	{
        AddSomeoneFromATeamViewModel viewModel;

        public AddSomeoneFromATeamPage ()
		{
			InitializeComponent ();
            BindingContext = viewModel = new AddSomeoneFromATeamViewModel();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();


            viewModel.LoadItemsCommand.Execute(null);
        }
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GeneralContactPage());
        }

        private async void Team_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TeamContactsPage());
        }

       
        private void CmbState_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

        }

        private void CmbTeam_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GroupsPage());
        }
    }
}