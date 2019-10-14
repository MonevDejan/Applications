using Newtonsoft.Json.Linq;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TakePhoto : ContentPage
    {

        ExpenseEntryViewModel viewModel;
        public TakePhoto(ExpenseEntryViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            InitializeComponent();
            viewModel = ViewModel;
            BindingContext = viewModel;
            if (viewModel.SelectedExpenseType == 1)
                Title = "Add Receipt";
            else if (viewModel.SelectedExpenseType == 2)
                Title = "Add Mileage";
            else if (viewModel.SelectedExpenseType == 3)
                Title = "Add Other";
            else
                Title = "Add";
        }

        private async void Miles_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewApprovalPage_UploadFile());
        }

       
        private async void CameraButton_Clicked(object sender, EventArgs e)
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
                        CompressionQuality=50
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

                        PhotoImage.IsVisible = true;


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
                        viewModel.PhotoArray = imageAsBytes;
                      


                        ParseReceipt(imageAsBytes);

                        PhotoImage.Source = photoSource;
                        viewModel.PhotoSource = photoSource;

                        btnTakePhoto.IsVisible = false;
                        btnFindPhoto.IsVisible = false;
                        lblSkip.Text = "Continue";
                        btnAntoher.IsVisible = true;
                    }
                }
                else
                {
                    await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                    //On iOS you may want to send your user to the settings screen.
                    //CrossPermissions.Current.OpenAppSettings();
                }
            }
            catch(Exception ex)
            {
                Common.Instance.ShowToastMessage(ex.Message, DoubleResources.DangerSnackBar);
            }
            viewModel.VisibleLoad = false;

        }

        private async void SkipButton_Tapped(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new Amount(viewModel));

        }

        private async void SlChooseFromLibrary_Tapped(object sender, EventArgs e)
        {
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

                ParseReceipt(imageAsBytes);

                viewModel.PhotoArray = imageAsBytes;

                viewModel.PhotoName = imageResult.FileName;


                ImageSource photoSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                PhotoImage.Source = photoSource;


                ImageSource photoSource2 = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                viewModel.PhotoSource = photoSource2;


                //viewModel.Photo = new MemoryStream(imageAsBytes);

                PhotoImage.IsVisible = true;
                btnTakePhoto.IsVisible = false;
                btnFindPhoto.IsVisible = false;

                lblSkip.Text = "Continue";
                btnAntoher.IsVisible = true;
            }
            else
            {
            }
            viewModel.VisibleLoad = false;
        }

        private void ParseReceipt(byte[] imageAsBytes)
        {
            //OCR

            try
            {
                var taggunApiKey = "c2fe32e021bf11e9bba4c5572eb43161";
                var taggunApiUrl = "https://api.taggun.io/api/receipt/v1/simple/file";
                using (var httpClient = new HttpClient { Timeout = new TimeSpan(0, 0, 0, 60, 0) })
                {
                    HttpResponseMessage response = null;

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", taggunApiKey);

                    var parentContent = new MultipartFormDataContent("----WebKitFormBoundaryfzdR3Imh7urK8qw");

                    var documentContent = new ByteArrayContent(imageAsBytes);
                    documentContent.Headers.Remove("Content-Type");
                    documentContent.Headers.Remove("Content-Disposition");
                    documentContent.Headers.TryAddWithoutValidation("Content-Type", "image/jpeg");
                    documentContent.Headers.TryAddWithoutValidation("Content-Disposition",
                    string.Format(@"form-data; name=""file""; filename=""{0}""", "test_receipt.jpg"));
                    parentContent.Add(documentContent);

                    var refreshContent = new StringContent("false");
                    refreshContent.Headers.Remove("Content-Type");
                    refreshContent.Headers.Remove("Content-Disposition");
                    refreshContent.Headers.TryAddWithoutValidation("Content-Disposition", @"form-data; name=""refresh""");
                    parentContent.Add(refreshContent);

                    response = httpClient.PostAsync(taggunApiUrl, parentContent).Result;
                    response.EnsureSuccessStatusCode();
                    var result = response.Content.ReadAsStringAsync().Result;

                    if (result != null)
                    {
                        var resParsed = JObject.Parse(result);

                        if (resParsed["totalAmount"] != null)
                        {
                            if (resParsed["totalAmount"]["confidenceLevel"] != null)
                            {
                                var confidenceLevelTotalAmount = (decimal)(resParsed["totalAmount"]["confidenceLevel"]);
                                if (confidenceLevelTotalAmount > 0.5m && resParsed["totalAmount"]["data"] != null)
                                {
                                    viewModel.Amount = (decimal)(resParsed["totalAmount"]["data"]);
                                }
                            }
                        }

                        if (resParsed["merchantName"] != null)
                        {
                            if (resParsed["merchantName"]["confidenceLevel"] != null)
                            {
                                var confidenceLevelMerchantName = (decimal)(resParsed["merchantName"]["confidenceLevel"]);
                                if (confidenceLevelMerchantName > 0.5m && resParsed["merchantName"]["data"] != null)
                                {
                                    viewModel.Description = resParsed["merchantName"]["data"].ToString();
                                }
                            }
                        }

                        if (resParsed["date"] != null)
                        {
                            if (resParsed["date"]["confidenceLevel"] != null)
                            {
                                var confidenceLevelDate = (decimal)(resParsed["date"]["confidenceLevel"]);
                                if (confidenceLevelDate > 0.5m && resParsed["date"]["data"] != null)
                                {
                                    viewModel.Date = (DateTime)(resParsed["date"]["data"]);
                                }
                            }
                        }
                        // Console.WriteLine(JObject.Parse(result).ToString());
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }


        private void slAnotherImage_Tapped(object sender, EventArgs e)
        {
           //viewModel = new ExpenseEntryViewModel();
            viewModel.PhotoArray = null;
            viewModel.PhotoSource = null;
            

            PhotoImage.IsVisible = false;
            btnTakePhoto.IsVisible = true;
            btnFindPhoto.IsVisible = true;
            btnAntoher.IsVisible = false;

            lblSkip.Text = "Skip";

        }
    }
}