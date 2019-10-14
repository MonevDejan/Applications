using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectInsightMobile.ViewModels
{
    public class CustomBaseViewModel : MvvmHelpers.BaseViewModel
    {
        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }
        bool isVisibleLoad;
        public bool VisibleLoad
        {
            get { return isVisibleLoad; }
            set { SetProperty(ref isVisibleLoad, value); }
        }

        public LoginSettings LoginSettings;

    }
}
