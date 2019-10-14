using ProjectInsight.Models.Comments;
using ProjectInsightMobile.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;

namespace ProjectInsightMobile.ViewModels
{
    public class MyWorkItem : BaseViewModel
    {
        [PrimaryKey, AutoIncrement]
        public int lId { get; set; }
        public Guid Id { get; set; }
        public string Icon { get; set; }
        //public string Description { get; set; }
        //public string StartEndDate { get; set; } = string.Empty;

        //public string Line2 { get; set; } = string.Empty;
        public string Line2s { get; set; } = string.Empty;
        public string Line2e { get; set; } = string.Empty;


        public string Line3 { get; set; } = string.Empty;
        public string Line4 { get; set; } = string.Empty;



        public string Url { get; set; } = string.Empty;
        public int  WorkspaceId { get; set; }
        public Guid ProjectId { get; set; }
        public ItemType ItemType { get; set; }

        public string ObjectType { get; set; }


        public bool IsActive { get; set; }

        [Ignore]
        public string TitleColor { get; set; }

        public bool isHeader { get; set; }
        public bool isItem { get; set; } = true;

        
        #region Check value for all controls

        public bool isLine2Visible
        {
            get
            {
                if ((Line2s != null && Line2s.Length > 0)|| (Line2e != null && Line2e.Length > 0))
                    return true;
                return false;
            }

        }
        public bool isLine3Visible
        {
            get
            {
                if (Line3 != null && Line3.Length > 0)
                    return true;
                return false;
            }
        }
        public bool isLine4Visible
        {
            get
            {
                if (Line4 != null && Line4.Length > 0)
                    return true;
                return false;
            }
        }

        #endregion

        string line2Color { get; set; } = "#000000";
        public string Line2Color
        {
            set
            {
                if (line2Color != value)
                {
                    line2Color = value;
                    OnPropertyChanged("Line2Color");
                }
            }
            get
            {
                return line2Color;
            }
        }

        int tasksCount { get; set; } = 0;
        public int TasksCount
        {
            set
            {
                if (tasksCount != value)
                {
                    tasksCount = value;
                    OnPropertyChanged("TasksCount");
                }
            }
            get
            {
                return tasksCount;
            }
        }

        public bool isTasksCountVisible
        {
            get
            {
                if (TasksCount> 0)
                    return true;
                return false;
            }

        }
    }

    public enum ItemType
    {
        Task = 1,
        Todo = 2,
        Issue = 3,
        ApprovalRequests = 4,
        CustomItem = 5,
        Proposals = 6,
        Projects = 7,
        TimeSheet = 8



    }
}