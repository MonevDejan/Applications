using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectInsightMobile.ViewModels
{
    public class ExpenseReportsViewModel : BaseViewModel
    {
        public ExpenseReportsViewModel()
        {
        }

        ObservableCollection<ProjectInsight.Models.TimeAndExpense.ExpenseReport> expenseReports;
        public ObservableCollection<ProjectInsight.Models.TimeAndExpense.ExpenseReport> ExpenseReports
        {
            set
            {
                if (expenseReports != value)
                {
                    expenseReports = value;

                    OnPropertyChanged("ExpenseReports");
                }
            }
            get
            {
                return expenseReports;
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

        decimal unsubmittedExpenseTotal;
        public decimal UnsubmittedExpenseTotal
        {
            set
            {
                if (unsubmittedExpenseTotal != value)
                {
                    unsubmittedExpenseTotal = value;
                    OnPropertyChanged("UnsubmittedExpenseTotal");
                }
            }
            get
            {
                return unsubmittedExpenseTotal;
            }
        }

        string unsubmittedPeriodFormated;
        public string UnsubmittedPeriodFormated
        {
            set
            {
                if (unsubmittedPeriodFormated != value)
                {
                    unsubmittedPeriodFormated = value;

                    OnPropertyChanged("UnsubmittedPeriodFormated");
                }
            }
            get
            {
                return unsubmittedPeriodFormated;
            }
        }

    }      
}
