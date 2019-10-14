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
	public partial class NewTaskPage : ContentPage
    {
      
        NewTaskViewModel viewModel;

        SettingsService settings = new SettingsService();
        Guid pId = Guid.Empty;

        public NewTaskPage(Guid? taskId = null, Guid? parentId = null)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           //HockeyApp.MetricsManager.TrackEvent("NewTaskPage Initialize");
            if (parentId.HasValue) pId = parentId.Value;

            InitializeComponent();

            viewModel = new NewTaskViewModel();

            viewModel.StartDate = DateTime.Now.Date;
            viewModel.EndDate = DateTime.Now.Date;

            StartLoading(selectedTaskId:taskId, selectedProjectId:parentId);

            BindingContext = viewModel;

            if (Common.UserGlobalCapability != null && Common.UserGlobalCapability.CanCreateProjects)
                slCreateProject.IsVisible = true;
            else
                slCreateProject.IsVisible = false;

            if (taskId != null && taskId != Guid.Empty)
            {
                Title = "Edit Task";
                slCreateProject.IsVisible = false;
                cmbProject.IsEnabled = false;
            }


            if (Device.RuntimePlatform.ToLower() == "android")
            {
                //cmbUser.MaximumDropDownHeight = 200;
              //  cmbProject.Margin = new Thickness(10,0,10,0);
              //  cmbStartDate.Margin = new Thickness(10, 0, 10, 0);
              //  cmbEndDate.Margin = new Thickness(10, 0, 10, 0);
              //   cmbUser.Margin = new Thickness(10, 0, 10, 0);
              //  txtName.Margin = new Thickness(10, 0, 10, 0);
              //  txtDescription.Margin = new Thickness(10, 0, 10, 0);
            }
            else
            {
                
            }
        }
        private async void StartLoading(Guid? selectedProjectId = null, Guid? selectedTaskId = null)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetAllProjects(selectedProjectId);
            isSuccess = await GetAllUsers(selectedProjectId: selectedProjectId);
          
           

             

            if (selectedTaskId != null && selectedTaskId != Guid.Empty)
                isSuccess = await GetTask(selectedTaskId.Value);
            else
            {
                if (selectedProjectId == null || selectedProjectId == Guid.Empty)
                {
                    var selProject = settings.Get("NewTask_SelectedProject");
                    if (selProject != string.Empty)
                        viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == new Guid(selProject)).FirstOrDefault();
                }
                else
                {
                    viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == selectedProjectId).FirstOrDefault();
                    slCreateProject.IsVisible = false;
                    viewModel.SelectProjectEnabled = false;
                }
                var selUser = settings.Get("NewTask_SelectedUser");
                if (selUser != string.Empty)
                    viewModel.SelectedUser = viewModel.Users.Where(x => x.Id == new Guid(selUser)).FirstOrDefault();
            }



            if (viewModel != null)
            {
                BindingContext = viewModel;
            }


            viewModel.VisibleLoad = false;
          
        }

        private async Task<bool> GetAllProjects(Guid? selectedProjectId = null)
        {
            try
            {
                if (selectedProjectId != null)
                {
                    viewModel.Projects = null;
                    viewModel.SelectedProject = null;
                }


                    var obsProjects = await ProjectsService.GetAllProjectsActiveAndPlanningUserCanView();
                ObservableCollection<ProjectInsight.Models.Projects.Project> projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(obsProjects);
                //viewModel.Projects = createInitElem(projects, new ProjectInsight.Models.Projects.Project());
                viewModel.Projects = projects;

                if (selectedProjectId != null)
                    viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == selectedProjectId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }

        private async Task<bool> GetAllUsers(Guid? selectedUserId = null, Guid ? selectedProjectId = null)
        {
            try
            {
                if (selectedUserId != null)
                {
                    viewModel.Users = null;
                    viewModel.SelectedUser = null;
                }

                // var listofUsers = await UsersService.ListByProjectForTaskAssignment(new Guid("a6cb5efa-4d1b-4c80-8deb-0981ada81826"));
                List<ProjectInsight.Models.Users.User> listofUsers = await UsersService.ListByProjectForTaskAssignment(selectedProjectId ?? Guid.Empty);

               
                ObservableCollection<ProjectInsight.Models.Users.User> users = new ObservableCollection<ProjectInsight.Models.Users.User>(listofUsers);
                // viewModel.Users = createInitElem(users, new ProjectInsight.Models.Users.User());
                viewModel.Users = users;

                if (selectedUserId != null)
                    viewModel.SelectedUser= viewModel.Users.Where(x => x.Id == selectedUserId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }
        private async Task<bool> GetTask(Guid TaskId )
        {
            try
            {
               

                ProjectInsight.Models.Tasks.Task task = await TasksService.GetTask(TaskId);

                viewModel.Name = task.Name;
                viewModel.Description = task.Description;
                viewModel.StartDate = task.StartDateTimeUTC.Value;
                viewModel.EndDate = task.EndDateTimeUTC.Value;

                viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == task.Project_Id).FirstOrDefault();
                viewModel.SelectedUser= viewModel.Users.Where(x => x.Id == task.TaskOwner_Id).FirstOrDefault();
                viewModel.Id = TaskId;

            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }
        private async void OnSaveAddTimeEntry(object sender, EventArgs e)
        {

            //selectiraniot Task e 
            //var selTask = viewModel.SelectedTask;
            bool validationError = false;
            if (string.IsNullOrEmpty(viewModel.Name))
            {

                viewModel.NameError = true;
                validationError = true;
            }
            else
            {
                //lblName.TextColor = (Color)Application.Current.Resources["DarkGrayTextColor"];
                viewModel.NameError = false;

            }
            if (viewModel.SelectedProject == null)
            {
                viewModel.ProjectError = true;
                frmProject.BorderColor = (Color)Application.Current.Resources["RedTextColor"]; ;
                cmbProject.WatermarkColor = (Color)Application.Current.Resources["RedTextColor"]; ;
                validationError = true;
            }
            else
            {
                frmProject.BorderColor = (Color)Application.Current.Resources["LightGrayTextColor"]; ;
                cmbProject.WatermarkColor = (Color)Application.Current.Resources["LightGrayTextColor"]; ;
                viewModel.ProjectError = false;
            }

            if (validationError) return;

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";



            ProjectInsight.Models.Tasks.Task task = new ProjectInsight.Models.Tasks.Task();
            task.Name = viewModel.Name;
            task.Description = viewModel.Description;
            task.Project_Id = viewModel.SelectedProject.Id;
            task.UserCreated_Id = Common.CurrentWorkspace.UserID;
            task.CreatedDateTimeUTC = DateTime.Now.ToUniversalTime();
            task.UserAssignedBy_Id = Common.CurrentWorkspace.UserID;
            
            task.TaskOwner_Id = viewModel.SelectedUser != null ? viewModel.SelectedUser.Id : Common.CurrentWorkspace.UserID;
            //task.SkipProjectScheduleCalculateOnSave = false;
            //task.ConstraintType = 3;


            task.StartDateTimeUTC = viewModel.StartDate;
            task.EndDateTimeUTC = viewModel.EndDate;
           // task.StartDateTimeUserLocal = viewModel.StartDate;
          //  task.EndDateTimeUserLocal = viewModel.EndDate;
            task.Id = viewModel.Id;



            ApiSaveResponse resp = await TasksService.Save(task);


           
            
            
            if (resp != null &&!resp.HasErrors)
            {
                Common.RefreshWorkList = true;
                Common.RefreshProjectsList = true;

                var succsave = settings.Set("NewTask_SelectedProject", viewModel.SelectedProject.Id.ToString());
                succsave = settings.Set("NewTask_SelectedUser", task.TaskOwner_Id.ToString());

                Common.Instance.ShowToastMessage("Task Saved", DoubleResources.SuccessSnackBar);
                MessagingCenter.Send(this, "TaskAdded", resp.SavedId);
                if (viewModel.Id != null || pId != Guid.Empty)
                {
                    MessagingCenter.Send(this, "TaskUpdated", resp.SavedId);
                    for (var counter = 1; counter < 2; counter++)
                    {
                        Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                    }
                    await Navigation.PopAsync();
                }
                else
                    await Navigation.PopToRootAsync();



            }
            else
            {
                if (resp != null && resp.Errors.Count > 0)
                {
                    Common.Instance.ShowToastMessage(resp.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                }
                else
                    Common.Instance.ShowToastMessage("Error, check again.", DoubleResources.DangerSnackBar);
            }

            viewModel.VisibleLoad = false;


        }

        private async void OnCancelAddTimeEntry(object sender, EventArgs e)
        {

            await Navigation.PopAsync();
        }

        private async void OnCreateProject_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<NewProjectPage, Guid>(this, "ProjectAdded", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    StartLoading(itemId);

                MessagingCenter.Unsubscribe<NewProjectPage, Guid>(this, "ProjectAdded");
            });

            await Navigation.PushAsync(new NewProjectPage(returnToPreviousPage:true));
        }


        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.NewTextValue))
                viewModel.NameError = false;

        }
        private void CmbProject_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            //if (viewModel.SelectedProject != null)
            //    viewModel.ProjectError = false;


            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
            {
                viewModel.SelectedProject = null;
                viewModel.ProjectError = false;
                frmProject.BorderColor = (Color)Application.Current.Resources["RedTextColor"]; ;
                cmbProject.WatermarkColor = (Color)Application.Current.Resources["RedTextColor"]; ;
            }
            else
            {
                frmProject.BorderColor = (Color)Application.Current.Resources["LightGrayTextColor"]; ;
                cmbProject.WatermarkColor = (Color)Application.Current.Resources["LightGrayTextColor"]; ;
            }

        }

        private void CmbUser_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
            {
                viewModel.SelectedUser = null;
            }

        }
    }
}