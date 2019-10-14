using ProjectInsight.Models.Companies;
using ProjectInsight.Models.Projects;
using ProjectInsight.Models.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ProjectInsightMobile.ViewModels
{
    public class NewApprovalViewModel : BaseViewModel
    {
        public NewApprovalViewModel()
        {
            DeadlineDate = DateTime.Now.Date;
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

        Guid? container_Id;
        public Guid? Container_Id
        {
            set
            {
                if (container_Id != value)
                {
                    container_Id = value;
                    OnPropertyChanged("Container_Id");
                }
            }
            get
            {
                return container_Id;
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
        bool nameError = false;
        public bool NameError
        {
            set
            {
                if (nameError != value)
                {
                    nameError = value;
                    OnPropertyChanged("NameError");
                }
            }
            get
            {
                return nameError;
            }
        }

        string description;
        public string Description
        {
            set
            {
                if (description != value)
                {
                    description = value;

                    OnPropertyChanged("Description");
                }
            }
            get
            {
                return description;
            }
        }

        DateTime deadlineDate;
        public DateTime DeadlineDate
        {
            set
            {
                if (deadlineDate != value)
                {
                    deadlineDate = value;

                    OnPropertyChanged("DeadlineDate");
                }
            }
            get
            {
                return deadlineDate;
            }
        }



        ObservableCollection<ProjectInsight.Models.Projects.Project> parentItems;
        public ObservableCollection<ProjectInsight.Models.Projects.Project> ParentItems
        {
            set
            {
                if (parentItems != value)
                {
                    parentItems = value;
                    OnPropertyChanged("ParentItems");
                }
            }
            get
            {
                return parentItems;
            }
        }

        ProjectInsight.Models.Projects.Project selectedParentItem;
        public ProjectInsight.Models.Projects.Project SelectedParentItem
        {
            set
            {
                if (selectedParentItem != value)
                {
                    selectedParentItem = value;
                    OnPropertyChanged("SelectedParentItem");
                }
            }
            get
            {
                return selectedParentItem;
            }
        }



        ObservableCollection<Approver> approvers;
        public ObservableCollection<Approver> Approvers
        {
            set
            {
                if (approvers != value)
                {
                    approvers = value;
                    OnPropertyChanged("Approvers");
                }
            }
            get
            {
                return approvers;
            }
        }

        List<Approver> selectedApprovers;
        public List<Approver> SelectedApprovers
        {
            set
            {
                if (selectedApprovers != value)
                {
                    selectedApprovers = value;

                    OnPropertyChanged("SelectedApprovers");

                    if ( selectedApprovers !=null && selectedApprovers.Count>1)
                        isOptionsVisible = true;
                    else
                        isOptionsVisible = false;
                    OnPropertyChanged("IsOptionsVisible");

                }
            }
            get
            {
                return selectedApprovers;
            }
        }

        bool selectedApproversError = false;
        public bool SelectedApproversError
        {
            set
            {
                if (selectedApproversError != value)
                {
                    selectedApproversError = value;
                    OnPropertyChanged("SelectedApproversError");
                }
            }
            get
            {
                return selectedApproversError;
            }
        }
        bool areAllApproversRequired = false;
        public bool AreAllApproversRequired
        {
            set
            {
                if (areAllApproversRequired != value)
                {
                    areAllApproversRequired = value;
                    OnPropertyChanged("AreAllApproversRequired");
                    if (!areAllApproversRequired)
                    {
                        isSequentialApproval = false;
                        OnPropertyChanged("IsSequentialApproval");
                    }
                }
            }
            get
            {
                return areAllApproversRequired;
            }
        }
        
             bool isSequentialApproval = false;
        public bool IsSequentialApproval
        {
            set
            {
                if (isSequentialApproval != value)
                {
                    isSequentialApproval = value;
                    OnPropertyChanged("IsSequentialApproval");
                    if (isSequentialApproval)
                    {
                        areAllApproversRequired = true;
                        OnPropertyChanged("AreAllApproversRequired");
                    }
                }
            }
            get
            {
                return isSequentialApproval;
            }
        }
        bool isOptionsVisible = false;
        public bool IsOptionsVisible
        {
            set
            {
                if (isOptionsVisible != value)
                {
                    isOptionsVisible = value;
                    OnPropertyChanged("IsOptionsVisible");
                   
                }
            }
            get
            {
                return isOptionsVisible;
            }
        }

        List<Project> projects;
        public List<Project> Projects
        {
            set
            {
                if (projects != value)
                {
                    projects = value;
                    OnPropertyChanged("Projects");
                }
            }
            get
            {
                return projects;
            }
        }

        Project selectedProject;
        public Project SelectedProject
        {
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    OnPropertyChanged("SelectedProject");
                }
            }
            get
            {
                return selectedProject;
            }
        }

        bool selectedProjectError = false;
        public bool SelectedProjectError
        {
            set
            {
                if (selectedProjectError != value)
                {
                    selectedProjectError = value;
                    OnPropertyChanged("SelectedProjectError");
                }
            }
            get
            {
                return selectedProjectError;
            }
        }

        //private List<object> selectedItem;
        //public List<object> SelectedItem1
        //{
        //    get { return selectedItem; }
        //    set
        //    {
        //        selectedItem = value;
        //        OnPropertyChanged("SelectedItem1");
        //    }
        //}


        public class Approver
        {

            private string avatarInitials;
            public string AvatarInitials
            {
                get { return avatarInitials; }
                set
                {
                    avatarInitials = value;
                }
            }

            private string avatarColor;
            public string AvatarColor
            {
                get { return avatarColor; }
                set
                {
                    avatarColor = value;
                }
            }
            private string photoURL;
            public string PhotoURL
            {
                get { return photoURL; }
                set
                {
                    photoURL = value;
                }
            }
            private string name;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            private Guid id;
            public Guid Id
            {
                get { return id; }
                set { id = value; }
            }
            private bool showAvatar;
            public bool ShowAvatar
            {
                get { return showAvatar; }
                set
                {
                    showAvatar = value;
                }
            }
            private bool showImage;
            public bool ShowImage
            {
                get { return showImage; }
                set
                {
                    showImage = value;
                }
            }
        }
    }
}
