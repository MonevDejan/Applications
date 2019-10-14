using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.ListView.XForms;
using System.Threading.Tasks;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using HtmlAgilityPack;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseReportDetails : ContentPage
    {
        ExpenseReportDetailsViewModel viewModel;
        public ExpenseReportDetails(bool getCurrent = false)
        {
          

            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            viewModel = new ExpenseReportDetailsViewModel();
            viewModel.VisibleLoad = true;
            BindingContext = viewModel;
            if (getCurrent)
            {

                Title = "Unsubmitted Expenses";
                viewModel.IsCurrent = true;
            }

            LoadData();
        }
      
        public ExpenseReportDetails(Guid itemId)
        {
            viewModel = new ExpenseReportDetailsViewModel();
            viewModel.Id = itemId;
            viewModel.VisibleLoad = true;
            BindingContext = viewModel;

            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            Title = "Past Expense Report";
            //HockeyApp.MetricsManager.TrackEvent("MyTimePage Initialize");

            LoadData();
        }
        protected override void OnAppearing()
        {

            base.OnAppearing();

          
         
        }

        private async void LoadData()
        {
            viewModel.VisibleLoad = true;

            viewModel.ExpenseEntries = new System.Collections.ObjectModel.ObservableCollection<ExpenseEntryDetailsViewModel>();
            List<ProjectInsight.Models.TimeAndExpense.ExpenseEntry> expenseEntries = new List<ProjectInsight.Models.TimeAndExpense.ExpenseEntry>();
            if (viewModel.IsCurrent)
            {
                listView.AllowSwiping = true;
                expenseEntries = await ExpenseEntryService.client.GetNotOnExpenseReportAsync(Common.CurrentWorkspace.UserID, new ProjectInsight.Models.Base.ModelProperties("default,UploadedFile,ProjectName,CompanyName,Task,Project;Project:ItemNumberFullAndNameDisplayPreference;Task:ItemNumberFullAndNameDisplayPreference"));

                expenseEntries = expenseEntries.OrderByDescending(x => x.UpdatedDateTimeUTC).ToList();
                viewModel.Name = String.Empty;
                var report = await ExpenseReportService.client.GetExpenseReportInfoAsync(Common.CurrentWorkspace.UserID);
                if (report != null)
                {
                    if (report.UnsubmittedExpenseCount != null && report.UnsubmittedExpenseCount > 0)
                        viewModel.IsVisibleSubmitButton = true;
                    else
                        viewModel.IsVisibleSubmitButton = false;
                    viewModel.ActualCost = report.UnsubmittedExpenseTotal != null ? report.UnsubmittedExpenseTotal.Value : 0;
                    if (report.EarliestExpenseEntryDate != null)
                    {
                        viewModel.PeriodFormated = string.Format("{0} - {1}", report.EarliestExpenseEntryDate.Value.ToString("M/d/yy"), "Present");
                    }
                }
            }
            else
            {
                if (viewModel.Id != null)
                {
                    ExpenseReport report = await ExpenseReportService.client.GetAsync(viewModel.Id.Value, new ProjectInsight.Models.Base.ModelProperties("default,UserApprover,ApprovalStatusDescription,ExpenseEntries,IsRejected,ExpenseEntries,User;ExpenseEntry:default,ProjectName,CompanyName,Task,Project;Project:ItemNumberFullAndNameDisplayPreference;Task:ItemNumberFullAndNameDisplayPreference;User:default,Photo,PhotoUrl,PhotoMediumUrl,PhotoThumbnailUrl,AvatarHtml"));
                    if (report != null)
                    {
                        viewModel.ActualCost = report.ActualCost != null ? report.ActualCost.Value : 0;
                        expenseEntries = report.ExpenseEntries.OrderByDescending(x => x.Date).ToList();

                        viewModel.Name = report.User.FirstName + " " + report.User.LastName;
                        viewModel.ApprovalStatusDescription = report.ApprovalStatusDescription;

                        listView.AllowSwiping = report.IsRejected ?? false;
                        if (report.StartDate == null || report.EndDate == null)
                            viewModel.PeriodFormated = String.Empty;
                        else
                            viewModel.PeriodFormated = string.Format("{0} - {1}", report.StartDate.Value.ToString("M/d/yy"), report.EndDate.Value.ToString("M/d/yy"));

                        int status = report.ApprovalStatus ?? 0;

                        if (report.UserApprover_Id.HasValue && report.UserApprover_Id == Common.CurrentWorkspace.UserID && status == 0)
                            viewModel.RequiresApprovalFromCurrentUser = true;
                        else
                            viewModel.RequiresApprovalFromCurrentUser = false;

                        viewModel.ShowStatus = true;

                        //----------------------
                        if (status == 5)
                            statusIcon.Source = "approved"; // "timesheet_approved.png";
                        else if (status == 6)
                            statusIcon.Source = "rejected";//"timesheet_rejected.png";
                        else
                            statusIcon.Source = "pending"; //"timesheet_pending.png";
                        slAvatar.Children.Clear();
                        if (string.IsNullOrEmpty(report.UserApprover.PhotoUrl))
                        {
                            if (!string.IsNullOrEmpty(report.UserApprover.AvatarHtml))
                            {
                                string userHTML = report.UserApprover.AvatarHtml;
                                //"<div class=\"user-avatar\" title=\"Gjoko Veljanoski\" style=\"background-color:#00bfff\">GV</div>"
                                //<img class="user-avatar" src="/ProjectInsight.WebApp/Sites/Files/a222e57233a14e15ab67d25e6dbab95e/UserPhotos/6fcc5602c49043c3a2d8d39175040e68/tn_avatar.png" alt="Gjoko Veljanoski" title="Gjoko Veljanoski" />
                                string myDiv = report.UserApprover.AvatarHtml;
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
                                    photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + report.UserApprover.PhotoUrl));
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
                            photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + report.UserApprover.PhotoUrl));
                            photo.HeightRequest = 50;
                            photo.WidthRequest = 50;
                            //photo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                            photo.VerticalOptions = LayoutOptions.CenterAndExpand;
                            slAvatar.Children.Add(photo);
                        }
                        //----------------------
                    }
                }
            }

            foreach (var entry in expenseEntries)
            {
                try
                {
                    ExpenseEntryDetailsViewModel newEntry = new ExpenseEntryDetailsViewModel();

                    newEntry.Id = entry.Id;
                    newEntry.FirstLine = string.Empty;

                    if (entry.Project != null && !string.IsNullOrEmpty(entry.Project.ItemNumberFullAndNameDisplayPreference))
                        newEntry.FirstLine = entry.Project.ItemNumberFullAndNameDisplayPreference;
                    else
                    {
                        if (!string.IsNullOrEmpty(entry.CompanyName))
                            newEntry.FirstLine = entry.CompanyName;
                    }

                    newEntry.SecondLine = entry.Date != null ? entry.Date.Value.ToString("M/d/yy") : String.Empty;
                    if (entry.Task != null && !string.IsNullOrEmpty(entry.Task.ItemNumberFullAndNameDisplayPreference))
                        newEntry.SecondLine += " | " + entry.Task.ItemNumberFullAndNameDisplayPreference;

                    newEntry.ThirdLine = entry.Description ?? String.Empty;
                    newEntry.ActualCost = entry.ActualCost ?? 0;

                    viewModel.ExpenseEntries.Add(newEntry);
                }
                catch (Exception ex)
                {

                }
            }

            int height = 10 + (viewModel.ExpenseEntries.Count() * 80);
            listView.HeightRequest = slParent.HeightRequest = height;

            viewModel.VisibleLoad = false;
        }

     
        private async void OnApprove(object sender, EventArgs e)
        {
            var resp = await DisplayActionSheet("Are you sure you want to approve?", "No", "Yes");
            if (resp != null && resp.ToString().Length > 0 && resp.Equals("Yes"))
            {
                ApiSaveResponse result = await ExpenseReportService.client.ApproveExpenseReportAsync(viewModel.Id.Value, "Approved");
                if (result.HasErrors)
                {
                    foreach (var error in result.Errors)
                    {
                        Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                }
                else
                {
                    Common.Instance.ShowToastMessage("Expense Report Approved!", DoubleResources.SuccessSnackBar);
                }
                MessagingCenter.Send(this, "ExpenseReportApproved", true);
                await Navigation.PopToRootAsync();
            }
        }

        private async void OnReject(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RejectExpenseReportPage(viewModel.Id.Value));
        }

        

        private void ListView_SwipeStarted(object sender, SwipeStartedEventArgs e)
        {
            itemIndex = -1;
        }

        private void ListView_SwipeEnded(object sender, SwipeEndedEventArgs e)
        {
            itemIndex = e.ItemIndex;
        }


      //  Image rightImage;
        int itemIndex = -1;
        //private void rightImage_BindingContextChanged(object sender, EventArgs e)
        //{
        //    if (rightImage == null)
        //    {
        //        rightImage = sender as Image;
        //        (rightImage.Parent as View).GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(Delete) });
        //        rightImage.Source = ImageSource.FromResource("rejected.png");
        //    }
        //}
        //private void Delete()
        //{
        //    if (itemIndex >= 0)
        //        viewModel.ExpenseEntries.RemoveAt(itemIndex);
        //    this.listView.ResetSwipe();
        //}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (itemIndex >= 0)
            {
                var resp = await DisplayActionSheet("Are you sure you want to delete?", "No", "Yes");
                if (resp != null && resp.ToString().Length > 0 && resp.Equals("Yes"))
                {
                    var item = viewModel.ExpenseEntries[itemIndex];
                    if (item != null)
                    {
                        try
                        {
                            ApiDeleteResponse response = await ExpenseEntryService.client.DeleteAsync(item.Id);

                            //ApiDeleteResponse response = await ExpenseEntryService.client.DeleteAsync(item.Id);
                            if (response != null && response.HasErrors)
                            {
                                Common.Instance.ShowToastMessage(response.Errors[0].ErrorMessage, DoubleResources.DangerSnackBar);
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Instance.ShowToastMessage(ex.Message, DoubleResources.DangerSnackBar);
                        }
                    }
                    viewModel.ExpenseEntries.RemoveAt(itemIndex);
                    int height = 10 + (viewModel.ExpenseEntries.Count() * 80);
                    listView.HeightRequest = slParent.HeightRequest = height;
                }
            }
            this.listView.ResetSwipe();
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
    }
}