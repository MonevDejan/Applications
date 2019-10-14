using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsight.Models.Tasks;
using ProjectInsightMobile.Services;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
using Syncfusion.SfChart.XForms;
//using Syncfusion.SfChart.XForms;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksChartCustomView : ContentView
    {




        public static Guid relatedId;
        public static string itemType;
        public static string parentName;
        public static int itemsCount;
        public static bool hasEditPermissions;

        //public ObservableCollection<MyWorkItem> listWork { get; set; }
        public static TasksChartViewModel viewModel;

        public TasksChartCustomView()
        {
            InitializeComponent();

            slTasks.BindingContext = viewModel = new TasksChartViewModel();
            viewModel.IsExpandedList = false;
        }

        public static readonly BindableProperty ItemIdProperty = BindableProperty.Create(
                                                       propertyName: "ItemId",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(TasksChartCustomView),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemIdPropertyChanged);

        public string ItemId
        {
            get { return base.GetValue(ItemIdProperty).ToString(); }
            set { base.SetValue(ItemIdProperty, value); }
        }

        private static void ItemIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TasksChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                relatedId = new Guid(newValue.ToString());
        }

        

        public static readonly BindableProperty ParentNameProperty = BindableProperty.Create(
                                                    propertyName: "ParentName",
                                                    returnType: typeof(string),
                                                    declaringType: typeof(TasksChartCustomView),
                                                    defaultValue: "",
                                                    defaultBindingMode: BindingMode.TwoWay,
                                                    propertyChanged: ParentNamePropertyChanged);

        public string ParentName
        {
            get { return base.GetValue(ParentNameProperty).ToString(); }
            set { base.SetValue(ParentNameProperty, value); }
        }

        private static void ParentNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TasksChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                parentName = newValue.ToString();
        }


        public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(
                                            propertyName: "ItemsCount",
                                            returnType: typeof(int),
                                            declaringType: typeof(TasksChartCustomView),
                                            defaultValue: -1,
                                            defaultBindingMode: BindingMode.OneWay,
                                            propertyChanged: ItemsCountPropertyChanged);

        public string ItemsCount
        {
            get { return base.GetValue(ItemsCountProperty).ToString(); }
            set { base.SetValue(ItemsCountProperty, value); }
        }

        private static void ItemsCountPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TasksChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            viewModel.IsExpandedList = false;
            viewModel.ShowHideIcon = "Arrow_right.png";

            if (newValue != null)
            {
                itemsCount = (int)newValue;
                if (itemsCount>0)
                    viewModel.HasPermissions = true;
                else
                    viewModel.HasPermissions = false;
                viewModel.TasksLabel = String.Format("All Tasks ({0})", itemsCount.ToString());
            }
        }

        public static readonly BindableProperty ItemTypeProperty = BindableProperty.Create(
                                                       propertyName: "ItemType",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(TasksChartCustomView),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemTypePropertyChanged);

        public string ItemType
        {
            get { return base.GetValue(ItemTypeProperty).ToString(); }
            set { base.SetValue(ItemTypeProperty, value); }
        }

        private static void ItemTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TasksChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

        

            if (newValue != null)
                itemType = newValue.ToString();
        }

        public static readonly BindableProperty HasEditPermissionsProperty = BindableProperty.Create(
                                    propertyName: "HasEditPermissions",
                                    returnType: typeof(bool),
                                    declaringType: typeof(TasksChartCustomView),
                                    defaultValue: true,
                                    defaultBindingMode: BindingMode.OneWay,
                                    propertyChanged: HasEditPermissionsPropertyChanged);

        public bool HasEditPermissions
        {
            get { return (bool)GetValue(HasEditPermissionsProperty); }
            set { SetValue(HasEditPermissionsProperty, value); }
        }

        private static void HasEditPermissionsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (TasksChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
            {
                hasEditPermissions = (bool)newValue;
                viewModel.HasPermissions = ((bool)newValue) && itemsCount > 0;
            }
        }

        public async void OnTappedTasks(object sender, EventArgs e)
        {
          //  if (itemsCount == 0) return;

                // Do stuff       
                if (viewModel.IsExpandedList)
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "Arrow_right.png";
                return;
            }

            //get Tasks
            List<ProjectInsight.Models.Tasks.Task> tasks = await TasksService.GetByProject(relatedId);

            viewModel.Items = new ObservableCollection<ProjectInsight.Models.Tasks.Task>(tasks);
            viewModel.TasksLabel = "All Tasks (" + viewModel.Items.Count + ")";
            viewModel.ShowHideIcon = "Arrow_down.png";
            viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;

            var tasksCount = tasks.GroupBy(x => x.WorkPercentCompleteType.Name)
                       .Select(x => new { Status = x.Key, Count = x.Distinct().Count() });

            viewModel.Data = new ObservableCollection<TasksChartViewModel.TaskChartItem>();
            foreach (var item in tasksCount)
            {
                viewModel.Data.Add(new TasksChartViewModel.TaskChartItem { Name = item.Status, Height = item.Count });

            }

            btnViewAllTasks.IsVisible = false;

            if (tasks == null || tasks.Count == 0)
                Chart.IsVisible = false;
            else
            {
                Chart.IsVisible = true;
                if (hasEditPermissions)
                    btnViewAllTasks.IsVisible = true;
                
            }
            viewModel.VisibleLoad = false;
           
        }
        public async void OnGoToTasks(object sender, EventArgs e)
        {

            //check permissions



            if (hasEditPermissions)// && itemsCount > 0)
            {
                MessagingCenter.Subscribe<TaskStatusPage, Guid>(this, "TaskStatusUpdated", async (obj, taskId) =>
                {
                    viewModel.IsExpandedList = false;
                    viewModel.ShowHideIcon = "Arrow_right.png";

                    MessagingCenter.Unsubscribe<TaskStatusPage, Guid>(this, "TaskStatusUpdated");
                });
                await Navigation.PushAsync(new ProjectTasksPage(relatedId, parentName));
            }
          
        }

        private void Chart_LegendItemCreated(object sender, ChartLegendItemCreatedEventArgs e)
        {
            object val = e.LegendItem.DataPoint;
            string value = ((TasksChartViewModel.TaskChartItem)val).Height.ToString();
            e.LegendItem.Label += ": " + value;
        }

        private async void btnAddTask_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage(parentId: relatedId));
        }
    }
}