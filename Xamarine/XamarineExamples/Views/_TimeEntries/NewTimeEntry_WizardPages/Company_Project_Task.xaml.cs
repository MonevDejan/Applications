using ProjectInsight.Models.ReferenceData;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ProjectInsightMobile.Views.TimeEntries
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Company_Project_Task : ContentPage
    {
        public ProjectInsight.Models.TimeAndExpense.TimeEntryInputSettingsAndLists inputSettings;
        public ProjectInsight.Models.Tasks.Task selTask;

        TimeEntryWizardViewModel viewModel;
        bool GetAllItems = false;



            public Company_Project_Task(TimeEntryWizardViewModel ViewModel)
        {
            NavigationPage.SetBackButtonTitle(this, "");


            InitializeComponent();
            viewModel = ViewModel;
            BindingContext = viewModel;

            StartLoading();


            Title = "Add Time Entry";

        }

        private async void StartLoading()
        {


            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetSettings();


            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
                //viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
        }

        private async Task<bool> GetSettings()
        {

            try
            {
                inputSettings = await TimeEntryService.client.GetTimeEntryInputSettingsAndListsAsync(
                    selectedCompanyId:  null,
                    selectedProjectId:  null
                   );

                ObservableCollection<ProjectInsight.Models.Companies.Company> companies = new ObservableCollection<ProjectInsight.Models.Companies.Company>(inputSettings.CompaniesList);
                viewModel.Companies = companies;

                ObservableCollection<ProjectInsight.Models.Projects.Project> projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(inputSettings.ProjectList);
                viewModel.Projects = projects;

                //List<TaskExt> tasks = new List<ProjectInsight.Models.Tasks.Task>().ConvertAll(x => (TaskExt)x);
                //List<TaskExt> TaskList = inputSettings.TaskList.ConvertAll(x => (TaskExt)x);

                List<TaskExt> TaskList = new List<TaskExt>();
                if (inputSettings.TaskList != null)
                {
                    foreach (ProjectInsight.Models.Tasks.Task task in inputSettings.TaskList)
                    {
                        TaskExt extTask = new TaskExt(task);
                        TaskList.Add(extTask);
                    }
                }

                ObservableCollection<TaskExt> tasks = new ObservableCollection<TaskExt>(TaskList);
                viewModel.Tasks = tasks;



                if (viewModel.Companies != null && inputSettings.SelectedCompanyId != null)
                    viewModel.SelectedCompany = viewModel.Companies.Where(x => x.Id == inputSettings.SelectedCompanyId).FirstOrDefault();
                else
                {
                    viewModel.SelectedCompany = null;
                    CompanyInitBinding = false;
                }

                if (viewModel.Projects != null && inputSettings.SelectedProjectId != null)
                    viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == inputSettings.SelectedProjectId).FirstOrDefault();
                else
                {
                    viewModel.SelectedProject = null;
                    ProjectInitBinding = false;
                }

                if (viewModel.Tasks != null && inputSettings.SelectedTaskId != null)
                    viewModel.SelectedTask = viewModel.Tasks.Where(x => x.Id == inputSettings.SelectedTaskId).FirstOrDefault();
                else
                {
                    viewModel.SelectedTask = null;
                    TaskInitBinding = false;
                }


                //ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(inputSettings.list ExpenseCodeList);
                //viewModel.ExpenseCodes = expenseCodes;

                //if (viewModel.ExpenseCodes != null && inputSettings.SelectedTimeEntryExpenseCodeId != null)
                //    viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedTimeEntryExpenseCodeId).FirstOrDefault();
                //else viewModel.SelectedExpenseCode = null;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private async void Continue_Tapped(object sender, EventArgs e)
        {

               await Navigation.PushAsync(new TimeEntries.ExpenseCode(viewModel));

        }

        bool CompanyInitBinding = true;
        bool ProjectInitBinding = true;
        bool TaskInitBinding = true;
        private void CmbCompanies_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

           


            if (CompanyInitBinding)
            {
                CompanyInitBinding = false;
                return;
            }
            viewModel.VisibleLoad = true;

            var cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedItem == null || string.IsNullOrEmpty(cmb.SelectedItem.ToString()))
            {
                //Clicked X for clearing the Company selection
                viewModel.SelectedCompany = null;

                viewModel.SelectedProject = null;
                ProjectInitBinding = true;

                viewModel.SelectedTask = null;
                TaskInitBinding = true;

               
            }

            inputSettings = TimeEntryService.client.GetTimeEntryInputSettingsAndLists(
           selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null);
            var inputSettings1 = TimeEntryService.client.GetTimeEntryInputSettingsAndLists();
            //viewModel.Projects.Clear();

            //viewModel.Tasks.Clear();

            ObservableCollection<ProjectInsight.Models.Projects.Project> projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(inputSettings.ProjectList);
            if (projects.Count > 0)
                viewModel.Projects = projects;
            else
                viewModel.Projects = null;


            viewModel.SelectedProject = null;

            try
            {
                cmbProjects.Text = string.Empty;
            }
            catch
            {

            }
            

            viewModel.Tasks = null;
            viewModel.SelectedTask = null;

            try
            {
                cmbTasks.Text = string.Empty;
            }
            catch
            {

            }

          
            viewModel.VisibleLoad = false;
        }
        private void CmbProjects_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {

            if (ProjectInitBinding)
            {
                ProjectInitBinding = false;
                return;
            }

        

            var cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedItem == null || string.IsNullOrEmpty(cmb.SelectedItem.ToString()))
            {

                viewModel.SelectedTask = null;
                TaskInitBinding = true;
                viewModel.SelectedProject = null;
                //viewModel.Tasks.Clear();
                viewModel.Tasks = new ObservableCollection<TaskExt>();
                return;
            }
            else
            {
                TaskInitBinding = false;
            }

            viewModel.VisibleLoad = true;
            inputSettings = TimeEntryService.client.GetTimeEntryInputSettingsAndLists(
                 selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null,
                  selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null);

           // viewModel.Tasks.Clear();
           
            List<TaskExt> TaskList = new List<TaskExt>();
            if (inputSettings.TaskList != null)
            {
                foreach (ProjectInsight.Models.Tasks.Task task in inputSettings.TaskList)
                {
                    TaskExt extTask = new TaskExt(task);
                    TaskList.Add(extTask);
                }
            }

            ObservableCollection<TaskExt> tasks = new ObservableCollection<TaskExt>(TaskList);
            viewModel.Tasks = tasks;


            viewModel.SelectedTask = null;

            try
            {
                cmbTasks.Text = string.Empty;
            }
            catch
            {

            }


            viewModel.VisibleLoad = false;
        }

        private void ShowAll_Toggled(object sender, ToggledEventArgs e)
        {
            GetAllItems = e.Value;
           // StartLoading();
        }
    }
}