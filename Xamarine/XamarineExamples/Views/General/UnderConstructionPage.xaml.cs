using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UnderConstructionPage : ContentPage
	{
        public UnderConstructionPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            token.IsVisible = false;
            IEnumerable<PushNotificationSettings> settings = Common.Instance.GetAll<PushNotificationSettings>();
            PushNotificationSettings setting = settings.FirstOrDefault();
            if (setting != null)
            {
                token.Text = setting.CreatedAt.ToString() + ": " + setting.Token;
            }
        }

        public async void ShowToken(object sender, EventArgs e)
        {
            token.IsVisible = true;
        }
    }
}