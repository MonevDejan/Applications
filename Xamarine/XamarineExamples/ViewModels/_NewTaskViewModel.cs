using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class NewTaskViewModel : BaseViewModel
    {
        public NewTaskViewModel()
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
                }
            }
            get
            {
                return users;
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
        

    }
}
