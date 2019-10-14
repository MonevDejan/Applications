using ProjectInsight.Models.Comments;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProjectInsightMobile.ViewModels
{
    public class ApprovalRequestViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StatusIcon { get; set; }
        public string StatusColor { get; set; }


        public DateTime ApprovedDate { get; set; }
        public string RequestedBy { get; set; }

        public string RequestedDate { get; set; }
        public string DeadlineDate { get; set; }
        
        public bool isRequestedByVisible
        {
            get
            {
                if (RequestedBy != null && RequestedBy.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isDescriptionVisible
        {
            get
            {
                if (Description != null && Description.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isStatusVisible
        {
            get
            {
                if (Status != null && Status.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isDeadlineDateVisible
        {
            get
            {
                if (!string.IsNullOrEmpty(DeadlineDate))
                    return true;
                return false;
            }
        }
        public bool isRequestedDateVisible
        {
            get
            {
                if (!string.IsNullOrEmpty(RequestedDate))
                    return true;
                return false;
            }
        }
        public bool isApprovedDateVisible
        {
            get
            {
                if (ApprovedDate != null && ApprovedDate != DateTime.MinValue)
                    return true;
                return false;
            }
        }

        public bool VisibleApproveDeny
        {

            get
            {
                if (Status != null && Status == ProjectInsightMobile.Enums.ApprovalRequestStateType.Pending.ToString())
                    return true;
                return false;
            }
        }

        public ApprovalRequestViewModel()
        {
            ShowHideIcon = "Arrow_right.png";
            this.Id = string.Empty;
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