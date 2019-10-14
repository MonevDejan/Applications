using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectInsightMobile.ViewModels
{
    public class TimeEntryWizardViewModel : BaseViewModel
    {

        public TimeEntryWizardViewModel()
        {

            this.date = DateTime.Now;
        }

        decimal? amount;
        public decimal? Amount
        {
            set
            {
                if (amount != value)
                {
                    amount = value;
                    OnPropertyChanged("Amount");
                    OnPropertyChanged("ShowContinueOnAmountPage");

                }
            }
            get
            {
                return amount;
            }
        }
        
        DateTime? date;
        public DateTime? Date
        {
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged("Date");
                    OnPropertyChanged("ShowContinueOnDatePage");

                }
            }
            get
            {
                return date;
            }
        }

        public ItemType ItemType { get; set; }
        public bool ShowContinueOnDatePage
        {
            get
            {
                return date != null;
            }
        }

        string description;
        public string Description
        {
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                    OnPropertyChanged("ShowContinueOnDescriptionPage");

                }
            }
            get
            {
                return description;
            }
        }
        public bool ShowContinueOnDescriptionPage
        {
            get
            {
                return !string.IsNullOrEmpty(description);
            }
        }
       
        public bool ProjectInitialBinding { get; set; }
        public bool CompanyInitialBinding { get; set; }
        public bool TaskInitialBinding { get; set; }


        TaskExt selectedTask;
        public TaskExt SelectedTask
        {
            set
            {
                if (selectedTask != value)
                {
                    selectedTask = value;

                    OnPropertyChanged("SelectedTask");
                }
            }
            get
            {
                return selectedTask;
            }
        }
        ObservableCollection<TaskExt> tasks;
        public ObservableCollection<TaskExt> Tasks
        {
            set
            {
                if (tasks != value)
                {
                    tasks = value;

                    OnPropertyChanged("Tasks");
                }
            }
            get
            {
                return tasks;
            }
        }


        ProjectInsight.Models.Companies.Company selectedCompany;
        public ProjectInsight.Models.Companies.Company SelectedCompany
        {
            set
            {
                if (selectedCompany != value)
                {
                    selectedCompany = value;
                    OnPropertyChanged("SelectedCompany");
                    OnPropertyChanged("ShowContinueOnCompanyProjectTaskPage");
                }
            }
            get
            {
                return selectedCompany;
            }
        }


        ObservableCollection<ProjectInsight.Models.Companies.Company> companies;
        public ObservableCollection<ProjectInsight.Models.Companies.Company> Companies
        {
            set
            {
                if (companies != value)
                {
                    companies = value;

                    OnPropertyChanged("Companies");
                    OnPropertyChanged("ShowContinueOnCompanyProjectTaskPage");

                }
            }
            get
            {
                return companies;
            }
        }


        ProjectInsight.Models.Projects.Project selectedProject;
        public ProjectInsight.Models.Projects.Project SelectedProject
        {
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;

                    OnPropertyChanged("SelectedProject");
                    OnPropertyChanged("IsProjectSelected");
                    OnPropertyChanged("ShowContinueOnCompanyProjectTaskPage");

                }
            }
            get
            {
                return selectedProject;
            }
        }
        public bool IsProjectSelected
        {
            get
            {
                return selectedProject != null;
            }
        }


        ObservableCollection<ProjectInsight.Models.Projects.Project> projects;
        public ObservableCollection<ProjectInsight.Models.Projects.Project> Projects
        {
            set
            {
                if (projects != value)
                {
                    projects = value;

                    OnPropertyChanged("Projects");
                }
            }
            get
            {
                return projects;
            }
        }

        public bool ShowContinueOnCompanyProjectTaskPage
        {
            get
            {
                return (selectedProject != null || selectedCompany != null );
            }
        }


        ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> expenseCodes;
        public ObservableCollection<ProjectInsight.Models.ReferenceData.ExpenseCode> ExpenseCodes
        {
            set
            {
                if (expenseCodes != value)
                {
                    expenseCodes = value;

                    OnPropertyChanged("ExpenseCodes");
                }
            }
            get
            {
                return expenseCodes;
            }
        }


        ProjectInsight.Models.ReferenceData.ExpenseCode selectedExpenseCode;
        public ProjectInsight.Models.ReferenceData.ExpenseCode SelectedExpenseCode
        {
            set
            {
                if (selectedExpenseCode != value)
                {
                    selectedExpenseCode = value;

                    OnPropertyChanged("SelectedExpenseCode");
                    OnPropertyChanged("ShowContinueOnCodePage");

                }
            }
            get
            {
                return selectedExpenseCode;
            }
        }
        public bool ShowContinueOnCodePage
        {
            get
            {
                return (selectedExpenseCode != null && !selectedExpenseCode.Name.Equals(" -- No Selection -- "));
            }
        }


        bool addingToTask = false;
        public bool AddingToTask
        {
            set
            {
                if (addingToTask != value)
                {
                    addingToTask = value;
                    OnPropertyChanged("AddingToTask");
                }
            }
            get
            {
                return addingToTask;
            }
        }

        bool addingToProject = false;
        public bool AddingToProject
        {
            set
            {
                if (addingToProject != value)
                {
                    addingToProject = value;
                    OnPropertyChanged("AddingToProject");
                }
            }
            get
            {
                return addingToProject;
            }
        }

        Boolean isBillable;
        public Boolean IsBillable
        {
            set
            {
                if (isBillable != value)
                {
                    isBillable = value;
                    OnPropertyChanged("IsBillable");
                }
            }
            get
            {
                return isBillable;
            }
        }

        Boolean showTimeInHours;
        public Boolean ShowTimeInHours
        {
            set
            {
                if (showTimeInHours != value)
                {
                    showTimeInHours = value;
                    OnPropertyChanged("ShowTimeInHours");
                }
            }
            get
            {
                return showTimeInHours;
            }
        }

        Boolean showTimeInDecimals;
        public Boolean ShowTimeInDecimals
        {
            set
            {
                if (showTimeInDecimals != value)
                {
                    showTimeInDecimals = value;
                    OnPropertyChanged("ShowTimeInDecimals");
                }
            }
            get
            {
                return showTimeInDecimals;
            }
        }

        Boolean showBillableCheckboxInput;
        public Boolean ShowBillableCheckboxInput
        {
            set
            {
                if (showBillableCheckboxInput != value)
                {
                    showBillableCheckboxInput = value;
                    OnPropertyChanged("ShowBillableCheckboxInput");
                }
            }
            get
            {
                return showBillableCheckboxInput;
            }
        }


        String actualHoursDec;
        public String ActualHoursDec
        {
            set
            {
                if (actualHoursDec != value)
                {
                    actualHoursDec = value;

                    OnPropertyChanged("ActualHoursDec");
                    OnPropertyChanged("ShowContinueOnActualTimePage");
                }
            }
            get
            {
                return actualHoursDec;
            }
        }

        string actualHours;
        public string ActualHours
        {
            set
            {
                if (actualHours != value)
                {
                    actualHours = value;

                    OnPropertyChanged("ActualHours");
                    OnPropertyChanged("ShowContinueOnActualTimePage");

                }
            }
            get
            {
                return actualHours;
            }
        }

        bool actualTimeError = false;
        public bool ActualTimeError
        {
            set
            {
                if (actualTimeError != value)
                {
                    actualTimeError = value;
                    OnPropertyChanged("ActualTimeError");
                }
            }
            get
            {
                return actualTimeError;
            }
        }
        bool actualTimeDecError = false;
        public bool ActualTimeDecError
        {
            set
            {
                if (actualTimeDecError != value)
                {
                    actualTimeDecError = value;
                    OnPropertyChanged("ActualTimeDecError");
                }
            }
            get
            {
                return actualTimeDecError;
            }
        }

        public bool ShowContinueOnActualTimePage
        {
            get
            {
                return (!string.IsNullOrEmpty(ActualHours) && ActualHours != "0" && ActualHours != "00:00")
                    || (ActualHoursDec != null && ActualHoursDec != "0");
            }
        }



        bool useHoursMinutesInput;
        public bool UseHoursMinutesInput
        {
            set
            {
                if (useHoursMinutesInput != value)
                {
                    useHoursMinutesInput = value;
                    OnPropertyChanged("UseHoursMinutesInput");
                }
            }
            get
            {
                return useHoursMinutesInput;
            }
        }
    }
}
