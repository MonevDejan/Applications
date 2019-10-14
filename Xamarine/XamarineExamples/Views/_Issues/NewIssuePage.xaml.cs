using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Services;
using System.Collections.ObjectModel;
using static ProjectInsightMobile.ViewModels.NewProjectViewModel;
using System.Linq;
using ProjectInsight.Models.Base.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewIssuePage : ContentPage
    {

        NewIssueViewModel viewModel;

        SettingsService settings = new SettingsService();
        Guid pId = Guid.Empty;

        public NewIssuePage(Guid? itemId = null, Guid? parentId = null)
		{
            NavigationPage.SetBackButtonTitle(this,"");
           //HockeyApp.MetricsManager.TrackEvent("NewTaskPage Initialize");
            if (parentId.HasValue) pId = parentId.Value;


            viewModel = new NewIssueViewModel();


            if (Common.WorkspaceCapability?.EnableIssueScheduling != null)
                viewModel.EnableIssueScheduling = Common.WorkspaceCapability.EnableIssueScheduling;
            else
                viewModel.EnableIssueScheduling = false;

            viewModel.StartDate = DateTime.Now.Date;
            viewModel.EndDate = DateTime.Now.Date;


            InitializeComponent();

            

            StartLoading(selectedItemId: itemId, selectedProjectId:parentId);

            BindingContext = viewModel;

            if (Common.UserGlobalCapability != null && Common.UserGlobalCapability.CanCreateProjects)
                slCreateProject.IsVisible = true;
            else
                slCreateProject.IsVisible = false;

            if (itemId != null && itemId != Guid.Empty)
            {
                Title = "Edit Issue";
                slCreateProject.IsVisible = false;
                //cmbProject.IsEnabled = false;
            }


        }
        private async void StartLoading(Guid? selectedProjectId = null, Guid? selectedItemId = null)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetAllProjects(selectedProjectId);
            isSuccess = await GetAllIssueStatusTypes();
            isSuccess = await GetAllIssueTypes();
            isSuccess = await GetAllIssuePriorityTypes();
            isSuccess = await GetAllIssueSeverityTypes();
            isSuccess = await GetAllUsers(selectedProjectId: selectedProjectId);
          
           

             

            if (selectedItemId != null && selectedItemId != Guid.Empty)
                isSuccess = await GetItem(selectedItemId.Value);
            else
            {
                if (selectedProjectId == null || selectedProjectId == Guid.Empty)
                {
                    var selProject = settings.Get("NewIssue_SelectedProject");
                    if (selProject != string.Empty)
                        viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == new Guid(selProject)).FirstOrDefault();
                }
                else
                {
                    viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == selectedProjectId).FirstOrDefault();
                    slCreateProject.IsVisible = false;
                    viewModel.SelectProjectEnabled = false;
                }
                var selUser = settings.Get("NewIssue_SelectedUser");
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

        private async Task<bool> GetAllIssueStatusTypes(Guid? selectedIssueStatusTypeId = null)
        {
            try
            {

                viewModel.IssueStatusTypes = null;
                if (selectedIssueStatusTypeId != null)
                    viewModel.SelectedIssueStatusType = null;

                viewModel.IssueStatusTypes = await IssueStatusTypeService.client.ListActiveAsync(new ProjectInsight.Models.Base.ModelProperties("default"));

                if (selectedIssueStatusTypeId != null)
                    viewModel.SelectedIssueStatusType = viewModel.IssueStatusTypes.Where(x => x.Id == selectedIssueStatusTypeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }

        private async Task<bool> GetAllIssueTypes(Guid? selectedIssueTypeId = null)
        {
            try
            {

                viewModel.IssueTypes = null;
                if (selectedIssueTypeId != null)
                    viewModel.SelectedIssueType = null;

                viewModel.IssueTypes = await IssueTypeService.client.ListActiveAsync(new ProjectInsight.Models.Base.ModelProperties("default"));

                if (selectedIssueTypeId != null)
                    viewModel.SelectedIssueType = viewModel.IssueTypes.Where(x => x.Id == selectedIssueTypeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }

        private async Task<bool> GetAllIssueSeverityTypes(Guid? selectedIssueSeverityTypeId = null)
        {
            try
            {

                viewModel.IssueSeverityTypes = null;
                if (selectedIssueSeverityTypeId != null)
                    viewModel.SelectedIssueSeverityType = null;

                viewModel.IssueSeverityTypes = await IssueSeverityTypeService.client.ListActiveAsync(new ProjectInsight.Models.Base.ModelProperties("default"));

                if (selectedIssueSeverityTypeId != null)
                    viewModel.SelectedIssueSeverityType = viewModel.IssueSeverityTypes.Where(x => x.Id == selectedIssueSeverityTypeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //AuthenticationService.Logout();
                return false;
            }
            return true;
        }
        private async Task<bool> GetAllIssuePriorityTypes(Guid? selectedIssuePriorityTypeId = null)
        {
            try
            {

                viewModel.IssuePriorityTypes = null;
                if (selectedIssuePriorityTypeId != null)
                    viewModel.SelectedIssuePriorityType = null;

                viewModel.IssuePriorityTypes = await IssuePriorityTypeService.client.ListActiveAsync(new ProjectInsight.Models.Base.ModelProperties("default"));

                if (selectedIssuePriorityTypeId != null)
                    viewModel.SelectedIssuePriorityType = viewModel.IssuePriorityTypes.Where(x => x.Id == selectedIssuePriorityTypeId).FirstOrDefault();
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
        private async Task<bool> GetItem(Guid ItemId )
        {
            try
            {
               

                ProjectInsight.Models.Issues.Issue  issue = await IssuesService.client.GetAsync(ItemId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Description,StartDateTimeUTC,EndDateTimeUTC,IssueType,IssueStatusType,IssueSeverity,IssuePriority,FoundBy,UserAssignedTo,UserAssignedTo_Id,FoundDateTimeUTC,ProjectResolution,ProjectResolution_Id,ProjectAffiliation_Id,ProjectAffiliation;Project:ItemNumberFullAndNameDisplayPreference"));

                viewModel.Name = issue.Name;

                if ( !string.IsNullOrEmpty( issue.Description))
                    viewModel.Description = issue.Description;

                if (issue.StartDateTimeUTC.HasValue)
                    viewModel.StartDate = issue.StartDateTimeUTC.Value;

                if (issue.EndDateTimeUTC.HasValue)
                    viewModel.EndDate = issue.EndDateTimeUTC.Value;



                if (viewModel.Projects != null && issue.ProjectAffiliation_Id != null)
                    viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == issue.ProjectAffiliation_Id).FirstOrDefault();

                if (viewModel.Users != null && issue.UserAssignedTo_Id != null)
                    viewModel.SelectedUser= viewModel.Users.Where(x => x.Id == issue.UserAssignedTo_Id).FirstOrDefault();

                if (issue.IssueStatusType != null)
                    viewModel.SelectedIssueStatusType = issue.IssueStatusType;
                if (issue.IssueType != null)
                    viewModel.SelectedIssueType = issue.IssueType;

                if (issue.IssueSeverity != null)
                    viewModel.SelectedIssueSeverityType = issue.IssueSeverity;
                if (issue.IssuePriority != null)
                    viewModel.SelectedIssuePriorityType = issue.IssuePriority;
                

                viewModel.Id = ItemId;

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



            ProjectInsight.Models.Issues.Issue issue = new ProjectInsight.Models.Issues.Issue();
            issue.Name = viewModel.Name;
            issue.Description = viewModel.Description;
            issue.ProjectAffiliation_Id = viewModel.SelectedProject.Id;
            issue.UserCreated_Id = Common.CurrentWorkspace.UserID;
            issue.CreatedDateTimeUTC = DateTime.Now.ToUniversalTime();
            issue.UserAssignedBy_Id = Common.CurrentWorkspace.UserID;
            issue.UserAssignedTo_Id = viewModel.SelectedUser != null ? viewModel.SelectedUser.Id : Common.CurrentWorkspace.UserID;

            issue.IssueStatusType_Id = viewModel.SelectedIssueStatusType != null ? viewModel.SelectedIssueStatusType.Id : null;
            issue.IssueType_Id = viewModel.SelectedIssueType != null ? viewModel.SelectedIssueType.Id : null;
            issue.IssueSeverity_Id = viewModel.SelectedIssueSeverityType != null ? viewModel.SelectedIssueSeverityType.Id : null;
            issue.IssuePriority_Id = viewModel.SelectedIssuePriorityType != null ? viewModel.SelectedIssuePriorityType.Id : null;




            //task.SkipProjectScheduleCalculateOnSave = false;
            //task.ConstraintType = 3;


            //task.StartDateTimeUTC = viewModel.StartDate;
            //task.EndDateTimeUTC = viewModel.EndDate;
            issue.StartDateTimeUserLocal = viewModel.StartDate;
            issue.EndDateTimeUserLocal = viewModel.EndDate;
            issue.Id = viewModel.Id;



            ApiSaveResponse resp = await IssuesService.client.SaveAsync(issue);


           

            
            if (!resp.HasErrors)
            {
                var succsave = settings.Set("NewIssue_SelectedProject", viewModel.SelectedProject.Id.ToString());
                succsave = settings.Set("NewIssue_SelectedUser", issue.UserAssignedTo_Id.ToString());

                Common.Instance.ShowToastMessage("Issue Saved", DoubleResources.SuccessSnackBar);
                MessagingCenter.Send(this, "IssueAdded", resp.SavedId);
                Common.RefreshWorkList = true;
                Common.RefreshProjectsList = true;
                if (viewModel.Id != null || pId != Guid.Empty)
                {
                    MessagingCenter.Send(this, "IssueUpdated", resp.SavedId);

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

        private void cmbStatusType_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
                viewModel.SelectedIssueStatusType = null;
        }

        private void cmbIssueTypes_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
                viewModel.SelectedIssueType = null;

        }

        
             private void cmbIssueSeverityTypes_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
                viewModel.SelectedIssueSeverityType = null;

        }
        private void cmbIssuePriorityTypes_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
                viewModel.SelectedIssuePriorityType = null;

        }
    }
}