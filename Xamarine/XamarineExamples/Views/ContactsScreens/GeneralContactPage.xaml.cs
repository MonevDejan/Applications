using SafeSportChat.Helpers;
using SafeSportChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralContactPage : ContentPage
    {

        GeneralContactViewModel viewModel;


        public GeneralContactPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new GeneralContactViewModel();

        }

        private async void Add_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WhoWouldYouLikeToInvitePage());
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();


            viewModel.LoadItemsCommand.Execute(null);
        }


        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
           viewModel.Filter = txtSearch.Text;
        }



    }
}