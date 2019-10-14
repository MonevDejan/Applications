using Plugin.Connectivity;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.Profile;
using SafeSportChat.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{

 


    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartupMasterPageContent : ContentPage
	{
        public StartupMasterPageContent()
        {
            InitializeComponent();

           
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            StackLayout s = ((StackLayout)sender);
            Label l = s.Children[1] as Label;
            if (l != null)
            {
                var app = Application.Current as App;
                var mainPage = (StartupMasterPage)app.MainPage;
                NavigationPage page = null;

                switch (l.Text)
                {
                    case "My Profile":
                        
                        page = new NavigationPage(new UserProfile());
                        mainPage.Detail = page;
                        mainPage.Title = page.Title;
                        mainPage.IsPresented = false;

                        break;
                    case "Contacts": 
                        
                        page = new NavigationPage(new GeneralContactPage());
                        mainPage.Detail = page;
                        mainPage.Title = page.Title;
                        mainPage.IsPresented = false;

                        break;

                    case "Messages":

                        page = new NavigationPage(new Conversations());
                        mainPage.Detail = page;
                        mainPage.Title = page.Title;
                        mainPage.IsPresented = false;

                        break;


                    default: Console.WriteLine("under construction"); break;
                }
            }

        }
    }

}