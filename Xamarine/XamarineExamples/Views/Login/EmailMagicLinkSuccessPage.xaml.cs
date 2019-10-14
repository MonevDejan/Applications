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
	public partial class EmailMagicLinkSuccessPage : ContentPage
	{
		public EmailMagicLinkSuccessPage ()
		{
			InitializeComponent ();
		}

        private async void OpenEmailAppButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EmailMagicLinkEmailOverlayPage());
        }
    }
}