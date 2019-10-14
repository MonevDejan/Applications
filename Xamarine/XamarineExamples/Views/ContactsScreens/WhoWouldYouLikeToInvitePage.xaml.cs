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
	public partial class WhoWouldYouLikeToInvitePage : ContentPage
	{
		public WhoWouldYouLikeToInvitePage ()
		{
			InitializeComponent ();
		}

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GeneralContactPage());
        }
        private async void Invite_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InviteANewContactPage());
        }
        private async void AddFromTeam_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddSomeoneFromATeamPage());
        }
    }
}