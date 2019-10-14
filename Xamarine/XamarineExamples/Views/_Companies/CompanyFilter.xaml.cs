using ProjectInsight.Models.Base;
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
    public partial class CompanyFilter : ContentPage
    {
        CompanyFilterViewModel viewModel;
        public CompanyFilter()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
            
            //HockeyApp.MetricsManager.TrackEvent("SubmitTimeSheetPage Initialize");

            viewModel = new CompanyFilterViewModel();
            BindingContext = viewModel;
            LoadData();
        }

        private async void LoadData()
        {
            viewModel.VisibleLoad = true;

            var companies = await CompanyService.client.ListActiveAsync();
            viewModel.Companies = companies;
            if (Common.Filter.CompanyId != null)
                viewModel.SelectedCompany = viewModel.Companies.FirstOrDefault(x => x.Id == Common.Filter.CompanyId);

            viewModel.IsBusy = true;
            viewModel.VisibleLoad = false;
        }

        private async void OnSubmit(object sender, EventArgs e)
        {
            try
            {
                viewModel.VisibleLoad = true;

                Common.Filter.CompanyId = viewModel.SelectedCompany.Id;
                

                var app = Application.Current as App;
                var mainPage = (StartupMasterPage)app.MainPage;
                NavigationPage page = null;

                Common.Instance.bottomNavigationViewModel.ActiveIcon = 1;
                page = new NavigationPage(new MyWorkPage());


                mainPage.Detail = page;
                mainPage.Title = page.Title;

            }
            catch (Exception ex)
            {

            }
            viewModel.VisibleLoad = false;
        }

        private async void OnCancel(object sender, EventArgs e)
        {

            Common.Filter.CompanyId = null;

            var app = Application.Current as App;
            var mainPage = (StartupMasterPage)app.MainPage;
            NavigationPage page = null;

            Common.Instance.bottomNavigationViewModel.ActiveIcon = 1;
            page = new NavigationPage(new MyWorkPage());


            mainPage.Detail = page;
            mainPage.Title = page.Title;
        }

        private void CmbCompanies_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Syncfusion.XForms.ComboBox.SfComboBox cmb = (Syncfusion.XForms.ComboBox.SfComboBox)sender;
            if (cmb.SelectedValue == null)
            {
                viewModel.SelectedCompany = null;
            }
        }
    }
}