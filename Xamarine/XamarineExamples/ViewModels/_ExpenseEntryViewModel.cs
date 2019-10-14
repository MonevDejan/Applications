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
    public class ExpenseEntryViewModel : BaseViewModel
    {

        public ExpenseEntryViewModel()
            {

            this.date = DateTime.Now;
            }

        //Receipt=1, Mileage=2, Other=3
        public int SelectedExpenseType { get; set; }

      
        public bool ShowContinueOnPhotoPage
        {
            get
            {
                return photoArray != null;
            }
        }
        public bool ShowSkipOnPhotoPage
        {
            get
            {
                return photoArray == null;
            }
        }

        ImageSource photoSource;
        public ImageSource PhotoSource
        {
            set
            {
                if (photoSource != value)
                {
                    photoSource = value;

                    OnPropertyChanged("PhotoSource");
                }
            }
            get
            {
                return photoSource;
            }
        }

        byte[] photoArray;
        public byte[] PhotoArray
        {
            set
            {
                if (photoArray != value)
                {
                    photoArray = value;

                    OnPropertyChanged("PhotoArray");
                    OnPropertyChanged("ShowContinueOnPhotoPage");
                    OnPropertyChanged("ShowSkipOnPhotoPage");
                }
            }
            get
            {
                return photoArray;
            }
        }
       string photoName;
        public string PhotoName
        {
            set
            {
                if (photoName != value)
                {
                    photoName = value;
                    OnPropertyChanged("PhotoName");
                }
            }
            get
            {
                return photoName;
            }
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
                    OnPropertyChanged("AmountError");
                    OnPropertyChanged("ShowContinueOnAmountPage");

                }
            }
            get
            {
                return amount;
            }
        }
        
       
        public bool ShowContinueOnAmountPage
        {
            get
            {
                return amount != null;
            }
        }

        public bool AmountError
        {
            get
            {
                return amount == null;
            }
        }
        decimal? costPerMile;
        public decimal? CostPerMile
        {
            set
            {
                if (costPerMile != value)
                {
                    costPerMile = value;
                    OnPropertyChanged("CostPerMile");
                    OnPropertyChanged("ShowContinueOnCostPerMilePage");

                }
            }
            get
            {
                return costPerMile;
            }
        }
        public bool ShowContinueOnCostPerMilePage
        {
            get
            {
                return costPerMile != null;
            }
        }
        decimal? total;
        public decimal? Total
        {
            set
            {
                if (total != value)
                {
                    total = value;
                    OnPropertyChanged("Total");
                    OnPropertyChanged("ShowContinueOnTotalPage");

                }
            }
            get
            {
                return total;
            }
        }


        public bool ShowContinueOnTotalPage
        {
            get
            {
                return total != null;
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
        Double unitPrice;
        public Double UnitPrice
        {
            set
            {
                if (unitPrice != value)
                {
                    unitPrice = value;
                    OnPropertyChanged("UnitPrice");
                }
            }
            get
            {
                return unitPrice;
            }
        }

        Double actualCost;
        public Double ActualCost
        {
            set
            {
                if (actualCost != value)
                {
                    actualCost = value;
                    OnPropertyChanged("ActualCost");
                }
            }
            get
            {
                return actualCost;
            }
        }  
    }

    public class TaskExt
    {
        public TaskExt(ProjectInsight.Models.Tasks.Task task)
        {

            this.Id = task.Id;
            this.FullName = task.ItemNumberFullAndNameDisplayPreference;
            this.TaskName = task.Name;
        }
        public string FullName { get; set; }
        public string TaskName { get; set; }

        public Guid? Id { get; set; }

    }
    public class ProjectExt
    {
        public ProjectExt(ProjectInsight.Models.Projects.Project project)
        {

            this.Id = project.Id;
            this.FullName = project.ItemNumberFullAndNameDisplayPreference;
        }
        public string FullName { get; set; }
        public Guid? Id { get; set; }

    }
}
