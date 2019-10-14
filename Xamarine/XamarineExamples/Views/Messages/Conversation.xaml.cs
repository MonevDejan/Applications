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
    public partial class Conversations : ContentPage
    {

        ConversationsViewModel viewModel;


        public Conversations()
        {
            InitializeComponent();

            BindingContext = viewModel = new ConversationsViewModel();

        }

        private async void Add_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewMessage());
        }


        private async void Back_Onclicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

        private async void OpenConversation_OnClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            int ConversationId = 0;
            Int32.TryParse(b.CommandParameter.ToString(), out ConversationId);

            await Navigation.PushAsync(new Messages(ConversationId));
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
        }


      

    }
}