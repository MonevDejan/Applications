using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsight.Models.Base.Responses;
using ProjectInsightMobile.Helpers;
using System.IO;
using ProjectInsight.WebApi.Client.Files;
using System.Net.Http;
using System.Text;
using ProjectInsight.WebApi.Client;
using ProjectInsightMobile.Views.MyTime;

namespace ProjectInsightMobile.Views.TimeEntries
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Congratulations : ContentPage
    {


        TimeEntryWizardViewModel viewModel;
        public Congratulations(TimeEntryWizardViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
            //NavigationPage.SetHasBackButton(this, false);

            InitializeComponent();
        
            BindingContext = viewModel = ViewModel;

            SetdData();


            Title = "Add Time Entry";

        }

        private async void SetdData()
        {
            viewModel.VisibleLoad = true;
           
            viewModel.LoadingMessage = "";
            slError.IsVisible = false;

            bool isSuccess = await SaveData();
            viewModel.IsBusy = true;
            viewModel.VisibleLoad = false;
        }
        private async Task<bool> SaveData()
        {
            ProjectInsight.Models.TimeAndExpense.TimeEntry timeEntry = new ProjectInsight.Models.TimeAndExpense.TimeEntry();


            try
            {


                if (viewModel.UseHoursMinutesInput)
                {

                    //decimal dActualHours = Convert.ToDecimal(TimeSpan.Parse(viewModel.ActualHours).TotalHours);
                    //timeEntry.ActualHours = dActualHours;

                    timeEntry.ActualTimeString = viewModel.ActualHours.Trim();
                }
                else
                {
                    decimal act = 0;
                    bool succPars = decimal.TryParse(viewModel.ActualHoursDec, out act);
                    if (succPars)
                    {
                        timeEntry.ActualHours = act;
                    }
                }

                timeEntry.User_Id = Common.CurrentWorkspace.UserID;
                timeEntry.Description = viewModel.Description;
                    timeEntry.Company_Id = viewModel.SelectedCompany != null ? viewModel.SelectedCompany.Id : null;

                    timeEntry.Project_Id = viewModel.SelectedProject != null ? viewModel.SelectedProject.Id : null;


                if (viewModel.ItemType == ItemType.Task)
                    timeEntry.Task_Id = viewModel.SelectedTask != null ? viewModel.SelectedTask.Id : null;
                else if (viewModel.ItemType == ItemType.Issue)
                    timeEntry.Issue_Id = viewModel.SelectedTask != null ? viewModel.SelectedTask.Id : null;
                else if (viewModel.ItemType == ItemType.Todo)
                    timeEntry.ToDo_Id = viewModel.SelectedTask != null ? viewModel.SelectedTask.Id : null;

                timeEntry.ExpenseCode_Id = viewModel.SelectedExpenseCode.Id;
                timeEntry.IsBillable = viewModel.IsBillable;
                timeEntry.Date = viewModel.Date;
                timeEntry.CreatedDateTimeUTC = DateTime.UtcNow;
                //timeEntry.TimerCreatedDateTimeUTC = DateTime.UtcNow;                
                ApiSaveResponse result = await TimeEntryService.client.SaveAsync(timeEntry);

                if (result.HasErrors)
                {
                    string errorMsg = string.Empty;
                    foreach (var error in result.Errors)
                    {
                        errorMsg += error.ErrorMessage + Environment.NewLine;
                        //Common.Instance.ShowToastMessage(error.ErrorMessage, DoubleResources.DangerSnackBar);
                    }
                    slHeader.Text = "Error";
                    slHeader.TextColor = (Color)Application.Current.Resources["RedTextColor"];
                    slError.IsVisible = true;
                    slError.Text = errorMsg;
                    imgIcon.Source = "error.png";

                }
                else
                {
                 
                }

            }
            catch (Exception ex)
            {
                slHeader.Text = "Error";
                slHeader.TextColor = (Color)Application.Current.Resources["RedTextColor"];
                slError.IsVisible = true;
                slError.Text = "Unknown Error!";
                imgIcon.Source = "error.png";
            }
            
            return true;
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {

                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private async void AddAnotherExpense_Tapped(object sender, EventArgs e)
        {
            var timeEntry = new Company_Project_Task(new TimeEntryWizardViewModel() { ItemType = ItemType.Task });
                
            await Navigation.PushAsync(timeEntry);
        }

        private async void BackToExpenseList_Tapped(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new ExpensesPage());

            var app = Application.Current as App;
            var mainPage = (StartupMasterPage)app.MainPage;
            NavigationPage page = null;
            page = new NavigationPage(new MyTimePage());
            mainPage.Detail = page;
            mainPage.Title = page.Title;


        }
    }
}