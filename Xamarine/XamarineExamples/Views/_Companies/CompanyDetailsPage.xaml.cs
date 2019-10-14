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
using ProjectInsightMobile.Views.General;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompanyDetailsPage : ContentPage
    {
        CompanyDetailsViewModel viewModel;
        ProjectInsight.Models.Companies.Company company;
        public CompanyDetailsPage(Guid itemId)
		{
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent ();
            BindingContext = viewModel = new CompanyDetailsViewModel();
            Get(itemId);
                       
        }
        private async void Get(Guid itemId)
        {

            viewModel.VisibleLoad = true;
            viewModel.LoadingMessage = "";

            bool isSuccess = await GetItem(itemId);

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

        private async Task<bool> GetItem(Guid itemId)
        {

            try
            {
                company = await CompanyService.client.GetAsync(itemId, modelProperties: new ProjectInsight.Models.Base.ModelProperties("default,Address1,Address2,City,Country,PostCode,Region,Fax,Phone,EmailAddress,IsActive,IsAutomaticallySetupCommunicateAddedCompany,IsPermissionAssignmentEnabled,IsInternal,IsProjectAssignmentEnabled,IsTimeAndExpenseEntryEnabled,UpdatedBy,CreatedBy,CreatedDateTimeUTC,CustomFieldValue_Id"));

                if (company != null)
                {
                    var itemVM = new CompanyDetailsViewModel
                    {
                        Id = company.Id.ToString(),
                        Title = company.Name,
                        Name = company.Name + Environment.NewLine,
                        Address1 = company.Address1,
                        Address2 = company.Address2,
                        City = company.City,
                        Country = company.Country,
                        PostCode = company.PostCode,
                        Region = company.Region,
                        Fax = company.Fax,
                        Phone = company.Phone,
                        EmailAddress = company.EmailAddress,
                        IsActive =  company.IsActive,
                        IsAutomaticallySetupCommunicateAddedCompany = company.IsAutomaticallySetupCommunicateAddedCompany,
                        IsPermissionAssignmentEnabled = company.IsPermissionAssignmentEnabled,
                        IsInternal = company.IsInternal,
                        IsProjectAssignmentEnabled = company.IsProjectAssignmentEnabled,
                        IsTimeAndExpenseEntryEnabled = company.IsTimeAndExpenseEntryEnabled,
                        UpdatedBy = company.UpdatedBy,
                        CreatedBy = company.CreatedBy,
                        CreatedDateTimeUTC = company.CreatedDateTimeUTC,
                        CustomFieldValue_Id = company.CustomFieldValue_Id
                    };
                    itemVM.AllDetails = (string.IsNullOrEmpty(company.Address1.Trim()) ? "" : company.Address1 + Environment.NewLine) +
                        (string.IsNullOrEmpty(company.Address2) ? "" : company.Address2.Trim() + Environment.NewLine) +
                        (string.IsNullOrEmpty(company.City) ? "" : company.City.Trim() + ", ") +
                        (string.IsNullOrEmpty(company.Region) ? "" : company.Region.Trim() + " ") +
                        (string.IsNullOrEmpty(company.PostCode) ? Environment.NewLine :  company.PostCode.Trim() + Environment.NewLine) +
                        (string.IsNullOrEmpty(company.Country) ? "" : company.Country.Trim() + Environment.NewLine) +
                        (string.IsNullOrEmpty(company.Phone) ? "" : company.Phone.Trim() + " (Phone)" + Environment.NewLine) +
                        (string.IsNullOrEmpty(company.Fax) ? "" : company.Fax.Trim() + " (Fax)" + Environment.NewLine);

                    


                    viewModel = itemVM;
                    BindingContext = viewModel;

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }




        private async void OnShowDescriptionAllContent(object sender, EventArgs e)
        {
            DescriptionViewModel descVM = new DescriptionViewModel();
            //descVM.Description = viewModel.Description;
            descVM.Title = viewModel.Title;
            await Navigation.PushAsync(new HtmlContentDescription(descVM));
        }

        public async void OnActionEmail(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:" + viewModel.EmailAddress));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyProjectsPage());
        }
    }
}