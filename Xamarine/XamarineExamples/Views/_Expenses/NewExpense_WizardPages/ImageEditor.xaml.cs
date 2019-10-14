using Syncfusion.SfImageEditor.XForms;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using ProjectInsightMobile.Helpers;





/// <summary>
/// /https://www.syncfusion.com/kb/9368/how-to-load-image-from-camera-and-gallery
/// https://www.c-sharpcorner.com/article/store-images-on-app-directory-in-xamarin-ios/
/// </summary>
namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageEditor : ContentPage
    {
        bool isTakePhoto = false, isOpenGallery = false;
        ImageModel model;
        public ImageEditor()
        {
            model = new ImageModel();
            BindingContext = model;
            InitializeComponent();
        }

        public void SwitchView(Stream file)
        {
            //Navigation.PushModalAsync(new SfImageEditorPage() { Stream = file });
            PhotoImage.IsVisible = true;
            PhotoImage.Source = ImageSource.FromStream(() => file);

        }

        public void SwitchView(string file)
        {
            //Navigation.PushModalAsync(new SfImageEditorPage() { FileName = file });



            PhotoImage.IsVisible = true;


            string localPath = System.IO.Path.Combine(Common.Instance.PicturesPath, file);
            PhotoImage.Source = ImageSource.FromFile(localPath);

        }

        async void OpenGalleryTapped(object sender, System.EventArgs e)
        {
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                storageStatus = results[Permission.Storage];
            }

            if (storageStatus == PermissionStatus.Granted)
            {

                if (!isOpenGallery)
                {
                    //OpenGallery.Source = model.ChooseImage;
                    isOpenGallery = true;
                }
                else
                {
                   // OpenGallery.Source = model.ChooseImageSelected;
                    isOpenGallery = false;
                }
                if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Windows)
                    DependencyService.Get<IImageEditorDependencyService>().UploadFromGallery(this);
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to access gallery.", "OK");
            }
        }


        void ImageTapped(object sender, System.EventArgs e)
        {
            LoadFromStream((sender as Image).Source);
        }

        void LoadFromStream(ImageSource source)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Navigation.PushModalAsync((new SfImageEditorPage() { ImageSource = source }));
            }
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                Navigation.PushModalAsync((new SfImageEditorPage() { ImageSource = source }));
            }
            else
            {
                Navigation.PushModalAsync((new SfImageEditorPage() { ImageSource = source }));
            }
        }
        async void TakeAPhotoTapped(object sender, System.EventArgs e)
        {

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



                if (!isTakePhoto)
                {
                    TakePhoto.Source = model.TakePicSelected;
                    isTakePhoto = true;
                }
                else
                {
                    TakePhoto.Source = model.TakePic;
                    isTakePhoto = false;
                }
                if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS || Device.OS == TargetPlatform.Windows)
                    DependencyService.Get<IImageEditorDependencyService>().UploadFromCamera(this);
            }
            else
            {
                await DisplayAlert("Permissions Denied", "Unable to take photos.", "OK");
                //On iOS you may want to send your user to the settings screen.
                //CrossPermissions.Current.OpenAppSettings();
            }
        }
    }


    class ImageModel
    {
        public ImageSource TakePic { get; set; }
        public ImageSource TakePicSelected { get; set; }
        public ImageSource ChooseImage { get; set; }
        public ImageSource ChooseImageSelected { get; set; }
        public ImageSource BroweImage1 { get; set; }
        public ImageSource BroweImage2 { get; set; }
        public ImageSource BroweImage3 { get; set; }

        public ImageModel()
        {
            ChooseImage = ImageSource.FromResource("ProjectInsightMobile.Icons.Gallery_S.png", typeof(App).GetTypeInfo().Assembly);//GallerySelected
            TakePic = ImageSource.FromResource("ProjectInsightMobile.Icons.Take_Photo_W.png", typeof(App).GetTypeInfo().Assembly);
            ChooseImageSelected = ImageSource.FromResource("ProjectInsightMobile.Icons.Gallery_W.png", typeof(App).GetTypeInfo().Assembly);
            TakePicSelected = ImageSource.FromResource("ProjectInsightMobile.Icons.Take_Photo_S.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage1 = ImageSource.FromResource("ProjectInsightMobile.Icons.image2.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage2 = ImageSource.FromResource("ProjectInsightMobile.Icons.image3.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage3 = ImageSource.FromResource("ProjectInsightMobile.Icons.image4.png", typeof(App).GetTypeInfo().Assembly);
        }
    }
}