using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class TimeEntryViewModel : BaseViewModel
    {
        public TimeEntryViewModel()
        {
            //tasks = new ObservableCollection<ProjectInsight.Models.Tasks.Task>();
            //tasks.Add(new Task())
        }
        public bool ProjectInitialBinding { get; set; }
        public bool CompanyInitialBinding { get; set; }
        public bool TaskInitialBinding { get; set; }

        public IEnumerable<TimeCode> TimeCodes { get; set; }


        DateTime date;
        public DateTime Date
        {
            set
            {
                if (date != value)
                {
                    date = value;

                    OnPropertyChanged("Date");
                }
            }
            get
            {
                return date;
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
                }
            }
            get
            {
                return description;
            }
        }


        ProjectInsight.Models.Tasks.Task selectedTask;
        public ProjectInsight.Models.Tasks.Task SelectedTask
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
        ObservableCollection<ProjectInsight.Models.Tasks.Task> tasks;
        public ObservableCollection<ProjectInsight.Models.Tasks.Task> Tasks
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
                }
            }
            get
            {
                return selectedProject;
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



        ProjectInsight.Models.ReferenceData.ExpenseCode selectedExpenseCode;
        public ProjectInsight.Models.ReferenceData.ExpenseCode SelectedExpenseCode
        {
            set
            {
                if (selectedExpenseCode != value)
                {
                    selectedExpenseCode = value;

                    OnPropertyChanged("SelectedExpenseCode");
                }
            }
            get
            {
                return selectedExpenseCode;
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
                }
            }
            get
            {
                return actualHours;
            }
        }



        bool companyError = false;
        public bool CompanyError
        {
            set
            {
                if (companyError != value)
                {
                    companyError = value;
                    OnPropertyChanged("CompanyError");
                }
            }
            get
            {
                return companyError;
            }
        }


        bool projectError = false;
        public bool ProjectError
        {
            set
            {
                if (projectError != value)
                {
                    projectError = value;
                    OnPropertyChanged("ProjectError");
                }
            }
            get
            {
                return projectError;
            }
        }
        bool taskError = false;
        public bool TaskError
        {
            set
            {
                if (taskError != value)
                {
                    taskError = value;
                    OnPropertyChanged("TaskError");
                }
            }
            get
            {
                return taskError;
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
    }

    public class TimeCode
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
