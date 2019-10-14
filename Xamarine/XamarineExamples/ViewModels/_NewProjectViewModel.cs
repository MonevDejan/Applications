using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class NewProjectViewModel : BaseViewModel
    {
        public NewProjectViewModel()
        {
        }

        ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectType> type;
        public ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectType> Types
        {
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged("Types");
                }
            }
            get
            {
                return type;
            }
        }

        ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectStatus> statuses;
        public ObservableCollection<ProjectInsight.Models.ReferenceData.ProjectStatus> Statuses
        {
            set
            {
                if (statuses != value)
                {
                    statuses = value;
                    OnPropertyChanged("Statuses");
                }
            }
            get
            {
                return statuses;
            }
        }
        ObservableCollection<myColor> colors;
        public ObservableCollection<myColor> Colors
        {
            set
            {
                if (colors != value)
                {
                    colors = value;
                    OnPropertyChanged("Colors");
                }
            }
            get
            {
                return colors;
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

        ProjectInsight.Models.ReferenceData.ProjectType selectedType;
        public ProjectInsight.Models.ReferenceData.ProjectType SelectedType
        {
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;

                    OnPropertyChanged("SelectedType");
                }
            }
            get
            {
                return selectedType;
            }
        }

        ProjectInsight.Models.ReferenceData.ProjectStatus selectedStatus;
        public ProjectInsight.Models.ReferenceData.ProjectStatus SelectedStatus
        {
            set
            {
                if (selectedStatus != value)
                {
                    selectedStatus = value;
                    OnPropertyChanged("SelectedStatus");
                }
            }
            get
            {
                return selectedStatus;
            }
        }

        myColor selectedColor;
        public myColor SelectedColor
        {
            set
            {
                if (selectedColor != value)
                {
                    selectedColor = value;

                    OnPropertyChanged("SelectedColor");
                }
            }
            get
            {
                return selectedColor;
            }
        }

       public class myColor
        {
            public string Id { get; set; }
            public string Name { get; set; }

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
    }
}
