using ProjectInsight.Models.Comments;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ProjectInsightMobile.ViewModels
{

    public class ProjectSelectViewModel : BaseViewModel
    {
       // public ExpenseReport ExpenseReport { get; set; }

        List<ProjectInsight.Models.Projects.Project> projects { get; set; }
        public List<ProjectInsight.Models.Projects.Project> Projects
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

       
    }

  
}