using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using ProjectInsight.Models.Comments;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.Views.ApprovalRequests;
using ProjectInsightMobile.Views.General;
using ProjectInsightMobile.Enums;
using ProjectInsightMobile.CustomControls;
using HtmlAgilityPack;
using ProjectInsightMobile.Views.Profile;
using ProjectInsight.Models.Base.Responses;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApprovalRequestPage : ContentPage
    {
        ApprovalRequestViewModel viewModel;
        public ProjectInsight.Models.ApprovalRequests.ApprovalRequest approval;

        public ApprovalRequestPage(Guid itemId)
        {
            NavigationPage.SetBackButtonTitle(this, "");
            //HockeyApp.MetricsManager.TrackEvent("ApprovalRequestPage Initialize");

            InitializeComponent();
            viewModel = new ApprovalRequestViewModel();
            viewModel.StatusColor = ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["BlackTextColor"]);
            BindingContext = viewModel;
            GetObicen(itemId);

        }
        private async void GetObicen(Guid taskId)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetApproval(taskId);

            viewModel.VisibleLoad = false;
            if (isSuccess)
            {
                viewModel.LoadingMessage = "Success";
            }
            else
            {
                viewModel.VisibleLoad = false;
                Common.Instance.ShowToastMessage("Error communication with server!", DoubleResources.DangerSnackBar);
            }
        }

        private async Task<bool> GetApproval(Guid itemId)
        {

            try
            {
                approval = await ApprovalRequestService.GetApprovalRequest(itemId);
                
                if (approval != null)
                {
                    var item = new ApprovalRequestViewModel
                    {
                        Id = approval.Id.ToString(),
                        Title = approval.ItemNumberFullAndNameDisplayPreference,
                        Description = HtmlToText.ConvertHtml(approval.Description),
                        //Status = ((ProjectInsightMobile.Enums.ApprovalRequestStateType)approval.ApprovalRequestStateType).ToString(),
                        RequiresApprovalFromCurrentUser = approval.RequiresApprovalFromCurrentUser ?? false,
                        Status = approval.ApprovalRequestStateTypeDescription,
                        ApprovedDate = approval.ApprovedDateTimeUTC != null ? approval.ApprovedDateTimeUTC.Value : approval.CreatedDateTimeUTC.Value,
                        RequestedBy = approval.Requestor != null ? "Requested By: " + approval.Requestor.Name : "",
                        RequestedDate = approval.CreatedDateTimeUTC.HasValue ? approval.CreatedDateTimeUTC.Value.ToString("ddd M/d/yy htt") : string.Empty,
                        DeadlineDate = approval.DeadlineDateTimeUTC.HasValue ? "Deadline: " + approval.DeadlineDateTimeUTC.Value.ToString("ddd M/d/yy htt") : string.Empty
                        
                    };
                    

                    switch ((ApprovalRequestStateType)approval.ApprovalRequestStateType)
                    {
                        case ApprovalRequestStateType.Pending:
                            item.StatusIcon = "pending.png";
                            item.StatusColor = ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["PendingColor"]);
                            break;
                        case ApprovalRequestStateType.Approved:
                            item.StatusIcon = "approved.png";
                            item.StatusColor = ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["ApprovedColor"]);
                            break;
                        case ApprovalRequestStateType.Denied:
                            item.StatusIcon = "rejected.png";
                            item.StatusColor = ExtensionMethods.ConvertColorToHex((Color)Application.Current.Resources["RejectedColor"]); ;
                            break;
                    }

                    //RequiresApprovalFromCurrentUser
                   // item.RequiresApprovalFromCurrentUser = true;
                    if (approval.ApprovalRequestApprovals != null && approval.ApprovalRequestApprovals.Count() > 0)
                    {
                   //     item.RequiresApprovalFromCurrentUser = approval.ApprovalRequestApprovals.Any(x => x.Approver.Id == Common.CurrentWorkspace.UserID);




                        foreach (var app in approval.ApprovalRequestApprovals)
                        {
                            if (app.Approver != null)
                            {

                                //----------------------------------

                                if (string.IsNullOrEmpty(app.Approver.PhotoUrl))
                                {
                                    if (!string.IsNullOrEmpty(app.Approver.AvatarHtml))
                                    {
                                        string myDiv = app.Approver.AvatarHtml;
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
                                        
                                        slAvatar.Margin = new Thickness(0);

                                        Label lblUserId = new Label();
                                        lblUserId.IsVisible = false;
                                        lblUserId.Text = app.Approver.Id.ToString();
                                        lblUserId.Margin = 0;
                                        slAvatar.Children.Add(lblUserId);

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

                                        var goToUserProfile = new TapGestureRecognizer();
                                        goToUserProfile.Tapped += (s, e) =>
                                        {
                                            StackLayout sl = (StackLayout)s;
                                            Label lbl = (Label)sl.Children[0];
                                            Navigation.PushAsync(new UserProfile(new Guid(lbl.Text)));
                                        };
                                        slAvatar.GestureRecognizers.Add(goToUserProfile);
                                        slApprovers.Children.Add(slAvatar);

                                    }
                                }
                                else
                                {
                                    StackLayout slAvatar = new StackLayout();
                                    slAvatar.HeightRequest = 50;
                                    slAvatar.WidthRequest = 50;
                                    
                                    slAvatar.VerticalOptions = LayoutOptions.StartAndExpand;
                                    slAvatar.HorizontalOptions = LayoutOptions.StartAndExpand;
                                    slAvatar.Margin = new Thickness(0);

                                    Label lblUserId = new Label();
                                    lblUserId.IsVisible = false;
                                    lblUserId.Text = app.Approver.Id.ToString();
                                    lblUserId.Margin = 0;
                                    slAvatar.Children.Add(lblUserId);

                                    Image photo = new Image();
                                    photo.Source = ImageSource.FromUri(new Uri(Common.CurrentWorkspace.WorkspaceURL + app.Approver.PhotoUrl));
                                    photo.HeightRequest = 50;
                                    photo.WidthRequest = 50;
                                    photo.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                    photo.VerticalOptions = LayoutOptions.CenterAndExpand;
                                    slAvatar.Children.Add(photo);

                                    var goToUserProfile = new TapGestureRecognizer();
                                    goToUserProfile.Tapped += (s, e) =>
                                    {
                                        StackLayout sl = (StackLayout)s;
                                        Label lbl = (Label)sl.Children[0];
                                        Navigation.PushAsync(new UserProfile(new Guid(lbl.Text)));
                                    };
                                    slAvatar.GestureRecognizers.Add(goToUserProfile);

                                    slApprovers.Children.Add(slAvatar);
                                }

                                //--------------------------------
                            }



                        }
                    }


                    if (approval.ApprovalRequestItems != null & approval.ApprovalRequestItems.Count>0)
                    {
                        foreach (var appitem in approval.ApprovalRequestItems)
                        {
                            string Line1 = string.Empty;
                            string Line2 = string.Empty;
                            string Line3 = string.Empty;
                            string Line4 = string.Empty;
                            string Icon  = string.Empty;
                            

                            Guid Id = appitem.Id.Value;
                            if (appitem.ItemType == 8)
                            {
                                //Project

                                Line1 = appitem.NameWithDisplayPreference;
                                Line2 = FormatPeriodRange(appitem.StartDate, appitem.EndDate);
                                Line3 = appitem.Status ?? string.Empty;
                                Line4 = appitem.PrimaryProjectManager ?? string.Empty;
                                Icon = "item_project.png";
                            }
                            else if (appitem.ItemType == 9)
                            {
                                //Task
                                Line1 = appitem.NameWithDisplayPreference;
                                Line2 = FormatPeriodRange(appitem.StartDate, appitem.EndDate);
                                Line3 = appitem.ProjectAffiliationNameAndNumber;
                                Icon = "item_task.png";
                            }
                            else if (appitem.ItemType == 10)
                            {
                                //To-Do
                                Line1 = appitem.NameWithDisplayPreference;
                                Line2 = FormatPeriodRange(appitem.StartDate, appitem.EndDate);
                                Line3 = appitem.Status ?? string.Empty;
                                Line4 = appitem.PrimaryProjectManager ?? string.Empty;
                                Icon = "item_project.png";
                            }
                            else if (appitem.ItemType == 21)
                            {
                                //Issue
                                Line1 = appitem.NameWithDisplayPreference;

                                string priority = appitem.IssuePriority ?? string.Empty;
                                string separator = " - ";
                                if (String.IsNullOrEmpty(priority))
                                    separator = "";

                                Line2 = String.Format("{0}{1}{2}", FormatPeriodRange(appitem.StartDate, appitem.EndDate), separator, priority);
                                Line3 = appitem.Status ?? string.Empty; ;
                                Line4  = appitem.ProjectAffiliationNameAndNumber ?? string.Empty;
                                Icon = "item_issue.png";
                            }
                            else if (appitem.ItemType == 22)
                            {
                                //Approval Request
                                Line1 = appitem.NameWithDisplayPreference;
                                Line2 = FormatPeriodRange(appitem.StartDate, appitem.EndDate);
                                Line3 = appitem.Status ?? string.Empty;
                                Line4 = appitem.PrimaryProjectManager ?? string.Empty;
                                Icon = "item_project.png";
                            }
                            else if (appitem.ItemType == 3)
                            {
                                //File
                                Line1 = appitem.NameWithDisplayPreference;
                                Line2 = FormatPeriodRange(appitem.StartDate, appitem.EndDate);
                                Line3 = appitem.Status ?? string.Empty;
                                Line4 = appitem.PrimaryProjectManager ?? string.Empty;
                                Icon = "item_project.png";
                            }
                            else
                            {
                                
                                Line1 = appitem.NameWithDisplayPreference;
                                Line2 = FormatPeriodRange(appitem.StartDate, appitem.EndDate);
                                Line3 = appitem.Status ?? string.Empty;
                                Line4 = appitem.PrimaryProjectManager ?? string.Empty;
                                Icon = "item_project.png";
                            }


                            StackLayout slItem = new StackLayout();
                            slItem.HorizontalOptions = LayoutOptions.FillAndExpand;
                            slItem.VerticalOptions = LayoutOptions.FillAndExpand;
                            slItem.Orientation = StackOrientation.Horizontal;
                            slItem.Padding = new Thickness(0);
                            slItem.Margin = new Thickness(20,0,20,0);
                            slItem.Spacing = 10;

                            slItemList.Children.Add(slItem);

                            StackLayout slIcon = new StackLayout();
                            slIcon.HorizontalOptions = LayoutOptions.Start;
                            slIcon.VerticalOptions = LayoutOptions.StartAndExpand;
                            slIcon.WidthRequest = 50;
                            slIcon.HeightRequest = 50;
                            slIcon.Padding = new Thickness(0,8,0,0);
                            slIcon.Margin = new Thickness(0);
                            slIcon.Spacing = 0;

                            slItem.Children.Add(slIcon);

                            Image icon = new Image();
                            icon.HeightRequest = 50;
                            icon.WidthRequest = 50;
                            icon.HorizontalOptions = LayoutOptions.Center;
                            icon.Source = Icon;
                            icon.Margin = new Thickness(0);

                            slIcon.Children.Add(icon);

                            StackLayout slDetails = new StackLayout();
                            slDetails.HorizontalOptions = LayoutOptions.FillAndExpand;
                            slDetails.VerticalOptions = LayoutOptions.FillAndExpand;
                            slDetails.Orientation = StackOrientation.Vertical;
                            slDetails.Padding = new Thickness(0);
                            slDetails.Margin = new Thickness(0);
                            slDetails.Spacing = 0;

                            slItem.Children.Add(slDetails);

                            StackLayout slLine1 = new StackLayout();
                            slLine1.HorizontalOptions = LayoutOptions.FillAndExpand;
                            slLine1.VerticalOptions = LayoutOptions.FillAndExpand;
                            slLine1.Orientation = StackOrientation.Vertical;
                            slLine1.IsVisible = !string.IsNullOrEmpty(Line1);
                            slLine1.Padding = new Thickness(0);
                            slLine1.Margin = new Thickness(0);
                            slLine1.Spacing = 0;

                            slDetails.Children.Add(slLine1);
                          

                            Label lblLine1 = new Label();
                            lblLine1.LineBreakMode = LineBreakMode.NoWrap;
                            lblLine1.FontSize = 20;
                            lblLine1.Text = Line1;
                            lblLine1.Margin = new Thickness(0);

                            slLine1.Children.Add(lblLine1);
                            
                            StackLayout slLine2 = new StackLayout();
                            slLine2.HorizontalOptions = LayoutOptions.FillAndExpand;
                            slLine2.VerticalOptions = LayoutOptions.FillAndExpand;
                            slLine2.Orientation = StackOrientation.Vertical;
                            slLine2.IsVisible = !string.IsNullOrEmpty(Line2);
                            slLine2.Padding = new Thickness(0);
                            slLine2.Margin = new Thickness(0);
                            slLine2.Spacing = 0;

                            slDetails.Children.Add(slLine2);

                            Label lblLine2 = new Label();
                            lblLine2.LineBreakMode = LineBreakMode.NoWrap;
                            lblLine2.FontSize = 14;
                            lblLine2.Text = Line2;
                            lblLine2.Margin = new Thickness(0);

                            slLine2.Children.Add(lblLine2);

                            StackLayout slLine3 = new StackLayout();
                            slLine3.HorizontalOptions = LayoutOptions.FillAndExpand;
                            slLine3.VerticalOptions = LayoutOptions.FillAndExpand;
                            slLine3.Orientation = StackOrientation.Vertical;
                            slLine3.IsVisible = !string.IsNullOrEmpty(Line3);
                            slLine3.Padding = new Thickness(0);
                            slLine3.Margin = new Thickness(0);
                            slLine3.Spacing = 0;
                            slDetails.Children.Add(slLine3);

                            Label lblLine3 = new Label();
                            lblLine3.LineBreakMode = LineBreakMode.NoWrap;
                            lblLine3.FontSize = 14;
                            lblLine3.Text = Line3;
                            lblLine3.Margin = new Thickness(0);
                            slLine3.Children.Add(lblLine3);

                            StackLayout slLine4 = new StackLayout();
                            slLine4.HorizontalOptions = LayoutOptions.FillAndExpand;
                            slLine4.VerticalOptions = LayoutOptions.FillAndExpand;
                            slLine4.Orientation = StackOrientation.Vertical;
                            slLine4.IsVisible = !string.IsNullOrEmpty(Line4);
                            slLine4.Padding = new Thickness(0);
                            slLine4.Margin = new Thickness(0);
                            slLine4.Spacing = 0;
                            slDetails.Children.Add(slLine4);

                            Label lblLine4 = new Label();
                            lblLine4.LineBreakMode = LineBreakMode.NoWrap;
                            lblLine4.FontSize = 14;
                            lblLine4.Text = Line4;
                            lblLine4.Margin = new Thickness(0);
                            slLine4.Children.Add(lblLine4);

                            BoxView line1 = new BoxView();
                            line1.HeightRequest = 0.5;
                            line1.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
                            line1.HorizontalOptions = LayoutOptions.FillAndExpand;
                            line1.VerticalOptions = LayoutOptions.End;
                            line1.Margin = new Thickness(0,2,0,2);
                            slItemList.Children.Add(line1);
                        }
                    }

                    viewModel = item;
                    BindingContext = viewModel;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private static string FormatPeriodRange(DateTime? StartDate, DateTime? EndDate)
        {
            string startDate = string.Empty;
            string endDate = string.Empty;

            if (StartDate.HasValue && EndDate.HasValue)
            {
                if (StartDate.Value.Date ==EndDate.Value.Date)
                {
                    startDate = string.Empty;
                    endDate = EndDate.Value.Date.ToString("M/d/yy");
                }
                else
                {
                    startDate = StartDate.Value.Date.ToString("M/d/yy") + " - ";
                    endDate = EndDate.Value.Date.ToString("M/d/yy");
                }
            }
            else
            {
                if (StartDate.HasValue)
                    startDate = StartDate.Value.Date.ToString("M/d/yy");
                else if (EndDate.HasValue)
                    endDate = EndDate.Value.Date.ToString("M/d/yy");
            }
            return startDate + endDate;
        }

        private async void OnMarkAproveDeny(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AproveDenyPage(viewModel, approval));
        }

        public void OnTappedDetails(object sender, EventArgs e)
        {
            // Do stuff       
            if (viewModel.IsExpandedList)
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "plus.png";
                return;
            }

            viewModel.ShowHideIcon = "minus.png";
            viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;
            viewModel.VisibleLoad = false;
        }

        private async void OnShowDescriptionAllContent(object sender, EventArgs e)
        {
            DescriptionViewModel descVM = new DescriptionViewModel();
            descVM.Description = viewModel.Description;
            descVM.Title = viewModel.Title;
            await Navigation.PushAsync(new HtmlContentDescription(descVM));
        }


        private async void OnApprove(object sender, EventArgs e)
        {

            if (approval.ApprovalRequestApprovals != null && approval.ApprovalRequestApprovals.Count() > 0)
            {
                foreach (var app in approval.ApprovalRequestApprovals)
                {
                    if (app.Approver != null && app.Approver.Id == Common.CurrentWorkspace.UserID)
                    {

                        ApiSaveResponse result = await ApprovalRequestApprovalService.client.ApproveApprovalRequestAsync(app.Id.Value);
                        if (result.HasErrors)
                        {
                            foreach (var error in result.Errors)
                            {
                                Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                            }
                        }
                        else
                        {
                            Common.Instance.ShowToastMessage("Approval Request approved!", DoubleResources.SuccessSnackBar);
                            Common.RefreshWorkList = true;
                            await Navigation.PopAsync();
                        }
                    }
                }
            }
              
            //await Navigation.PopAsync();
        }

        private async void OnReject(object sender, EventArgs e)
        {
          

            if (approval.ApprovalRequestApprovals != null && approval.ApprovalRequestApprovals.Count() > 0)
            {
                foreach (var app in approval.ApprovalRequestApprovals)
                {
                    if (app.Approver != null && app.Approver.Id == Common.CurrentWorkspace.UserID)
                    {
                        await Navigation.PushAsync(new RejectApprovalRequestPage(app.Id.Value));
                        break;
                    }
                }
            }

        }
        public async void OnTappedItems(object sender, EventArgs e)
        {

            // Do stuff       
            if (viewModel.IsExpandedList)
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "Arrow_right.png";
                return;
            }


            //viewModel.Items = new ObservableCollection<RelatedItem>(relatedItems);
           // viewModel.RelatedItemsLabel = "Related Items (" + viewModel.Items.Count + ")";
            viewModel.ShowHideIcon = "Arrow_down.png";
            //viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;

        }
    }
}