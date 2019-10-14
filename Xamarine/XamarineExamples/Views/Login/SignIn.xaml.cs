
using Plugin.Connectivity;
using ProjectInsight.Models.Devices;
using ProjectInsight.WebApi.Client;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.General;
using SafeSportChat.Views.Login;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{

    public class SignInViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public SignInViewModel()
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
    public partial class SignIn : ContentPage
    {
        CustomBaseViewModel vm;
        SignInViewModel viewModel;

        public SignIn(Workspace workspace = null)
        {
            //workspace = new Workspace();
            //workspace.WorkspaceURL = "https://dev30-gjokov.sandbox.projectinsight.net/ProjectInsight.WebApp";
            //workspace.isActive = true;
            //Common.CurrentWorkspace = workspace;



            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();

            BindingContext = viewModel = new SignInViewModel();
            //HockeyApp.MetricsManager.TrackEvent("SignIn Initialize");
            slNoConnection.Children.Clear();
            slNoConnection.Children.Add(new NoConnectionBand());

            vm = new CustomBaseViewModel();
            vm.LoginSettings = new LoginSettings();
            //if (workspace == null)
            //{
            //    var workspaces = Common.Instance.GetWorkspaces();

            //    Workspace ws = workspaces.FirstOrDefault(x => x.isActive);
            //    if (ws != null)
            //    {
            //        workspace = ws;
            //    }
            //}

            if (workspace != null)
            {
                vm.LoginSettings.SSOEnabled = workspace.SSOEnabled;
                vm.LoginSettings.EmailPasswordEnabled = workspace.EmailPasswordEnabled;
                vm.LoginSettings.WorkspaceName = workspace.WorkspaceURL.Replace("https://", "").Replace("http://", "");
            }
            else
            {
                App.Current.MainPage = new NavigationPage(new StartUp());
            }



            BindingContext = vm;

            LoginSettings.BindingContext = vm.LoginSettings;

            // GetData();
        }
        
        private async void OnSignIn(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            if (txtEmail.Text == null || txtPassword.Text == null || String.IsNullOrWhiteSpace(txtEmail.Text))
            {
                Common.Instance.ShowToastMessage("No value is set for controls", DoubleResources.DangerSnackBar);
            }
            //else if (!Common.Instance.IsValidEmailAddress(emailValue))
            //{

            //    Common.Instance.ShowToastMessage("Email is not in a valid format", DoubleResources.DangerSnackBar);
            //}
            else
            {
                txtEmail.Text = txtEmail.Text.Trim();
                var emailValue = txtEmail.Text;
                var passwordValue = txtPassword.Text;

                IsBusy = true;
                vm.VisibleLoad = true;
                vm.LoadingMessage = "";

                AuthenticationService cs = new AuthenticationService();
                var checkLoginRequestResponse = await cs.Login(txtEmail.Text, txtPassword.Text);


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

                    await Navigation.PushAsync(new SignIn());


                }
                else
                {
                    Common.Instance.ShowToastMessage("Invalid Login", DoubleResources.DangerSnackBar);
                }
            }
        }

        private async void loginWithSSO(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;
            //Create a DeviceActivationRequest object with the following properties:
            //ClientPlatform - The platform or operating system the activation request was made by(optional). 
            //For example, Windows 10, iOS 9.
            //ClientDevice - The device or computer type the activation request was made by(optional). 
            //For example Computer, iPhone, Droid.
            //ClientAgent - The device user agent string. (for example Safari, Chrome, Edge).Required when making a device activation request.
            //ClientIPAddress - The client declared IP address the activation was made from(optional)
            //ClientHostName - [Host Name] - The host name of the computer or device which requested the activation(optional)



            DeviceActivationRequest deviceActivationRequest = new DeviceActivationRequest();
            deviceActivationRequest.ClientPlatform = "Android 7.1.1";
            deviceActivationRequest.ClientDevice = "Droid";
            deviceActivationRequest.ClientAgent = "Chrome";
            deviceActivationRequest.ClientIPAddress = "185.83.252.175";
            deviceActivationRequest.ClientHostName = "Test";

            //Post the DeviceActivationRequest object to:
            //POST / device - activation / create - request
            //This endpoint returns a DeviceActivationRequestPending object with the following properties:
            //DeviceActivationId
            //DeviceActivationRequestUrl
            //ExpirationDateTimeUTC


            ProjectInsightWebApiClient client = new ProjectInsightWebApiClient(Common.WorkspaceApi);

            try
            {
                DeviceActivationRequestPending pendingRequest = await client.DeviceActivationRequest.CreateRequestAsync(deviceActivationRequest);

                //Direct the user the URL provided by the DeviceActivationRequestUrl property of the DeviceActivationRequestPending object.
                //Once logged in the user will be provided an activation code to be used in the next step.
                Xamarin.Forms.Device.OpenUri(new Uri(pendingRequest.DeviceActivationRequestUrl.Replace("dev28", "dev30")));

                await Navigation.PushAsync(new ActivateCode(pendingRequest.DeviceActivationId));

            }
            catch (Exception ex)
            {

            }

        }
        private async void havingTrouble(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;
            await Navigation.PushAsync(new ForgotNameOrPassword());
        }


        private async void changeWorkspace(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;
            await Navigation.PushAsync(new ChooseWorkspace());
        }

        private async void SignUp_Clicked(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;
            await Navigation.PushAsync(new CreateAccountPage());
        }
        public void ShowPass(object sender, EventArgs args)
        {
            txtPassword.IsPassword = txtPassword.IsPassword ? false : true;
        }
        private void entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }
        public void Validate()
        {
            viewModel.isValid = false;
            if (txtEmail.Text != null)
            {
                bool isEmail = Regex.IsMatch(txtEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (isEmail)
                {

                    if (txtPassword.Text != null && txtPassword.Text.Length > 10)
                    {

                        viewModel.isValid = true;
                    }
                }
            }


        }
    }
}
