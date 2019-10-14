using ProjectInsight.Models.Comments;
using ProjectInsightMobile.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;

namespace ProjectInsightMobile.ViewModels
{
    public class TaskListItem : BaseViewModel
    {
        public Guid Id { get; set; }

        string icon { get; set; }
        public string Icon
        {
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged("Icon");
                }
            }
            get
            {
                return icon;
            }
        }
        public string Line2s { get; set; } = string.Empty;
        public string Line2e { get; set; } = string.Empty;

        public string Line3 { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public int TaskScheduleCurrentState { get; set; }

        //int rowHeight { get; set; } 
        //public int RowHeight
        //{
        //    set
        //    {
        //        if (rowHeight != value)
        //        {
        //            rowHeight = value;
        //            OnPropertyChanged("RowHeight");
        //        }
        //    }
        //    get
        //    {
        //        return rowHeight;
        //    }
        //}

        bool isHeader { get; set; } = false;
        public bool IsHeader
        {
            set
            {
                if (isHeader != value)
                {
                    isHeader = value;
                    OnPropertyChanged("IsHeader");
                }
            }
            get
            {
                return isHeader;
            }
        }

        string line2Color { get; set; } 
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

        bool isItemVisible { get; set; } = true;
        public bool IsItemVisible
        {
            set
            {
                if (isItemVisible != value)
                {
                    isItemVisible = value;
                    OnPropertyChanged("IsItemVisible");
                }
            }
            get
            {
                return isItemVisible;
            }
        }

        //bool isVisible { get; set; }
        //public bool IsVisible
        //{
        //    set
        //    {
        //        if (isVisible != value)
        //        {
        //            isVisible = value;
        //            OnPropertyChanged("IsVisible");
        //        }
        //    }
        //    get
        //    {
        //        return isVisible;
        //    }
        //}
        bool isExpanded { get; set; }
        public bool IsExpanded
        {
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
            get
            {
                return isExpanded;
            }
        }
        public bool isLine2Visible
        {
            get
            {
                if ((Line2s != null && Line2s.Length > 0) || (Line2e != null && Line2e.Length > 0))
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


    }
}
