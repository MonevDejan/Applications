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


using SQLite;
using ProjectInsight.Models.ApprovalRequests;

namespace ProjectInsightMobile.ViewModels
{
    public class MyWorkViewModel : BaseViewModel
    {
        ObservableCollection<MyWorkItem> items;
        public ObservableCollection<MyWorkItem> Items
        {
            set
            {
                if (items != value)
                {
                    items = value;

                    OnPropertyChanged("Items");


                }
            }
            get
            {
                return items;
            }
        }
        public bool LoadLocalContent { get; set; }
        bool isItemsEmpty = false;
        public bool IsItemsEmpty
        {
            set
            {
                if (isItemsEmpty != value)
                {
                    isItemsEmpty = value;
                    OnPropertyChanged("IsItemsEmpty");
                }
            }
            get
            {
                return isItemsEmpty;
            }
        }
       
        public Command LoadItemsCommand { get; set; }

        public MyWorkViewModel(Guid? parentId = null, String ObjectType = null)
        {
            Items = new ObservableCollection<MyWorkItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(parentId, ObjectType));
        }                                          
        async Task ExecuteLoadItemsCommand(Guid? parentId, String ObjectType)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            bool isFilteredList = true;
            if ((parentId == null || parentId == Guid.Empty) && ObjectType == null)
                isFilteredList = false;

            try
            {
                Common.Instance.CreateTable<MyWorkItem>();
                Items.Clear();
                bool refresh = Common.RefreshWorkList;
                if (LoadLocalContent && !refresh)
                {
                    //getting from local storage
                    SQLiteConnection connection = Common.Instance.InitializeDatabase();
                    Common.Instance.CreateTable<MyWorkItem>();

                    var userWork = Common.Instance.GetUserWork().Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.ObjectType != "Project");

                    if (userWork == null || userWork.Count() == 0)
                        LoadLocalContent = false;
                    else
                    {
                        if (isFilteredList)
                        {
                            var filteredList = userWork.Where(x => (x.ObjectType == ObjectType || ObjectType == "All") && x.ProjectId == parentId);
                            foreach (var item in filteredList)
                                Items.Add(item);
                        }
                        else
                        {

                            foreach (var item in userWork)
                                Items.Add(item);
                        }
                    }
                }
                if (!LoadLocalContent || refresh)
                {
                    //getting new data


                    MyWork myItems = await UsersService.GetMyWorkAsync(Common.CurrentWorkspace.UserID);

                    Common.Instance.bottomNavigationViewModel.NumberProjectItems = (myItems.Projects == null || Common.Filter.CompanyId != null ? 0 : myItems.Projects.Count());

                    List<ApiModelBase> items;


                    items = myItems.ItemsInOrder;

                    if (items != null)
                    {


                        foreach (var item in items)
                        {

                            try
                            {
                                if (isFilteredList)
                                {
                                    if (!(item.Type == ObjectType || ObjectType == "All"))
                                        continue;
                                }

                                if (item.Type == "Task")
                                {
                                    MyWorkItem newItem = new MyWorkItem();
                                    ProjectInsight.Models.Tasks.Task task = (ProjectInsight.Models.Tasks.Task)item;

                                    if (isFilteredList)
                                    {
                                        if (task.Project_Id == null || task.Project_Id.Value != parentId)
                                            continue;
                                    }


                                    if (Common.Filter.CompanyId != null && Common.Filter.CompanyId.HasValue)
                                    {
                                        if (task.Project.Companies != null && task.Project.Companies.Count > 0)
                                        {
                                            if (!task.Project.Companies.Any(x => x.Id == Common.Filter.CompanyId))
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                            continue;
                                    }


                                    newItem.Id = task.Id.Value;
                                    newItem.Title = task.Name;

                                    bool endDateIsPast = false;
                                    //string displayDate = string.Empty;
                                    string startDate = string.Empty;
                                    string endDate = string.Empty;

                                    if (task.StartDateTimeUTC.HasValue && task.EndDateTimeUTC.HasValue)
                                    {
                                        if (task.StartDateTimeUTC.Value.Date == task.EndDateTimeUTC.Value.Date)
                                        {
                                            startDate = string.Empty;
                                            endDate = task.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        else
                                        {
                                            startDate = task.StartDateTimeUTC.Value.Date.ToString("M/d/yy") + " - ";
                                            endDate = task.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        if (task.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                            endDateIsPast = true;
                                    }
                                    else
                                    {
                                        if (task.StartDateTimeUTC.HasValue)
                                        {
                                            startDate = task.StartDateTimeUTC.Value.Date.ToString("M/d/yy") + " - ";
                                        }
                                        if (task.EndDateTimeUTC.HasValue)
                                        {
                                            endDate = task.EndDateTimeUTC.Value.Date.ToString("M/d/yy");

                                            if (task.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                                endDateIsPast = true;
                                        }
                                    }

                                    //newItem.Line2a = displayDate;
                                    newItem.Line2s = startDate;
                                    newItem.Line2e = endDate;

                                    newItem.Line2Color = endDateIsPast ? ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["RedTextColor"]) : ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);

                                    if (task.Project != null)
                                        newItem.Line3 = task.Project.ItemNumberFullAndNameDisplayPreference;
                                    //newItem.Line3 = task.ItemNumberFull + " - " + task.Project.Name;

                                    newItem.Icon = "item_task.png";
                                    newItem.ItemType = ItemType.Task;

                                    newItem.WorkspaceId = Common.CurrentWorkspace.Id;
                                    newItem.ProjectId = task.Project_Id != null ? task.Project_Id.Value : Guid.Empty;
                                    newItem.ObjectType = task.Type;
                                    newItem.TitleColor = "#000000";
                                    Items.Add(newItem);
                                }
                                else if (item.Type == "Issue")
                                {
                                    MyWorkItem newItem = new MyWorkItem();
                                    ProjectInsight.Models.Issues.Issue issue = (ProjectInsight.Models.Issues.Issue)item;

                                    if (issue.ProjectAffiliation != null)
                                    {

                                    }
                                    if (isFilteredList)
                                    {
                                        if (issue.ProjectAffiliation_Id == null || issue.ProjectAffiliation_Id.Value != parentId)
                                            continue;
                                    }

                                    if (Common.Filter.CompanyId != null && Common.Filter.CompanyId.HasValue)
                                    {
                                        if (issue.ProjectAffiliation.Companies != null && issue.ProjectAffiliation.Companies.Count > 0)
                                        {
                                            if (!issue.ProjectAffiliation.Companies.Any(x => x.Id == Common.Filter.CompanyId))
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                            continue;
                                    }
                                    newItem.Id = issue.Id.Value;
                                    newItem.Title = issue.Name;

                                    bool endDateIsPast = false;
                                    //string displayDate = string.Empty;
                                    string startDate = string.Empty;
                                    string endDate = string.Empty;
                                    if (issue.StartDateTimeUTC.HasValue && issue.EndDateTimeUTC.HasValue)
                                    {
                                        if (issue.StartDateTimeUTC.Value.Date == issue.EndDateTimeUTC.Value.Date)
                                        {
                                            startDate = string.Empty;
                                            endDate = issue.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        else
                                        {
                                            startDate = issue.StartDateTimeUTC.Value.Date.ToString("M/d/yy") + " - ";
                                            endDate = issue.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        if (issue.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                            endDateIsPast = true;
                                    }
                                    else
                                    {
                                        if (issue.StartDateTimeUTC.HasValue)
                                        {
                                            startDate = issue.StartDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        if (issue.EndDateTimeUTC.HasValue)
                                        {
                                            endDate = issue.EndDateTimeUTC.Value.Date.ToString("M/d/yy");

                                            if (issue.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                                endDateIsPast = true;
                                        }
                                    }

                                    //newItem.Line2a = displayDate;
                                    newItem.Line2s = startDate;
                                    //newItem.Line2e = endDate;

                                    newItem.Line2Color = endDateIsPast ? ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["RedTextColor"]) : ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);

                                    string priority = issue.IssuePriority != null ? issue.IssuePriority.Name : "";
                                    string status = issue.IssueStatusType != null ? issue.IssueStatusType.Name : "";
                                    string separator = " - ";
                                    if (String.IsNullOrEmpty(priority))
                                        separator = "";

                                    newItem.Line2e = String.Format("{0}{1}{2}", endDate, separator, priority); 

                                    //newItem.Line3 = String.Format("{0}{1}{2}", prority, separator, status);
                                    newItem.Line3 = status;

                                    //newItem.Line4 = issue.ItemNumberFull + " - " + issue.ProjectAffiliation.Name;
                                    if (issue.ProjectAffiliation != null)
                                        newItem.Line4 = issue.ProjectAffiliation.ItemNumberFullAndNameDisplayPreference;
                                    newItem.Icon = "item_issue.png";
                                    newItem.ItemType = ItemType.Issue;

                                    newItem.WorkspaceId = Common.CurrentWorkspace.Id;
                                    newItem.ProjectId = issue.ProjectAffiliation_Id != null ? issue.ProjectAffiliation_Id.Value : Guid.Empty;
                                    newItem.ObjectType = issue.Type;
                                    newItem.TitleColor = "#000000";
                                    Items.Add(newItem);
                                }
                                else if (item.Type == "ToDo")
                                {
                                    MyWorkItem newItem = new MyWorkItem();
                                    ProjectInsight.Models.ToDos.ToDo toDo = (ProjectInsight.Models.ToDos.ToDo)item;
                                    if (isFilteredList)
                                    {
                                        if (toDo.ProjectAffiliation_Id == null || toDo.ProjectAffiliation_Id.Value != parentId)
                                            continue;
                                    }

                                    if (Common.Filter.CompanyId != null && Common.Filter.CompanyId.HasValue)
                                    {
                                        if (toDo.ProjectAffiliation.Companies != null && toDo.ProjectAffiliation.Companies.Count > 0)
                                        {
                                            if (!toDo.ProjectAffiliation.Companies.Any(x => x.Id == Common.Filter.CompanyId))
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                            continue;
                                    }

                                    newItem.Id = toDo.Id.Value;
                                    newItem.Title = toDo.Name;
                                    bool endDateIsPast = false;
                                    //string displayDate = string.Empty;
                                    string startDate = string.Empty;
                                    string endDate = string.Empty;
                                    if (toDo.StartDateTimeUTC.HasValue && toDo.EndDateTimeUTC.HasValue)
                                    {
                                        if (toDo.StartDateTimeUTC.Value.Date == toDo.EndDateTimeUTC.Value.Date)
                                        {
                                            startDate = string.Empty;
                                            endDate = toDo.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        else
                                        {
                                            startDate = toDo.StartDateTimeUTC.Value.Date.ToString("M/d/yy") + " - ";
                                            endDate = toDo.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        if (toDo.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                            endDateIsPast = true;
                                    }
                                    else
                                    {
                                        if (toDo.StartDateTimeUTC.HasValue)
                                        {
                                            startDate = toDo.StartDateTimeUTC.Value.Date.ToString("M/d/yy");
                                        }
                                        if (toDo.EndDateTimeUTC.HasValue)
                                        {
                                            endDate = toDo.EndDateTimeUTC.Value.Date.ToString("M/d/yy");

                                            if (toDo.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                                endDateIsPast = true;
                                        }
                                    }

                                    //newItem.Line2a = displayDate;
                                    newItem.Line2s = startDate;
                                    newItem.Line2e = endDate;

                                    newItem.Line2Color = endDateIsPast ? ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["RedTextColor"]) : ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);

                                    //newItem.Line3 = toDo.ItemNumberFull + " - " + toDo.ProjectAffiliation.Name;
                                    if (toDo.ProjectAffiliation != null)
                                        newItem.Line3 = toDo.ProjectAffiliation != null ? toDo.ProjectAffiliation.ItemNumberFullAndNameDisplayPreference : String.Empty;

                                    newItem.Icon = "item_todo.png";
                                    newItem.ItemType = ItemType.Todo;

                                    newItem.WorkspaceId = Common.CurrentWorkspace.Id;
                                    newItem.ProjectId = toDo.ProjectAffiliation_Id != null ? toDo.ProjectAffiliation_Id.Value : Guid.Empty;
                                    newItem.ObjectType = toDo.Type;
                                    newItem.TitleColor = "#000000";
                                    Items.Add(newItem);
                                }
                                else if (item.Type == "ApprovalRequest")
                                {
                                    if (isFilteredList)
                                        continue;

                                    if (Common.Filter.CompanyId != null && Common.Filter.CompanyId.HasValue)
                                    {
                                            continue;
                                    }



                                    MyWorkItem newItem = new MyWorkItem();
                                    ProjectInsight.Models.ApprovalRequests.ApprovalRequest approval = (ProjectInsight.Models.ApprovalRequests.ApprovalRequest)item;

                                    newItem.Id = approval.Id.Value;
                                    newItem.Title = approval.Name;

                                    //string Approvers = string.Empty;
                                    //if (approval.ApprovalRequestApprovals != null && approval.ApprovalRequestApprovals.Count > 0)
                                    //{

                                    //    if (approval.ApprovalRequestApprovals.Count > 1)
                                    //        Approvers = "Approvers: ";
                                    //    else
                                    //        Approvers = "Approver: ";

                                    //    foreach (ApprovalRequestApproval app in approval.ApprovalRequestApprovals)
                                    //    {
                                    //        if (app.Approver == null) continue;
                                    //        string approver = app.Approver.FirstName + " " + app.Approver.LastName + ", ";
                                    //        Approvers += approver;

                                    //    }

                                    //    Approvers = Approvers.Substring(0, Approvers.Length - 2);

                                    //}
                                    //newItem.Line2s = Approvers;

                                    string deadLine = string.Empty;
                                    if (approval.DeadlineDateTimeUTC != null)
                                    {
                                        //if (approval.DeadlineDateTimeUTC.Value.Date < DateTime.Now.Date)
                                        //    newItem.Line2Color =ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["RedTextColor"]);
                                        //else
                                        //    newItem.Line2Color = ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);

                                        deadLine  = approval.DeadlineDateTimeUTC.Value.Date.ToString("M/d/yy");
                                    }

                                    //Not Active, Pending, Approved, Denied
                                    switch (approval.ApprovalRequestStateType.Value)
                                    {
                                        case 0:
                                            newItem.Line3 = "Not Active ";
                                            break;
                                        case 1:
                                            newItem.Line3 = "Pending ";
                                            break;
                                        case 2:
                                            newItem.Line3 = "Approved ";
                                            break;
                                        case 3:
                                            newItem.Line3 = "Denied ";
                                            break;
                                    }

                                    newItem.Line2s = deadLine;
                                    newItem.Line3 = ((Enums.ApprovalRequestStateType)approval.ApprovalRequestStateType).ToString();

                                    newItem.Icon = "item_approval_request.png";
                                    newItem.ItemType = ItemType.ApprovalRequests;

                                    newItem.WorkspaceId = Common.CurrentWorkspace.Id;
                                    newItem.ProjectId = Guid.Empty;
                                    newItem.ObjectType = approval.Type;
                                    newItem.TitleColor = "#000000";
                                    Items.Add(newItem);
                                }
                                else if(item.Type == "TimeSheet")
                                {
                                    MyWorkItem newItem = new MyWorkItem();
                                    ProjectInsight.Models.TimeAndExpense.TimeSheet timeSheet = (ProjectInsight.Models.TimeAndExpense.TimeSheet)item;
                                    
                                    if (isFilteredList)
                                            continue;
                                    if (Common.Filter.CompanyId != null && Common.Filter.CompanyId.HasValue)
                                    {
                                            continue;
                                    }

                                    newItem.Id = timeSheet.Id.Value;
                                    newItem.Title = timeSheet.Name;
                                    if (timeSheet.StartDate.HasValue)
                                        newItem.Line2s = "Dates: " + timeSheet.StartDate.Value.ToString("M/d/yy") + " - ";

                                    if (timeSheet.EndDate.HasValue)
                                        newItem.Line2e = timeSheet.EndDate.Value.ToString("M/d/yy");
                                    bool endDateIsPast = false;
                                    newItem.Line2Color = endDateIsPast ? ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["RedTextColor"]) : ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);

                                    if (!string.IsNullOrEmpty(timeSheet.Status))
                                        newItem.Line3 = "Status: " + timeSheet.Status;
                                    newItem.Icon = "item_timesheet.png";
                                    newItem.ItemType = ItemType.TimeSheet;
                                    newItem.WorkspaceId = Common.CurrentWorkspace.Id;
                                    newItem.ProjectId = Guid.Empty;
                                    newItem.ObjectType =timeSheet.Type;
                                    newItem.TitleColor = "#000000";
                                    Items.Add(newItem);
                                }
                                else
                                {

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }

                    if (!isFilteredList)
                    {
                        var listWork = Common.Instance.GetUserWork().Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.ObjectType != "Project");
                        if (listWork != null && listWork.ToList().Count > 0)
                        {
                            foreach (var item in listWork)
                            {
                                Common.Instance._sqlconnection.Delete(item);
                            }
                        }

                        if (Items != null)
                        {
                            foreach (MyWorkItem item in Items)
                            {
                                Common.Instance._sqlconnection.Insert(item);
                            }
                        }
                       
                    }
                }
                
                

                if (Items == null || Items.Count == 0)
                    IsItemsEmpty = true;
                else
                    IsItemsEmpty = false;

                if (!isFilteredList)
                {
                    Common.Instance.bottomNavigationViewModel.NumberWorkListItems = (Items == null ? 0 : Items.Count());
                   
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

        class ProjectHeader
        {
            public int Position { get; set; }
            public int ProjectState { get; set; }
            public string Color { get; set; }
        }
       
    }
}