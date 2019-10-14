using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using ProjectInsight.Models.Users;
using System.Linq;
using ProjectInsightMobile.Helpers;
using System.Collections.Generic;
using ProjectInsightMobile.Services;
using SQLite;

namespace ProjectInsightMobile.ViewModels
{
    public class MyProjectsViewModel : BaseViewModel
    {
       
        ObservableCollection<MyWorkItem> projects;
        public ObservableCollection<MyWorkItem> Projects
        {
            set
            {
                if (projects != value)
                {
                    projects = value;

                    OnPropertyChanged("Projects");


                }
            }
            get
            {
                return projects;
            }
        }
        public bool LoadLocalContent { get; set; }
      
        bool isProjectsEmpty = false;
        public bool IsProjectsEmpty
        {
            set
            {
                if (isProjectsEmpty != value)
                {
                    isProjectsEmpty = value;
                    OnPropertyChanged("IsProjectsEmpty");
                }
            }
            get
            {
                return isProjectsEmpty;
            }
        }
        public Command LoadItemsCommand { get; set; }

        public MyProjectsViewModel()
        {
            Projects = new ObservableCollection<MyWorkItem>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }                                          
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;


            try
            {
                Common.Instance.CreateTable<MyWorkItem>();
                Projects.Clear();
                bool refresh = Common.RefreshProjectsList;
                if (LoadLocalContent && !refresh)
                {
                    //getting from local storage
                    SQLiteConnection connection = Common.Instance.InitializeDatabase();
                    Common.Instance.CreateTable<MyWorkItem>();

                    List<MyWorkItem> userWork = Common.Instance.GetUserWork().Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.ObjectType == "Project").OrderByDescending(x => x.IsActive).ToList();
                    if (userWork == null || userWork.Count == 0)
                    {
                        LoadLocalContent = false;
                    }
                    else
                    {
                        var orderedUserWork = GroupProjectsByStatus(userWork);

                        foreach (var item in orderedUserWork)
                        {
                            Projects.Add(item);
                        }
                    }
                }
                if (!LoadLocalContent || refresh)
                {
                    //getting new data


                    //Clear local storage
                    var listWork = Common.Instance.GetUserWork().Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.ObjectType == "Project");

                    if (listWork != null && listWork.ToList().Count > 0)
                    {
                        foreach (var item in listWork)
                        {
                            Common.Instance._sqlconnection.Delete(item);
                        }
                    }

                    MyWork myItems = await UsersService.GetMyWorkAsync(Common.CurrentWorkspace.UserID);
                    //MyWork myItems = await UsersService.GetMyProjectsAsync(Common.CurrentWorkspace.UserID);
                 
                    if (myItems.Projects != null)
                    {
                        List<MyWorkItem> MyProjects = new List<MyWorkItem>();
                        foreach (var item in myItems.Projects)
                        {
                            try
                            {

                                if (Common.Filter.CompanyId != null && Common.Filter.CompanyId.HasValue)
                                {
                                    if (item.Companies != null && item.Companies.Count > 0)
                                    {
                                        if (!item.Companies.Any(x => x.Id == Common.Filter.CompanyId))
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                        continue;
                                }

                                ProjectInsight.Models.Projects.Project project = (ProjectInsight.Models.Projects.Project)item;
                                MyWorkItem newItem = new MyWorkItem();
                                newItem.Id = project.Id.Value;
                                newItem.Title = project.ItemNumberFullAndNameDisplayPreference; // project.ItemNumberFull + " - " + project.Name;

                                bool endDateIsPast = false;
                                //string displayDate = string.Empty;
                                string startDate = string.Empty;
                                string endDate = string.Empty;
                                if (project.StartDateTimeUTC.HasValue && project.EndDateTimeUTC.HasValue)
                                {
                                    if (project.StartDateTimeUTC.Value.Date == project.EndDateTimeUTC.Value.Date)
                                    {
                                        startDate = string.Empty;
                                        endDate = project.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                    }
                                    else
                                    {
                                        startDate = project.StartDateTimeUTC.Value.Date.ToString("M/d/yy") + " - ";
                                        endDate = project.EndDateTimeUTC.Value.Date.ToString("M/d/yy");
                                    }
                                    if (project.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                        endDateIsPast = true;
                                }
                                else
                                {
                                    if (project.StartDateTimeUTC.HasValue)
                                    {
                                        startDate = project.StartDateTimeUTC.Value.Date.ToString("M/d/yy");
                                    }
                                    if (project.EndDateTimeUTC.HasValue)
                                    {
                                        endDate = project.EndDateTimeUTC.Value.Date.ToString("M/d/yy");

                                        if (project.EndDateTimeUTC.Value.Date < DateTime.Now.Date)
                                            endDateIsPast = true;
                                    }
                                }

                                //newItem.Line2a = displayDate;
                                newItem.Line2s = startDate;
                                newItem.Line2e = endDate;

                                newItem.Line2Color = endDateIsPast ? ConvertColorToHex((Color)Application.Current.Resources["RedTextColor"]) : ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);


                                newItem.Line3 = project.ProjectStatus != null ? project.ProjectStatus.Name : "";
                                newItem.Line4 = project.PrimaryProjectManager != null ? project.PrimaryProjectManager.FirstName + " " + project.PrimaryProjectManager.LastName : "";

                                newItem.Icon = "item_project.png";
                                newItem.ItemType = ItemType.Projects;
                                newItem.Url = item.UrlFull;

                                newItem.WorkspaceId = Common.CurrentWorkspace.Id;
                                newItem.ProjectId = Guid.Empty;
                                newItem.ObjectType = project.Type;
                                newItem.isItem = true;
                                //string color = ExtensionMethods.GetHexString((Color)Application.Current.Resources["BlackTextColor"]);
                                newItem.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);
                                // Remarks:
                                //     Possible Values: 0 - Active 1 - Archive 2 - Planning 3 - Template 4 - Project
                                //     Baseline 5 - To Be Deleted 6 - Change Order

                                //newItem.ProjectState = project.ProjectState ?? 0;
                                newItem.IsActive = project.ProjectStatus != null ? project.ProjectStatus.IsActive.Value : false;

                                var userWork = Common.Instance.GetUserWork().Where(x => x.WorkspaceId == Common.CurrentWorkspace.Id && x.ProjectId == project.Id);
                                if (userWork != null && userWork.Count() > 0)
                                    newItem.TasksCount = userWork.Count();


                                MyProjects.Add(newItem);
                            }
                            catch (Exception ex)
                            {

                            }
                        }


                        var orderedUserWork = GroupProjectsByStatus(MyProjects.OrderByDescending(x=>x.IsActive).ToList());

                        foreach (var item in orderedUserWork)
                        {
                            Projects.Add(item);
                            Common.Instance._sqlconnection.Insert(item);
                        }
                    }
                }
                
                

                if (Projects == null || Projects.Count == 0)
                    IsProjectsEmpty = true;
                else
                    IsProjectsEmpty = false;

                Common.Instance.bottomNavigationViewModel.NumberProjectItems = (Projects == null ? 0 : Projects.Where(x=>x.isItem).Count());
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
        private List<MyWorkItem> GroupProjectsByStatus(List<MyWorkItem> listWork)
        {
            //var listWork = new ObservableCollection<MyWorkItem>(Projects.OrderByDescending(x => x.IsActive));

            List<ProjectHeader> PositionsForInsert = new List<ProjectHeader>();
            bool? previousState = null;
            for (int x = 0; x < listWork.Count(); x++)
            {
                if (listWork[x].TitleColor == null) listWork[x].TitleColor = "#000000";

                if (x < listWork.Count() - 1)
                {

                    if (listWork[x].IsActive != previousState)
                    {
                        ProjectHeader header = new ProjectHeader();
                        header.Position = x;
                        //header.ProjectState = listWork[x].ProjectState;
                        if (listWork[x].IsActive)
                            header.ProjectState = 0;
                        else
                            header.ProjectState = 2;

                        PositionsForInsert.Add(header);
                        previousState = listWork[x].IsActive;
                        //previousState = listWork[x].ProjectState;

                    }
                }

            }

            int counter = 0;
            foreach (ProjectHeader header in PositionsForInsert)
            {
                MyWorkItem head = new MyWorkItem();
                switch (header.ProjectState)
                {
                    case 0:
                        head.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["GreenTextColor"]);
                        //head.TitleColor = "#07aa07";
                        head.Title = "Active";
                        break;
                    case 1:
                        head.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["LightGrayTextColor"]);
                        head.Title = "Archive";
                        break;
                    case 2:
                        head.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["LightGrayTextColor"]);
                        //head.TitleColor = "#828282";
                        head.Title = "Planning";
                        break;

                    case 3:
                        head.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["LightGrayTextColor"]);
                        head.Title = "Template";
                        break;
                    case 4:
                        head.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["LightGrayTextColor"]);
                        head.Title = "Project Baseline";
                        break;
                    case 5:
                        head.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["LightGrayTextColor"]);
                        head.Title = "To Be Deleted";
                        break;
                    case 6:
                        head.TitleColor = ConvertColorToHex((Color)Application.Current.Resources["LightGrayTextColor"]);
                        head.Title = "Change Order";
                        break;
                }

                head.isHeader = true;
                head.isItem = false;
                listWork.Insert(header.Position + counter, head);
                counter++;
            }

            return listWork;
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
    }
}