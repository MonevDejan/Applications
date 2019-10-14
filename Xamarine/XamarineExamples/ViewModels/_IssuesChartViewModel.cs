using MvvmHelpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ProjectInsight.Models.Issues;
using System.Reflection;

namespace ProjectInsightMobile.ViewModels
{
    public class IssuesChartViewModel : MvvmHelpers.BaseViewModel
    {

        public Guid Id { get; set; }
        public bool isWebLink { get; set; }

        public class IssueChartItem
        {
            public string Name { get; set; }

            public double Value { get; set; }
        }
        //public List<Person> Data { get; set; }
        public IssuesChartViewModel()
        {
            Items = new ObservableCollection<Issue>();
            ShowHideIcon = "Arrow_right.png";
          
        }
        ObservableCollection<IssueChartItem> data;
        public ObservableCollection<IssueChartItem> Data
        {
            get { return data; }
            set { SetProperty(ref data, value); }


        }

        ObservableCollection<Issue> items;
        public ObservableCollection<Issue> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }


        }
        Issue issue;
        public Issue Issue
        {
            get { return issue; }
            set { SetProperty(ref issue, value); }
        }                   

        string issuesLabel = "Issues";
        public string IssuesLabel
        {
            get { return issuesLabel; }
            set { SetProperty(ref issuesLabel, value); }
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
