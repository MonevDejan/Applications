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

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Congratulations : ContentPage
    {
      

        ExpenseEntryViewModel viewModel;
        public Congratulations(ExpenseEntryViewModel ViewModel)
		{
            NavigationPage.SetBackButtonTitle(this,"");
            //NavigationPage.SetHasBackButton(this, false);

            InitializeComponent();
        
            BindingContext = viewModel = ViewModel;

            SetdData();

            if (viewModel.SelectedExpenseType == 1)
                Title = "Add Receipt";
            else if (viewModel.SelectedExpenseType == 2)
                Title = "Add Mileage";
            else if (viewModel.SelectedExpenseType == 3)
                Title = "Add Other";
            else
                Title = "Add";

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
            ProjectInsight.Models.TimeAndExpense.ExpenseEntry expenseEntry = new ProjectInsight.Models.TimeAndExpense.ExpenseEntry();


            try
            {
               
                if (viewModel.SelectedExpenseType == 1)
                {
                    //RECEIPT
                    expenseEntry.ActualCost = viewModel.Amount;
                   
                    if (viewModel.PhotoArray != null)
                    {
                        try
                        {
                            ProjectInsight.Models.Files.FileUpload file = new ProjectInsight.Models.Files.FileUpload();

                            if (string.IsNullOrEmpty(viewModel.PhotoName))
                                file.FileName =  "IMG_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".jpg"; //string.Format("Receipt{0}.jpg", viewModel.Date.Value.ToString("_yyyyMMdd"));
                            else
                                file.FileName = viewModel.PhotoName;

                            string fileEncodedString = Convert.ToBase64String(viewModel.PhotoArray);

                            byte[] PhotoArray = Encoding.UTF8.GetBytes(fileEncodedString);
                            System.IO.Stream stream = new System.IO.MemoryStream(PhotoArray);

                            file.FileContentsAsStream = stream;

                            file.FileContentsBase64Encoded = fileEncodedString;
                            expenseEntry.UploadedFile = file;

                        }
                        catch (Exception ex)
                        {

                        }
                    }


                }
                else if (viewModel.SelectedExpenseType == 2)
                {
                    //MILEAGE

                    expenseEntry.Qty = viewModel.Amount;
                    expenseEntry.UnitPrice = viewModel.CostPerMile;
                    expenseEntry.ActualCost = expenseEntry.Qty * expenseEntry.UnitPrice;
                }
                else if (viewModel.SelectedExpenseType == 3)
                {
                    //Other
                    expenseEntry.Qty = viewModel.Total;
                    expenseEntry.UnitPrice = viewModel.Amount;
                    expenseEntry.ActualCost = expenseEntry.Qty * expenseEntry.UnitPrice;
                }

                expenseEntry.Date = viewModel.Date;
                expenseEntry.Description = viewModel.Description;

                if (viewModel.SelectedCompany != null)
                    expenseEntry.Company_Id = viewModel.SelectedCompany.Id;

                if (viewModel.SelectedProject != null)
                    expenseEntry.Project_Id = viewModel.SelectedProject.Id;

                if (viewModel.SelectedTask != null)
                    expenseEntry.Task_Id = viewModel.SelectedTask.Id;

                if (viewModel.SelectedExpenseCode != null)
                    expenseEntry.ExpenseCode_Id = viewModel.SelectedExpenseCode.Id;

                expenseEntry.UserCreated_Id = Common.CurrentWorkspace.UserID;
                expenseEntry.User_Id = Common.CurrentWorkspace.UserID;

                ApiSaveResponse result = await ExpenseEntryService.client.SaveAsync(expenseEntry);

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

            await Navigation.PushAsync(new ChooseExpense());
        }

        private async void BackToExpenseList_Tapped(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new ExpensesPage());

            var app = Application.Current as App;
            var mainPage = (StartupMasterPage)app.MainPage;
            NavigationPage page = null;
            page = new NavigationPage(new ExpensesPage());
            mainPage.Detail = page;
            mainPage.Title = page.Title;


        }
    }
}