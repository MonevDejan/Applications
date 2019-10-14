using ProjectInsightMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ProjectInsightMobile.Services;
using System.Collections.Generic;
using ProjectInsight.Models.Items;
using ProjectInsightMobile.Helpers;

namespace ProjectInsightMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewApprovalPage_SearchItem : ContentPage
    {

        ApprovalSearchItemViewModel viewModel;
        int ItemType = -1;
        public NewApprovalPage_SearchItem(int itemType)
		{
            NavigationPage.SetBackButtonTitle(this,"");

            ItemType = itemType;
            InitializeComponent();

            viewModel = new ApprovalSearchItemViewModel();

            BindingContext = viewModel;

        }
     

  


        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length > 2)
            {
                listView.IsRefreshing = true;

                viewModel.SearchResult = null;

                try
                {
                    List<ItemSearchResult> searchResult = await SearchesService.client.GetItemsByNumberFullOrNameAsync(numberFullOrName: e.NewTextValue,itemType: ItemType);

                    if (searchResult != null && searchResult.Count > 0)
                    {
                        foreach (var item in searchResult)
                        {
                            item.ItemIconUrl = Common.CurrentWorkspace.WorkspaceURL + item.ItemIconUrl.Replace("/projectinsight.webapp", "");
                        }
                        viewModel.SearchResult = searchResult;
                    }
                }
                catch (Exception ex)
                {
                }

                listView.IsRefreshing = false;
            }

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is Grid)
            {
                var templateGrid = (Grid)sender;
                if (templateGrid.Parent != null && templateGrid.Parent.BindingContext != null)
                {

                    var item = ((ItemSearchResult)templateGrid.Parent.BindingContext);
                    await Navigation.PushAsync(new NewApprovalPage_Details(container_Id: item.Item_Id));

                }
            }

        }
    }
}