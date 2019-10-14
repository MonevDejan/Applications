using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Services;
using System.Collections.ObjectModel;
using static ProjectInsightMobile.ViewModels.NewProjectViewModel;
using System.Linq;
using ProjectInsight.Models.Base.Responses;
using Plugin.FilePicker;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewDocumentPage : ContentPage
    {
      
        public static readonly string TAG_NO_SELECTION = " -- No Selection -- ";
        NewProjectViewModel viewModel;
        Guid folderId;
        public NewDocumentPage(Guid containerId)
		{
            folderId = containerId;
            NavigationPage.SetBackButtonTitle(this,"");
           //HockeyApp.MetricsManager.TrackEvent("NewItemPage Initialize");

            InitializeComponent();

            viewModel = new NewProjectViewModel();

            viewModel.StartDate = DateTime.Now.Date;

            StartLoading();
            BindingContext = viewModel;

            if (Device.RuntimePlatform.ToLower() == "android")
            {
             

            }
            else
            {
                
            }
        }
        public NewDocumentPage()
        { 
            InitializeComponent();
        }
        private async void StartLoading()
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            //viewModel.Statuses = await ProjectStatusService.GetAllStatuses();
            //viewModel.Types = await ProjectTypeService.GetAllTypes();

            bool isSuccess = await GetProjectTypes();
            isSuccess = await GetProjectStatuses();
            isSuccess = GetAllColors();

            if (viewModel != null)
            {
                BindingContext = viewModel;
            }


            viewModel.VisibleLoad = false;
            //if (isSuccess)
            //{
            //    viewModel.LoadingMessage = "Success";
            //}
            //else
            //{
            //    viewModel.VisibleLoad = false;
            //    Common.Instance.ShowToastMessage("Error communication with server!",DoubleResources.DangerSnackBar);
            //}
        }

        private async Task<bool> GetProjectTypes()
        {
            try
            {

                var obsTypes= await ProjectTypeService.GetAllTypes();
                ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectType> types = new ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectType>(obsTypes);
                viewModel.Types = types;
                viewModel.SelectedType = obsTypes.FirstOrDefault();
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }

        private async Task<bool> GetProjectStatuses()
        {
            try
            {
                var obsStatuses = await ProjectStatusService.GetAllStatuses();
                ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectStatus> statuses= new ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectStatus>(obsStatuses);
                viewModel.Statuses = statuses;
                viewModel.SelectedStatus = obsStatuses.FirstOrDefault();
            }
            catch (Exception ex)
            {
               // AuthenticationService.Logout();
                return false;
            }
            return true;
        }

        private bool GetAllColors()
        {
            //this is temporary, until API method is developed
            string sColors = "#1abc9c,#2ecc71,#3498db,#9b59b6,#344955e,#fc5c65,#fd9644,#fed330,#26de81,#2BCBBA,#16a085,#27ae60,#2980b9,#8e44ad,#2c3e50,#eb3b5a,#fa8231,#f7b731,#20bf6b,#0fb9b1,#f1c40f,#e67e22,#e74c3c,#ecf0f1,#95a5a6,#45aaf2,#4b7bec,#a55eea,#d1d8e0,#778ca3,#f39c12,#d35400,#c0392b,#bdc3c7,#7f8c8d,#2d98da,#3867d6,#8854d0,#a5b1c2,#4b6584";
            string[] colors = sColors.Split(',');


            ObservableCollection<myColor> col = new ObservableCollection<myColor>();
            foreach (var color in colors)
            {
                col.Add(new myColor() {Id=color, Name = color });
            }
            viewModel.Colors = col;
            viewModel.SelectedColor = col.FirstOrDefault();
            return true;
        }
      

        private async void OnCancelAddTimeEntry(object sender,DateChangedEventArgs e)
        {
            MessagingCenter.Send(this, "ProjectAdded", Guid.Empty);
            await Navigation.PopAsync();
        }

        private async void AddFolder_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewFolderPage(folderId));

        }

       
        private async void AddDocument_Tapped(object sender, EventArgs e)
        {
            try
            {
                var myResult = await Plugin.FilePicker.CrossFilePicker.Current.PickFile();


                viewModel.VisibleLoad = true;
                viewModel.LoadingMessage = "";
                if (myResult != null && !string.IsNullOrEmpty(myResult.FileName))
                {
                    string FileContent = Convert.ToBase64String(myResult.DataArray);

                    var result = await DocumentsService.UploadFile(FileContent, myResult.FileName, folderId);
                    if (!result.HasErrors)
                    {
                        Common.Instance.ShowToastMessage("Success", DoubleResources.SuccessSnackBar);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        Common.Instance.ShowToastMessage("Error.." + result.Errors.FirstOrDefault(), DoubleResources.DangerSnackBar);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Instance.ShowToastMessage(ex.Message, DoubleResources.DangerSnackBar);

            }
            viewModel.VisibleLoad = false;
            //var crossFilePicker = CrossFilePicker.Current;
            //var myResult = await crossFilePicker.PickFile();
            //if (myResult != null && !string.IsNullOrEmpty(myResult.FilePath))
            //{
            //    string FileContent = Convert.ToBase64String(myResult.DataArray);

            //    var result = await DocumentsService.UploadFile(FileContent, myResult.FileName, folderId);
            //    if (!result.HasErrors)
            //    {
            //        Common.Instance.ShowToastMessage("Success", DoubleResources.SuccessSnackBar);
            //        await Navigation.PopAsync();
            //    }
            //    else
            //    {
            //        Common.Instance.ShowToastMessage("Error.." + result.Errors.FirstOrDefault(), DoubleResources.DangerSnackBar);
            //    }

            //}
        }
    }
}