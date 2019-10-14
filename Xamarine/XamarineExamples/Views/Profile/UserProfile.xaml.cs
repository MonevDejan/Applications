using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using SafeSportChat.Views.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile
    {
        UserProfileViewModel viewModel;
        Guid UserID = Guid.Empty;
        public UserProfile(Guid? UserID=null)
        {
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("UserProfile Initialize");

            InitializeComponent();
            //User user = Common.Instance.GetActiveUser();
            if (UserID != null)
                Title = "Profile";
            this.UserID = UserID.Value;
            BindingContext = viewModel = new UserProfileViewModel();

            GetData();

        }
        public UserProfile()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            //User user = Common.Instance.GetActiveUser();
            BindingContext = viewModel = new UserProfileViewModel();

            GetData();

        }

        private async void GetData()
        {
            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetUser();

            if (isSuccess)
            {
                viewModel.LoadingMessage = "Success";
            }
            else
            {                                   
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
            viewModel.VisibleLoad = false;
        }
        public async Task<bool> GetUser()
        {
            try
            {

                ProjectInsight.Models.Users.User user = new ProjectInsight.Models.Users.User();
                if (UserID == null || UserID == Guid.Empty)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        user = UsersService.GetMe();
                    });
                }
                else
                {
                    user = await UsersService.GetUser(UserID);
                }

                viewModel.Id = user.Id.ToString();
                viewModel.Name = user.Name;
                viewModel.Email = user.EmailAddress;
                
                viewModel.Company = user.Company != null ? user.Company.Name : String.Empty;
                if (viewModel.Company != String.Empty)
                {
                    viewModel.IsCompanyVisible = true;
                }


                viewModel.UserFirstLastName = user.FirstName + " " + user.LastName;
                viewModel.Address1 = user.Address1;
                viewModel.Address2 = user.Address2;

                var region = user.Region != null && user.Region.Length > 0 ? ", " +user.Region : "";
                var postCode = user.PostCode != null && user.PostCode.Length > 0 ? ", " + user.PostCode : "";
                viewModel.CityRegionZip = user.City + region + postCode;
                viewModel.ZIP = user.PostCode;
                viewModel.Phone = user.Phone;
                viewModel.City = user.City;
                viewModel.Region = user.Region;
                viewModel.Country = user.Country;

                if (!string.IsNullOrEmpty(user.PhotoUrl))
                {
                    viewModel.Photo = Common.CurrentWorkspace.WorkspaceURL + user.PhotoUrl;
                    viewModel.IsPhotoVisible = true;
                    viewModel.IsAvatarVisible = false;

                    //imgPhoto234.IsVisible = true;
                    wvAvatar.IsVisible = false;
                }
                else
                {
                    viewModel.IsPhotoVisible = false;
                    viewModel.IsAvatarVisible = true;

                    //imgPhoto234.IsVisible = false;
                    wvAvatar.IsVisible = true;

                    if (user.AvatarHtml != String.Empty)
                    {
                        viewModel.AvatarHtml = "<style>.user-avatar {font-family: 'Open Sans',segoe ui,verdana,helvetica;width: 125px!important;height: 125px!important;border-radius: 50%;line-height: 125px!important;font-size: 62px!important;color: #fff;text-align: center;margin: 0 !important;vertical-align: middle;overflow: hidden;cursor: pointer;display: inline-block;}</style>";
                        viewModel.AvatarHtml += user.AvatarHtml;
                    }
                }

                viewModel.Department = user.Department != null ? user.Department.Name : String.Empty;

                if (viewModel.Department != String.Empty)
                {
                    viewModel.IsDepartmentVisible=true;
                }
                if (viewModel.Address2 != null && viewModel.Address2.Length > 0)
                {
                    viewModel.IsAddress2Visible = true;
                }
                if (viewModel.Address1 != null && viewModel.Address1.Length > 0)
                {
                    viewModel.IsAddress1Visible = true;
                }
                if (viewModel.Country != null && viewModel.Country.Length > 0)
                {
                    viewModel.IsCountryVisible = true;
                }
                if (viewModel.Phone != null && viewModel.Phone.Length > 0)
                {
                    viewModel.IsPhoneVisible = true;
                }
                if (viewModel.City != null && viewModel.City.Length > 0)
                {
                    viewModel.IsCityVisible = true;
                }
                
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public async void OnActionPhone(object sender, EventArgs e)
        {

            DependencyService.Get<IPhoneCall>().makeCall(viewModel.Phone);
        }
        public async void OnActionEmail(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:" + viewModel.Email));
        }

        private async void EditProfileButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditUserProfilePage());
        }
    }
}