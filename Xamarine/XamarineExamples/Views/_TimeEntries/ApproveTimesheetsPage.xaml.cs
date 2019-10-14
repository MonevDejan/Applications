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
    public partial class ApproveTimesheetsPage : ContentPage
    {
        ApproveTimesheetViewModel viewModel;
        public ApproveTimesheetsPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("ApproveTimesheetsPage Initialize");
            
        }
        protected override void OnAppearing()
        {
            viewModel = new ApproveTimesheetViewModel();
            GetData();
            BindingContext = viewModel;
        }

        private void GetData()
        {
            viewModel.VisibleLoad = true;

            List<TimeSheet> result = TimeSheetService.GetTimesheetsForApproval();
            lstAllPeriod.Children.Clear();
            if (result != null && result.Count > 0)
            {
                int i = 0;
                foreach (var timeSheet in result)
                {
                    i++;
                    TimeSheetPeriod item = new TimeSheetPeriod();
                    item.StartDate = timeSheet.StartDate.Value;
                    item.EndDate = timeSheet.EndDate.Value;
                    item.ActualHours = (timeSheet.ActualHours ?? 0);
                    item.ActualHoursFormated = timeSheet.ActualHoursFormattedString;
                    item.Status = timeSheet.ApprovalStatus ?? 0;
                    item.Name = timeSheet.User.FirstName +  " "  + timeSheet.User.LastName;
                    //item.TimeEntries = timeSheet.TimeEntries;
                    item.TimeSheetId = timeSheet.Id;
                    item.User_Id = timeSheet.User_Id;
                    viewModel.TimeSheets.Add(item);

                    //AddChildTimeSheets(timeSheet);
                    if (i== result.Count)
                        PreviousPeriods_GeneratedRows(item,true);
                    else
                        PreviousPeriods_GeneratedRows(item);
                }
            }
            else
            {
                lblNoNotif.IsVisible = true;
            }
            viewModel.VisibleLoad = false;

        }

        private void PreviousPeriods_GeneratedRows(TimeSheetPeriod item, bool AddLineAtEnd = false)
        {
            BoxView line = new BoxView();
            line.HeightRequest = 0.5;
            line.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
            line.HorizontalOptions = LayoutOptions.FillAndExpand;
            line.Margin = 0;
            Label lblId = new Label();
            lblId.IsVisible = false;
            lblId.Text = item.TimeSheetId.ToString();
            lblId.Margin = 0;

            StackLayout main = new StackLayout();
            main.HorizontalOptions = LayoutOptions.FillAndExpand;
            main.VerticalOptions = LayoutOptions.StartAndExpand;
            main.Orientation = StackOrientation.Vertical;
            main.Padding = 0;
            main.Margin = 0;
            main.Spacing = 0;
            main.BackgroundColor =  (Color)Application.Current.Resources["WhiteTextColor"];
            main.HeightRequest = 80;
            main.Children.Add(line);
            //main.Children.Add(lblId);

            StackLayout first = new StackLayout();
            first.HorizontalOptions = LayoutOptions.FillAndExpand;
            first.BackgroundColor = (Color)Application.Current.Resources["WhiteTextColor"];
            first.Padding = new Thickness(15, 5, 15, 5);
            first.Orientation = StackOrientation.Horizontal;
            main.Children.Add(first);




            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                StackLayout sl = (StackLayout)s;

                Label lbl = (Label)sl.Children[0];
                string periodId = lbl.Text;

                var clickedItem = viewModel.TimeSheets.Where(x => x.TimeSheetId ==  new Guid(periodId)).FirstOrDefault();
                if (clickedItem.ShowPendingApprovalIcon)
                {
                    Navigation.PushAsync(new ApproveTimesheetPage(clickedItem));
                }

            };
            //main.GestureRecognizers.Add(tapGestureRecognizer);
            lstAllPeriod.Children.Add(main);


            //< StackLayout HorizontalOptions = "FillAndExpand" VerticalOptions = "StartAndExpand" Orientation = "Horizontal" >


            StackLayout firstFirst = new StackLayout();
            firstFirst.VerticalOptions = LayoutOptions.Center;
            firstFirst.HorizontalOptions = LayoutOptions.FillAndExpand;
            firstFirst.Orientation = StackOrientation.Vertical;
            //firstFirst.BackgroundColor = Color.Blue;

            first.Children.Add(firstFirst);
            firstFirst.Children.Add(lblId);
            firstFirst.GestureRecognizers.Add(tapGestureRecognizer);

            Label lbl1 = new Label();
            lbl1.FontSize = 26;
            lbl1.HorizontalTextAlignment = TextAlignment.Start;
            lbl1.HorizontalOptions = LayoutOptions.FillAndExpand;
            lbl1.VerticalTextAlignment = TextAlignment.Center;
            lbl1.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
            lbl1.Text = item.Name;
            firstFirst.Children.Add(lbl1);

            Label lbl2 = new Label();
            lbl2.FontSize = 16;
            lbl2.HorizontalTextAlignment = TextAlignment.Start;
            lbl2.HorizontalOptions = LayoutOptions.FillAndExpand;
            lbl2.VerticalTextAlignment = TextAlignment.Center;
            lbl2.TextColor = (Color)Application.Current.Resources["BlackTextColor"];
            lbl2.LineBreakMode = LineBreakMode.NoWrap;
            lbl2.Text = item.ActualHoursFormated + " | " + item.PeriodFormated;

            firstFirst.Children.Add(lbl2);

            StackLayout firstSecond = new StackLayout();
            firstSecond.VerticalOptions = LayoutOptions.CenterAndExpand;
            firstSecond.HorizontalOptions = LayoutOptions.End;
            //firstSecond.BackgroundColor = Color.Pink;
            firstSecond.Padding = 0;
            firstSecond.Margin = 0;
            firstSecond.Spacing = 0;
            first.Children.Add(firstSecond);

                Label lblId1 = new Label();
                lblId1.IsVisible = false;
                lblId1.Text = item.TimeSheetId.ToString();
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
                    var clickedItem = viewModel.TimeSheets.Where(x => x.TimeSheetId == new Guid(periodId)).FirstOrDefault();
                    if (clickedItem.ShowPendingApprovalIcon)
                    {
                        Navigation.PushAsync(new ApproveTimesheetPage(clickedItem));
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
                lblSubmit.Text = "Approve";
                frmSubmit.Content = lblSubmit;
           

            if (AddLineAtEnd)
            {
                BoxView line1 = new BoxView();
                line1.HeightRequest = 0.5;
                line1.Color = (Color)Application.Current.Resources["DarkGrayTextColor"];
                line1.HorizontalOptions = LayoutOptions.FillAndExpand;
                line1.VerticalOptions= LayoutOptions.End;
                line1.Margin = 0;

                
                lstAllPeriod.Children.Add(line1);
            }


        }


        private async void ApproveAll_Clicked(object sender, EventArgs e)
        {
            if (viewModel.TimeSheets == null || viewModel.TimeSheets.Count == 0)
            {
                Common.Instance.ShowToastMessage("No pending timesheets waiting for approval", DoubleResources.DangerSnackBar);
                return;
            }
            var resp = await DisplayActionSheet("Do you want to approve all time sheets?", "No", "Yes");
            if (resp != null && resp.ToString().Length > 0 && resp.Equals("Yes"))
            {
                bool isSussessfull = true;
                foreach (TimeSheetPeriod ts in viewModel.TimeSheets)
                {

                    ApiSaveResponse result = await TimeSheetService.ApproveAsync(ts.TimeSheetId.Value);
                    if (result.HasErrors)
                    {
                        isSussessfull = false;
                        foreach (var error in result.Errors)
                        {
                            Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                        }
                    }
                    
                 
                      
                }
                if (isSussessfull)
                    Common.Instance.ShowToastMessage("All timesheets approved!", DoubleResources.SuccessSnackBar);

                GetData();
            }

        }
    }
}