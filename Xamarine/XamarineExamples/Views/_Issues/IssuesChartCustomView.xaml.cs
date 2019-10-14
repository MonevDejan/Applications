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
using ProjectInsight.Models.Issues;
using ProjectInsightMobile.Services;
using Plugin.FilePicker.Abstractions;
using Plugin.FilePicker;
//using Syncfusion.SfChart.XForms;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IssuesChartCustomView : ContentView
    {

   


        public static Guid relatedId;
        public static string itemType;
        public static string parentName;
        public static int itemsCount;
        public static IssuesChartViewModel viewModel;

        public IssuesChartCustomView()
		{
			InitializeComponent ();
            slIssues.BindingContext = viewModel = new IssuesChartViewModel();
            viewModel.IsExpandedList = false;
        }
      
        public static readonly BindableProperty ItemIdProperty = BindableProperty.Create(
                                                       propertyName: "ItemId",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(IssuesChartCustomView),
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
            var control = (IssuesChartCustomView)bindable;
            if (control == null) return;
           // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                relatedId = new Guid(newValue.ToString());
        }

        public static readonly BindableProperty ParentNameProperty = BindableProperty.Create(
                                                   propertyName: "ParentName",
                                                   returnType: typeof(string),
                                                   declaringType: typeof(IssuesChartCustomView),
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
            var control = (IssuesChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                parentName = newValue.ToString();
        }


        public static readonly BindableProperty ItemsCountProperty = BindableProperty.Create(
                                    propertyName: "ItemsCount",
                                    returnType: typeof(int),
                                    declaringType: typeof(IssuesChartCustomView),
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
            var control = (IssuesChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            viewModel.IsExpandedList = false;
            viewModel.ShowHideIcon = "Arrow_right.png";

            if (newValue != null)
            {
                itemsCount = (int)newValue;
                viewModel.IssuesLabel = String.Format("All Issues ({0})", itemsCount.ToString());
            }
        }

        public static readonly BindableProperty ItemTypeProperty = BindableProperty.Create(
                                                       propertyName: "ItemType",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(IssuesChartCustomView),
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
            var control = (IssuesChartCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                itemType = newValue.ToString();
        }

        public async void OnTappedIssues(object sender, EventArgs e)
        {

            // Do stuff       
            if (viewModel.IsExpandedList)
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "Arrow_right.png";
                return;
            }

            //get Issues
            List<ProjectInsight.Models.Issues.Issue> issues = null;
            try
            {
                issues = await IssuesService.client.GetByProjectAsync(relatedId, new ProjectInsight.Models.Base.ModelProperties("IssueStatusType"));
            }
            catch (Exception ex)
            {

            }
        
           
            viewModel.ShowHideIcon = "Arrow_down.png";
            viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;
            viewModel.Data = new ObservableCollection<IssuesChartViewModel.IssueChartItem>();
            if (issues != null)
            {
                viewModel.Items = new ObservableCollection<ProjectInsight.Models.Issues.Issue>(issues);
                viewModel.IssuesLabel = "All Issues (" + viewModel.Items.Count + ")";
                foreach (var item in issues)
                {
                    if (item.IssueStatusType == null)
                    {
                        item.IssueStatusType = new ProjectInsight.Models.ReferenceData.IssueStatusType() { Name = "Without Status Type" };
                    }

                }

                var issuesCount = issues.GroupBy(x => x.IssueStatusType.Name)
                           .Select(x => new { Status = x.Key, Count = x.Distinct().Count() });

                if (issuesCount != null)
                {
                    foreach (var item in issuesCount)
                    {
                        viewModel.Data.Add(new IssuesChartViewModel.IssueChartItem { Name = item.Status, Value = item.Count });
                    }
                }
            }
            if (issues == null || issues.Count == 0)
                Chart.IsVisible = false;
            else
                Chart.IsVisible = true;
            viewModel.VisibleLoad = false;
        }

        public async void OnIssueSelected(object sender, EventArgs e)
        {

            var templateGrid = (StackLayout)sender;
                                                     
            var item = (ProjectInsight.Models.Issues.Issue)templateGrid.Parent.BindingContext;

            await Navigation.PushAsync(new IssueDetailsPage(item.Id.Value));

           
        }

        private void Chart_LegendItemCreated(object sender, Syncfusion.SfChart.XForms.ChartLegendItemCreatedEventArgs e)
        {
            object val = e.LegendItem.DataPoint;
            string value = ((IssuesChartViewModel.IssueChartItem)val).Value.ToString();
            e.LegendItem.Label += ": " + value;
        }
        public async void OnGoToTasks(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<IssueStatusPage, Guid>(this, "IssueStatusUpdated", async (obj, taskId) =>
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "Arrow_right.png";

                MessagingCenter.Unsubscribe<IssueStatusPage, Guid>(this, "IssueStatusUpdated");
            });

            await Navigation.PushAsync(new MyWorkPage(relatedId, "Issue", parentName));

        }

        private async void btnAddIssue_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewIssuePage(parentId: relatedId));
        }
    }
}