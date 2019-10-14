using ProjectInsight.Models.Items;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.RelatedItems
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelatedItemDetailsPage : ContentPage
    {
        RelatedItem relatedItem;
        string ItemType;
        public RelatedItemsViewModel viewModel;

        public RelatedItemDetailsPage(RelatedItem relatedItem, string ItemType, RelatedItemsViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.RelatedItem = this.relatedItem = relatedItem;
            this.ItemType = ItemType;

            InitializeComponent();
            if (relatedItem.UrlThumbnailFull.Contains(".svg"))
            {
                svgImg.IsVisible = true;
                svgImg.Source = relatedItem.UrlIconFull;
            }
            else
            {
                Img.IsVisible = true;
                Img.Source = relatedItem.UrlThumbnailFull;
            }

            //User user = Common.Instance.GetActiveUser();
            if (relatedItem.UserCreated.Id.Value.Equals(Common.CurrentWorkspace.UserID))
                deleteContent.IsVisible = true;
            BindingContext = this.viewModel = viewModel;
        }

        public async void OnDeleteRelatedItem(object sender, EventArgs e)
        {                                     
            await Navigation.PopToRootAsync();
        }

        public async void OnDownload(object sender, EventArgs e)
        {
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


            if (fileItemClient != null)
            {
                var answereSelect = await DisplayActionSheet("Are you want to save file:", "Save", null, relatedItem.Name);
                if(answereSelect != null && answereSelect.ToString().Length > 0 && answereSelect.Equals("Save"))
                {
                    viewModel.VisibleLoad = true;
                    var result = await FilesFoldersService.DownloadFile(fileItemClient, relatedItem.Id);

                    if (!result)
                    {
                        Common.Instance.ShowToastMessage("Error with downloading file!", DoubleResources.DangerSnackBar);
                    }
                    else
                    {
                        Common.Instance.ShowToastMessage("Success.", DoubleResources.SuccessSnackBar);
                    }

                    viewModel.VisibleLoad = false;
                }
                
            }
        }
    }

}