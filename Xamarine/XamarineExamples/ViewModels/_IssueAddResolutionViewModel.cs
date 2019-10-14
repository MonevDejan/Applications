using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using ProjectInsight.Models.ReferenceData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class IssueAddResolutionViewModel : BaseViewModel
    {
        public IssueAddResolutionViewModel()
        {
        
        }

       

        Guid? selectedIssueId;
        public Guid? SelectedIssueId
        {
            set
            {
                if (selectedIssueId != value)
                {
                    selectedIssueId = value;

                    OnPropertyChanged("SelectedIssueId");
                }
            }
            get
            {
                return selectedIssueId;
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
