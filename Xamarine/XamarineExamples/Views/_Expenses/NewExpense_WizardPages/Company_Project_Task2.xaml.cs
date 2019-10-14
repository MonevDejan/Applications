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


namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Company_Project_Task2 : ContentPage
    {
        public ProjectInsight.Models.TimeAndExpense.ExpenseEntryInputSettingsAndLists inputSettings;
        public ProjectInsight.Models.Tasks.Task selTask;

        ExpenseEntryViewModel viewModel;

       

            public Company_Project_Task2(ExpenseEntryViewModel ViewModel)
        {
            NavigationPage.SetBackButtonTitle(this, "");


            InitializeComponent();
            viewModel = ViewModel;
            BindingContext = viewModel;

            StartLoading();

           
           
            if (viewModel.SelectedExpenseType == 1)
                Title = "Add Receipt";
            else if (viewModel.SelectedExpenseType == 2)
                Title = "Add Mileage";
            else if (viewModel.SelectedExpenseType == 3)
                Title = "Add Other";
            else
                Title = "Add";

        }

        private async void StartLoading()
        {


            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetSettings();

            //isSuccess = await GetDammyData();

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
                inputSettings = await ExpenseEntryService.client.GetExpenseEntryInputSettingsAndListsAsync(
                    includeCompanySelectionList: true,
                    includeProjectSelectionList: true,
                    includeTaskSelectionList: true,
                    includeExpenseCodeSelectionList: true,
                    lookupSelectedFromLastEntry: true, 
                    includeUserSelectionList:true);


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

                if (viewModel.SelectedExpenseType != 2)
                {
                    ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(inputSettings.ExpenseCodeList);
                    viewModel.ExpenseCodes = expenseCodes;

                    if (viewModel.ExpenseCodes != null && inputSettings.SelectedExpenseCodeId != null)
                        viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedExpenseCodeId).FirstOrDefault();
                    else viewModel.SelectedExpenseCode = null;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private async void Continue_Tapped(object sender, EventArgs e)
        {

            if (viewModel.SelectedExpenseType != 2)
                await Navigation.PushAsync(new ExpenseCode(viewModel));
            else
                await Navigation.PushAsync(new Description(viewModel));


        }

        bool CompanyInitBinding = true;
        bool ProjectInitBinding = true;
        bool TaskInitBinding = true;
        private async void CmbCompanies_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
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

                

                inputSettings = await ExpenseEntryService.client.GetExpenseEntryInputSettingsAndListsAsync(
                       includeCompanySelectionList: false,
                       includeProjectSelectionList: true,
                       includeTaskSelectionList: false,
                       includeExpenseCodeSelectionList: true,
                       lookupSelectedFromLastEntry: false,
                       selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null
                    //   selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null
                       );
            }
            else
            {

                inputSettings = await ExpenseEntryService.client.GetExpenseEntryInputSettingsAndListsAsync(
                       includeCompanySelectionList: false,
                       includeProjectSelectionList: true,
                       includeTaskSelectionList: true,
                       includeExpenseCodeSelectionList: true,
                       lookupSelectedFromLastEntry: true,
                       selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null
                       //selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null
                       );
            }

            viewModel.Projects.Clear();
            viewModel.Tasks.Clear();
           
            ObservableCollection<ProjectInsight.Models.Projects.Project> projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(inputSettings.ProjectList);
            if (projects.Count > 0)
                viewModel.Projects = projects;

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
           


            if (viewModel.Projects != null && inputSettings.SelectedProjectId != null)
                viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == inputSettings.SelectedProjectId).FirstOrDefault();
            else viewModel.SelectedProject = null;

            if (viewModel.Tasks != null && inputSettings.SelectedTaskId != null)
                viewModel.SelectedTask = viewModel.Tasks.Where(x => x.Id == inputSettings.SelectedTaskId).FirstOrDefault();
            else viewModel.SelectedTask = null;
            if (viewModel.SelectedExpenseType != 2)
            {
                viewModel.ExpenseCodes.Clear();

                ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(inputSettings.ExpenseCodeList);
                viewModel.ExpenseCodes = expenseCodes;
                if (viewModel.ExpenseCodes != null && inputSettings.SelectedExpenseCodeId != null)
                    viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedExpenseCodeId).FirstOrDefault();
                else viewModel.SelectedExpenseCode = null;
            }
            viewModel.VisibleLoad = false;
        }
        private async void CmbProjects_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
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
                viewModel.Tasks.Clear();
                return;
            }
            else
            {
                TaskInitBinding = false;
            }

            viewModel.VisibleLoad = true;
            inputSettings = await ExpenseEntryService.client.GetExpenseEntryInputSettingsAndListsAsync(
                  includeCompanySelectionList: false,
                  includeProjectSelectionList: false,
                  includeTaskSelectionList: true,
                  includeExpenseCodeSelectionList: true,
                  lookupSelectedFromLastEntry: false,
                   selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null,
                   selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null
                  );


            viewModel.Tasks.Clear();
           
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
           

            if (viewModel.Tasks != null && inputSettings.SelectedTaskId != null)
                viewModel.SelectedTask = viewModel.Tasks.Where(x => x.Id == inputSettings.SelectedTaskId).FirstOrDefault();
            else viewModel.SelectedTask = null;

            if (viewModel.SelectedExpenseType != 2)
            {
                viewModel.ExpenseCodes.Clear();

                ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(inputSettings.ExpenseCodeList);
                viewModel.ExpenseCodes = expenseCodes;

                if (viewModel.ExpenseCodes != null && inputSettings.SelectedExpenseCodeId != null)
                    viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedExpenseCodeId).FirstOrDefault();
                else viewModel.SelectedExpenseCode = null;
            }
            viewModel.VisibleLoad = false;
        }

        private async  void CmbTasks_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (TaskInitBinding)
            {
                TaskInitBinding = false;
                return;
            }
            var cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedItem == null || string.IsNullOrEmpty(cmb.SelectedItem.ToString()))
            {
                viewModel.SelectedTask = null;
                return;
            }


            viewModel.VisibleLoad = true;
            inputSettings = await ExpenseEntryService.client.GetExpenseEntryInputSettingsAndListsAsync(
                  includeCompanySelectionList: false,
                  includeProjectSelectionList: false,
                  includeTaskSelectionList: false,
                  includeExpenseCodeSelectionList: true,
                  lookupSelectedFromLastEntry: true,
                  selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null,
                  selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null,
                  selectedTaskId: viewModel.SelectedTask!= null ? viewModel.SelectedTask.Id : null
                  );
        //includeCompanySelectionList: false,
        //          includeProjectSelectionList: false,
        //          includeTaskSelectionList: false,
        //          includeExpenseCodeSelectionList: true,
        //          lookupSelectedFromLastEntry: true,
        //           selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null,
        //           selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null,
        //           selectedTaskId: viewModel.SelectedTask != null ? viewModel.SelectedTask.Id : null

        //includeCompanySelectionList: true,
        //            includeProjectSelectionList: true,
        //            includeTaskSelectionList: true,
        //            includeExpenseCodeSelectionList: true,
        //            lookupSelectedFromLastEntry: true, 
        //            includeUserSelectionList: true


            if (viewModel.SelectedExpenseType != 2)
            {
                viewModel.ExpenseCodes.Clear();

                ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(inputSettings.ExpenseCodeList);
                viewModel.ExpenseCodes = expenseCodes;

                if (viewModel.ExpenseCodes != null && inputSettings.SelectedExpenseCodeId != null)
                    viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedExpenseCodeId).FirstOrDefault();
                else viewModel.SelectedExpenseCode = null;
            }
            viewModel.VisibleLoad = false;

        }

    }
}