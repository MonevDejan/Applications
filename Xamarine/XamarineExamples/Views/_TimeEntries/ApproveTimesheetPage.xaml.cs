using HtmlAgilityPack;
using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApproveTimesheetPage : ContentPage
    {
        ApproveTimeSheetViewModel viewModel;
        TimeSheetPeriod tsp;
        Guid tId;
        bool IsCurrent = false;

        public ApproveTimesheetPage(TimeSheetPeriod item, bool isCurrent = false)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            tsp = item;
            if (isCurrent)
            {
                slButtonsForUnsubmited.IsVisible = true;
            }
            else
            {
                slButtonsForUnsubmited.IsVisible = item.ShowSubmitButton;
                if (item.ShowPendingApprovalIcon && item.User_Id == Common.CurrentWorkspace.UserID)
                    ToolbarItems.Add(new ToolbarItem() { Text = "", Icon = "unsubmit_icon.png", Command = new Command(this.btnDeleteTimesheet_Clicked) });
            }
            viewModel = new ApproveTimeSheetViewModel();
            viewModel.VisibleLoad = true;
            BindingContext = viewModel;
            LoadData();

            //HockeyApp.MetricsManager.TrackEvent("MyTimePage Initialize");
        }

        public ApproveTimesheetPage(bool isCurrent = false)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            if (isCurrent)
            {
                slButtonsForUnsubmited.IsVisible = true;
                IsCurrent = isCurrent;

            }
            viewModel = new ApproveTimeSheetViewModel();
            viewModel.VisibleLoad = true;
            BindingContext = viewModel;
            LoadData();
           
        }

        public ApproveTimesheetPage(Guid itemId)
        {

            tId = itemId;
          
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            viewModel = new ApproveTimeSheetViewModel();
            viewModel.VisibleLoad = true;
            BindingContext = viewModel;
            LoadData();
            
            //HockeyApp.MetricsManager.TrackEvent("MyTimePage Initialize");
        }

        private void GenerateTimeEntryGrid()
        {
            var settings = TimeEntryService.GetSettings();
            bool UseHoursMinutes = false;
            if (settings != null)
                UseHoursMinutes = settings.UseHoursMinutesInput;

            lstAllPeriod.Children.Clear();
            int count = 0;
            DateTime previousDate = new DateTime();

            foreach (TimeEntry timeEntry in viewModel.TimeEntries)
            {
                count++;
                
                if (timeEntry.Date.Value.Date != previousDate)
                {
                    //create header
                    previousDate = timeEntry.Date.Value;

                    BoxView line = new BoxView();
                    line.HeightRequest = 0.5;
                    line.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
                    line.HorizontalOptions = LayoutOptions.FillAndExpand;
                    line.Margin = 0;
                    lstAllPeriod.Children.Add(line);

                    StackLayout main = new StackLayout();
                    main.HorizontalOptions = LayoutOptions.FillAndExpand;
                    main.VerticalOptions = LayoutOptions.CenterAndExpand;
                    main.Orientation = StackOrientation.Horizontal;
                    main.Padding = new Thickness(10, 15, 10, 10);
                    main.Margin = 0;
                    main.Spacing = 0;
                    main.BackgroundColor = (Color)Application.Current.Resources["WhiteTextColor"];

                    Label header = new Label();
                    header.FontSize = 24;
                    header.FontAttributes = FontAttributes.Bold;
                    if (Device.RuntimePlatform.ToLower() == "android")
                        header.FontFamily = "OpenSans-SemiBold.ttf#Open Sans";// Application.Current.Resources["BoldFont"].ToString();
                    else
                        header.FontFamily = "OpenSans-SemiBold";

                    header.HorizontalTextAlignment = TextAlignment.Start;
                    header.HorizontalOptions = LayoutOptions.StartAndExpand;
                    header.VerticalTextAlignment = TextAlignment.Center;
                    header.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
                    header.Text = timeEntry.Date.Value.Date.ToString("dddd M/d/yy");
                    header.LineBreakMode = LineBreakMode.TailTruncation;
                    main.Children.Add(header);

                    Label totalHours = new Label();
                    totalHours.FontSize = 24;
                    totalHours.FontAttributes = FontAttributes.Bold;
                    if (Device.RuntimePlatform.ToLower() == "android")
                        totalHours.FontFamily = "OpenSans-SemiBold.ttf#Open Sans";// Application.Current.Resources["BoldFont"].ToString();
                    else
                        totalHours.FontFamily = "OpenSans-SemiBold";

                    totalHours.HorizontalTextAlignment = TextAlignment.End;
                    totalHours.HorizontalOptions = LayoutOptions.End;
                    totalHours.VerticalTextAlignment = TextAlignment.Center;
                    totalHours.TextColor = (Color)Application.Current.Resources["BlackTextColor"];

                    decimal total = viewModel.TimeEntries.Where(x => x.Date.Value.Date == previousDate.Date).Sum(z=>z.ActualHours).Value;
                    if (UseHoursMinutes)
                    {
                        TimeSpan timespan = TimeSpan.FromHours((double)total);
                        string totalFormated = timespan.ToString("h\\:mm");
                        totalHours.Text = totalFormated;
                    }
                    else
                        totalHours.Text = Math.Round(total, 2).ToString("F2");
                    totalHours.LineBreakMode = LineBreakMode.NoWrap;
                    main.Children.Add(totalHours);
                    lstAllPeriod.Children.Add(main);
                }



                TimeEntryGrid_GenerateRow(timeEntry, count == viewModel.TimeEntries.Count ? true :false);
            }


        }

        private async void LoadData()
        {
            viewModel.VisibleLoad = true;

            viewModel.CurrentPeriod = tsp;
            if (IsCurrent)
            {
                List<TimeSheetInfo> result = await TimeSheetService.GetRecentTimeSheetInformation();
                var settings = await TimeEntryService.Get();
                bool UseHoursMinutes = false;
                if (settings != null)
                {
                    UseHoursMinutes = settings.UseHoursMinutesInput;
                }
                if (result != null && result.Count > 0)
                {
                    lstAllPeriod.Children.Clear();
                    viewModel.CurrentPeriod = new TimeSheetPeriod();
                    for (int i = 0; i <= result.Count - 1; i++)
                    {
                        var item = result[i];
                        if (i == 0)
                        {
                            viewModel.CurrentPeriod.StartDate = item.StartDate;
                            viewModel.CurrentPeriod.EndDate = item.EndDate;
                            viewModel.CurrentPeriod.ActualHours = (item.ActualHoursNotOnTimeSheet ?? 0);
                            viewModel.CurrentPeriod.ActualHoursFormated = string.IsNullOrEmpty(item.ActualHoursNotOnTimeSheetFormattedString) ? (UseHoursMinutes ? "00:00" : "0") : item.ActualHoursNotOnTimeSheetFormattedString;
                            viewModel.CurrentPeriod.Status = 0;
                            viewModel.CurrentPeriod.TimeEntries = item.TimeSheet != null ? item.TimeSheet.TimeEntries : null;
                            viewModel.CurrentPeriod.ShowSubmitButton = true;

                            viewModel.CurrentPeriod.RequiresApprovalFromCurrentUser = false;
                            var timeEntries = TimeEntryService.client.GetByUserUnsubmittedForDateRange(viewModel.CurrentPeriod.StartDate, viewModel.CurrentPeriod.EndDate, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Name,Task_Id,ToDo_Id,Issue_Id,Company_Id"));
                            viewModel.TimeEntries = timeEntries;

                        }
                        else
                            break;
                    }
                }

            }
            else
            {
                if (viewModel.CurrentPeriod != null)
                {


                    if (viewModel.CurrentPeriod.TimeSheetId.HasValue)
                    {
                        try
                        {
                            var ts = await TimeSheetService.client.GetAsync(viewModel.CurrentPeriod.TimeSheetId.Value, new ProjectInsight.Models.Base.ModelProperties(
                            "default,UserApprover,ApprovalStatusDescription,RequiresApprovalFromCurrentUser,User,TimeEntries;TimeEntry:default,Name,Task_Id,ToDo_Id,Issue_Id,Company_Id;User:default,Photo,PhotoUrl,PhotoMediumUrl,PhotoThumbnailUrl,AvatarHtml"));
                            viewModel.TimeEntries = ts.TimeEntries;
                            viewModel.CurrentPeriod.Name = ts.User.FirstName + " " + ts.User.LastName;
                            viewModel.CurrentPeriod.RequiresApprovalFromCurrentUser = ts.RequiresApprovalFromCurrentUser ?? false;
                            viewModel.CurrentPeriod.ActualHoursFormated = ts.ActualHoursFormattedString;
                            viewModel.timeSheet = ts;

                            viewModel.ShowStatus = true;
                            int status = ts.ApprovalStatus ?? 0;
                            //----------------------
                            if (status == 5)
                                statusIcon.Source = "approved"; // "timesheet_approved.png";
                            else if (status == 6)
                                statusIcon.Source = "rejected";//"timesheet_rejected.png";
                            else
                                statusIcon.Source = "pending"; //"timesheet_pending.png";
                            slAvatar.Children.Clear();
                            if (string.IsNullOrEmpty(ts.UserApprover.PhotoUrl))
                            {
                                if (!string.IsNullOrEmpty(ts.UserApprover.AvatarHtml))
                                {
                                    string userHTML = ts.UserApprover.AvatarHtml;
                                    //"<div class=\"user-avatar\" title=\"Gjoko Veljanoski\" style=\"background-color:#00bfff\">GV</div>"
                                    //<img class="user-avatar" src="/ProjectInsight.WebApp/Sites/Files/a222e57233a14e15ab67d25e6dbab95e/UserPhotos/6fcc5602c49043c3a2d8d39175040e68/tn_avatar.png" alt="Gjoko Veljanoski" title="Gjoko Veljanoski" />
                                    string myDiv = ts.UserApprover.AvatarHtml;
                                    HtmlDocument doc = new HtmlDocument();
                                    doc.LoadHtml(myDiv);
                                    HtmlNode node = doc.DocumentNode.SelectSingleNode("div");
                                    string AvatarInitials = "US";
                                    string AvatarColor = "#fff";
                                    string PhotoURL = string.Empty;

                                    if (node != null)
                                    {
                                        AvatarInitials = (node.ChildNodes[0]).OuterHtml;
                                        foreach (HtmlAttribute attr in node.Attributes)
                                        {
                                            if (attr.Name.ToLower() == "style")
                                            {
                                                string[] parts = attr.Value.Split('#');
                                                if (parts != null && parts.Length > 1)
                                                {
                                                    AvatarColor = parts[1];
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        HtmlNode node2 = doc.DocumentNode.SelectSingleNode("img");
                                        if (node2 != null)
                                        {
                                            foreach (HtmlAttribute attr in node2.Attributes)
                                            {
                                                if (attr.Name.ToLower() == "src")
                                                {
                                                    string imageUrl = attr.Value.Replace("/ProjectInsight.WebApp", "");
                                                    PhotoURL = Common.CurrentWorkspace.WorkspaceURL + imageUrl;
                                                }
                                            }
                                        }
                                    }

                                    if (string.IsNullOrEmpty(PhotoURL))
                                    {
                                        slAvatar.BackgroundColor = Color.FromHex(AvatarColor);

                                        Label lbInitials = new Label();
                                        lbInitials.HeightRequest = 50;
                                        lbInitials.WidthRequest = 50;
                                        lbInitials.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                        lbInitials.VerticalOptions = LayoutOptions.CenterAndExpand;
                                        lbInitials.HorizontalTextAlignment = TextAlignment.Center;
                                        lbInitials.VerticalTextAlignment = TextAlignment.Center;
                                        lbInitials.TextColor = Color.White;
                                        lbInitials.Text = AvatarInitials;
                                        lbInitials.FontSize = 26;
                                        if (Device.RuntimePlatform.ToLower() == "android")
                                            lbInitials.FontFamily = "OpenSans-SemiBold.ttf#Open Sans";
                                        else
                                            lbInitials.FontFamily = "OpenSans-SemiBold";
                                        slAvatar.Children.Add(lbInitials);
                                    }
                                    else
                                    {
                                        Image photo = new Image();
                                        photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + ts.UserApprover.PhotoUrl));
                                        photo.HeightRequest = 50;
                                        photo.WidthRequest = 50;
                                        photo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                        photo.VerticalOptions = LayoutOptions.CenterAndExpand;
                                        slAvatar.Children.Add(photo);
                                    }
                                    slStatus.Children.Add(slAvatar);

                                }
                            }
                            else
                            {
                                Image photo = new Image();
                                photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + ts.UserApprover.PhotoUrl));
                                photo.HeightRequest = 50;
                                photo.WidthRequest = 50;
                                //photo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                photo.VerticalOptions = LayoutOptions.CenterAndExpand;
                                slAvatar.Children.Add(photo);
                            }
                            //----------------------
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        try
                        {
                            viewModel.CurrentPeriod.RequiresApprovalFromCurrentUser = false;
                            var timeEntries = TimeEntryService.client.GetByUserUnsubmittedForDateRange(viewModel.CurrentPeriod.StartDate, viewModel.CurrentPeriod.EndDate, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Name,Task_Id,ToDo_Id,Issue_Id,Company_Id"));
                            viewModel.TimeEntries = timeEntries;
                            //viewModel.CurrentPeriod.ShowSubmitButton = true;
                            slCreateTimeEntry.IsVisible = false;

                        }
                        catch (Exception ex)
                        {

                        }
                    }


                }
                else
                {
                    if (tId != null)
                    {
                        viewModel.CurrentPeriod = new TimeSheetPeriod();
                        var ts = TimeSheetService.client.Get(tId, new ProjectInsight.Models.Base.ModelProperties("default,RequiresApprovalFromCurrentUser,User,TimeEntries;TimeEntry:default,Name,Task_Id,ToDo_Id,Issue_Id,Company_Id;User:FirstName,LastName;"));
                        viewModel.CurrentPeriod.Name = ts.User.FirstName + " " + ts.User.LastName;
                        viewModel.CurrentPeriod.RequiresApprovalFromCurrentUser = ts.RequiresApprovalFromCurrentUser ?? false;
                        viewModel.CurrentPeriod.TimeSheetId = ts.Id;
                        viewModel.CurrentPeriod.StartDate = ts.StartDate.Value;
                        viewModel.CurrentPeriod.EndDate = ts.EndDate.Value;
                        viewModel.CurrentPeriod.ActualHours = ts.ActualHours;
                        viewModel.CurrentPeriod.ActualHoursFormated = ts.ActualHoursFormattedString;
                        viewModel.CurrentPeriod.Status = ts.ApprovalStatus;
                        viewModel.CurrentPeriod.TimeEntries = ts.TimeEntries;
                        viewModel.CurrentPeriod.User_Id = ts.User_Id;
                        viewModel.timeSheet = ts;
                        viewModel.TimeEntries = ts.TimeEntries;

                        if (viewModel.CurrentPeriod.ShowPendingApprovalIcon && viewModel.CurrentPeriod.User_Id == Common.CurrentWorkspace.UserID)
                            ToolbarItems.Add(new ToolbarItem() { Text = "", Icon = "unsubmit_icon.png", Command = new Command(this.btnDeleteTimesheet_Clicked) });

                    }
                }
            }
            if (viewModel.TimeEntries != null && viewModel.TimeEntries.Count > 0)
                GenerateTimeEntryGrid();

            viewModel.VisibleLoad = false;
        }

        private void TimeEntryGrid_GenerateRow(TimeEntry item, bool AddLineAtBottom = false)
        {

            BoxView line = new BoxView();
            line.HeightRequest = 0.5;
            line.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
            line.HorizontalOptions = LayoutOptions.FillAndExpand;
            line.Margin = 0;
            lstAllPeriod.Children.Add(line);

            StackLayout main = new StackLayout();
            main.HorizontalOptions = LayoutOptions.FillAndExpand;
            main.VerticalOptions = LayoutOptions.CenterAndExpand;
            main.Orientation = StackOrientation.Horizontal;
            main.Padding = new Thickness(0, 5, 0, 5);
            main.Margin = 0;
            main.Spacing = 0;
            main.BackgroundColor = (Color)Application.Current.Resources["WhiteTextColor"];
            
            //main.Children.Add(lblId);

            StackLayout first = new StackLayout();
            first.HorizontalOptions = LayoutOptions.StartAndExpand;
            first.BackgroundColor = (Color)Application.Current.Resources["WhiteTextColor"];
            first.Padding = new Thickness(10, 5, 10, 5);
            first.Orientation = StackOrientation.Horizontal;
       

            lstAllPeriod.Children.Add(main);


            //< StackLayout HorizontalOptions = "FillAndExpand" VerticalOptions = "StartAndExpand" Orientation = "Horizontal" >

            Image imgEntryType = new Image();

            imgEntryType.HeightRequest = 50;
            imgEntryType.HorizontalOptions = LayoutOptions.Start;

            if (item.Issue_Id.HasValue)
            {
                imgEntryType.Source = "item_issue";
            }
            else if (item.Task_Id.HasValue)
            {
                imgEntryType.Source = "item_task";
            }
            else if (item.ToDo_Id.HasValue)
            {
                imgEntryType.Source = "item_todo";
            }
            else if (item.Company_Id.HasValue)
            {
                imgEntryType.Source = "item_company";
            }
            else
            {
                imgEntryType.Source = "item_task";
            }

            // "timesheet_approved.png";
            first.Children.Add(imgEntryType);


            Label lbl1 = new Label();
            lbl1.FontSize = 22;
            lbl1.HorizontalTextAlignment = TextAlignment.Start;
            lbl1.HorizontalOptions = LayoutOptions.Start;
            lbl1.VerticalTextAlignment = TextAlignment.Center;
            lbl1.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
            lbl1.Text = item.Name;
            lbl1.LineBreakMode = LineBreakMode.TailTruncation;
            first.Children.Add(lbl1);
            main.Children.Add(first);

            StackLayout second = new StackLayout();
            second.Padding = new Thickness(0, 0, 10, 0);
            second.HorizontalOptions = LayoutOptions.EndAndExpand;
            second.VerticalOptions = LayoutOptions.CenterAndExpand;

            Label lblHours = new Label();
            lblHours.FontSize = 22;
            lblHours.HorizontalTextAlignment = TextAlignment.End;
            lblHours.HorizontalOptions = LayoutOptions.End;
            lblHours.VerticalTextAlignment = TextAlignment.Center;
            lblHours.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
            lblHours.LineBreakMode = LineBreakMode.NoWrap;
            if (item.ActualHoursFormattedString.EndsWith(":00"))
            {
                item.ActualHoursFormattedString = item.ActualHoursFormattedString.Substring(0, item.ActualHoursFormattedString.Length - 3);
            }
            lblHours.Text = item.ActualHoursFormattedString;
            second.Children.Add(lblHours);

            main.Children.Add(second);

           

            if (AddLineAtBottom)
            {
                BoxView line1 = new BoxView();
                line1.HeightRequest = 0.5;
                line1.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
                line1.HorizontalOptions = LayoutOptions.FillAndExpand;
                line1.VerticalOptions = LayoutOptions.End;
                line1.Margin = 0;
                lstAllPeriod.Children.Add(line1);
            }
        }


        private async void OnApprove(object sender, EventArgs e)
        {
            viewModel.VisibleLoad = true;
            ApiSaveResponse result = await TimeSheetService.ApproveAsync(viewModel.CurrentPeriod.TimeSheetId.Value);
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                }
            }
            else
            {
                Common.Instance.ShowToastMessage("Timesheet approved!", DoubleResources.SuccessSnackBar);
            }
            viewModel.VisibleLoad = false;
            MessagingCenter.Send(this, "TimeSheetApproved", true);
            await Navigation.PopToRootAsync();
        }

        private async void OnReject(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RejectTimeSheetPage(viewModel.timeSheet));
        }

        private async void btnDeleteTimesheet_Clicked()
        {
            var resp = await DisplayActionSheet("Do you want to unsubmit the timesheet?", "No", "Yes");
            if (resp != null && resp.ToString().Length > 0 && resp.Equals("Yes"))
            {
                ApiDeleteResponse result = await TimeSheetService.Delete(viewModel.CurrentPeriod.TimeSheetId.Value);
                if (result != null)
                {
                    if (result.HasErrors)
                    {
                        foreach (var error in result.Errors)
                        {
                            Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                        }
                    }
                    else
                    {
                        Common.Instance.ShowToastMessage("Timesheet deleted!", DoubleResources.SuccessSnackBar);
                        MessagingCenter.Send(this, "TimeSheetApproved", true);
                        await Navigation.PopToRootAsync();
                    }
                    //await Navigation.PopAsync();
                }
                else
                {
                    Common.Instance.ShowToastMessage("Error contacting server!", DoubleResources.DangerSnackBar);
                }
            }
        }
        private async void OnCreateTimeEntry(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new CreateTimeEntryPage());
        }
        private async void OnSubmitTimeSheet(object sender, EventArgs e)
        {
            var clickedItem = viewModel.CurrentPeriod.PeriodFormated;
            await Navigation.PushAsync(new SubmitTimeSheetPage(viewModel.CurrentPeriod));
        }
    }
}