using MvvmHelpers;
using Plugin.Connectivity;
using ProjectInsight.Models.SysAdmin;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.General;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SafeSportChat.Views.Login
{

    public class SignInPasswordViewModel : ProjectInsightMobile.ViewModels.BaseViewModel, INotifyPropertyChanged
    {
        public SignInPasswordViewModel()
        {
            isValid = false;
        }

        public bool _isValid { get; set; }
        public bool isValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                base.OnPropertyChanged("isValid");
            }
        }
    }

  


    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignInPasswordPage : ContentPage
	{
        SignInPasswordViewModel viewModel;
        CustomBaseViewModel vm;


        public SignInPasswordPage ()
		{
            NavigationPage.SetBackButtonTitle(this, "");

            InitializeComponent();
            BindingContext = viewModel = new SignInPasswordViewModel();

            

            //slNoConnection.Children.Clear();
            //slNoConnection.Children.Add(new NoConnectionBand());
            vm = new CustomBaseViewModel();
            vm.LoginSettings = new LoginSettings();

            if (Common.CurrentWorkspace != null)
            {
                vm.LoginSettings.SSOEnabled = Common.CurrentWorkspace.SSOEnabled;
                vm.LoginSettings.EmailPasswordEnabled = Common.CurrentWorkspace.EmailPasswordEnabled;
                vm.LoginSettings.WorkspaceName = Common.CurrentWorkspace.WorkspaceURL.Replace("https://", "").Replace("http://", "");
            }
            else
            {
                ProjectInsightMobile.App.Current.MainPage = new NavigationPage(new ProjectInsightMobile.Views.StartUp());
            }

     

        }




        private void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text.Length > 7)
            {
                viewModel.isValid = true;
            }
            else
            {
                viewModel.isValid = false;
            }
        }

        private async void ForgotPasswordButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }




        private async void OnSignIn(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            string email = "gjokov";

            if (email == null || txtPassword.Text == null || String.IsNullOrWhiteSpace(email))
            {
                Common.Instance.ShowToastMessage("No value is set for controls", DoubleResources.DangerSnackBar);
            }         
            else
            {
                
                var emailValue = email;
                var passwordValue = txtPassword.Text;

                IsBusy = true;
                vm.VisibleLoad = true;
                vm.LoadingMessage = "";

                AuthenticationService cs = new AuthenticationService();
                var checkLoginRequestResponse = await cs.Login(email, txtPassword.Text);


                IsBusy = false;
                vm.VisibleLoad = false;
                if (checkLoginRequestResponse != null)
                {

                    //User user = new User();
                    //user.Email = emailValue;
                    //user.FirstName = checkLoginRequestResponse.User.FirstName;
                    //user.LastName = checkLoginRequestResponse.User.LastName;
                    //user.DateTimeCreated = DateTime.Now;
                    //user.UserToken = checkLoginRequestResponse.Token;
                    //user.UserID = checkLoginRequestResponse.UserId.Value;
                    //Common.Instance.user = user ;
                    //SQLiteConnection connection = Common.Instance.InitializeDatabase();
                    //Common.Instance.CreateTable<User>();
                    //Common.Instance._sqlconnection.Insert(user);

                    var currWorkspace = Common.CurrentWorkspace;
                    currWorkspace.UserID = checkLoginRequestResponse.UserId.Value;
                    currWorkspace.ApiToken = checkLoginRequestResponse.Token;
                    Common.Instance._sqlconnection.Update(currWorkspace);

                    //var workspaces = Common.Instance.GetWorkspaces();
                    //Workspace ws = workspaces.FirstOrDefault(x => x.WorkspaceURL.ToLower() == Common.WorkspaceUrl.ToLower());
                    //if (ws != null)
                    //{
                    //    ws.ApiToken = checkLoginRequestResponse.Token;
                    //    Common.Instance._sqlconnection.Update(ws);
                    //}
                    //else
                    //    return;

                    APIsInitialization.InitializeApis();

                    ProjectInsight.Models.Users.User userMe = UsersService.GetSimpleMe();
                    Common.UserGlobalCapability = userMe.UserGlobalCapability;
                    Common.WorkspaceCapability = WorkspaceService.GetWorkspaceCapability();

                    string notifCount = string.Empty;
                    //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = false;
                    if (userMe.NotificationNewCount != null && userMe.NotificationNewCount.Value > 0)
                    {
                        //Common.Instance.bottomNavigationViewModel.IsNottificationContVisible = true;
                        if (userMe.NotificationNewCount.Value > 99)
                            notifCount = "99+";
                        else
                            notifCount = userMe.NotificationNewCount.Value.ToString();
                    }

                    Common.Instance.bottomNavigationViewModel.NumberNottificationItems = notifCount;
                    Common.Instance.bottomNavigationViewModel.NumberWorkListItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType != ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;
                    Common.Instance.bottomNavigationViewModel.NumberProjectItems = new ObservableCollection<MyWorkItem>(Common.Instance.GetUserWork().Where(x => x.ItemType == ItemType.Projects && x.WorkspaceId == Common.CurrentWorkspace.Id)).Count;


                    //Common.Instance.ShowToastMessage("Success", DoubleResources.SuccessSnackBar);
                    //await Navigation.PushAsync(new StartupMasterPage());
                    Common.Instance.bottomNavigationViewModel.ActiveIcon = 1;
                    //App.Current.MainPage = new StartupMasterPage();

                    await Navigation.PushAsync(new Conversations());


                }
                else
                {
                    Common.Instance.ShowToastMessage("Invalid Login", DoubleResources.DangerSnackBar);
                }
            }
        }


    }
}