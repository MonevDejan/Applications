using MvvmHelpers;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ProjectInsight.Models.Tasks;
using System.Reflection;

namespace ProjectInsightMobile.ViewModels
{
    public class TasksChartViewModel : MvvmHelpers.BaseViewModel
    {

        public Guid Id { get; set; }
        public bool isWebLink { get; set; }

        public class TaskChartItem
        {
            public string Name { get; set; }

            public double Height { get; set; }
        }
        //public List<Person> Data { get; set; }
        public TasksChartViewModel()
        {
            Items = new ObservableCollection<Task>();
            ShowHideIcon = "Arrow_right.png";
          
        }
        ObservableCollection<TaskChartItem> data;
        public ObservableCollection<TaskChartItem> Data
        {
            get { return data; }
            set { SetProperty(ref data, value); }


        }

        ObservableCollection<Task> items;
        public ObservableCollection<Task> Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }


        }
        Task task;
        public Task Task
        {
            get { return task; }
            set { SetProperty(ref task, value); }
        }                   

        string tasksLabel = "All Tasks";
        public string TasksLabel
        {
            get { return tasksLabel; }
            set { SetProperty(ref tasksLabel, value); }
        }

        string loadingMessage;
        public string LoadingMessage
        {
            get { return loadingMessage; }
            set { SetProperty(ref loadingMessage, value); }
        }

        bool hasPermissions;
        public bool HasPermissions
        {
            get { return hasPermissions; }
            set { SetProperty(ref hasPermissions, value); }
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
