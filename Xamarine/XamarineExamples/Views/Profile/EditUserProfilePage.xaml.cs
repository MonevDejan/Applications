using Newtonsoft.Json.Linq;
using Octane.Xamarin.Forms.VideoPlayer;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static System.Net.Mime.MediaTypeNames;

namespace SafeSportChat.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserProfilePage : ContentPage
    {
        //ExpenseEntryViewModel viewModel;
        UserProfileViewModel viewModel;

        public EditUserProfilePage(UserProfileViewModel ViewModel)
        {
            NavigationPage.SetBackButtonTitle(this, "");

            InitializeComponent();
            viewModel = ViewModel;
            BindingContext = viewModel;

        }



        Guid UserID = Guid.Empty;






        public EditUserProfilePage(Guid? UserID = null)
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
        public EditUserProfilePage()
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

                var region = user.Region != null && user.Region.Length > 0 ? ", " + user.Region : "";
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
                    viewModel.IsDepartmentVisible = true;
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




        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void BtnDone_Clicked(object sender, EventArgs e)
        {
            // 1 save modified data 

            // 2 save profile image

            await Navigation.PopAsync();
        }

        private void BtnNewProfileImage_Clicked(object sender, EventArgs e)
        {
            // 1 open intent for camera or gallery

            // 2 if cancel nothing

            // 3 if image was selected or taken shown that image in imgPhoto control

            Common.Instance.ShowToastMessage("thank you for choosing new profile image", DoubleResources.DangerSnackBar);

        }


        async void OnEditProfileImage_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Open", "Cancel", null, "Camera", "Choose from gallery");
            Debug.WriteLine("Action: " + action);

            if (action.Equals("Camera"))
            {

                try
                {

                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("No Camera", "No camera available.", "OK");
                        return;
                    }

                    var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                    var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                    if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                        cameraStatus = results[Permission.Camera];
                        storageStatus = results[Permission.Storage];
                    }

                    if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                    {
                        viewModel.VisibleLoad = true;
                        var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync
                        (
                        new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                        {
                            RotateImage = false,
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 800,
                            CompressionQuality = 50
                            //Directory = "Receipts",
                            //SaveToAlbum = true


                        }
                        );

                        if (photo != null)
                        {
                            // await DisplayAlert("Camera", "Photo taken", "OK");
                            //Get the public album path
                            //var aPpath = photo.AlbumPath;

                            //Get private path
                            //var path = photo.Path;

                            imgPhoto.IsVisible = true;
                            wvAvatar.IsVisible = false;


                            ImageSource photoSource = ImageSource.FromStream(() =>
                            {
                                var source = photo.GetStream();
                                // viewModel.Photo = source;
                                return source;

                            });


                            byte[] imageAsBytes = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                photo.GetStream().CopyTo(memoryStream);
                                //photo.Dispose();
                                imageAsBytes = memoryStream.ToArray();

                            }
                         



                            

                            imgPhoto.Source = photoSource;
                            viewModel.PhotoSource = photoSource;

                            // zoki btnTakePhoto.IsVisible = false;
                            //btnFindPhoto.IsVisible = false;
                            //lblSkip.Text = "Continue";
                            //btnAntoher.IsVisible = true;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                        //On iOS you may want to send your user to the settings screen.
                        //CrossPermissions.Current.OpenAppSettings();
                    }
                }
                catch (Exception ex)
                {
                    Common.Instance.ShowToastMessage(ex.Message, DoubleResources.DangerSnackBar);
                }
                viewModel.VisibleLoad = false;


            }

            if (action.Equals("Choose from gallery"))
            {
                Console.WriteLine("drry");

                viewModel.VisibleLoad = true;
                ImageResult imageResult = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

                if (imageResult != null && imageResult.ImageSource != null)
                {

                    byte[] imageAsBytesOrig = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        imageResult.ImageSource.CopyTo(memoryStream);
                        imageAsBytesOrig = memoryStream.ToArray();

                    }



                    var imageAsBytes = DependencyService.Get<IImageResizer>().ResizeImage(imageAsBytesOrig, 800, 800);

                  

                    

                    viewModel.PhotoName = imageResult.FileName;


                    ImageSource photoSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                    imgPhoto.Source = photoSource;


                    ImageSource photoSource2 = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                    viewModel.PhotoSource = photoSource2;


                    //viewModel.Photo = new MemoryStream(imageAsBytes);

                    imgPhoto.IsVisible = true;
                    wvAvatar.IsVisible = false;



                    // zoki btnTakePhoto.IsVisible = false;
                    //btnFindPhoto.IsVisible = false;

                    //lblSkip.Text = "Continue";
                    // btnAntoher.IsVisible = true;
                }
                else
                {
                }
                viewModel.VisibleLoad = false;
            }
            //}
        }

       

    }









}
