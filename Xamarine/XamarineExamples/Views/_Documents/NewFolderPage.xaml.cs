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
using ProjectInsight.Models.Folders;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewFolderPage : ContentPage
    {

        ProjectInsightMobile.ViewModels.BaseViewModel viewModel;
        Guid containerId;
        public NewFolderPage(Guid FolderId)
		{
            viewModel = new NewProjectViewModel();
            containerId = FolderId;
            viewModel.VisibleLoad = false;
            viewModel.LoadingMessage = "";
            NavigationPage.SetBackButtonTitle(this,"");
           //HockeyApp.MetricsManager.TrackEvent("NewFolderPage Initialize");
            InitializeComponent();
            BindingContext = viewModel;

        }
        public NewFolderPage()
        { 
            InitializeComponent();
        }
      





        private async void SaveFolder_Tapped(object sender, EventArgs e)
        {
            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            Folder folder = new Folder();
            folder.Name = txtName.Text;
            folder.ItemContainer_Id = containerId;
            ApiSaveResponse resp =  await DocumentsService.AddFolder(folder);

            if (!resp.HasErrors)
            {



                MessagingCenter.Send(this, "FolderAdded", Guid.Empty);

                //for (var counter = 1; counter < 2; counter++)
                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

                await Navigation.PopAsync();
            }
            else
            {
                Common.Instance.ShowToastMessage("Error creating new folder. Please try again", DoubleResources.DangerSnackBar);
            }
            viewModel.VisibleLoad = false;
        }
        private async  void CancelFolder_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}