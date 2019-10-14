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
	public partial class YouAreAllSetPage : ContentPage
	{
		public YouAreAllSetPage ()
		{
			InitializeComponent ();
		}

        private async void InviteSomeoneButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InviteSomeoneElsePage());
        }

        private async void BtnStartChat_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GeneralContactPage());
        }
    }
}