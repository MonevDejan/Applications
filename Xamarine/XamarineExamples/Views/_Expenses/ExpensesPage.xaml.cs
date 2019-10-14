using HtmlAgilityPack;
using ProjectInsight.Models.Base;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensesPage : ContentPage
    {
        ExpenseReportsViewModel viewModel;
        public ExpensesPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();


            viewModel = new ExpenseReportsViewModel();
            viewModel.IsBusy = false;
            slExpenseReportForApproval.Children.Clear();
            slExpenseReportForApproval.Children.Add(new ExpenseReportApprovalBand());

            //Common.WorkspaceCapability.EnableTimeEntry = false;
            //Common.UserGlobalCapability.CanEnterTime = false;

            mainScreen.IsVisible = false;
            lblNoTime1.IsVisible = false;
            //lblNoTime2.IsVisible = false;
            lblNoTime3.IsVisible = false;

            if (Common.WorkspaceCapability.EnableExpenseEntry)
            {
                if (Common.UserGlobalCapability.CanEnterExpense)
                {
                    // Allow to use the time sheet pages
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
            try
            {
                ExpenseReportInfo currentReport = await ExpenseReportService.client.GetExpenseReportInfoAsync(Common.CurrentWorkspace.UserID);
                if (currentReport != null)
                {
                    if (currentReport.UnsubmittedExpenseCount != null && currentReport.UnsubmittedExpenseCount > 0)
                        viewModel.IsVisibleSubmitButton = true;
                    else
                        viewModel.IsVisibleSubmitButton = false;

                    viewModel.UnsubmittedExpenseTotal = currentReport.UnsubmittedExpenseTotal != null ? currentReport.UnsubmittedExpenseTotal.Value : 0;
                    if (currentReport.EarliestExpenseEntryDate != null)
                        viewModel.UnsubmittedPeriodFormated = string.Format("{0} - {1}", currentReport.EarliestExpenseEntryDate.Value.ToString("M/d/yy"), "Present");
                    else
                        viewModel.UnsubmittedPeriodFormated = "Present";

                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                List<ExpenseReport> mostRecentReports = await ExpenseReportService.client.GetMostRecentReportsForUserAsync(Common.CurrentWorkspace.UserID, modelProperties: new ModelProperties("default,IsRejected,ExpenseEntries,SubmittedDateTimeUTC,UpdatedDateTimeUTC,UserApprover;User:default,Photo,PhotoUrl,PhotoMediumUrl,PhotoThumbnailUrl,AvatarHtml"));
                
                if (mostRecentReports == null || mostRecentReports.Count() == 0)
                    lblNoPreviousRecords.IsVisible = true;
                else
                {
                    mostRecentReports = mostRecentReports.OrderByDescending(x => x.UpdatedDateTimeUTC).ToList();
                    lblNoPreviousRecords.IsVisible = false;
                    viewModel.ExpenseReports = new System.Collections.ObjectModel.ObservableCollection<ProjectInsight.Models.TimeAndExpense.ExpenseReport>(mostRecentReports);

                    lstAllPeriod.Children.Clear();

                    int count = 0;
                    foreach (var item in viewModel.ExpenseReports)
                    {
                        count++;

                        if (count == viewModel.ExpenseReports.Count)
                            PreviousPeriods_GeneratedRows(item, true);
                        else
                            PreviousPeriods_GeneratedRows(item);
                    }
                }

            }
            catch (Exception ex)
            {

            }
          
            return true;

        }

        private void PreviousPeriods_GeneratedRows(ExpenseReport item, bool AddLineAtBottom= false)
        {
            
            BoxView line = new BoxView();
            line.HeightRequest = 0.5;
            line.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
            line.HorizontalOptions = LayoutOptions.FillAndExpand;
            line.Margin = 0;
            Label lblId = new Label();
            lblId.IsVisible = false;

            lblId.Text = "id:" + item.Id.ToString();

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

            if (Common.UserGlobalCapability.IsExpenseReportApprover)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += (s, e) =>
                {
                    StackLayout sl = (StackLayout)s;

                    Label lbl = (Label)sl.Children[0];
                    string periodId = lbl.Text;

                    if (periodId.StartsWith("id:"))
                    {
                        //var clickedItem = viewModel.ExpenseReports.Where(x => x.Id == new Guid(periodId.Substring(3))).FirstOrDefault();
                        MessagingCenter.Subscribe<ExpenseReportDetails, bool>(this, "ExpenseReportApproved", async (obj, reloadData) =>
                        {
                            if (reloadData)
                                LoadData();

                            MessagingCenter.Unsubscribe<ExpenseReportDetails, bool>(this, "ExpenseReportApproved");
                        });
                        MessagingCenter.Subscribe<RejectExpenseReportPage, bool>(this, "ExpenseReportRejected", async (obj, reloadData) =>
                        {
                            if (reloadData)
                                LoadData();

                            MessagingCenter.Unsubscribe<RejectExpenseReportPage, bool>(this, "ExpenseReportRejected");
                        });
                        Navigation.PushAsync(new ExpenseReportDetails(new Guid(periodId.Substring(3))));
                    }
                };
                firstFirst.GestureRecognizers.Add(tapGestureRecognizer);
            }
            CultureInfo usaCulture = new CultureInfo("en-US");

            Label lbl1 = new Label();
            lbl1.FontSize = 28;
            lbl1.HorizontalTextAlignment = TextAlignment.Start;
            lbl1.HorizontalOptions = LayoutOptions.FillAndExpand;
            lbl1.VerticalTextAlignment = TextAlignment.Center;
            lbl1.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
            lbl1.Text = item.ActualCost.Value.ToString("C", usaCulture);
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
            

            lbl2.Text = item.Name;

            firstFirst.Children.Add(lbl2);

            StackLayout firstSecond = new StackLayout();
            firstSecond.VerticalOptions = LayoutOptions.CenterAndExpand;
            firstSecond.HorizontalOptions = LayoutOptions.End;
            //firstSecond.BackgroundColor = Color.Pink;
            firstSecond.Padding = 0;
            firstSecond.Margin = 0;
            firstSecond.Spacing = 0;
            first.Children.Add(firstSecond);


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
                if (item.IsApproved.Value)
                    img1.Source = "approved"; // "timesheet_approved.png";
                else if (item.IsRejected.Value)
                    img1.Source = "rejected";//"timesheet_rejected.png";
                else
                    img1.Source = "pending"; //"timesheet_pending.png";
                slStatus.Children.Add(img1);

                if (string.IsNullOrEmpty(item.UserApprover.PhotoUrl))
                {
                    if (!string.IsNullOrEmpty(item.UserApprover.AvatarHtml))
                    {
                        string userHTML = item.UserApprover.AvatarHtml;
                        //"<div class=\"user-avatar\" title=\"Gjoko Veljanoski\" style=\"background-color:#00bfff\">GV</div>"
                        //<img class="user-avatar" src="/ProjectInsight.WebApp/Sites/Files/a222e57233a14e15ab67d25e6dbab95e/UserPhotos/6fcc5602c49043c3a2d8d39175040e68/tn_avatar.png" alt="Gjoko Veljanoski" title="Gjoko Veljanoski" />
                        string myDiv = item.UserApprover.AvatarHtml;
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
                            photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + item.UserApprover.PhotoUrl));
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
                    photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + item.UserApprover.PhotoUrl));
                    photo.HeightRequest = 50;
                    photo.WidthRequest = 50;
                    photo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    photo.VerticalOptions = LayoutOptions.CenterAndExpand;
                    slAvatar.Children.Add(photo);

                    slStatus.Children.Add(slAvatar);
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

            await Navigation.PushAsync(new ChooseExpense());
        }
        private async void OnSubmitExpenseReport(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<SubmitExpenseReportPage, bool>(this, "CurrentExpenseReportSubmited", async (obj, reloadData) =>
            {
                if (reloadData)
                    LoadData();

                MessagingCenter.Unsubscribe<SubmitExpenseReportPage, bool>(this, "CurrentExpenseReportSubmited");
            });
            await Navigation.PushAsync(new SubmitExpenseReportPage());
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

        private void NotOnExpenseReport_Tapped(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<SubmitExpenseReportPage, bool>(this, "CurrentExpenseReportSubmited", async (obj, reloadData) =>
            {
                if (reloadData)
                    LoadData();

                MessagingCenter.Unsubscribe<SubmitExpenseReportPage, bool>(this, "CurrentExpenseReportSubmited");
            });
            Navigation.PushAsync(new ExpenseReportDetails(getCurrent:true));
        }
    }
}