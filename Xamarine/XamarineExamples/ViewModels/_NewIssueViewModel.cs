using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using ProjectInsight.Models.ReferenceData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class NewIssueViewModel : BaseViewModel
    {
        public NewIssueViewModel()
        {
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

        ObservableCollection<ProjectInsight.Models.Users.User> users;
        public ObservableCollection<ProjectInsight.Models.Users.User> Users
        {
            set
            {
                if (users != value)
                {
                    users = value;
                    OnPropertyChanged("Users");
                    OnPropertyChanged("ShowUsers");

                }
            }
            get
            {
                return users;
            }
        }

        public bool ShowUsers
        {
            get
            {
                return Users != null && Users.Count > 0;
            }
        }


        Guid? id;
        public Guid? Id
        {
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
            get
            {
                return id;
            }
        }

        string name;
        public string Name
        {
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
            get
            {
                return name;
            }
        }
        bool nameError = false;
        public bool NameError
        {
            set
            {
                if (nameError != value)
                {
                    nameError = value;
                    OnPropertyChanged("NameError");
                }
            }
            get
            {
                return nameError;
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

        DateTime startDate;
        public DateTime StartDate
        {
            set
            {
                if (startDate != value)
                {
                    startDate = value;

                    OnPropertyChanged("StartDate");
                }
            }
            get
            {
                return startDate;
            }
        }

        DateTime endDate;
        public DateTime EndDate
        {
            set
            {
                if (endDate != value)
                {
                    endDate = value;

                    OnPropertyChanged("EndDate");
                }
            }
            get
            {
                return endDate;
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
        
            bool enableIssueScheduling = false;
        public bool EnableIssueScheduling
        {
            set
            {
                if (enableIssueScheduling != value)
                {
                    enableIssueScheduling = value;
                    OnPropertyChanged("EnableIssueScheduling");
                }
            }
            get
            {
                return enableIssueScheduling;
            }
        }
        ProjectInsight.Models.Users.User selectedUser;
        public ProjectInsight.Models.Users.User SelectedUser
        {
            set
            {
                if (selectedUser != value)
                {
                    selectedUser = value;

                    OnPropertyChanged("SelectedUser");
                }
            }
            get
            {
                return selectedUser;
            }
        }

        bool selectProjectEnabled { get; set; } = true;
        public bool SelectProjectEnabled
        {
            set
            {
                if (selectProjectEnabled != value)
                {
                    selectProjectEnabled = value;

                    OnPropertyChanged("SelectProjectEnabled");
                }
            }
            get
            {
                return selectProjectEnabled;
            }
        }

        List<IssueStatusType> issueStatusTypes;
        public List<IssueStatusType> IssueStatusTypes
        {
            set
            {
                if (issueStatusTypes != value)
                {
                    issueStatusTypes = value;

                    OnPropertyChanged("IssueStatusTypes");
                    OnPropertyChanged("ShowStatusTypes");
                }
            }
            get
            {
                return issueStatusTypes;
            }
        }

        public bool ShowStatusTypes
        {
            get
            {
                return IssueStatusTypes != null && IssueStatusTypes.Count > 0;
            }
        }

        IssueStatusType selectedIssueStatusType;
        public IssueStatusType SelectedIssueStatusType
        {
            set
            {
                if (selectedIssueStatusType != value)
                {
                    selectedIssueStatusType = value;

                    OnPropertyChanged("SelectedIssueStatusType");
                }
            }
            get
            {
                return selectedIssueStatusType;
            }
        }

        List<IssueType> issueTypes;
        public List<IssueType> IssueTypes
        {
            set
            {
                if (issueTypes != value)
                {
                    issueTypes = value;

                    OnPropertyChanged("IssueTypes");
                    OnPropertyChanged("ShowIssueTypes");

                }
            }
            get
            {
                return issueTypes;
            }
        }
        public bool ShowIssueTypes
        {
            get
            {
                return IssueTypes != null && IssueTypes.Count > 0;
            }
        }
        IssueType selectedIssueType;
        public IssueType SelectedIssueType
        {
            set
            {
                if (selectedIssueType != value)
                {
                    selectedIssueType = value;

                    OnPropertyChanged("SelectedIssueType");
                }
            }
            get
            {
                return selectedIssueType;
            }
        }

        List<IssueSeverityType> issueSeverityTypes;
        public List<IssueSeverityType> IssueSeverityTypes
        {
            set
            {
                if (issueSeverityTypes != value)
                {
                    issueSeverityTypes = value;

                    OnPropertyChanged("IssueSeverityTypes");
                    OnPropertyChanged("ShowSeverityTypes");

                }
            }
            get
            {
                return issueSeverityTypes;
            }
        }

        public bool ShowSeverityTypes
        {
            get
            {
                return IssueSeverityTypes != null && IssueSeverityTypes.Count > 0;
            }
        }

        IssueSeverityType selectedIssueSeverityType;
        public IssueSeverityType SelectedIssueSeverityType
        {
            set
            {
                if (selectedIssueSeverityType != value)
                {
                    selectedIssueSeverityType = value;

                    OnPropertyChanged("SelectedIssueSeverityType");
                }
            }
            get
            {
                return selectedIssueSeverityType;
            }
        }

        List<IssuePriorityType> issuePriorityTypes;
        public List<IssuePriorityType> IssuePriorityTypes
        {
            set
            {
                if (issuePriorityTypes != value)
                {
                    issuePriorityTypes = value;

                    OnPropertyChanged("IssuePriorityTypes");
                    OnPropertyChanged("ShowPriorityTypes");
                }
            }
            get
            {
                return issuePriorityTypes;
            }
        }
        public bool ShowPriorityTypes
        {
            get
            {
                return IssuePriorityTypes != null && IssuePriorityTypes.Count > 0;
            }
        }

        IssuePriorityType selectedIssuePriorityType;
        public IssuePriorityType SelectedIssuePriorityType
        {
            set
            {
                if (selectedIssuePriorityType != value)
                {
                    selectedIssuePriorityType = value;

                    OnPropertyChanged("SelectedIssuePriorityType");
                }
            }
            get
            {
                return selectedIssuePriorityType;
            }
        }
    }
}
