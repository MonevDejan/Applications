using ProjectInsightMobile.Helpers;
using ProjectInsightMobile.ViewModels;
using ProjectInsightMobile.Models;
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
	public partial class LinksCustomView : ContentView
    {
        public static Guid relatedId;
        public LinkItemsViewModel viewModel;
        public LinksCustomView()
		{
			InitializeComponent ();
           //HockeyApp.MetricsManager.TrackEvent("LinksCustomView Initialize");

            BindingContext = viewModel = new LinkItemsViewModel();
        }

        public void OnTappedLinksItems(object sender, EventArgs e)
        {                                                            
            // Do stuff       
            if (viewModel.IsExpandedList)
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "plus.png";
                return;
            }

            viewModel.ShowHideIcon = "minus.png";
            viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;       
            viewModel.VisibleLoad = false;
        }



        public static readonly BindableProperty ItemIdProperty = BindableProperty.Create(
                                                       propertyName: "ItemId",
                                                       returnType: typeof(string),
                                                       declaringType: typeof(LinksCustomView),
                                                       defaultValue: "",
                                                       defaultBindingMode: BindingMode.TwoWay,
                                                       propertyChanged: ItemIdPropertyChanged);

        public string ItemId
        {
            get { return base.GetValue(ItemIdProperty).ToString(); }
            set { base.SetValue(ItemIdProperty, value); }
        }

        private static void ItemIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (LinksCustomView)bindable;
            if (control == null) return;
            // control.TextEntry.Text = newValue.ToString();

            if (newValue != null)
                relatedId = new Guid(newValue.ToString());
        }

        public async void OnAddLink(object sender, EventArgs e)
        {

            viewModel.ShowHideIcon = "plus.png";
            viewModel.IsExpandedList = false;                                    
        }
    }
}