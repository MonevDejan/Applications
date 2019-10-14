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
	public partial class NewToDoPage : ContentPage
    {
      
        NewTaskViewModel viewModel;
        SettingsService settings = new SettingsService();

        public NewToDoPage(Guid? taskId = null)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           //HockeyApp.MetricsManager.TrackEvent("NewToDoPage Initialize");

            InitializeComponent();

            viewModel = new NewTaskViewModel();

            viewModel.StartDate = DateTime.Now.Date;
            viewModel.EndDate = DateTime.Now.Date;

            StartLoading(selectedTodoId: taskId);

            BindingContext = viewModel;

            if (Common.UserGlobalCapability != null && Common.UserGlobalCapability.CanCreateProjects)
                slCreateProject.IsVisible = true;
            else
                slCreateProject.IsVisible = false;

            if (taskId != null && taskId != Guid.Empty)
            {
                Title = "Edit ToDo";
                slCreateProject.IsVisible = false;
                cmbProject.IsEnabled = false;
            }

            if (Device.RuntimePlatform.ToLower() == "android")
            {
                //cmbProject.Margin = new Thickness(10,0,10,0);
                //cmbStartDate.Margin = new Thickness(10, 0, 10, 0);
                //cmbEndDate.Margin = new Thickness(10, 0, 10, 0);
                //cmbUser.Margin = new Thickness(10, 0, 10, 0);
                //txtName.Margin = new Thickness(10, 0, 10, 0);
                //txtDescription.Margin = new Thickness(10, 0, 10, 0);

            }
            else
            {
                
            }
        }
        private async void StartLoading(Guid? selectedProjectId = null, Guid? selectedTodoId = null)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetAllProjects(selectedProjectId);
             isSuccess = await GetAllUsers(selectedProjectId: selectedProjectId);
           


            if (selectedTodoId != null && selectedTodoId != Guid.Empty)
                isSuccess = await GetTodo(selectedTodoId.Value);
            else
            {
                if (selectedProjectId == null || selectedProjectId == Guid.Empty)
                {
                    var selProject = settings.Get("NewToDo_SelectedProject");
                    if (selProject != string.Empty)
                        viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == new Guid(selProject)).FirstOrDefault();
                }

                var selUser = settings.Get("NewToDo_SelectedUser");
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

                List<ProjectInsight.Models.Users.User> listofUsers = await UsersService.ListByProjectForTaskAssignment(selectedProjectId ?? Guid.Empty);

               
                ObservableCollection<ProjectInsight.Models.Users.User> users = new ObservableCollection<ProjectInsight.Models.Users.User>(listofUsers);
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

        private async Task<bool> GetTodo(Guid TodoId)
        {
            try
            {


                ProjectInsight.Models.ToDos.ToDo todo = await ToDoService.GetItem(TodoId);

                viewModel.Name = todo.Name;
                viewModel.Description = todo.Description;
                viewModel.StartDate = todo.StartDateTimeUTC != null ? todo.StartDateTimeUTC.Value : DateTime.Now;
                viewModel.EndDate = todo.EndDateTimeUTC != null ? todo.EndDateTimeUTC.Value : DateTime.Now;

                viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == todo.ProjectAffiliation_Id).FirstOrDefault();
                viewModel.SelectedUser = viewModel.Users.Where(x => x.Id == todo.UserAssignedTo_Id).FirstOrDefault();
                viewModel.Id = TodoId;

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

            //Create ToDo item
            ProjectInsight.Models.ToDos.ToDo toDo = new ProjectInsight.Models.ToDos.ToDo();
            toDo.Name = viewModel.Name;
            toDo.Description = viewModel.Description;
            toDo.StartDateTimeUTC = viewModel.StartDate;
            toDo.EndDateTimeUTC = viewModel.EndDate;
            toDo.UserCreated_Id = Common.CurrentWorkspace.UserID;
            toDo.CreatedDateTimeUTC = DateTime.Now.ToUniversalTime();
            toDo.UserAssignedBy_Id = Common.CurrentWorkspace.UserID;
            toDo.UserAssignedTo_Id = viewModel.SelectedUser != null ? viewModel.SelectedUser.Id : Common.CurrentWorkspace.UserID;
            if (viewModel.Id == null)
            {
                toDo.ItemContainer_Id = viewModel.SelectedProject.Id;
                toDo.ProjectAffiliation_Id = viewModel.SelectedProject.Id;
            }
            toDo.Id = viewModel.Id;

            ApiSaveResponse resp = await ToDoService.SaveTodo(toDo);

            if (!resp.HasErrors)
            {
                Common.RefreshWorkList = true;
                Common.RefreshProjectsList = true;
                Common.Instance.ShowToastMessage("ToDo Saved", DoubleResources.SuccessSnackBar);
                var succsave = settings.Set("NewToDo_SelectedProject", viewModel.SelectedProject.Id.ToString());
                succsave = settings.Set("NewToDo_SelectedUser", toDo.UserAssignedTo_Id.ToString());
                MessagingCenter.Send(this, "TodoAdded", resp.SavedId);
                if (viewModel.Id != null)
                {
                    MessagingCenter.Send(this, "TodoUpdated", resp.SavedId);
                    await Navigation.PopAsync();
                }
                else
                    await Navigation.PopToRootAsync();

            }
            else
            {
                if (resp.Errors.Count > 0)
                {
                    Common.Instance.ShowToastMessage(resp.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                }
                else
                    Common.Instance.ShowToastMessage("Error,check again.", DoubleResources.DangerSnackBar);
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

            await Navigation.PushAsync(new NewProjectPage(returnToPreviousPage: true));
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