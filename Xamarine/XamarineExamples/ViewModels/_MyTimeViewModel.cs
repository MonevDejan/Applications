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
    public class MyTimeViewModel : BaseViewModel
    {
        //public TimeSheetPeriod CurrentPeriod { get; set; }
        TimeSheetPeriod currentPeriod;
        public TimeSheetPeriod CurrentPeriod
        {
            set
            {
                    currentPeriod = value;
                    OnPropertyChanged("CurrentPeriod");
            }
            get { return currentPeriod; }
        }

        ObservableCollection<TimeSheetPeriod> pastPeriods;
        public ObservableCollection<TimeSheetPeriod> PastPeriods
        {
            set
            {
                if (pastPeriods != value)
                {
                    pastPeriods = value;

                    OnPropertyChanged("PastPeriods");
                }
            }
            get
            {
                return pastPeriods;
            }
        }
        public string Id { get; set; }
        public String CurrentUnsubmittedTime { get; set; }
        public String LastPeriodSubmittedTime { get; set; }
        public String LastPeriodSubmittedTimeFormated
        {

            get
            {
                return String.Format("[Last Week: {0} hrs]", LastPeriodSubmittedTime.ToString());
            }
        }
        public MyTimeViewModel()
        {
            this.Id = string.Empty;
            
            CurrentPeriod = new TimeSheetPeriod();
        }
    }
    public class ApproveTimeSheetViewModel : BaseViewModel
    {
        //public TimeSheetPeriod CurrentPeriod { get; set; }
        TimeSheetPeriod currentPeriod;
        public TimeSheetPeriod CurrentPeriod
        {
            set
            {
                currentPeriod = value;
                OnPropertyChanged("CurrentPeriod");
            }
            get { return currentPeriod; }
        }
        public List<TimeEntry> TimeEntries { get; set; }

        public string Id { get; set; }
        public ApproveTimeSheetViewModel()
        {
            this.Id = string.Empty;
            CurrentPeriod = new TimeSheetPeriod();
        }

        public TimeSheet timeSheet { get; set; }


        bool showStatus = false;
        public bool ShowStatus
        {
            set
            {
                if (showStatus != value)
                {
                    showStatus = value;

                    OnPropertyChanged("ShowStatus");
                }
            }
            get
            {
                return showStatus;
            }
        }

    }
    public class ApproveTimesheetViewModel : BaseViewModel
    {
        public ObservableCollection<TimeSheetPeriod> TimeSheets { get; set; }
        public ApproveTimesheetViewModel()
        {
            TimeSheets = new ObservableCollection<TimeSheetPeriod>();
        }
    }

    public class SubmitTimeSheetViewModel : BaseViewModel
    {
        public TimeSheetPeriod TimeSheet { get; set; }
        //public List<ProjectInsight.Models.Users.User> TimeSheetApprovers { get; set; }
        List<ProjectInsight.Models.Users.User> timeSheetApprovers;
        public List<ProjectInsight.Models.Users.User> TimeSheetApprovers
        {
            set
            {
                if (timeSheetApprovers != value)
                {
                    timeSheetApprovers = value;

                    OnPropertyChanged("TimeSheetApprovers");
                }
            }
            get
            {
                return timeSheetApprovers;
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

        bool canSelectTimeSheetApprover;
        public bool CanSelectTimeSheetApprover
        {
            set
            {
                if (canSelectTimeSheetApprover != value)
                {
                    canSelectTimeSheetApprover = value;

                    OnPropertyChanged("CanSelectTimeSheetApprover");
                }
            }
            get
            {
                return canSelectTimeSheetApprover;
            }
        }

    }

        public class TimeSheetPeriod : BaseViewModel
    {
        public Guid? TimeSheetId { get; set; }
        public Guid? User_Id { get; set; }
        public decimal? ActualHours { get; set; }
        DateTime startDate;
        public DateTime StartDate
        {
            set
            {
                    startDate = value;
                    OnPropertyChanged("StartDate");
                OnPropertyChanged("PeriodFormated");
            }
            get { return startDate; }
        }
        public bool UseHoursFormat { get; set; }
        DateTime endDate;
        public DateTime EndDate
        {
            set
            {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                OnPropertyChanged("PeriodFormated");
            }
            get { return endDate; }
        }
        public List<TimeEntry> TimeEntries { get; set; }

        // Remarks:
        //     Possible Values: 0 - Unsubmitted 1 - Pending PM(s) Approval 2 - Pending - Approved
        //     by PMs 3 - Rejected By PM(s) 4 - Pending 5 - Approved 6 - Rejected 7 - Cancelled
        //     PM Approval
        public int? Status { get; set; }
        public string Name { get; set; }

        public string UserAvatar { get; set; }
        public string UserPhotoUrl { get; set; }
        


        private string actualHoursFormated;
        public string ActualHoursFormated
        {
            get { return actualHoursFormated; }
            set { actualHoursFormated = value; OnPropertyChanged("ActualHoursFormated"); }
        }

      

        public string PeriodFormated
        {
            get
            {
                if (StartDate == new DateTime() || EndDate == new DateTime())
                    return String.Empty;
                else
                    return string.Format("{0} - {1}", StartDate.ToString("M/d/yy"), EndDate.ToString("M/d/yy"));

            }
        }

        public string PeriodFormatedLong
        {
            get
            {
                return string.Format("{0} - {1}", StartDate.ToString("ddd M/d/yy"), EndDate.ToString("ddd M/d/yy"));
            }
        }


        //Possible Values: 
        //0 - Unsubmitted 
        //1 - Pending PM(s) Approval 
        //2 - Pending - Approved by PMs 
        //3 - Rejected By PM(s) 
        //4 - Pending 
        //5 - Approved 
        //6 - Rejected 
        //7 - Cancelled PM Approval


        //public bool ShowSubmitButton
        //{
        //    get
        //    {
        //        if (this.Status == 0 && ActualHours != null && ActualHours > 0)
        //            return true;
        //        return false;
        //    }
        //}
        //public bool ShowSubmitButton { get; set; } = false;
        private bool showSubmitButton = false;
        public bool ShowSubmitButton
        {
            get { return showSubmitButton; }
            set { showSubmitButton = value; OnPropertyChanged("ShowSubmitButton"); }
        }
        public bool ShowApprovedIcon
        {
            get
            {
                if (Status == 5) return true;
                return false;
            }
        }

        public bool ShowRejectedIcon
        {
            get
            {
                if (Status == 3 || Status == 6) return true;
                return false;
            }
        }

        public bool ShowPendingApprovalIcon
        {
            get
            {
                if (Common.UserGlobalCapability.IsTimeSheetApprover && (Status == 1 || Status == 2 || Status == 4)) return true;
                return false;
            }
        }


        bool requiresApprovalFromCurrentUser = false;
        public bool RequiresApprovalFromCurrentUser
        {
            set
            {
                if (requiresApprovalFromCurrentUser != value)
                {
                    requiresApprovalFromCurrentUser = value;

                    OnPropertyChanged("RequiresApprovalFromCurrentUser");
                }
            }
            get
            {
                return requiresApprovalFromCurrentUser;
            }
        }

    }
}