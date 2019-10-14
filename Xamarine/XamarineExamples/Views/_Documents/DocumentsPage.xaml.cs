using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.CustomControls;
using ProjectInsightMobile.Models;
using ProjectInsightMobile.Services;
using ProjectInsightMobile.ViewModels;
using SQLite;                            
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DocumentsPage : ContentPage
    {
        public ObservableCollection<DocumentViewModel> listWork { get; set; } 

        DocumentsViewModel viewModel;

        Guid? folderId = null;
        public DocumentsPage(Guid parendId, bool isProject = false)
		{
            folderId = parendId;

            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent ();
            BindingContext = viewModel = new DocumentsViewModel(parendId, isProject);
           //HockeyApp.MetricsManager.TrackEvent("DocumentsPage Initialize");

            if (Device.RuntimePlatform.ToLower() == "android")
            {
                ItemsListViewDroid.IsVisible = true;
                ItemsListViewiOS.IsVisible = false;
            }
            else
            {
                ItemsListViewDroid.IsVisible = false;
                ItemsListViewiOS.IsVisible = true;
            }
           

        }

        private async void GridTemplate_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid || sender is Grid)
                {
                    var templateGrid = (Grid)sender;      
                    if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                    {           
                        var item = ((DocumentViewModel)templateGrid.Parent.BindingContext);
                        var itemID = item.Id;
                        if (item.IsFolder)
                        {
                            await Navigation.PushAsync(new DocumentsPage(itemID));
                        }
                        else
                        {
                            await Navigation.PushAsync(new FileDetailsPage(itemID));
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
          // await Navigation.PushAsync(new NewDocumentPage(folderId.Value));

            await Navigation.PushAsync(new UploadItem(folderId.Value, "Project"));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //SQLiteConnection connection = Common.Instance.InitializeDatabase();
            //listWork = new ObservableCollection<DocumentViewModel>(Common.Instance.GetUserWork().Where(x => x.ItemType == ItemType.Projects));
            //if (listWork.ToList().Count == 0)
            //{
                //if (viewModel.Items.Count == 0)
                    viewModel.LoadItemsCommand.Execute(new Guid());
            //}
            //else
            //{
            //    ////voa za da gi procite lokalnite 
            //    if (Device.RuntimePlatform.ToLower() == "android")
            //    {
            //        ItemsListViewDroid.ItemsSource = null;
            //        ItemsListViewDroid.ItemsSource = listWork;

            //        ItemsListViewiOS.ItemsSource = null;
            //    }
            //    else
            //    {
            //        ItemsListViewiOS.ItemsSource = null;
            //        ItemsListViewiOS.ItemsSource = listWork;

            //        ItemsListViewDroid.ItemsSource = null;
            //    }

            //}                                                                                       
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
        private void GridTemplate_SwipeLeft(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = (Grid)sender;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                        layoutShowHidden.IsVisible = true;
                        templateGrid.ColumnDefinitions[1].Width = new GridLength(50, GridUnitType.Absolute);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void GridTemplate_SwipeRight(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeGestureGrid)
                {
                    var templateGrid = (Grid)sender;
                    if (templateGrid != null && templateGrid is Grid)
                    {
                        layoutShowHidden.IsVisible = false;
                        templateGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnHide(object sender, EventArgs e)
        {
            try
            {
                if (sender is MenuItem)
                {
                    var templateGrid = (MenuItem)sender;
                    if (templateGrid != null) //&& templateGrid is Grid)
                    {
                        layoutShowHidden.IsVisible = false;
                        //templateGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Absolute);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
      

    }
}