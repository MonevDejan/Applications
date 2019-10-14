using Plugin.FilePicker;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_UploadFile : ContentPage
    {
      
        NewApprovalViewModel viewModel;


        public NewApprovalPage_UploadFile()
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();


            BindingContext = viewModel = new NewApprovalViewModel();
           
        }

        public async void FileUpload_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android &&
                  !await this.CheckPermissionsAsync())
                {
                    return;
                }


                var crossFilePicker = CrossFilePicker.Current;
                var myResult = await crossFilePicker.PickFile();
                if (myResult != null && !string.IsNullOrEmpty(myResult.FileName))
                {
                    string FileContent = Convert.ToBase64String(myResult.DataArray);
                    //create new file item
                    ProjectInsight.Models.Files.FileItem myFile = new ProjectInsight.Models.Files.FileItem();
                    //myFile.ItemContainer_Id = id;
                    myFile.Name = myResult.FileName;

                    //create FileUpload object for file item
                    ProjectInsight.Models.Files.FileUpload fileUpload = new ProjectInsight.Models.Files.FileUpload();
                    fileUpload.FileName = myResult.FileName;
                    fileUpload.FileContentsBase64Encoded = FileContent; //Content of the file to upload
                    myFile.UploadedFile = fileUpload;

                    //Save and return a response
                    ProjectInsight.Models.Base.Responses.ApiSaveResponse saveFileResp = FileItemService.client.Save(myFile);


                    if (saveFileResp != null && !saveFileResp.HasErrors)
                    {
                        await Navigation.PushAsync(new NewApprovalPage_Details(container_Id: saveFileResp.SavedId));

                    }
                    else if(saveFileResp != null && saveFileResp.HasErrors)
                    {
                        Common.Instance.ShowToastMessage(saveFileResp.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                }
                else
                {

                    //Common.Instance.ShowToastMessage("Error with file!", DoubleResources.DangerSnackBar);
                }

            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage(ex.ToString(), DoubleResources.DangerSnackBar);
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
    }
}