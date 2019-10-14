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
	public partial class IssueDetailsCustomView : ContentView
    {                                                                   
        public IssueDetailsCustomView()
		{
           //HockeyApp.MetricsManager.TrackEvent("IssueDetailsCustomView Initialize");

            InitializeComponent();                                     
        }

        public void OnTappedRelatedItems(object sender, EventArgs e)
        {
            var viewModel = ((IssueDetailsViewModel)this.BindingContext);
            // Do stuff       
            if (viewModel.IsExpandedList)
            {
                viewModel.IsExpandedList = false;
                viewModel.ShowHideIcon = "Arrow_right.png";
                return;
            }

            viewModel.ShowHideIcon = "Arrow_down.png";
            viewModel.VisibleLoad = true;
            viewModel.IsExpandedList = true;       
            viewModel.VisibleLoad = false;
        }
    }
}