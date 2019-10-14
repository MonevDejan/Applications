using HtmlAgilityPack;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.General;
using ProjectInsightMobile.Views.TimeEntries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views.MyTime
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTimePage : ContentPage
    {
        MyTimeViewModel viewModel;
        public MyTimePage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            //HockeyApp.MetricsManager.TrackEvent("MyTimePage Initialize");
            viewModel = new MyTimeViewModel();
            viewModel.IsBusy = false;
            slTimesheetForApproval.Children.Clear();
            slTimesheetForApproval.Children.Add(new TimesheetApprovalBand());

            //Common.WorkspaceCapability.EnableTimeEntry = false;
            //Common.UserGlobalCapability.CanEnterTime = false;
            mainScreen.IsVisible = false;
            lblNoTime1.IsVisible = false;
            //lblNoTime2.IsVisible = false;
            lblNoTime3.IsVisible = false;

            if (Common.WorkspaceCapability.EnableTimeEntry)
            {
                if (Common.UserGlobalCapability.CanEnterTime)
                {
                    //Allow to use the time sheet pages
                    mainScreen.IsVisible = true;
                    LoadData();
                }
                else
                {
                    // SHOW SCREEN 3
                    //Time tracking isn't currently available. Please contact your system administrator for more information.
                   
                    lblNoTime3.IsVisible = true;
                }
            }
            else
            {
                // SHOW SCREEN 1
                //Currently, you don’t have the time add-on installed. Tap below to install.
                if (Xamarin.Forms.Device.RuntimePlatform.ToLower() == "android")
                {
                    btnGetAddOn.IsVisible = true;
                }
                lblNoTime1.IsVisible = true;
            }
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
         
        }

        private async void LoadData()
        {
            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetData();
            viewModel.IsBusy = true;
            viewModel.VisibleLoad = false;
        }
        private async Task<bool> GetData()
        {
            List<TimeSheetInfo> result = await TimeSheetService.GetRecentTimeSheetInformation();
            viewModel.PastPeriods = new ObservableCollection<TimeSheetPeriod>();
            var settings = await TimeEntryService.Get();
            bool UseHoursMinutes = false;
            if (settings != null)
            {
                UseHoursMinutes = settings.UseHoursMinutesInput;
            }
            if (result != null && result.Count > 0)
            {
                    lstAllPeriod.Children.Clear();

                for (int i = 0; i <= result.Count - 1; i++)
                {
                    var item = result[i];
                    if (i == 0)
                    {
                        viewModel.CurrentPeriod.StartDate = item.StartDate;
                        viewModel.CurrentPeriod.EndDate = item.EndDate;
                        viewModel.CurrentPeriod.ActualHours = (item.ActualHoursNotOnTimeSheet ?? 0);
                        viewModel.CurrentPeriod.ActualHoursFormated =string.IsNullOrEmpty(item.ActualHoursNotOnTimeSheetFormattedString) ? (UseHoursMinutes ? "00:00" : "0") : item.ActualHoursNotOnTimeSheetFormattedString;
                        viewModel.CurrentPeriod.Status = 0;
                        viewModel.CurrentPeriod.TimeEntries = item.TimeSheet != null ? item.TimeSheet.TimeEntries : null;
                        viewModel.CurrentPeriod.ShowSubmitButton = true;
                        //viewModel.CurrentPeriod.Name = "";
                        if (item.AllTimeSheetsForPeriod != null && item.AllTimeSheetsForPeriod.Count > 0)
                        {
                            foreach (var ts in item.AllTimeSheetsForPeriod)
                                AddTimeSheet(ts);
                        }
                    }
                    else
                    {
                        AddPreviousTimeSheets(item);
                    }
                }
                
              
                if (viewModel.PastPeriods == null || viewModel.PastPeriods.Count() == 0)
                    lblNoPreviousRecords.IsVisible = true;
                else
                    lblNoPreviousRecords.IsVisible = false;

                int count = 0;
                foreach (var item in viewModel.PastPeriods)
                {
                    count++;

                    if (count == viewModel.PastPeriods.Count)
                        PreviousPeriods_GeneratedRows(item, true);
                    else
                        PreviousPeriods_GeneratedRows(item);
                }
            }
            return true;

        }

        private void AddPreviousTimeSheets(TimeSheetInfo item)
        {

            if (item.ActualHoursNotOnTimeSheet.HasValue && item.ActualHoursNotOnTimeSheet > 0)
            {
                TimeSheetPeriod PastPeriod = new TimeSheetPeriod();
                PastPeriod.StartDate = item.StartDate;
                PastPeriod.EndDate = item.EndDate;
                PastPeriod.ActualHours = (item.ActualHoursNotOnTimeSheet ?? 0);
                PastPeriod.ActualHoursFormated = item.ActualHoursNotOnTimeSheetFormattedString;
                PastPeriod.Status = item.TimeSheet != null ? item.TimeSheet.ApprovalStatus : 0;
                PastPeriod.ShowSubmitButton = true;
                
                viewModel.PastPeriods.Add(PastPeriod);

                if (item.AllTimeSheetsForPeriod != null && item.AllTimeSheetsForPeriod.Count > 0)
                    foreach (var ts in item.AllTimeSheetsForPeriod)
                        AddTimeSheet(ts);
            }
            else
            {
                if (item.AllTimeSheetsForPeriod != null && item.AllTimeSheetsForPeriod.Count > 0)
                    foreach (var ts in item.AllTimeSheetsForPeriod)
                        AddTimeSheet(ts);
            }
        }


        private void AddTimeSheet(TimeSheet item)
        {
            
            TimeSheetPeriod timeSheet = new TimeSheetPeriod();
            timeSheet.TimeSheetId = item.Id;
            timeSheet.StartDate = item.StartDate.Value;
            timeSheet.EndDate = item.EndDate.Value;
            timeSheet.ActualHours = item.ActualHours;
            timeSheet.ActualHoursFormated = item.ActualHoursFormattedString;
            timeSheet.Status = item.ApprovalStatus;
            timeSheet.TimeEntries = item.TimeEntries;
            timeSheet.User_Id = item.User_Id;
           
            if (item.UserApproved != null)
            {
                timeSheet.UserAvatar = item.UserApproved.AvatarHtml;
                timeSheet.UserPhotoUrl = item.UserApproved.PhotoUrl;
            }
            else if (item.UserRejected != null)
            {
                timeSheet.UserAvatar = item.UserRejected.AvatarHtml;
                timeSheet.UserPhotoUrl = item.UserRejected.PhotoUrl;
            }
            else if (item.UserApprover != null)
            {
                timeSheet.UserAvatar = item.UserApprover.AvatarHtml;
                timeSheet.UserPhotoUrl = item.UserApprover.PhotoUrl;
            }

            //timeSheet.Name = item.User.FirstName + " " + item.User.LastName;
            viewModel.PastPeriods.Add(timeSheet);
        }

        private void PreviousPeriods_GeneratedRows(TimeSheetPeriod item, bool AddLineAtBottom= false)
        {
            
            BoxView line = new BoxView();
            line.HeightRequest = 0.5;
            line.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
            line.HorizontalOptions = LayoutOptions.FillAndExpand;
            line.Margin = 0;
            Label lblId = new Label();
            lblId.IsVisible = false;
            if (item.TimeSheetId != null)
                lblId.Text = "id:" + item.TimeSheetId.ToString();
            else
            {
                lblId.Text = item.PeriodFormated;
            }
            lblId.Margin = 0;

            StackLayout main = new StackLayout();
            main.HorizontalOptions = LayoutOptions.FillAndExpand;
            main.VerticalOptions = LayoutOptions.CenterAndExpand;
            main.Orientation = StackOrientation.Vertical;
            main.Padding = 0;
            main.Margin = 0;
            main.Spacing = 0;
            main.BackgroundColor = (Color)Application.Current.Resources["WhiteTextColor"];
            main.Children.Add(line);
            //main.Children.Add(lblId);

            StackLayout first = new StackLayout();
            first.HorizontalOptions = LayoutOptions.FillAndExpand;
            first.BackgroundColor = (Color)Application.Current.Resources["WhiteTextColor"];
            first.Padding = new Thickness(18,5,15,5);
            first.Orientation = StackOrientation.Horizontal;
            main.Children.Add(first);



           
            lstAllPeriod.Children.Add(main);


            //< StackLayout HorizontalOptions = "FillAndExpand" VerticalOptions = "StartAndExpand" Orientation = "Horizontal" >
           

            StackLayout firstFirst = new StackLayout();
            firstFirst.VerticalOptions = LayoutOptions.Center;
            firstFirst.HorizontalOptions = LayoutOptions.FillAndExpand;
            firstFirst.Orientation = StackOrientation.Vertical;
            firstFirst.Padding = 0;
            firstFirst.Spacing = 0;
            firstFirst.Margin = 0;
            //firstFirst.BackgroundColor = Color.Blue;

            first.Children.Add(firstFirst);
            firstFirst.Children.Add(lblId);

            if (Common.UserGlobalCapability.IsTimeSheetApprover)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    StackLayout sl = (StackLayout)s;

                    Label lbl = (Label)sl.Children[0];
                    string periodId = lbl.Text;

                    MessagingCenter.Subscribe<ApproveTimesheetPage, bool>(this, "TimeSheetApproved", async (obj, reloadData) =>
                    {
                        if (reloadData)
                            LoadData();

                        MessagingCenter.Unsubscribe<ApproveTimesheetPage, bool>(this, "TimeSheetApproved");
                    });

                    MessagingCenter.Subscribe<RejectTimeSheetPage, bool>(this, "TimeSheetRejected", async (obj, reloadData) =>
                    {
                        if (reloadData)
                            LoadData();

                        MessagingCenter.Unsubscribe<RejectTimeSheetPage, bool>(this, "TimeSheetRejected");
                    });

                    if (periodId.StartsWith("id:"))
                    {
                        var clickedItem = viewModel.PastPeriods.Where(x => x.TimeSheetId == new Guid(periodId.Substring(3))).FirstOrDefault();
                        Navigation.PushAsync(new ApproveTimesheetPage(clickedItem));
                    }
                    else
                    {
                        var clickedItem = viewModel.PastPeriods.Where(x => x.PeriodFormated == periodId).FirstOrDefault();
                        Navigation.PushAsync(new ApproveTimesheetPage(clickedItem));
                    }
                   

                };
                firstFirst.GestureRecognizers.Add(tapGestureRecognizer);
            }
           

            Label lbl1 = new Label();
            lbl1.FontSize = 28;
            lbl1.HorizontalTextAlignment = TextAlignment.Start;
            lbl1.HorizontalOptions = LayoutOptions.FillAndExpand;
            lbl1.VerticalTextAlignment = TextAlignment.Center;
            lbl1.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
            lbl1.Text = item.ActualHoursFormated;
            lbl1.Margin = 0;

            firstFirst.Children.Add(lbl1);

            Label lbl2 = new Label();
            lbl2.FontSize = 16;
            lbl2.HorizontalTextAlignment = TextAlignment.Start;
            lbl2.HorizontalOptions = LayoutOptions.FillAndExpand;
            lbl2.VerticalTextAlignment = TextAlignment.Start;
            lbl2.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
            lbl2.LineBreakMode = LineBreakMode.NoWrap;
            lbl2.Margin = 0;
            

            lbl2.Text = item.PeriodFormatedLong;

            firstFirst.Children.Add(lbl2);

            StackLayout firstSecond = new StackLayout();
            firstSecond.VerticalOptions = LayoutOptions.CenterAndExpand;
            firstSecond.HorizontalOptions = LayoutOptions.End;
            //firstSecond.BackgroundColor = Color.Pink;
            firstSecond.Padding = 0;
            firstSecond.Margin = 0;
            firstSecond.Spacing = 0;
            first.Children.Add(firstSecond);

            if (item.ShowSubmitButton)
            {
                Label lblId1 = new Label();
                lblId1.IsVisible = false;

                if (item.TimeSheetId != null)
                    lblId1.Text = "id:" + item.TimeSheetId.ToString();
                else
                {
                    lblId1.Text = item.PeriodFormated;
                }
                lblId1.Margin = 0;
                firstSecond.Children.Add(lblId1);

                StackLayout btnSl = new StackLayout();
                btnSl.Padding = new Thickness(0, 0, 10, 0);
                btnSl.HorizontalOptions = LayoutOptions.EndAndExpand;
                btnSl.VerticalOptions = LayoutOptions.CenterAndExpand;

                var submitGestureRecognized = new TapGestureRecognizer();
                submitGestureRecognized.Tapped += (s, e) =>
                {
                    StackLayout sl = (StackLayout)s;
                    Label lbl = (Label)sl.Children[0];
                    string periodId = lbl.Text;
                    var clickedItem = viewModel.PastPeriods.Where(x => x.PeriodFormated == periodId).FirstOrDefault();
                    if (clickedItem.ShowSubmitButton)
                    {
                        MessagingCenter.Subscribe<SubmitTimeSheetPage, bool>(this, "TimeSheetSubmited", async (obj, reloadData) =>
                        {
                            if (reloadData)
                                LoadData();

                            MessagingCenter.Unsubscribe<SubmitTimeSheetPage, bool>(this, "TimeSheetSubmited");
                        });
                        Navigation.PushAsync(new SubmitTimeSheetPage(clickedItem));
                    }
                };
                firstSecond.GestureRecognizers.Add(submitGestureRecognized);
                firstSecond.Children.Add(btnSl);

               

                Frame frmSubmit = new Frame();
                frmSubmit.HasShadow = false;
                frmSubmit.Padding = new Thickness(10, 7, 10, 7);
                frmSubmit.CornerRadius = 5;
                frmSubmit.BackgroundColor = (Color)Application.Current.Resources["Primary"];
                frmSubmit.VerticalOptions = LayoutOptions.CenterAndExpand;
                frmSubmit.VerticalOptions = LayoutOptions.End;

                frmSubmit.WidthRequest = 100;
                firstSecond.Children.Add(frmSubmit);


                Label lblSubmit = new Label();
                lblSubmit.FontSize = 22;
                lblSubmit.HorizontalTextAlignment = TextAlignment.Start;
                lblSubmit.HorizontalOptions = LayoutOptions.CenterAndExpand;
                lblSubmit.VerticalTextAlignment = TextAlignment.Center;

                lblSubmit.TextColor = (Color)Application.Current.Resources["WhiteTextColor"];
                lblSubmit.Text = item.PeriodFormatedLong;
                lblSubmit.HeightRequest = 30;
                lblSubmit.Text = "Submit";
                frmSubmit.Content = lblSubmit;
            }
            else
            {




                StackLayout slStatus = new StackLayout();
                slStatus.VerticalOptions = LayoutOptions.CenterAndExpand;
                slStatus.HorizontalOptions = LayoutOptions.FillAndExpand;
                slStatus.Orientation = StackOrientation.Horizontal;
                //firstSecond.BackgroundColor = Color.Pink;
                slStatus.Padding = 0;
                slStatus.Margin = 0;
                slStatus.Spacing = 0;
                firstSecond.Children.Add(slStatus);


                Image img1 = new Image();

                img1.HeightRequest = 30;
                img1.HorizontalOptions = LayoutOptions.CenterAndExpand;
                if (item.ShowApprovedIcon)
                    img1.Source = "approved"; // "timesheet_approved.png";
                else if (item.ShowRejectedIcon)
                    img1.Source = "rejected";//"timesheet_rejected.png";
                else
                    img1.Source = "pending"; //"timesheet_pending.png";
                slStatus.Children.Add(img1);

                if (string.IsNullOrEmpty(item.UserPhotoUrl))
                {
                    if (!string.IsNullOrEmpty(item.UserAvatar))
                    {
                        string userHTML = item.UserAvatar;
                        //"<div class=\"user-avatar\" title=\"Gjoko Veljanoski\" style=\"background-color:#00bfff\">GV</div>"
                        //<img class="user-avatar" src="/ProjectInsight.WebApp/Sites/Files/a222e57233a14e15ab67d25e6dbab95e/UserPhotos/6fcc5602c49043c3a2d8d39175040e68/tn_avatar.png" alt="Gjoko Veljanoski" title="Gjoko Veljanoski" />
                        string myDiv = item.UserAvatar;
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

                        StackLayout slAvatar = new StackLayout();
                        slAvatar.HeightRequest = 50;
                        slAvatar.WidthRequest = 50;
                        slAvatar.VerticalOptions = LayoutOptions.StartAndExpand;
                        slAvatar.HorizontalOptions = LayoutOptions.StartAndExpand;
                       
                        slAvatar.Margin = new Thickness(15, 0, 0, 0);

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
                            photo.Source = ImageSource.FromUri(new Uri(PhotoURL));
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
                    StackLayout slAvatar = new StackLayout();
                    slAvatar.HeightRequest = 50;
                    slAvatar.WidthRequest = 50;
                    slAvatar.VerticalOptions = LayoutOptions.StartAndExpand;
                    slAvatar.HorizontalOptions = LayoutOptions.StartAndExpand;
                    slAvatar.Margin = new Thickness(15, 0, 0, 0);

                    Image photo = new Image();
                    photo.Source = ImageSource.FromUri(new Uri(item.UserPhotoUrl));
                    photo.HeightRequest = 50;
                    photo.WidthRequest = 50;
                    photo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    photo.VerticalOptions = LayoutOptions.CenterAndExpand;
                    slAvatar.Children.Add(photo);

                    slStatus.Children.Add(slAvatar);
                }

            }

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
        

        private async void OnCreateTimeEntry(object sender, EventArgs e)
        {

            //await Navigation.PushAsync(new CreateTimeEntryPage());


            TimeEntryWizardViewModel ViewModel = new TimeEntryWizardViewModel();
            ViewModel.ItemType = ItemType.Task;
            await Navigation.PushAsync(new Company_Project_Task(ViewModel));
            
        }
        private async void OnSubmitTimeSheet(object sender, EventArgs e)
        {
            var clickedItem = viewModel.CurrentPeriod.PeriodFormated;
            if (viewModel.CurrentPeriod.ShowSubmitButton)
            {
                MessagingCenter.Subscribe<SubmitTimeSheetPage, bool>(this, "TimeSheetSubmited", async (obj, reloadData) =>
                {
                    if (reloadData)
                        LoadData();

                    MessagingCenter.Unsubscribe<SubmitTimeSheetPage, bool>(this, "TimeSheetSubmited");
                });

                await Navigation.PushAsync(new SubmitTimeSheetPage(viewModel.CurrentPeriod));
            }
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {

        }

        private void GetTimeAddOn_Tapped(object sender, EventArgs e)
        {
            Xamarin.Forms.Device.OpenUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + "/pi-adm/apps/list-all"));
        }

        //private void TurnOnTimeAddOn_Tapped(object sender, EventArgs e)
        //{
        //    Xamarin.Forms.Device.OpenUri(new Uri(Common.CurrentWorkspace.WorkspaceURL));
        //}

        private async void GoToApproval_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ApproveTimesheetsPage());
        }

        private async void CurrentTimeSheet_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<SubmitTimeSheetPage, bool>(this, "TimeSheetSubmited", async (obj, reloadData) =>
            {
                if (reloadData)
                    LoadData();

                MessagingCenter.Unsubscribe<SubmitTimeSheetPage, bool>(this, "TimeSheetSubmited");
            });
            await Navigation.PushAsync(new ApproveTimesheetPage(isCurrent: true));
        }
    }
}