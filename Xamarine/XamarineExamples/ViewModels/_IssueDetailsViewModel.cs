using ProjectInsight.Models.Comments;
using ProjectInsight.Models.ReferenceData;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectInsightMobile.ViewModels
{
    public class IssueDetailsViewModel : BaseViewModel
    {
        public string Id { get; set; }

        public string ProjectName { get; set; }
        public string Priority { get; set; }
        public bool isPriorityVisible { get { return !string.IsNullOrEmpty(Priority); } }
        public string Severity { get; set; }
        public bool isSeverityVisible { get { return !string.IsNullOrEmpty(Severity); } }

        public string FoundBy { get; set; }
        public bool isFoundByVisible { get { return !string.IsNullOrEmpty(FoundBy); } }

        public DateTime? FoundDate { get; set; }
        public bool isFoundDateVisible { get { return !(FoundDate == null); } }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string UserAssignedBy { get; set; }
        public bool isUserAssignedByVisible { get { return !string.IsNullOrEmpty(UserAssignedBy); } }
        
        public string UserAssignedTo { get; set; }
        public bool isUserAssignedToVisible { get { return !string.IsNullOrEmpty(UserAssignedTo); } }

        public string Description { get; set; }
        public bool isDescriptionVisible
        {
            get
            {
                if (Description != null && Description.Length > 0)
                    return true;
                return false;
            }
        }
        string workStatus = string.Empty;
        public string WorkStatus
        {
            get
            {
                if (StatusType == null)
                {
                    return  "Status";
                }
                else
                    return StatusType.Name;
            }
           
        }


        IssueStatusType statusType { get; set; }
        public IssueStatusType StatusType
        {
            set
            {
                if (statusType != value)
                {
                    statusType = value;
                    OnPropertyChanged("StatusType");
                    OnPropertyChanged("WorkStatus");
                }
            }
            get
            {
                return statusType;
            }
        }



        Guid? project_Id { get; set; }
        public Guid? Project_Id
        {
            set
            {
                if (project_Id != value)
                {
                    project_Id = value;
                    OnPropertyChanged("Project_Id");
                }
            }
            get
            {
                return project_Id;
            }
        }

        Guid? userAssignedTo_Id { get; set; }
        public Guid? UserAssignedTo_Id
        {
            set
            {
                if (userAssignedTo_Id != value)
                {
                    userAssignedTo_Id = value;
                    OnPropertyChanged("UserAssignedTo_Id");
                }
            }
            get
            {
                return userAssignedTo_Id;
            }
        }

        public bool isIssueStatusTypeVisible { get { return !(StatusType == null); } }

        public IssueDetailsViewModel()
        {
            ShowHideIcon = "Arrow_right.png";
            this.Id = string.Empty;
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






        bool isExpandedList;
        public bool IsExpandedList
        {
            set
            {
                if (isExpandedList != value)
                {
                    isExpandedList = value;
                    OnPropertyChanged("IsExpandedList");
                }
            }
            get
            {
                return isExpandedList;
            }
        }

        string showHideIcon;
        public string ShowHideIcon
        {
            set
            {
                if (showHideIcon != value)
                {
                    showHideIcon = value;
                    OnPropertyChanged("ShowHideIcon");
                }
            }
            get
            {
                return showHideIcon;
            }
        }

    }
}