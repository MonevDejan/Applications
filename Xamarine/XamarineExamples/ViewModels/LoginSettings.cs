using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.ViewModels
{
    public class LoginSettings: BaseViewModel
    {
       
        bool emailPasswordEnabled;
        public bool EmailPasswordEnabled
        {
            set
            {
                if (emailPasswordEnabled != value)
                {
                    emailPasswordEnabled = value;

                    OnPropertyChanged("EmailPasswordEnabled");
                }
            }
            get
            {
                return emailPasswordEnabled;
            }
        }
        bool ssoEnabled;
        public bool SSOEnabled
        {
            set
            {
                if (ssoEnabled != value)
                {
                    ssoEnabled = value;

                    OnPropertyChanged("SSOEnabled");
                }
            }
            get
            {
                return ssoEnabled;
            }
        }

        String workspaceName;
        public String WorkspaceName
        {
            set
            {
                if (workspaceName != value)
                {
                    workspaceName = value;

                    OnPropertyChanged("WorkspaceName");
                }
            }
            get
            {
                return workspaceName;
            }
        }
    }
}
