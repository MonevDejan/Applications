using SafeSportChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.ContactsScreens
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditGroupsPage : ContentPage
	{
        GroupsPageViewModel viewModel;
        public EditGroupsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new GroupsPageViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();


            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void BackButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GroupsPage());
        }
    }
}