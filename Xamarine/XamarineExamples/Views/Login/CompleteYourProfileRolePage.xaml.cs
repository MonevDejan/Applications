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
	public partial class CompleteYourProfileRolePage : ContentPage
	{
		public CompleteYourProfileRolePage ()
		{
			InitializeComponent ();
		}

        private async void AthleteButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompleteYourProfilePage());
        }
    }
}