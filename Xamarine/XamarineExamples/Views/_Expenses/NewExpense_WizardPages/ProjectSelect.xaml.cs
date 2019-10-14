using ProjectInsight.Models.Projects;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectSelect : ContentPage
    {
        ProjectSelectViewModel viewModel;
        public ProjectSelect()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();

            //HockeyApp.MetricsManager.TrackEvent("SubmitTimeSheetPage Initialize");

            viewModel = new ProjectSelectViewModel();
            BindingContext = viewModel;
            LoadData();
        }

        private async void LoadData()
        {
            viewModel.VisibleLoad = true;

            var projects = await ProjectsService.client.ActiveAsync();
            viewModel.Projects = projects;

      
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

            var contacts = obj as Project;
            if (contacts.Name.ToLower().Contains(searchBar.Text.ToLower())
                 || contacts.Name.ToLower().Contains(searchBar.Text.ToLower()))
                return true;
            else
                return false;
        }

        

        private async void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {

            if (e.ItemData is Project)
            {
                await Navigation.PushAsync(new ExpensesBulkEntry(((Project)e.ItemData).Name, ((Project)e.ItemData).Id.Value));
            }


        }
    }
}