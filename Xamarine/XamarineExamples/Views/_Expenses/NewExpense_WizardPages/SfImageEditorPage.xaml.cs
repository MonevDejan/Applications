using ProjectInsightMobile.Helpers;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SfImageEditorPage : ContentPage
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
                OpenImageEditor(_fileName);
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
                OpenImageEditor(_imageSource);
            }
        }
        private Stream _stream;
        public Stream Stream
        {
            get { return _stream; }
            set
            {
                _stream = value;
                OnPropertyChanged();
                OpenImageEditor(_stream);
            }
        }

        public SfImageEditorPage()
        {
            InitializeComponent();
        }
        void OpenImageEditor(string file)
        {


            // string localPath = System.IO.Path.Combine(Common.Instance.ImagesFilePath, file);
           //var fileName = "ImageEditor_Photo_20190116231716860.jpg";
            string localPath = System.IO.Path.Combine(Common.Instance.PicturesPath, file);
            editor.Source = ImageSource.FromFile(localPath);

        }
        void OpenImageEditor(Stream stream)
        {
            editor.Source = ImageSource.FromStream(() => stream);
        }
        void OpenImageEditor(ImageSource imageSource)
        {
            editor.Source = imageSource;
        }
    }

    public interface IImageEditorDependencyService
    {
        void UploadFromCamera(ImageEditor editor);

        void UploadFromGallery(ImageEditor editor);
    }

}