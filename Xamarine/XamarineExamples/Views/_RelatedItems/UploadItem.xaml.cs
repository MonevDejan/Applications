using ProjectInsightMobile.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using System.Linq;
using System.Threading.Tasks;
using ProjectInsightMobile.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ProjectInsightMobile.CustomControls;
using System.IO;
using Plugin.Media;
using ProjectInsightMobile.Models;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UploadItem : ContentPage
    {
        Guid? ParentId = null;
        string ItemType = string.Empty;
        ProjectInsightMobile.ViewModels.BaseViewModel viewModel;
        public UploadItem(Guid? parentId, string itemType)
		{
            NavigationPage.SetBackButtonTitle(this,"");

            
            ItemType = itemType;
            ParentId = parentId;
            InitializeComponent();
            if (itemType.Equals("Project"))
                cmbCreateFolder.IsVisible = true;
            BindingContext = viewModel = new ViewModels.BaseViewModel();
        }

      
        private async void CameraRoll_Tapped(object sender, EventArgs e)
        {
            viewModel.VisibleLoad = true;
            //viewModel.VisibleLoad = true;
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



                await SaveRelatedItem(imageAsBytes, imageResult.FileName);
                //ImageSource photoSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));

                //PhotoImage.IsVisible = true;

            }
            else
            {
            }
           viewModel.VisibleLoad = false;

        }

       
        private async void TakePhoto_Tapped(object sender, EventArgs e)
        {
            try
            {
                viewModel.VisibleLoad = true;

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
                    //viewModel.VisibleLoad = true;
                    var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync
                    (
                    new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                    {
                        RotateImage = false,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 800,
                        CompressionQuality = 50,
                        //Directory = "Receipts",
                        SaveToAlbum = true


                    }
                    );

                    if (photo != null)
                    {
                     

                        //PhotoImage.IsVisible = true;


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
                        var fileName = "IMG_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";
                        if (!string.IsNullOrEmpty(photo.AlbumPath))
                            fileName = Path.GetFileName(photo.Path);

                        await SaveRelatedItem(imageAsBytes, fileName);
                     

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


        private async void Other_Tapped(object sender, EventArgs e)
        {
            try
            {
                viewModel.VisibleLoad = true;
                if (Device.RuntimePlatform == Device.Android &&
                  !await this.CheckPermissionsAsync())
                {
                    return;
                }


                var crossFilePicker = CrossFilePicker.Current;
                var myResult = await crossFilePicker.PickFile();
                if (myResult != null && !string.IsNullOrEmpty(myResult.FileName))
                {
                    await SaveRelatedItem(myResult.DataArray, myResult.FileName);
                }
                else
                {

                    //Common.Instance.ShowToastMessage("Error with file!", DoubleResources.DangerSnackBar);
                }

            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage("Error - " + ex.ToString(), DoubleResources.DangerSnackBar);
                ex.ToString(); //"Only one operation can be active at a time"
            }
            viewModel.VisibleLoad = false;

        }

        private async Task SaveRelatedItem(byte[] content, string FileName)
        {
            string FileContent = Convert.ToBase64String(content);


            if (string.IsNullOrEmpty(FileName))
                FileName = "IMG_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg";


            ProjectInsight.WebApi.Client.Files.FileItemClient fileItemClient = null;

            if (ItemType.Equals("Task")) //Task link
            {
                fileItemClient = TasksService.client.ProjectInsightWebApiClient.FileItem;
            }
            else if (ItemType.Equals("Issue")) //Issue link
            {
                fileItemClient = IssuesService.client.ProjectInsightWebApiClient.FileItem;
            }
            else if (ItemType.Equals("ToDo")) //ToDo link
            {
                fileItemClient = ToDoService.client.ProjectInsightWebApiClient.FileItem;
            }
            else if (ItemType.Equals("ApprovalRequest")) //ToDo link
            {
                fileItemClient = ApprovalRequestService.client.ProjectInsightWebApiClient.FileItem;
            }
            else if (ItemType.Equals("Project"))  //Project
            {
                fileItemClient = DocumentsService.clientFiles;//.ProjectInsightWebApiClient.FileItem;
    }

          
            var result = await FilesFoldersService.UploadFile(fileItemClient, FileContent, FileName, FileName, ParentId.Value);

            if (result != null && !result.HasErrors)
            {
                MessagingCenter.Send(this, "RelatedItemsAdded", result.SavedId);
                await Navigation.PopAsync();
            }
            else if(result != null && result.HasErrors)
            {

                Common.Instance.ShowToastMessage("Error.." + result.Errors.FirstOrDefault(), DoubleResources.DangerSnackBar);
            }
        }

        private async Task<bool> CheckPermissionsAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        //await DisplayAlert("Xamarin.Forms Sample", "Storage permission is needed for file picking", "OK");
                        Common.Instance.ShowToastMessage("Storage permission is needed for file picking", DoubleResources.DangerSnackBar);

                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);

                    if (results.ContainsKey(Permission.Storage))
                    {
                        status = results[Permission.Storage];
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else if (status != PermissionStatus.Unknown)
                {
                    Common.Instance.ShowToastMessage("Storage permission was denied.", DoubleResources.DangerSnackBar);
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async void CreateFolder_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewFolderPage(ParentId.Value));
        }
    }
}