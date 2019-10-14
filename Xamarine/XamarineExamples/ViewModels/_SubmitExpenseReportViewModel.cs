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

    public class SubmitExpenseReportViewModel : BaseViewModel
    {
       // public ExpenseReport ExpenseReport { get; set; }

        List<ProjectInsight.Models.Users.User> expenseReportApprovers { get; set; }
        public List<ProjectInsight.Models.Users.User> ExpenseReportApprovers
        {
            set
            {
                if (expenseReportApprovers != value)
                {
                    expenseReportApprovers = value;

                    OnPropertyChanged("ExpenseReportApprovers");
                }
            }
            get
            {
                return expenseReportApprovers;
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

        bool canSelectExpenseReportApprover;
        public bool CanSelectExpenseReportApprover
        {
            set
            {
                if (canSelectExpenseReportApprover != value)
                {
                    canSelectExpenseReportApprover = value;

                    OnPropertyChanged("CanSelectExpenseReportApprover");
                }
            }
            get
            {
                return canSelectExpenseReportApprover;
            }
        }

        decimal actualCost;
        public decimal ActualCost
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
        string periodFormated;
        public string PeriodFormated
        {
            set
            {
                if (periodFormated != value)
                {
                    periodFormated = value;

                    OnPropertyChanged("PeriodFormated");
                }
            }
            get
            {
                return periodFormated;
            }
        }
        DateTime earliestExpenseEntryDate;
        public DateTime EarliestExpenseEntryDate
        {
            set
            {
                if (earliestExpenseEntryDate != value)
                {
                    earliestExpenseEntryDate = value;
                    OnPropertyChanged("EarliestExpenseEntryDate");
                }
            }
            get
            {
                return earliestExpenseEntryDate;
            }
        }
    }

  
}