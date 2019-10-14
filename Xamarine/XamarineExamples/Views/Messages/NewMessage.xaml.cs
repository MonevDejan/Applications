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
    public partial class NewMessage : ContentPage
    {

        NewMessageViewModel viewModel;


        public NewMessage()
        {
            InitializeComponent();

            BindingContext = viewModel = new NewMessageViewModel();

        }

        private async void AddParticipants_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }


        

        private async void Send_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }


        private async void Like_OnClicked(object sender, EventArgs e)
        {
        }


        private async void Attachment_OnClicked(object sender, EventArgs e)
        {
        }




        private async void Cancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
        }

    }
}