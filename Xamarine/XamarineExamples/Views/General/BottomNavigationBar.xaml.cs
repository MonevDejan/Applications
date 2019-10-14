using Plugin.Connectivity;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.MyTime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BottomNavigationBar : ContentView
	{
		public BottomNavigationBar ()
		{
			InitializeComponent ();

            //BottomNavigationViewModel bottomNavigationViewModel = new BottomNavigationViewModel();
            //bottomNavigationViewModel.NumberWorkListItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType != ItemType.Projects)).Count;
            //bottomNavigationViewModel.NumberWorkListItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType == ItemType.Projects)).Count;
            //bottomNavigationViewModel.NumberNottificationItems = new ObservableCollection<NotificationLocal>(Common.Instance.GetUserNotifications()).Count;
            BindingContext = Common.Instance.bottomNavigationViewModel;

          

            string btnMyWork_isActive = "";
            string btnMyProjects_isActive = "";
            string btnNotification_isActive = "";
            string btnTime_isActive = "";
            string btnExpenseEntry_isActive = "";

            if (Common.Instance.bottomNavigationViewModel.ActiveIcon == 1)
                btnMyWork_isActive = "_active_l";
            else if (Common.Instance.bottomNavigationViewModel.ActiveIcon == 2)
                btnMyProjects_isActive = "_active_l";
            else if (Common.Instance.bottomNavigationViewModel.ActiveIcon == 3)
                btnNotification_isActive = "_active_l";
            else if (Common.Instance.bottomNavigationViewModel.ActiveIcon == 4)
                btnTime_isActive = "_active_l";
            else if (Common.Instance.bottomNavigationViewModel.ActiveIcon == 5)
                btnExpenseEntry_isActive = "_active_l";


            btnMyWork.Source = string.Format("bottom_MyWork{0}.png", btnMyWork_isActive);
            btnMyProjects.Source = string.Format("bottom_MyProjects{0}.png", btnMyProjects_isActive);
            btnNotification.Source = string.Format("bottom_Notifications{0}.png", btnNotification_isActive);
            btnTime.Source = string.Format("bottom_MyTime{0}.png", btnTime_isActive);
            btnExpenseEntry.Source = string.Format("bottom_Expenses{0}.png", btnExpenseEntry_isActive); ;


        }


        async  void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {

            //global ref of app
            var app = Application.Current as App;                                    
            var mainPage = (StartupMasterPage)app.MainPage;
            NavigationPage page = null;

            Guid Id = new Guid();
            if (sender is AbsoluteLayout)
                Id = ((AbsoluteLayout)sender).Id;
            else
                Id = ((Image)sender).Id;

            if (Id == loMyWork.Id)
            {
                Common.Instance.bottomNavigationViewModel.ActiveIcon = 1;
                page = new NavigationPage(new MyWorkPage());
                
            }
            else if (Id == loMyProjects.Id)
            {
                Common.Instance.bottomNavigationViewModel.ActiveIcon = 2;
                page = new NavigationPage(new MyProjectsPage());
            }
            
            else if (Id == loNotification.Id)
            {
                if (!CrossConnectivity.Current.IsConnected) return;
                Common.Instance.bottomNavigationViewModel.ActiveIcon = 3;

                page = new NavigationPage(new NotificationsPage());
            }
            else if (Id == loTime.Id)
            {
                if (!CrossConnectivity.Current.IsConnected) return;
                Common.Instance.bottomNavigationViewModel.ActiveIcon = 4;

                page = new NavigationPage(new MyTimePage());
            }
            else if (Id == loExpenseEntry.Id)
            {
                if (!CrossConnectivity.Current.IsConnected) return;
                Common.Instance.bottomNavigationViewModel.ActiveIcon = 5;
                //page = new NavigationPage(new UnderConstructionPage());
                page = new NavigationPage(new ExpensesPage());

            }

            try
            {
                mainPage.Detail = page;
                mainPage.Title = page.Title;
            }
            catch(Exception ex)
            {

            }
        }
    }
}