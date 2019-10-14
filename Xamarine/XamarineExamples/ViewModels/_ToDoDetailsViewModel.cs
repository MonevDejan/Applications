using ProjectInsight.Models.Comments;
using ProjectInsight.Models.ReferenceData;
using ProjectInsight.Models.Users;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;

namespace ProjectInsightMobile.ViewModels
{
    public class ToDoDetailsViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Date { get; set; }
        public string Priority { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime ActualDates { get; set; }

        string projectName;
        public string ProjectName
        {
            set
            {
                if (projectName != value)
                {
                    projectName = value;
                    OnPropertyChanged("ProjectName");
                }
            }
            get
            {
                return projectName;
            }
        }




        public string WorkStatus { get; set; }
        public User UserCreated { get; set; }
        //public User UserAssignedBy { get; set; }            
        //public User UserAssignedTo { get; set; }

        public string UserAssignedBy { get; set; }
        public string UserAssignedTo { get; set; }
        public decimal WorkPercentComplete { get; set; }
        public WorkPercentCompleteType WorkPercentCompleteType { get; set; }
        public ToDoDetailsViewModel()
        {
            this.Id = string.Empty;
        }
        public bool isDone
        {

            get
            {
                if (WorkPercentComplete == 100)
                    return false;
                return true;
            }
        }
        public string StartEndDate
        {

            get
            {
                var startDate = string.Empty;
                if (StartDate != null && StartDate != DateTime.MinValue)
                {
                    startDate = StartDate.ToString("M/d/yy hh tt");
                }
                var endDate = string.Empty;
                if (EndDate != null && EndDate != DateTime.MinValue)
                {
                    endDate = EndDate.Value.ToString("M/d/yy hh tt");
                }
                if (string.IsNullOrEmpty(endDate))
                    return string.Format("{0}", startDate);
                else
                {
                    if (startDate != endDate)
                        return string.Format("{0} - {1}", startDate, endDate);
                    else
                        return string.Format("{0}", startDate);

                }
            }
        }
        public bool isStartEndDateVisible
        {
            get
            {
                if (StartEndDate != null && StartEndDate.Length > 0)
                    return true;
                return false;
            }
        }
        #region Check value for all controls
        public bool isDescriptionVisible
        {
            get
            {
                if (Description != null && Description.Length > 0)
                    return true;
                return false;
            }
        }
       
        public bool isPriorityVisible
        {
            get
            {
                if (Priority != null && Priority.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isIconVisible
        {
            get
            {
                if (Icon != null && Icon.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isProjectNameVisible
        {
            get
            {
                if (ProjectName != null && ProjectName.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isWorkStatusVisible
        {
            get
            {
                if (WorkStatus != null && WorkStatus.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isDateVisible
        {
            get
            {
                if (Date != null && Date.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isStartDateVisible
        {
            get
            {
                if (StartDate != null && StartDate != DateTime.MinValue)
                    return true;
                return false;
            }
        }
        public bool isEndDateVisible
        {
            get
            {
                if (EndDate != null && EndDate != DateTime.MinValue)
                    return true;
                return false;
            }
        }
        public bool isActualDatesVisible
        {
            get
            {
                if (ActualDates != null && ActualDates != DateTime.MinValue)
                    return true;
                return false;
            }
        }
        #endregion
        //public string AssignedBy
        //{                      
        //    get
        //    {
        //        return string.Format("{0} {1}", UserAssignedBy.FirstName, UserAssignedBy.LastName);
        //    }
        //}

        //public string AssignedTo
        //{

        //    get
        //    {
        //        return string.Format("{0} {1}", UserAssignedTo.FirstName, UserAssignedTo.LastName);
        //    }
        //}
    }
}