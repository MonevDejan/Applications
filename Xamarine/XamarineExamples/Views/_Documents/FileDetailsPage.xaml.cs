using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using SQLite;                            
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsight.Models.Base.Responses;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileDetailsPage : ContentPage
    {

        FileDetailsViewModel viewModel;

        public FileDetailsPage(Guid fileId)
        {
           //HockeyApp.MetricsManager.TrackEvent("FileDetailsPage Initialize");
            NavigationPage.SetBackButtonTitle(this, "");
            BindingContext = viewModel = new FileDetailsViewModel();
            InitializeComponent();

            GetObicen(fileId);
        }

        public FileDetailsPage()
        {
            InitializeComponent();
        }

        private async void GetObicen(Guid fileId)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetFile(fileId);

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
                viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Your session has expired!", DoubleResources.DangerSnackBar);
            }
        }

        private async Task<bool> GetFile(Guid fileId)
        {
            try
            {
                var file = await DocumentsService.GetFile(fileId);
                
                var item = new FileDetailsViewModel
                {
                    Id = fileId.ToString(),
                    Name = file.Name,
                    Title = file.Name,
                    Icon = Common.CurrentWorkspace.WorkspaceURL.ToLower().Replace("/projectinsight.webapp", "") + file.UrlIcon
                };

                if (!string.IsNullOrEmpty(file.UrlThumbnail))
                {
                    item.Icon = Common.CurrentWorkspace.WorkspaceURL + file.UrlThumbnail.ToLower().Replace("/projectinsight.webapp", "") + "?api-token=" + Common.CurrentWorkspace.ApiToken;
                }
                if (file.UserCreated.Id.Value.Equals(Common.CurrentWorkspace.UserID))
                    item.CanDelete = true;

                if (file.UserCreated != null)
                {
                    item.Created = file.UserCreated.FirstName + " " + file.UserCreated.LastName + " ";
                }
                if (file.CreatedDateTimeUTC != null)
                {
                    item.Created += file.CreatedDateTimeUTC.Value.ToString("ddd M/d/yy h:mmtt");
                }
                if (!string.IsNullOrEmpty(item.Created))
                    item.Created = "Created: " + item.Created;


                if (file.UserUpdated != null)
                {
                    item.Updated = file.UserUpdated.FirstName + " " + file.UserUpdated.LastName + " ";
                }
                if (file.UpdatedDateTimeUTC != null)
                {
                    item.Updated += file.UpdatedDateTimeUTC.Value.ToString("ddd M/d/yy h:mmtt");
                }
                if (!string.IsNullOrEmpty(item.Updated))
                    item.Updated = "Updated: " + item.Updated;


                viewModel = item;
                BindingContext = viewModel;
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }

        private async void OnDownload_Tapped(object sender, EventArgs e)
        {

           
                var resp = await DisplayActionSheet("Do you want to download the file?","Save","Cancel");
                if (resp != null && resp.ToString().Length > 0 && resp.Equals("Save"))
                {
                    viewModel.VisibleLoad = true;
                    var result = await DocumentsService.DownloadFile(viewModel);

                    if (!result)
                    {
                        Common.Instance.ShowToastMessage("Error downloading file!", DoubleResources.DangerSnackBar);
                    }
                    else
                    {
                        Common.Instance.ShowToastMessage("File downloaded", DoubleResources.SuccessSnackBar);
                    }

                    viewModel.VisibleLoad = false;
                }
        }

        private async void OnDelete_Tapped(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            try
            {
                var resp = await DisplayActionSheet("Are you sure you want to delete the file?", "No", "Yes");
                if (resp != null && resp.ToString().Length > 0 && resp.Equals("Yes"))
                {
                    Guid id = new Guid(viewModel.Id);
                    ApiDeleteResponse response =  DocumentsService.clientFiles.Delete(id);
                    if (response == null || response.HasErrors)
                    {
                        if (response == null)
                        {
                            Common.Instance.ShowToastMessage("An unexpected error is keeping you from deleting the file", DoubleResources.DangerSnackBar);
                        }
                        else if (response.HasErrors)
                        {
                            Common.Instance.ShowToastMessage(response.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                        }
                    }
                    else
                    {
                        MessagingCenter.Send(this, "FileDeleted", Guid.Empty);
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            viewModel.VisibleLoad = false;
        }
    }
}