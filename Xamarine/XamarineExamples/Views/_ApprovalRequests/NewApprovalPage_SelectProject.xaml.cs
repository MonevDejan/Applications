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
using static ProjectInsightMobile.ViewModels.NewApprovalViewModel;
using ProjectInsight.Models.Users;
using HtmlAgilityPack;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_SelectProject : ContentPage
    {
      
        public static readonly string TAG_NO_SELECTION = "No Selection";
        NewApprovalViewModel viewModel;
        SettingsService settings = new SettingsService();

        Guid pId = Guid.Empty;

        public NewApprovalPage_SelectProject()
		{
            NavigationPage.SetBackButtonTitle(this,"");
           
            
            InitializeComponent();

            viewModel = new NewApprovalViewModel();

            StartLoading();

            BindingContext = viewModel;

            if (viewModel.Id.HasValue)
            {
                Title = "Edit Approval";
            }
        }
        private async void StartLoading()
        {
            viewModel.VisibleLoad = true;

            bool isSuccess = await GetAllProjects();

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
                //    ObservableCollection<ProjectInsight.Models.Projects.Project> projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(obsProjects);

                foreach (var pr in obsProjects )
                {
                    //pr.UrlIcon = Common.CurrentWorkspace.WorkspaceURL + pr.UrlIcon.Replace("/projectinsight.webapp", "");
                    pr.UrlIcon = "item_project.png";
                }
                viewModel.Projects = obsProjects;
                //viewModel.Projects = projects;

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
         private async void OnSave_Tapped(object sender, EventArgs e)
        {

            //selectiraniot Task e 
            //var selTask = viewModel.SelectedTask;
            bool validationError = false;
            if (viewModel.SelectedProject == null)
            {
                viewModel.SelectedProjectError = true;
                validationError = true;
            }

            if (validationError) return;


            await Navigation.PushAsync(new NewApprovalPage_Details());
           


        }

        private async void OnCancel_Tapped(object sender,DateChangedEventArgs e)
        {

            await Navigation.PopAsync();
        }

       


        public ObservableCollection<T> createInitElem<T>(ObservableCollection<T> listItems, T initTask)
        {
            initTask.GetType().GetProperty("Name", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).SetValue(initTask, TAG_NO_SELECTION);
            listItems.Insert(0, initTask);
            return listItems;
        }


        void Handle_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            List<Approver> items = new List<Approver>();
            if (e.Value != null && (((e.Value is string && (string)e.Value != string.Empty)) || (e.Value is System.Collections.IList && (e.Value as System.Collections.IList).Count > 0)))
            {

                    for (int ii = 0; ii < (e.Value as List<object>).Count; ii++)
                    {
                        var collection = (e.Value as List<object>)[ii];
                        items.Add((Approver)collection);
                    }
                
                viewModel.SelectedApprovers = items;
                if (items.Count > 0)
                {
                    //listView.SeparatorColor = Color.Black;
                }
                else
                {
                    //listView.SeparatorColor = Color.Transparent;
                }
            }
            else
            {
                viewModel.SelectedApprovers = null;
            }

        }

    

    }
}