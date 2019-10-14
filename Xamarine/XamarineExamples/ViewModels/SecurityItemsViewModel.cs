using MvvmHelpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ProjectInsight.Models.Items;
using System.Reflection;

namespace ProjectInsightMobile.ViewModels
{
    public class SecurityItemsViewModel : MvvmHelpers.BaseViewModel
    {                                                                     

        public Guid Id { get; set; }        

        public SecurityItemsViewModel()
        {                                                    
            ShowHideIcon = "plus.png";
        }      
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

        bool isExpandedList;
        public bool IsExpandedList
        {
            get { return isExpandedList; }
            set { SetProperty(ref isExpandedList, value); }
        }

        string showHideIcon;
        public string ShowHideIcon
        {
            get { return showHideIcon; }
            set { SetProperty(ref showHideIcon, value); }
        }
    }
}
