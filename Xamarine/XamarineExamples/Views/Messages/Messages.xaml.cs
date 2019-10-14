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
    public partial class Messages : ContentPage
    {

        MessagesViewModel viewModel;


        public Messages(int ConversationId)
        {
            InitializeComponent();

            BindingContext = viewModel = new MessagesViewModel();

        }

        private async void Like_OnClicked(object sender, EventArgs e)
        {
        }

        private async void Attachment_OnClicked(object sender, EventArgs e)
        {
        }


        private async void Back_Onclicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WhoWouldYouLikeToInvitePage());
        }

        


        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadItemsCommand.Execute(null);
        }


      


    }
}