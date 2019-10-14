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
	public partial class LogInMagicLinkPage : ContentPage
	{
		public LogInMagicLinkPage ()
		{
			InitializeComponent ();
		}

        private async void EmailMagicLinkButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MagicLinkEmailInputPage());
        }

        private async void PasswordButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInPasswordPage());
        }
    }
}