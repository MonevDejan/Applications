using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectInsightMobile.ViewModels
{
    public class ExpenseEntryDetailsViewModel : BaseViewModel
    {
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
        string firstLine;
        public string FirstLine
        {
            set
            {
                if (firstLine != value)
                {
                    firstLine = value;

                    OnPropertyChanged("FirstLine");
                }
            }
            get
            {
                return firstLine;
            }
        }
        string secondLine;
        public string SecondLine
        {
            set
            {
                if (secondLine != value)
                {
                    secondLine = value;

                    OnPropertyChanged("SecondLine");
                }
            }
            get
            {
                return secondLine;
            }
        }
        string thirdLine;
        public string ThirdLine
        {
            set
            {
                if (thirdLine != value)
                {
                    thirdLine = value;

                    OnPropertyChanged("ThirdLine");
                }
            }
            get
            {
                return thirdLine;
            }
        }

        decimal? actualCost;
        public decimal? ActualCost
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
    }
    public class ExpenseReportDetailsViewModel : BaseViewModel
    {
        public ExpenseReportDetailsViewModel()
        {
            this.isCurrent = false;
        }

        ObservableCollection<ExpenseEntryDetailsViewModel> expenseEntries;
        public ObservableCollection<ExpenseEntryDetailsViewModel> ExpenseEntries
        {
            set
            {
                if (expenseEntries != value)
                {
                    expenseEntries = value;

                    OnPropertyChanged("ExpenseEntries");
                }
            }
            get
            {
                return expenseEntries;
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

        bool isCurrent;
        public bool IsCurrent 
        {
            set
            {
                if (isCurrent != value)
                {
                    isCurrent = value;

                    OnPropertyChanged("IsCurrent");
                }
            }
            get
            {
                return isCurrent;
            }
        }

        
             bool isVisibleSubmitButton;
        public bool IsVisibleSubmitButton
        {
            set
            {
                if (isVisibleSubmitButton != value)
                {
                    isVisibleSubmitButton = value;

                    OnPropertyChanged("IsVisibleSubmitButton");
                }
            }
            get
            {
                return isVisibleSubmitButton;
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
        

              string approvalStatusDescription;
        public string ApprovalStatusDescription
        {
            set
            {
                if (approvalStatusDescription != value)
                {
                    approvalStatusDescription = value;

                    OnPropertyChanged("ApprovalStatusDescription");
                }
            }
            get
            {
                return approvalStatusDescription;
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

        bool isEnabledEditEntries = false;
        public bool IsEnabledEditEntries
        {
            set
            {
                if (isEnabledEditEntries != value)
                {
                    isEnabledEditEntries = value;

                    OnPropertyChanged("IsEnabledEditEntries");
                }
            }
            get
            {
                return isEnabledEditEntries;
            }
        }


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

        string firstLine;
        public string FirstLine
        {
            set
            {
                if (firstLine != value)
                {
                    firstLine = value;
                    OnPropertyChanged("FirstLine");
                }
            }
            get
            {
                return firstLine;
            }
        }

    }      
}
