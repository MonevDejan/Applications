using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Items;
using ProjectInsight.Models.Projects;
using ProjectInsight.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class ApprovalSearchItemViewModel : BaseViewModel
    {
        public ApprovalSearchItemViewModel()
        {

        }



        List<ItemSearchResult> searchResult;
        public List<ItemSearchResult> SearchResult
        {
            set
            {
                if (searchResult != value)
                {
                    searchResult = value;
                    OnPropertyChanged("SearchResult");
                }
            }
            get
            {
                return searchResult;
            }
        }

    }
}
