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
    public class RelatedItemsViewModel : MvvmHelpers.BaseViewModel
    {
        //public ObservableCollection<RelatedItem> Items { get; set; }

        public Guid Id { get; set; }
        public bool isWebLink { get; set; }
        public RelatedItemsViewModel()
        {
            Items = new ObservableCollection<RelatedItem>();
            ShowHideIcon = "Arrow_right.png";
        }        
        ObservableCollection<RelatedItem> items;
        public ObservableCollection<RelatedItem> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }


        }
        RelatedItem relatedItem;
        public RelatedItem RelatedItem
        {
            get { return relatedItem; }
            set { SetProperty(ref relatedItem, value); }
        }                   

        string relatedItemsLabel = "Related Items";
        public string RelatedItemsLabel
        {
            get { return relatedItemsLabel; }
            set { SetProperty(ref relatedItemsLabel, value); }
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
