using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class TaskStatusViewModel : BaseViewModel
    {
        public TaskStatusViewModel()
        {
        
        }

        ProjectInsight.Models.ReferenceData.WorkPercentCompleteType selectedWorkPercentCompleteType;
        public ProjectInsight.Models.ReferenceData.WorkPercentCompleteType SelectedWorkPercentCompleteType
        {
            set
            {
                if (selectedWorkPercentCompleteType != value)
                {
                    selectedWorkPercentCompleteType = value;

                    OnPropertyChanged("SelectedWorkPercentCompleteType");
                }
            }
            get
            {
                return selectedWorkPercentCompleteType;
            }
        }

        List<ProjectInsight.Models.ReferenceData.WorkPercentCompleteType> workPercentCompleteTypes;
        public List<ProjectInsight.Models.ReferenceData.WorkPercentCompleteType> WorkPercentCompleteTypes
        {
            set
            {
                if (workPercentCompleteTypes != value)
                {
                    workPercentCompleteTypes = value;

                    OnPropertyChanged("WorkPercentCompleteTypes");
                }
            }
            get
            {
                return workPercentCompleteTypes;
            }
        }

        Guid? selectedTaskId;
        public Guid? SelectedTaskId
        {
            set
            {
                if (selectedTaskId != value)
                {
                    selectedTaskId = value;

                    OnPropertyChanged("SelectedTaskId");
                }
            }
            get
            {
                return selectedTaskId;
            }
        }

        String comment;
        public String Comment
        {
            set
            {
                if (comment != value)
                {
                    comment = value;

                    OnPropertyChanged("Comment");
                }
            }
            get
            {
                return comment;
            }
        }



    }


}
