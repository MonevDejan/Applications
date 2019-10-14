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
using Plugin.Connectivity;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyProjectsPage : ContentPage
    {
        MyProjectsViewModel viewModel;

        public MyProjectsPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            InitializeComponent();
           //HockeyApp.MetricsManager.TrackEvent("MyProjectsPage Initialize");

            BindingContext = viewModel = new MyProjectsViewModel() { Title = "Projects" };

            InitListViews();

        }

        private void InitListViews()
        {
            //ItemsListViewDroid.SeparatorColor = Color.Gray;
            ItemsListViewiOS.SeparatorColor = Color.Gray;

            //if (Device.RuntimePlatform.ToLower() == "android")
            //{
            //    ItemsListViewDroid.IsVisible = true;
            //    ItemsListViewiOS.IsVisible = false;
            //    ItemsListViewDroid.ItemsSource = viewModel.Projects;
            //}
            //else
            {
               // ItemsListViewDroid.IsVisible = false;
                ItemsListViewiOS.IsVisible = true;
                ItemsListViewiOS.ItemsSource = viewModel.Projects;
            }
        }
        private async void GridTemplate_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected) return;

                if (sender is SwipeGestureGrid || sender is Grid)
                {
                    var templateGrid = (Grid)sender;
                    if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                    {

                        var item = ((MyWorkItem)templateGrid.Parent.BindingContext);
                        var itemID = item.Id;
                        if (item.ItemType == ItemType.Projects)
                        {
                            await Navigation.PushAsync(new ProjectDetailsPage(itemID));
                        }
                        else
                        {
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

            if (!CrossConnectivity.Current.IsConnected) return;

            MessagingCenter.Subscribe<NewProjectPage, Guid>(this, "ProjectAdded", async (obj, itemId) =>
            {
                if (itemId != Guid.Empty)
                    LoadData(null,null);

                MessagingCenter.Unsubscribe<NewProjectPage, Guid>(this, "ProjectAdded");
            });

            await Navigation.PushAsync(new NewProjectPage());
        }

      

       

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            slNoConnection.Children.Clear();
            slNoConnection.Children.Add(new NoConnectionBand());


            viewModel.LoadLocalContent = true;
            viewModel.LoadItemsCommand.Execute(null);
        }

       

        private string ConvertColorToHex(Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = string.Format("#{0:X2}{1:X2}{2:X2}", red, green, blue);
            return  hex;
        }
        public async void LoadData(object sender, EventArgs e)
        {
            IsBusy = true;
            viewModel.LoadLocalContent = false;
            viewModel.LoadItemsCommand.Execute(null);
            IsBusy = false;
        }

        public void OnDelete(object sender, EventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected) return;

            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
        private void GridTemplate_SwipeLeft(object sender, EventArgs e)
        {
            //TODO Swipe left is disabled temporary because not implemented
            return;
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
            //TODO Swipe left is disabled temporary because not implemented
            return;
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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {

                if (!CrossConnectivity.Current.IsConnected) return;
                if (sender is StackLayout)
                {
                    //  lstCurrentItems.BeginRefresh();

                    var menuItem = ((StackLayout)sender);
                    if (menuItem != null && menuItem.BindingContext != null)
                    {
                        MyWorkItem item = ((MyWorkItem)menuItem.BindingContext);

                        await Navigation.PushAsync(new MyWorkPage(item.Id, "All", item.Title));

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}