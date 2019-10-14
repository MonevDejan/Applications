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

    public class CompanyFilterViewModel : BaseViewModel
    {
       // public ExpenseReport ExpenseReport { get; set; }

        List<ProjectInsight.Models.Companies.Company> companies { get; set; }
        public List<ProjectInsight.Models.Companies.Company> Companies
        {
            set
            {
                if (companies != value)
                {
                    companies = value;

                    OnPropertyChanged("Companies");
                }
            }
            get
            {
                return companies;
            }
        }
        ProjectInsight.Models.Companies.Company selectedCompany;
        public ProjectInsight.Models.Companies.Company SelectedCompany
        {
            set
            {
                if (selectedCompany != value)
                {
                    selectedCompany = value;

                    OnPropertyChanged("SelectedCompany");
                }
            }
            get
            {
                return selectedCompany;
            }
        }

       
    }

  
}