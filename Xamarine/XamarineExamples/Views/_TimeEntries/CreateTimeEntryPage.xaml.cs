using ProjectInsight.Models.ReferenceData;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.MyTime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TEditor;
using TEditor.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateTimeEntryPage : ContentPage
    {
        public ProjectInsight.Models.Tasks.Task selTask;
        public ProjectInsight.Models.TimeAndExpense.TimeEntryInputSettingsAndLists inputSettings;
        public static readonly string TAG_NO_SELECTION = " -- No Selection -- ";
        TimeEntryViewModel viewModel;
        public CreateTimeEntryPage()
		{
            NavigationPage.SetBackButtonTitle(this, "");
           //HockeyApp.MetricsManager.TrackEvent("TimeEntriesPage Initialize");

            InitializeComponent();

            viewModel = new TimeEntryViewModel();

            viewModel.Date = DateTime.Now.Date;

            StartLoading();
            BindingContext = viewModel;

            if (Device.RuntimePlatform.ToLower() == "android")
            {
                sfProjects.ShowHint = true;
                sfCompanies.ShowHint = true;
                sfTasks.ShowHint = true;
                sfTimeCode.ShowHint = true;
                cmbProjects.Margin = new Thickness(10, 0, 10, 0);
                cmbCompanies.Margin = new Thickness(10, 0, 10, 0);
                cmbTasks.Margin = new Thickness(10, 0, 10, 0);
                cmbTimeCode.Margin = new Thickness(10, 0, 10, 0);
                cmbDate.Margin = new Thickness(10, 0, 10, 0);
            }
            else
            {
                sfProjects.ShowHint = false;
                sfCompanies.ShowHint = false;
                sfTasks.ShowHint = false;
                sfTimeCode.ShowHint = false;
            }
        }
        public CreateTimeEntryPage(ProjectInsight.Models.Tasks.Task task)
        {
           //HockeyApp.MetricsManager.TrackEvent("TimeEntriesPage Initialize from Task");

            this.selTask = task;


            InitializeComponent();
            gvTE.RowDefinitions[0].Height = 0;
            gvTE.RowDefinitions[1].Height = 0;
            gvTE.RowDefinitions[2].Height = 0;

            if (Device.RuntimePlatform.ToLower() == "android")
                sfTimeCode.ShowHint = true;
            else
                sfTimeCode.ShowHint = false;
            viewModel = new TimeEntryViewModel();
            viewModel.Date = DateTime.Now.Date;
            StartLoading();
            BindingContext = viewModel;

        }
        private async void StartLoading()
        {
            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";
          


            bool isSuccess = await GetSettings();

            cmbCompanies.SelectedIndexChanged += Company_OnSelectionChanged;
            cmbProjects.SelectedIndexChanged += Project_OnSelectionChanged;
            cmbTasks.SelectedIndexChanged += Task_OnSelectionChanged;

            //isSuccess = await GetDammyData();

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
                ////viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
        }

       

        public ObservableCollection<T> createInitElem<T>(ObservableCollection<T> listItems, T initTask)
        {
            initTask.GetType().GetProperty("Name", BindingFlags.Public | BindingFlags.Instance).SetValue(initTask, TAG_NO_SELECTION);
            listItems.Insert(0, initTask);
            return listItems;
        }

        public void setModelsByTimeEntryService(int? type = null)
        {

            if (type == null || type == 1 )
            {
                ObservableCollection<ProjectInsight.Models.Projects.Project> projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(inputSettings.ProjectList);
                viewModel.Projects = createInitElem(projects, new ProjectInsight.Models.Projects.Project());         
            }
            if (type == null)
            {                                                                                      

                ObservableCollection<ProjectInsight.Models.Companies.Company> companies = new ObservableCollection<ProjectInsight.Models.Companies.Company>(inputSettings.CompaniesList);
                viewModel.Companies = createInitElem(companies, new ProjectInsight.Models.Companies.Company());
            }
            if (type == null || type == 1 || type == 2)
            { 
                ObservableCollection<ProjectInsight.Models.Tasks.Task> tasks = new ObservableCollection<ProjectInsight.Models.Tasks.Task>(inputSettings.TaskList);
                viewModel.Tasks = createInitElem(tasks, new ProjectInsight.Models.Tasks.Task());
                                                                                                               
            }             

        }

        private async Task<bool> GetSettings()
        {

            try
            {
                inputSettings = await TimeEntryService.Get(
                    selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null, 
                    selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null);



                if (inputSettings.EnableTimeEntry == false)
                {
                    viewModel.VisibleLoad = false;
                //    Common.Instance.ShowToastMessage("Time entry is not enabled!", DoubleResources.DangerSnackBar);
                    return false;
                }

                if (selTask == null)
                {
                    //set Default Value
                    setModelsByTimeEntryService();


                    if (inputSettings.SelectedCompanyId.HasValue)
                        viewModel.SelectedCompany = viewModel.Companies.Where(x => x.Id == inputSettings.SelectedCompanyId).FirstOrDefault();

                    if (inputSettings.SelectedProjectId.HasValue)
                    {
                        viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == inputSettings.SelectedProjectId).FirstOrDefault();
                        rowTask.IsVisible = true;
                    }
                    else
                    {
                        rowTask.IsVisible = false;
                    }

                    if (inputSettings.SelectedTaskId.HasValue)
                    {
                        viewModel.SelectedTask = viewModel.Tasks.Where(x => x.Id == inputSettings.SelectedTaskId).FirstOrDefault();

                        if (inputSettings.SelectedTaskTimeEntryInputSettings != null && inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                        {
                            viewModel.ShowBillableCheckboxInput = inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                            viewModel.IsBillable = inputSettings.SelectedTaskTimeEntryInputSettings.IsBillableDefaultOn.Value;
                            //viewModel.ShowBillableCheckboxInput = false;
                        }
                    }
                    else
                    {
                        if (inputSettings.SelectedProjectId.HasValue)
                        {
                            if (inputSettings.SelectedProjectTimeEntryInputSettings != null && inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                            {
                                viewModel.ShowBillableCheckboxInput = inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                                viewModel.IsBillable = inputSettings.SelectedProjectTimeEntryInputSettings.IsBillableDefaultOn.Value;
                            }
                        }
                        else
                        {
                            if (inputSettings.SelectedCompanyId.HasValue)
                            {
                                if (inputSettings.SelectedCompanyTimeEntryInputSettings != null && inputSettings.SelectedCompanyTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                                {
                                    viewModel.ShowBillableCheckboxInput = inputSettings.SelectedCompanyTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                                    viewModel.IsBillable = inputSettings.SelectedCompanyTimeEntryInputSettings.IsBillableDefaultOn.Value;

                                }
                            }
                        }
                    }

                }
                else
                {
                    if (inputSettings.SelectedTaskTimeEntryInputSettings != null && inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                    {
                        viewModel.ShowBillableCheckboxInput = inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                        viewModel.IsBillable = inputSettings.SelectedTaskTimeEntryInputSettings.IsBillableDefaultOn.Value;
                    }
                }

                List<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes = await ExpenseCodeService.GetActive();
                viewModel.ExpenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>(expenseCodes);
                //viewModel.ExpenseCodes = new ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode>();
                //foreach (var item in expenseCodes)
                //    viewModel.ExpenseCodes.Add(item);
                
                viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedTimeEntryExpenseCodeId).FirstOrDefault();

                viewModel.ShowTimeInHours = inputSettings.UseHoursMinutesInput;
                viewModel.ShowTimeInDecimals = !inputSettings.UseHoursMinutesInput;
                


            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        async void Company_OnSelectionChanged(object sender, EventArgs e)
        {

          
            if (selTask != null || viewModel.CompanyInitialBinding || viewModel.SelectedCompany == null ||  (viewModel.SelectedCompany != null && viewModel.SelectedCompany.Id == null))
            {
                viewModel.CompanyInitialBinding = false;
                return;
            }
            try
            {

                viewModel.VisibleLoad = true;
                viewModel.LoadingMessage = "";
                
                viewModel.ProjectInitialBinding = true;
                viewModel.TaskInitialBinding = true;

                //viewModel.SelectedProject = null;
                //viewModel.SelectedTask = null;
                viewModel.SelectedExpenseCode = null;
                if (viewModel.SelectedCompany == null || viewModel.SelectedCompany.Name.Equals(TAG_NO_SELECTION))
                {

                    inputSettings = await TimeEntryService.Get(selectedCompanyId:null, selectedProjectId:null);
                }
                else
                {
                    viewModel.CompanyError = false;
                    viewModel.ProjectError = false;
                    inputSettings = await TimeEntryService.Get(selectedCompanyId: viewModel.SelectedCompany.Id, selectedProjectId: null);

                }

                setModelsByTimeEntryService(1);
                //viewModel.Projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(inputSettings.ProjectList);
                //viewModel.Tasks = new ObservableCollection<ProjectInsight.Models.Tasks.Task>(inputSettings.TaskList);

                viewModel.SelectedProject = viewModel.Projects.Where(x => x.Id == inputSettings.SelectedProjectId).FirstOrDefault();
                if (!inputSettings.SelectedProjectId.HasValue) viewModel.ProjectInitialBinding = false;

                viewModel.SelectedTask = viewModel.Tasks.Where(x => x.Id == inputSettings.SelectedTaskId).FirstOrDefault();
                if (!inputSettings.SelectedTaskId.HasValue) viewModel.TaskInitialBinding = false;

                viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedTimeEntryExpenseCodeId).FirstOrDefault();

                viewModel.ShowTimeInHours = inputSettings.UseHoursMinutesInput;
                viewModel.ShowTimeInDecimals = !inputSettings.UseHoursMinutesInput;

                //if (inputSettings.SelectedProjectTimeEntryInputSettings != null)
                //viewModel.ShowBillableCheckboxInput = inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.Value;

                if (inputSettings.SelectedTaskId.HasValue)
                {
                    if (inputSettings.SelectedTaskTimeEntryInputSettings != null && inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                        viewModel.ShowBillableCheckboxInput = inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                }
                else
                {
                    if (inputSettings.SelectedProjectId.HasValue)
                    {
                        if (inputSettings.SelectedProjectTimeEntryInputSettings != null && inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                            viewModel.ShowBillableCheckboxInput = inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                    }
                    else
                    {
                        if (inputSettings.SelectedCompanyId.HasValue)
                        {
                            if (inputSettings.SelectedCompanyTimeEntryInputSettings != null && inputSettings.SelectedCompanyTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                                viewModel.ShowBillableCheckboxInput = inputSettings.SelectedCompanyTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                        }
                    }
                }


                viewModel.VisibleLoad = false;


                //Picker entry = sender as Picker;
                //inputSettings = await TimeEntryService.Get(selectedCompanyId: viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null, selectedProjectId: viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null);
                //viewModel.Projects = new ObservableCollection<ProjectInsight.Models.Projects.Project>(inputSettings.ProjectList); ;
                //viewModel.Tasks = new ObservableCollection<ProjectInsight.Models.Tasks.Task>(inputSettings.TaskList);

            }
            catch (Exception ex)
            {

            }

         
        }
        async void Project_OnSelectionChanged(object sender, EventArgs e)
        {
          
            if (viewModel.SelectedProject == null || viewModel.SelectedProject.Name.Equals(TAG_NO_SELECTION))
            {
                rowTask.IsVisible = false;
            }
            else
            {
                viewModel.CompanyError = false;
                viewModel.ProjectError = false;
                rowTask.IsVisible = true;
            }
            if (selTask != null || viewModel.ProjectInitialBinding || viewModel.SelectedProject == null || viewModel.SelectedProject.Name.Equals(TAG_NO_SELECTION))

            {
               
                viewModel.ProjectInitialBinding = false;
                return;
            }
            
            try
            {

                viewModel.TaskInitialBinding = true;

                viewModel.VisibleLoad = true;
                viewModel.LoadingMessage = "";

               // viewModel.SelectedTask = null;
                viewModel.SelectedExpenseCode = null;



                



                if (viewModel.SelectedProject == null || viewModel.SelectedProject.Name.Equals(TAG_NO_SELECTION))
                {

                    inputSettings = await TimeEntryService.Get(selectedProjectId: null);
                }
                else
                {
                    inputSettings = await TimeEntryService.Get(selectedProjectId: viewModel.SelectedProject.Id);
                }




                setModelsByTimeEntryService(2);
                //viewModel.Tasks =  new ObservableCollection<ProjectInsight.Models.Tasks.Task>(inputSettings.TaskList);


                viewModel.SelectedTask = viewModel.Tasks.Where(x => x.Id == inputSettings.SelectedTaskId).FirstOrDefault();
                if (!inputSettings.SelectedTaskId.HasValue)
                    viewModel.TaskInitialBinding = false;


                viewModel.SelectedExpenseCode = viewModel.ExpenseCodes.Where(x => x.Id == inputSettings.SelectedTimeEntryExpenseCodeId).FirstOrDefault();

                viewModel.ShowTimeInHours = inputSettings.UseHoursMinutesInput;
                viewModel.ShowTimeInDecimals = !inputSettings.UseHoursMinutesInput;

                //if (inputSettings.SelectedProjectTimeEntryInputSettings != null)
                //    viewModel.ShowBillableCheckboxInput = inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                if (inputSettings.SelectedTaskId.HasValue)
                {
                    if (inputSettings.SelectedTaskTimeEntryInputSettings != null && inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                        viewModel.ShowBillableCheckboxInput = inputSettings.SelectedTaskTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                }
                else
                {
                    if (inputSettings.SelectedProjectId.HasValue)
                    {
                        if (inputSettings.SelectedProjectTimeEntryInputSettings != null && inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                            viewModel.ShowBillableCheckboxInput = inputSettings.SelectedProjectTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                    }
                    else
                    {
                        if (inputSettings.SelectedCompanyId.HasValue)
                        {
                            if (inputSettings.SelectedCompanyTimeEntryInputSettings != null && inputSettings.SelectedCompanyTimeEntryInputSettings.ShowBillableCheckboxInput.HasValue)
                                viewModel.ShowBillableCheckboxInput = inputSettings.SelectedCompanyTimeEntryInputSettings.ShowBillableCheckboxInput.Value;
                        }
                    }
                }

                viewModel.VisibleLoad = false;
                
            }
            catch (Exception ex)
            {

            }

        }
        async void Task_OnSelectionChanged(object sender, EventArgs e)
        {
            if (selTask != null || viewModel.TaskInitialBinding || viewModel.SelectedTask == null)
            {
                viewModel.TaskInitialBinding = false;
                return;
            }       
         
            try
            {
                viewModel.VisibleLoad = true;
                viewModel.LoadingMessage = "";

                if (viewModel.SelectedTask == null || viewModel.SelectedTask.Name.Equals(TAG_NO_SELECTION))
                {
                    //viewModel.SelectedTask = null;
                }
             
                if (viewModel.SelectedTask.Id.HasValue && inputSettings.TaskTimeEntryInputSettingsList != null)
                {
                    var settings = inputSettings.TaskTimeEntryInputSettingsList.Where(x => x.Id == viewModel.SelectedTask.Id).FirstOrDefault();
                    viewModel.ShowBillableCheckboxInput = settings.ShowBillableCheckboxInput.Value;
                }
                viewModel.VisibleLoad = false;

            }
            catch (Exception ex)
            {

            }
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            var a = e.NewDate.ToString();
        }


        private async void OnSaveAddTimeEntry(object sender, EventArgs e)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            if (selTask == null)
            {
                if ((viewModel.SelectedCompany == null || viewModel.SelectedCompany.Name.Equals(TAG_NO_SELECTION)) && (viewModel.SelectedProject == null || viewModel.SelectedProject.Name.Equals(TAG_NO_SELECTION)))
                {
                    viewModel.CompanyError = true;
                    viewModel.ProjectError = true;

                    viewModel.VisibleLoad = false;
                    return;
                }
                else
                {
                    viewModel.CompanyError = false;
                    viewModel.ProjectError = false;
                }
            }
            if (inputSettings.UseHoursMinutesInput && (viewModel.ActualHours == null || viewModel.ActualHours == string.Empty))
            {
                viewModel.ActualTimeError = true;
                viewModel.VisibleLoad = false;
                return;
            }
            else
            {
                viewModel.ActualTimeError = false;
            }



            if (!inputSettings.UseHoursMinutesInput && (viewModel.ActualHoursDec == null || viewModel.ActualHoursDec.Trim() == string.Empty))
            {
                viewModel.ActualTimeDecError = true;
                viewModel.VisibleLoad = false;
                return;
            }
            else
            {
                viewModel.ActualTimeDecError = false;
                TimeEntry timeEntry = new TimeEntry();


                if (inputSettings.UseHoursMinutesInput)
                {

                    //decimal dActualHours = Convert.ToDecimal(TimeSpan.Parse(viewModel.ActualHours).TotalHours);
                    //timeEntry.ActualHours = dActualHours;

                    timeEntry.ActualTimeString = viewModel.ActualHours.Trim();
                }
                else
                {
                    decimal act = 0;
                    bool succPars = decimal.TryParse(viewModel.ActualHoursDec, out act);
                    if (succPars)
                    {
                        timeEntry.ActualHours = act;
                    }
                }

                timeEntry.User_Id = Common.CurrentWorkspace.UserID;
                timeEntry.Description = viewModel.Description;
                if (selTask == null)
                {
                    timeEntry.Company_Id = viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null;
                    timeEntry.Project_Id = viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null;
                    timeEntry.Task_Id = viewModel.SelectedTask != null ? viewModel.SelectedTask.Id : null;
                }else
                {
                    timeEntry.Project_Id = selTask.Project_Id;
                    timeEntry.Task_Id = selTask.Id;
                }
                timeEntry.ExpenseCode_Id = viewModel.SelectedExpenseCode.Id;
                timeEntry.IsBillable = isBillable.IsChecked; //  viewModel.IsBillable;
                timeEntry.Date = viewModel.Date;
                timeEntry.CreatedDateTimeUTC = DateTime.UtcNow;
                //timeEntry.TimerCreatedDateTimeUTC = DateTime.UtcNow;                
                bool isSuccess = await TimeEntryService.Save(timeEntry);

                if (isSuccess)
                {
                   // Common.Instance.ShowToastMessage("Success.", DoubleResources.SuccessSnackBar);
                   // await Navigation.PopAsync();

                    var app = Application.Current as App;
                    var mainPage = (StartupMasterPage)app.MainPage;
                    NavigationPage page = null;
                    page = new NavigationPage(new MyTimePage());
                    mainPage.Detail = page;
                    mainPage.Title = page.Title;

                } else
                {
                    Common.Instance.ShowToastMessage("Error, check again.", DoubleResources.DangerSnackBar);    
                }

            }


            viewModel.VisibleLoad = false;
        
        }

        private async void OnCancelAddTimeEntry(object sender, EventArgs e)
        {

            await Navigation.PopAsync();
        }

        int restrictCount = 7;



        void ActualHours_OnTextChanged(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;
            String val = entry.Text;
            if (val.Length > 0)
            {
                string old_val = val.Remove(val.Length - 1);
                char lastChar = val[val.Length - 1];
                if (
                    !(Char.IsDigit(lastChar) || (lastChar == '.' && !old_val.Contains('.') && !old_val.Contains(':')) || (lastChar == ':' && !old_val.Contains('.') && !old_val.Contains(':')))
                    || val.Length > restrictCount
                    )
                {
                    entry.Text = old_val; //Set the Old value
                }
            }
        }
        void ActualTimey_Unfocussed(object sender, TextChangedEventArgs e)
        {
            //var oldText = e.OldTextValue;
            //var newText = e.NewTextValue.ToString();

            if (viewModel == null || viewModel.ActualHours == null) return;

            string oldValue = viewModel.ActualHours;

            

            if (oldValue.Contains(":"))
            {
                char splitter = ':';
                viewModel.ActualHours = TransformTime(oldValue, splitter);
            }
            else if (oldValue.Contains("."))
            {
                char splitter = '.';
                viewModel.ActualHours = TransformTime(oldValue, splitter);
            }
            else
            {
                int hours = 0;
                int.TryParse(oldValue, out hours);
                viewModel.ActualHours = hours.ToString().PadLeft(2, '0') + ":00";
            }


        }
        private static string TransformTime(string oldValue, char splitter, int nearestSegment = 0, string roundDirection="1")
        {
            int hours = 0;
            int minutes = 0;
            if (splitter == '.')
            {
                Double timeDbl = 0;
                Double.TryParse(oldValue, out timeDbl);
                TimeSpan timespan = TimeSpan.FromHours(timeDbl);
                oldValue = timespan.ToString("h\\:mm");
                splitter = ':';
            }
            var parts = oldValue.Split(splitter);
            int.TryParse(parts[0], out hours);
            if (parts.Length > 1)
            {
                int.TryParse(parts[1], out minutes);
            }

            TimeSpan ts = TimeSpan.FromMinutes(minutes);
            hours += ts.Hours;
            minutes = ts.Minutes;


            var factor = nearestSegment;
            int minRounded = minutes;
            if (factor > 0)
            {
                if (roundDirection == "1" || roundDirection.ToString().ToLower() == "closest")
                {
                    decimal dMin = ((decimal)minutes / factor);
                    minRounded = Convert.ToInt16(Math.Round(dMin, 0) * factor);
                }
                else if (roundDirection == "2" || roundDirection.ToString().ToLower() == "up")
                {
                    decimal dMin = ((decimal)minutes / factor);
                    minRounded = Convert.ToInt16(Math.Ceiling(dMin) * factor);
                }
                else if (roundDirection == "3" || roundDirection.ToString().ToLower() == "down")
                {
                    decimal dMin = ((decimal)minutes / factor);
                    minRounded = Convert.ToInt16(Math.Floor(dMin) * factor);
                }
            }




            //int wholeMinutes = (int)Math.Floor(minutes / 10.0) * 10;
            //int lastDigit = minutes - wholeMinutes;
            //if (lastDigit > 0 && lastDigit < 5)
            //{
            //    minutes = wholeMinutes;
            //}
            //else if ((lastDigit > 5 && lastDigit < 10))
            //{
            //    minutes = wholeMinutes + 5;
            //}

            return hours.ToString().PadLeft(2, '0') + ":" + minRounded.ToString().PadLeft(2, '0');
        }

      

        private void NumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Value.ToString()))
                viewModel.ActualTimeDecError = false;
        }

        private void ActualHours_OnTextChanged(object sender, TextChangedEventArgs e)
        {
                if (!String.IsNullOrEmpty(e.NewTextValue))
                    viewModel.ActualTimeError = false;

        }
    }
}