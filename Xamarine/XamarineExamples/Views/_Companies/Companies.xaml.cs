using ProjectInsight.Models.Base;
using ProjectInsight.Models.Base.Responses;
using ProjectInsight.Models.Companies;
using ProjectInsight.Models.TimeAndExpense;
using ProjectInsight.WebApi.Client.TimeAndExpense;
using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Views.General;
using Syncfusion.ListView.XForms;
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
    public partial class Companies : ContentPage
    {
        CompanyFilterViewModel viewModel;
        public Companies()
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

            viewModel.SelectedCompany = viewModel.Companies.FirstOrDefault(x => x.Id == Common.Filter.CompanyId);

            viewModel.IsBusy = true;
            viewModel.VisibleLoad = false;
        }



        SearchBar searchBar = null;
        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            searchBar = (sender as SearchBar);
            if (listView.DataSource != null)
            {
                this.listView.DataSource.Filter = FilterContacts;
                this.listView.DataSource.RefreshFilter();
            }
        }

        private bool FilterContacts(object obj)
        {
            if (searchBar == null || searchBar.Text == null)
                return true;

            var contacts = obj as Company;
            if (contacts.Name.ToLower().Contains(searchBar.Text.ToLower())
                 || contacts.Name.ToLower().Contains(searchBar.Text.ToLower()))
                return true;
            else
                return false;
        }

        private async void ListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Company selCompany = (Company)e.AddedItems[0];
                await Navigation.PushAsync(new CompanyDetailsPage(selCompany.Id.Value));
            }
        }

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {

            if (e.ItemData is Company)
            {
                await Navigation.PushAsync(new CompanyDetailsPage(((Company)e.ItemData).Id.Value));
            }


        }
    }
}