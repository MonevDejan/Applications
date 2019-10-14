using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using ProjectInsightMobile.Models;
using ProjectInsightMobile.Views;
using ProjectInsight.Models.Users;
using System.Linq;
using ProjectInsightMobile.Helpers;
using System.Collections.Generic;
using ProjectInsightMobile.Services;
using ProjectInsight.Models.Base;
using System.ComponentModel;

namespace ProjectInsightMobile.ViewModels
{
    public class TasksViewModel : BaseViewModel
    {
        public ObservableCollection<TaskListItem> Items { get; set; }
        //public ObservableCollection<TaskListItem> CurrentItems { get; set; }

        ObservableCollection<TaskListItem> currentItems;
        public ObservableCollection<TaskListItem> CurrentItems
        {
            set
            {
                if (currentItems != value)
                {
                    currentItems = value;

                    OnPropertyChanged("CurrentItems");
                }
            }
            get
            {
                return currentItems;
            }
        }
    


        public Command LoadItemsCommand { get; set; }

        public TasksViewModel(Guid? parentId = null)
        {
            Items = new ObservableCollection<TaskListItem>();

            CurrentItems = new ObservableCollection<TaskListItem>();
        

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(parentId));


        }
        async Task ExecuteLoadItemsCommand(Guid? parentId)
        {



            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                Items.Clear();
                CurrentItems.Clear();
                //BehindItems.Clear();
                //FutureItems.Clear();
                //CompletedItems.Clear();

                List<ProjectInsight.Models.Tasks.Task> items = await TasksService.GetByProject(parentId.Value);


                if (items != null)
                {

                    int total = items.Count();
                    int brojace = 0;

                    foreach (var item in items)
                    {
                        brojace++;
                        TaskListItem newItem = new TaskListItem();
                        newItem.Id = item.Id.Value;
                        newItem.Title = item.Name;


                        bool endDateIsPast = false;
                        //string displayDate = string.Empty;
                        string startDate = string.Empty;
                        string endDate = string.Empty;

                        if (item.StartDateTimeUserLocal.HasValue && item.EndDateTimeUserLocal.HasValue)
                        {
                            if (item.StartDateTimeUserLocal.Value.Date == item.EndDateTimeUserLocal.Value.Date)
                            {
                                startDate = string.Empty;
                                endDate = item.EndDateTimeUserLocal.Value.Date.ToString("M/d/yy");
                            }
                            else
                            {
                                startDate = item.StartDateTimeUserLocal.Value.Date.ToString("M/d/yy") + " - ";
                                endDate = item.EndDateTimeUserLocal.Value.Date.ToString("M/d/yy");

                            }
                            if (item.EndDateTimeUserLocal.Value.Date < DateTime.Now.Date)
                                endDateIsPast = true;
                        }
                        else
                        {
                            if (item.StartDateTimeUserLocal.HasValue)
                            {
                                //displayDate = task.StartDateTimeUserLocal.Value.Date.ToString("M/d/yy") + " - ";
                                startDate = item.StartDateTimeUserLocal.Value.Date.ToString("M/d/yy") + " - ";
                            }
                            if (item.EndDateTimeUserLocal.HasValue)
                            {
                                //displayDate += task.EndDateTimeUserLocal.Value.Date.ToString("M/d/yy");
                                endDate = item.EndDateTimeUserLocal.Value.Date.ToString("M/d/yy");

                                if (item.EndDateTimeUserLocal.Value.Date < DateTime.Now.Date)
                                    endDateIsPast = true;
                            }
                        }

                        //newItem.Line2a = displayDate;
                        newItem.Line2s = startDate;
                        newItem.Line2e = endDate;

                        newItem.Line2Color = endDateIsPast ? ConvertColorToHex((Color)Application.Current.Resources["RedTextColor"]) : ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);

                        string workStatus = item.WorkPercentCompleteType != null ? item.WorkPercentCompleteType.Name : string.Empty;
                        string TaskOwner = string.Empty;

                        ///TODO to be removed this, TaskOwner should be used
                        if (item.TaskOwner != null)
                        {
                            TaskOwner = item.TaskOwner.FirstName + " " + item.TaskOwner.LastName;
                        }
                        //else
                        //{
                        //    if (item.TaskOwner_Id.HasValue)
                        //    {
                        //        User taskOwner = await UsersService.GetUser(item.TaskOwner_Id.Value);
                        //        if (taskOwner != null)
                        //            TaskOwner = taskOwner.FirstName + " " + taskOwner.LastName;
                        //    }
                        //}

                        newItem.Line3 = (workStatus != string.Empty ? workStatus + " - " : "") + TaskOwner;
                        newItem.Icon = "item_task.png";
                        newItem.ProjectId = item.Project_Id != null ? item.Project_Id.Value : Guid.Empty;

                        int stat = 1;
                        if (brojace <= total / 4)
                            stat = 1;
                        else if (brojace <= total / 2)
                            stat = 2;
                        else if (brojace <= 3 * total / 4)
                            stat = 3;
                        else
                            stat = 4;
                        if (parentId == new Guid("d8ed1090-69b6-45b1-9c36-04fb26e64e7a"))
                            newItem.TaskScheduleCurrentState = stat;
                        else
                            newItem.TaskScheduleCurrentState = (item.TaskScheduleCurrentState ?? 1);

                        Items.Add(newItem);
                    
                    }
                    GroupProjectsByStatus();

                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }
        private string ConvertColorToHex(Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
            return hex;
        }
        class ListHeader
        {
            public int Position { get; set; }
            public int TaskScheduleCurrentState { get; set; }
            public string Color { get; set; }
        }
        private void GroupProjectsByStatus()
        {
            var listWork = Items.OrderBy(x => x.TaskScheduleCurrentState).ToList();
            List<ListHeader> PositionsForInsert = new List<ListHeader>();
            int previousState = -1;

            int TotalCurrent = 0;
            int TotalBehind = 0;
            int TotalFuture = 0;
            int TotalCompleted = 0;

            for (int x = 0; x < listWork.Count(); x++)
            {
                if (listWork[x].TaskScheduleCurrentState == 1)
                    TotalCurrent++;
                else if (listWork[x].TaskScheduleCurrentState == 2)
                    TotalBehind++;
                else if (listWork[x].TaskScheduleCurrentState == 3)
                    TotalFuture++;
                else if (listWork[x].TaskScheduleCurrentState == 4)
                    TotalCompleted++;

                    if (x <= listWork.Count() - 1)
                    {

                        if (listWork[x].TaskScheduleCurrentState != previousState)
                        {
                            ListHeader header = new ListHeader();
                            header.Position = x;
                            header.TaskScheduleCurrentState = listWork[x].TaskScheduleCurrentState;

                            PositionsForInsert.Add(header);
                            previousState = listWork[x].TaskScheduleCurrentState;
                        }
                    }

            }

            int counter = 0;
            foreach (ListHeader header in PositionsForInsert)
            {
                TaskListItem head = new TaskListItem();
                //head.Icon = "Arrow_right.png";
                head.Icon = "Arrow_down.png";
                head.IsHeader = true;
                //head.IsVisible = true;
                head.IsItemVisible = false;
                head.IsExpanded = true;
                head.Line2Color = ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);
                switch ((TaskScheduleCurrentState)header.TaskScheduleCurrentState)
                {
                    case TaskScheduleCurrentState.Current:
                        head.Title = "Current (" + TotalCurrent + ")";
                        //head.Icon = "Arrow_down.png";
                        head.IsExpanded = true;
                        head.TaskScheduleCurrentState = (int)TaskScheduleCurrentState.Current; ;
                        if (TotalCurrent > 0)
                        {
                            listWork.Insert(header.Position + counter, head);
                            counter++;
                        }
                        break;
                    case TaskScheduleCurrentState.Behind:
                        head.Title = "Behind (" + TotalBehind + ")";
                        head.TaskScheduleCurrentState = (int)TaskScheduleCurrentState.Behind; ;
                        if (TotalBehind > 0)
                        {
                            listWork.Insert(header.Position + counter, head);
                            counter++;
                        }
                        break;
                    case TaskScheduleCurrentState.Future:
                        head.Title = "Future (" + TotalFuture + ")";
                        head.TaskScheduleCurrentState = (int)TaskScheduleCurrentState.Future; ;
                        if (TotalFuture > 0)
                        {
                            listWork.Insert(header.Position + counter, head);
                            counter++;
                        }
                        break;
                    case TaskScheduleCurrentState.Completed:
                        head.Title = "Completed (" + TotalCompleted + ")";
                        head.TaskScheduleCurrentState = (int)TaskScheduleCurrentState.Completed;
                        if (TotalCompleted > 0)
                        {
                            listWork.Insert(header.Position + counter, head);
                            counter++;
                        }
                        break;
                }
                
            }

            foreach (TaskListItem item in listWork)
            {
                //if (!item.IsHeader)
                //{
                //    if (item.TaskScheduleCurrentState == 1)
                //        item.IsItemVisible = true;
                //    else
                //        item.IsItemVisible = false;
                //}
                CurrentItems.Add(item);
            }
        }


        public enum TaskScheduleCurrentState
        {
            /// <summary>
            /// Current
            /// </summary>
            [Description("Current")]
            Current = 1,

            /// <summary>
            /// Behind
            /// </summary>
            [Description("Behind")]
            Behind = 2,

            /// <summary>
            /// Future
            /// </summary>
            [Description("Future")]
            Future = 3,


            /// <summary>
            /// Completed
            /// </summary>
            [Description("Completed")]
            Completed = 4,


            /// <summary>
            /// Unknown
            /// </summary>
            [Description("Unknown")]
            Unknown = 0

        }
    }
}