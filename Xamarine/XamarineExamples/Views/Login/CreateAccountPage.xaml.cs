using ProjectInsightMobile.Views;
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
	public partial class CreateAccountPage : ContentPage
	{
		public CreateAccountPage ()
		{
			InitializeComponent ();
		}

        private async void CreateAccountByPhoneNumber_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccountByPhoneNumberPage());
        }
        private async void CreateAccountByEmail_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateAccountByEmailPage());
        }
        private async void SignInButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignInUsingPhoneOrEmailPage());
        }
    }
}