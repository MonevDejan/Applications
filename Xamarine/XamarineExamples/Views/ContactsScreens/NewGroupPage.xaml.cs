using ProjectInsightMobile;
using ProjectInsightMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.ContactsScreens
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewGroupPage : ContentPage
	{
		public NewGroupPage ()
		{
			InitializeComponent ();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            var app = Application.Current as App;
            var mainPage = (StartupMasterPage)app.MainPage;
            NavigationPage page = null;

            page = new NavigationPage(new GroupsPage());
            mainPage.Detail = page;
            mainPage.Title = page.Title;
            mainPage.IsPresented = false;
        }
    }
}