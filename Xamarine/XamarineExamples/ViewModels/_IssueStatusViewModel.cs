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
    public class IssueStatusViewModel : BaseViewModel
    {
        public IssueStatusViewModel()
        {
        
        }

        IssueStatusType selectedStatusType;
        public IssueStatusType SelectedStatusType
        {
            set
            {
                if (selectedStatusType != value)
                {
                    selectedStatusType = value;

                    OnPropertyChanged("SelectedStatusType");
                }
            }
            get
            {
                return selectedStatusType;
            }
        }

        List<IssueStatusType> issueStatusTypes;
        public List<IssueStatusType> IssueStatusTypes
        {
            set
            {
                if (issueStatusTypes != value)
                {
                    issueStatusTypes = value;

                    OnPropertyChanged("IssueStatusTypes");
                }
            }
            get
            {
                return issueStatusTypes;
            }
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


        ObservableCollection<ProjectInsight.Models.Users.User> users;
        public ObservableCollection<ProjectInsight.Models.Users.User> Users
        {
            set
            {
                if (users != value)
                {
                    users = value;
                    OnPropertyChanged("Users");
                }
            }
            get
            {
                return users;
            }
        }

    }

}
