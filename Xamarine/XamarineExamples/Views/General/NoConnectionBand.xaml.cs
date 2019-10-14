using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace ProjectInsightMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoConnectionBand : ContentView
    {
        public NoConnectionBand()
        {
            InitializeComponent();

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            lblDescription.Text = String.Format("No Internet Connection!");
          

            if (!CrossConnectivity.Current.IsConnected)
            {
                el1.IsVisible = true;
                el2.IsVisible = true;
                el3.IsVisible = true;
                lblDescription.IsVisible = true;
            }
        }

        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (e.IsConnected)
            {
                el1.IsVisible = false;
                el2.IsVisible = false;
                el3.IsVisible = false;
                lblDescription.IsVisible = false;
            }
            else
            {
                el1.IsVisible = true;
                el2.IsVisible = true;
                el3.IsVisible = true;
                lblDescription.IsVisible = true;
            }
        }
    }
}