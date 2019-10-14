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
using System.Collections.Generic;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewProjectPage : ContentPage
    {
      
        
        NewProjectViewModel viewModel;
        bool ReturnToPreviousPage = false;
        public NewProjectPage(bool returnToPreviousPage = false)
		{
            ReturnToPreviousPage = returnToPreviousPage;
            NavigationPage.SetBackButtonTitle(this,"");
           //HockeyApp.MetricsManager.TrackEvent("NewProjectPage Initialize");

            InitializeComponent();

            viewModel = new NewProjectViewModel();

            viewModel.StartDate = DateTime.Now.Date;

            StartLoading();
            BindingContext = viewModel;

        }
        private async void StartLoading()
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            //viewModel.Statuses = await ProjectStatusService.GetAllStatuses();
            //viewModel.Types = await ProjectTypeService.GetAllTypes();

            bool isSuccess = await GetProjectTypes();
            isSuccess = await GetProjectStatuses();
            //isSuccess = GetAllColors();

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
                //viewModel.SelectedType = obsTypes.FirstOrDefault();
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
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }

        private bool GetAllColors()
        {
            //this is temporary, until API method is developed
            //string sColors = "#1abc9c,#2ecc71,#3498db,#9b59b6,#344955e,#fc5c65,#fd9644,#fed330,#26de81,#2BCBBA,#16a085,#27ae60,#2980b9,#8e44ad,#2c3e50,#eb3b5a,#fa8231,#f7b731,#20bf6b,#0fb9b1,#f1c40f,#e67e22,#e74c3c,#ecf0f1,#95a5a6,#45aaf2,#4b7bec,#a55eea,#d1d8e0,#778ca3,#f39c12,#d35400,#c0392b,#bdc3c7,#7f8c8d,#2d98da,#3867d6,#8854d0,#a5b1c2,#4b6584";
            //string[] colors = sColors.Split(',');


            //ObservableCollection<myColor> col = new ObservableCollection<myColor>();
            //foreach (var color in colors)
            //{
            //    col.Add(new myColor() {Id=color, Name = color });
            //}
            ObservableCollection<myColor> col = new ObservableCollection<myColor>();
            col.Add(new myColor() { Id = "#e74c3c", Name = "Red" });
            col.Add(new myColor() { Id = "#e67e22", Name = "Orange" });
            col.Add(new myColor() { Id = "#f1c40f", Name = "Yellow" });
            col.Add(new myColor() { Id = "#2ecc71", Name = "Green" });
            col.Add(new myColor() { Id = "#3498db", Name = "Blue" });
            col.Add(new myColor() { Id = "#34495e", Name = "Navy" });


            viewModel.Colors = col;
            viewModel.SelectedColor = col.FirstOrDefault();
            return true;
        }
        private async void OnSaveAddTimeEntry(object sender, EventArgs e)
        {

            //selectiraniot Task e 
            //var selTask = viewModel.SelectedTask;
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                viewModel.NameError = true;
                //lblName.TextColor = Color.DarkRed;
                return;
            }
            else
            {
                //lblName.TextColor = (Color)Application.Current.Resources["DarkGrayTextColor"];
                viewModel.NameError = false;
            }


            ProjectInsight.Models.Projects.Project project = new ProjectInsight.Models.Projects.Project();
            project.Name = viewModel.Name;
            project.Description = viewModel.Description;
            project.ScheduleStartDate = viewModel.StartDate;
            if (viewModel.SelectedType != null)
                project.ProjectType_Id = viewModel.SelectedType.Id;
            project.ProjectStatus_Id = viewModel.SelectedStatus.Id;
            project.UserCreated_Id = Common.CurrentWorkspace.UserID;
            project.CreatedDateTimeUTC = DateTime.Now.ToUniversalTime();
            
            //TODO we need to create project without itemcontainer_id
            //project.ItemContainer_Id = new Guid("49eeed111c7b45cc8bd6347c54b7f1f5");

            //List<ProjectInsight.Models.Folders.Folder> folders = DocumentsService.GetFoldersByContainer(parentId);
            //Folder docFolder = folders.Where(x => x.Name == "Documents").FirstOrDefault();
            //if (docFolder != null)
            //{
            //    parentId = docFolder.Id.Value;
            //}



            //project.Color = viewModel.SelectedColor;

            ApiSaveResponse resp = await ProjectsService.Save(project);

            if (!resp.HasErrors)
            {
                Common.RefreshProjectsList = true;
                Common.Instance.ShowToastMessage("Project Saved", DoubleResources.SuccessSnackBar);
                MessagingCenter.Send(this, "ProjectAdded", resp.SavedId);
                if (ReturnToPreviousPage)
                    await Navigation.PopAsync();
                else
                    await Navigation.PopToRootAsync();
        }
            else
            {
                if (resp.Errors != null && resp.Errors.Count > 0)
                {
                    Common.Instance.ShowToastMessage(resp.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                }
                else
                    Common.Instance.ShowToastMessage("Error,check again.", DoubleResources.DangerSnackBar);
            }
        }

        private async void OnCancelAddTimeEntry(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "ProjectAdded", Guid.Empty);
            await Navigation.PopAsync();
        }
        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.NewTextValue))
                viewModel.NameError = false;

        }
        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbColor.SelectedIndex == -1)
            {
                boxColor.Color = Color.Default;
            }
            else
            {
              //string colorName = cmbColor.Items[cmbColor.SelectedIndex];
                boxColor.Color = Color.FromHex(viewModel.SelectedColor.Id);
            }
        }

    }
}